using System;
using System.IO;
using SQLite;
using StudentDriver.iOS;
using Xamarin.Forms;

[assembly: Dependency (typeof (SQLite_iOS))]
namespace StudentDriver.iOS
{
	public class SQLite_iOS : ISQLite
	{
		private string dbName = "StudentDriver.db3";
		public SQLiteAsyncConnection GetAsyncConnection ()
		{
			string docPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string libraryPath = Path.Combine (docPath, "..", "Library");
			var path = Path.Combine (libraryPath, dbName);
			var conn = new SQLite.SQLiteAsyncConnection (path);
			return conn;
		}

		[Obsolete ("Use GetAsyncConnection instead")]
		public SQLiteConnection GetConnection ()
		{
			string docPath = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
			string libraryPath = Path.Combine (docPath, "..", "Library");
			var path = Path.Combine (libraryPath, dbName);
			var conn = new SQLite.SQLiteConnection (path);
			return conn;
		}
	}
}
