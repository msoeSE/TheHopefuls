﻿using System;
using System.Collections.Generic;

using Xamarin.Forms;
using StudentDriver.Services;
using StudentDriver.Helpers;
using System.Threading.Tasks;
using Acr.UserDialogs;

namespace StudentDriver
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar (this, false);
			//Just placeholder states to resize the elements
			statePicker.Items.Add ("Wisconsin");
			statePicker.SelectedIndexChanged += StateSelected;
			logOutButton.Clicked += async (object sender, EventArgs e) => {
				await LogOutTapped (sender, e);
			};
		}

		void StateSelected (object sender, EventArgs e)
		{

		}

		async Task LogOutTapped (object sender, EventArgs e)
		{
			//TODO Fix UserDialogs call? it's not working?
			if (await WebService.GetInstance ().OAuthLogout ()) {
				App.LoginAction.Invoke ();
			} else {
				UserDialogs.Instance.ShowError ("Unable to Logout");
			}

		}
	}
}
