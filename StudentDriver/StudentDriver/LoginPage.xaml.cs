using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Auth;
using Acr.UserDialogs;

namespace StudentDriver
{
	public partial class LoginPage : ContentPage
	{

		public LoginPage ()
		{
			InitializeComponent ();
			facebookLogin.GestureRecognizers.Add (new TapGestureRecognizer (async (View view, object obj) => {
				await FacebookFrameTapped (view, obj);
			}));
			googleLogin.GestureRecognizers.Add (new TapGestureRecognizer (async (View view, object obj) => {
				await GoogleLoginTapped (view, obj);
			}));
		}
		async Task GoogleLoginTapped (View view, object obj)
		{
			googleLogin.Opacity = 0.25;
			googleLogin.FadeTo (1.0);
			//await Navigation.PushModalAsync (new GoogleLoginPage ());
			App.Current.MainPage = new StudentDriverPage ();
		}

		async Task FacebookFrameTapped (View view, object obj)
		{
			facebookLogin.Opacity = 0.25;
			await facebookLogin.FadeTo (1.0);
			await Navigation.PushModalAsync (new FacebookLoginPage ());

		}
	}
}
