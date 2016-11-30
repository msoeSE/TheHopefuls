using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace StudentDriver
{
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
			facebookLogin.GestureRecognizers.Add (new TapGestureRecognizer (async (View arg1, object arg2) => {
				await FacebookFrameTapped (arg1, arg2);
			}));
			googleLogin.GestureRecognizers.Add (new TapGestureRecognizer (async (View arg1, object arg2) => {
				await GoogleLoginTapped (arg1, arg2);
			}));
		}

		async Task GoogleLoginTapped (View arg1, object arg2)
		{
			googleLogin.Opacity = 0.25;
			await googleLogin.FadeTo (1.0);
			App.Current.MainPage = new StudentDriverPage ();
		}

		async Task FacebookFrameTapped (View arg1, object arg2)
		{
			facebookLogin.Opacity = 0.25;
			await facebookLogin.FadeTo (1.0);
			App.Current.MainPage = new StudentDriverPage ();
		}
	}
}
