using System;
using System.Net;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;

using Xamarin.Auth;
using Xamarin.Utilities;

namespace Xamarin.Auth
{

#if XAMARIN_AUTH_INTERNAL
	internal class Request
#else
	public class DummyRequest
#endif
	{
		private HttpWebRequest _request;

		public string Method { get; protected set; }
		public Uri Url { get; protected set; }
		public IDictionary<string, string> Parameters { get; protected set; }
		public string JSONString { get; private set; }
		public virtual Account Account { get; set; }
		private readonly string AuthorizationHeaderKey = "access_token";
		public static string Get = "GET";
		public static string Post = "POST";

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.Request"/> class.
		/// </summary>
		/// <param name='method'>
		/// The HTTP method.
		/// </param>
		/// <param name='url'>
		/// The URL.
		/// </param>
		/// <param name='parameters'>
		/// Parameters that will pre-populate the <see cref="Parameters"/> property or null.
		/// </param>
		/// <param name='account'>
		/// The account used to authenticate this request.
		/// </param>
		public DummyRequest(string method, Uri url, Account account, IDictionary<string, string> parameters = null)
		{
			Method = method;
			Url = url;
			Parameters = parameters == null ?
							 new Dictionary<string, string>() :
							 new Dictionary<string, string>(parameters);
			Account = account;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Xamarin.Auth.request"/> class, this one uses a JSON string 
		/// instead of parameters.
		/// </summary>
		/// <param name='method'>
		/// The HTTP method.
		/// </param>
		/// <param name='url'>
		/// The URL.
		/// </param>
		/// <param name='parameters'>
		/// Parameters that will pre-populate the <see cref="Parameters"/> property or null.
		/// </param>
		/// <param name='account'>
		/// The account used to authenticate this request.
		/// </param>

		public DummyRequest(string method, Uri url, Account account, string jsonString)
		{
			Method = method;
			Url = url;
			Parameters = new Dictionary<string, string>();
			JSONString = jsonString;
			Account = account;
		}

		/// <summary>
		/// A single part of a multipart request.
		/// </summary>
		protected class Part
		{
			/// <summary>
			/// The data.
			/// </summary>
			public Stream Data;

			/// <summary>
			/// The optional textual representation of the <see cref="Data"/>.
			/// </summary>
			public string TextData;

			/// <summary>
			/// The name.
			/// </summary>
			public string Name;

			/// <summary>
			/// The MIME type.
			/// </summary>
			public string MimeType;

			/// <summary>
			/// The filename of this part if it represents a file.
			/// </summary>
			public string Filename;
		}

		/// <summary>
		/// The parts of a multipart request.
		/// </summary>
		protected readonly List<Part> Multiparts = new List<Part>();

		/// <summary>
		/// Adds a part to the request. Doing so will make this request be sent as multipart/form-data.
		/// </summary>
		/// <param name='name'>
		/// Name of the part.
		/// </param>
		/// <param name='data'>
		/// Text value of the part.
		/// </param>
		public void AddMultipartData(string name, string data)
		{
			Multiparts.Add(new Part
			{
				TextData = data,
				Data = new MemoryStream(Encoding.UTF8.GetBytes(data)),
				Name = name,
				MimeType = "",
				Filename = "",
			});
		}

		/// <summary>
		/// Adds a part to the request. Doing so will make this request be sent as multipart/form-data.
		/// </summary>
		/// <param name='name'>
		/// Name of the part.
		/// </param>
		/// <param name='data'>
		/// Data used when transmitting this part.
		/// </param>
		/// <param name='mimeType'>
		/// The MIME type of this part.
		/// </param>
		/// <param name='filename'>
		/// The filename of this part if it represents a file.
		/// </param>
		public virtual void AddMultipartData(string name, Stream data, string mimeType = "", string filename = "")
		{
			Multiparts.Add(new Part
			{
				Data = data,
				Name = name,
				MimeType = mimeType,
				Filename = filename,
			});
		}

		/// <summary>
		/// Gets the response.
		/// </summary>
		/// <returns>
		/// The response.
		/// </returns>
		public virtual Task<DummyResponse> GetResponseAsync()
		{
			return GetResponseAsync(CancellationToken.None);
		}

		/// <summary>
		/// Gets the response.
		/// </summary>
		/// <remarks>
		/// Service implementors should override this method to modify the PreparedWebRequest
		/// to authenticate it.
		/// </remarks>
		/// <param name="cancellationToken"></param>
		/// <returns>
		/// The response.
		/// </returns>
		public virtual Task<DummyResponse> GetResponseAsync(CancellationToken cancellationToken)
		{
			var request = GetPreparedWebRequest();
			request.Headers[AuthorizationHeaderKey] = GetAuthorizationHeader(Account);

			if (Multiparts.Count > 0)
			{
				var boundary = "---------------------------" + new Random().Next();
				request.ContentType = "multipart/form-data; boundary=" + boundary;

				return Task.Factory
						.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null)
						.ContinueWith(reqStreamtask =>
						{

							using (reqStreamtask.Result)
							{
								WriteMultipartFormData(boundary, reqStreamtask.Result);
							}

							return Task.Factory
											.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null)
											.ContinueWith(resTask =>
											{
												return new DummyResponse((HttpWebResponse)resTask.Result);
											}, cancellationToken);
						}, cancellationToken).Unwrap();
			}
			else if (Method == "POST" && Parameters.Count == 0 && !string.IsNullOrEmpty(JSONString))
			{
				var body = JSONString;
				var bodyData = System.Text.Encoding.UTF8.GetBytes(body);
				request.ContentType = "application/json";

				return Task.Factory
						.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null)
						.ContinueWith(reqStreamTask =>
						{

							using (reqStreamTask.Result)
							{
								reqStreamTask.Result.Write(bodyData, 0, bodyData.Length);
							}

							return Task.Factory
										.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null)
											.ContinueWith(resTask => !resTask.IsFaulted ? new DummyResponse((HttpWebResponse)resTask.Result) : null, cancellationToken);
						}, cancellationToken).Unwrap();
			}
			else if (Method == "POST" && Parameters.Count > 0)
			{
				var body = Parameters.FormEncode();
				var bodyData = System.Text.Encoding.UTF8.GetBytes(body);
				request.ContentType = "application/x-www-form-urlencoded";

				return Task.Factory
						.FromAsync<Stream>(request.BeginGetRequestStream, request.EndGetRequestStream, null)
						.ContinueWith(reqStreamTask =>
						{

							using (reqStreamTask.Result)
							{
								reqStreamTask.Result.Write(bodyData, 0, bodyData.Length);
							}

							return Task.Factory
										.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null)
											.ContinueWith(resTask => !resTask.IsFaulted ? new DummyResponse((HttpWebResponse)resTask.Result) : null, cancellationToken);
						}, cancellationToken).Unwrap();
			}
			else
			{
				return Task.Factory
						.FromAsync<WebResponse>(request.BeginGetResponse, request.EndGetResponse, null)
						.ContinueWith(resTask => !resTask.IsFaulted ? new DummyResponse((HttpWebResponse)resTask.Result) : null, cancellationToken);
			}
		}

		void WriteMultipartFormData(string boundary, Stream s)
		{
			//var boundaryBytes = Encoding.ASCII.GetBytes("--" + boundary);

			var boundaryBytes = Encoding.UTF8.GetBytes("--" + boundary);

			foreach (var p in Multiparts)
			{
				s.Write(boundaryBytes, 0, boundaryBytes.Length);
				s.Write(CrLf, 0, CrLf.Length);

				//
				// Content-Disposition
				//
				var header = "Content-Disposition: form-data; name=\"" + p.Name + "\"";
				if (!string.IsNullOrEmpty(p.Filename))
				{
					header += "; filename=\"" + p.Filename + "\"";
				}
				//var headerBytes = Encoding.ASCII.GetBytes(header);

				var headerBytes = Encoding.UTF8.GetBytes(header);
				s.Write(headerBytes, 0, headerBytes.Length);
				s.Write(CrLf, 0, CrLf.Length);

				//
				// Content-Type
				//
				if (!string.IsNullOrEmpty(p.MimeType))
				{
					header = "Content-Type: " + p.MimeType;
					//headerBytes = Encoding.ASCII.GetBytes(header);

					headerBytes = Encoding.UTF8.GetBytes(header);


					s.Write(headerBytes, 0, headerBytes.Length);
					s.Write(CrLf, 0, CrLf.Length);
				}

				//
				// End Header
				//
				s.Write(CrLf, 0, CrLf.Length);

				//
				// Data
				//
				p.Data.CopyTo(s);
				s.Write(CrLf, 0, CrLf.Length);
			}

			//
			// End
			//
			s.Write(boundaryBytes, 0, boundaryBytes.Length);
			s.Write(DashDash, 0, DashDash.Length);
			s.Write(CrLf, 0, CrLf.Length);
		}

		static readonly byte[] CrLf = new byte[] { (byte)'\r', (byte)'\n' };
		static readonly byte[] DashDash = new byte[] { (byte)'-', (byte)'-' };

		/// <summary>
		/// Gets the prepared URL.
		/// </summary>
		/// <remarks>
		/// Service implementors should override this function and add any needed parameters
		/// from the Account to the URL before it is used to get the response.
		/// </remarks>
		/// <returns>
		/// The prepared URL.
		/// </returns>
		protected virtual Uri GetPreparedUrl()
		{
			var url = Url.AbsoluteUri;

			if (Parameters.Count > 0 && Method != "POST")
			{
				var head = Url.AbsoluteUri.Contains("?") ? "&" : "?";
				foreach (var p in Parameters)
				{
					url += head;
					url += Uri.EscapeDataString(p.Key);
					url += "=";
					url += Uri.EscapeDataString(p.Value);
					head = "&";
				}
			}

			return new Uri(url);
		}

		/// <summary>
		/// Returns the <see cref="T:System.Net.HttpWebRequest"/> that will be used for this <see cref="T:Xamarin.Auth.Request"/>. All properties
		/// should be set to their correct values before accessing this object.
		/// </summary>
		/// <remarks>
		/// Service implementors should modify the returned request to add whatever
		/// authentication data is needed before getting the response.
		/// </remarks>
		/// <returns>
		/// The prepared HTTP web request.
		/// </returns>
		protected virtual HttpWebRequest GetPreparedWebRequest()
		{
			if (_request == null)
			{
				_request = (HttpWebRequest)WebRequest.Create(GetPreparedUrl());
				_request.Method = Method;
			}

			if (_request.CookieContainer == null && Account != null)
			{
				_request.CookieContainer = Account.Cookies;
			}

			return _request;
		}


		private static string GetAuthorizationHeader(Account account)
		{
			if (account == null)
			{
				throw new ArgumentNullException("account");
			}
			if (!account.Properties.ContainsKey("access_token"))
			{
				throw new ArgumentException("OAuth2 account is missing required access_token property.", "account");
			}

			return account.Properties["access_token"];
		}
	}
}
