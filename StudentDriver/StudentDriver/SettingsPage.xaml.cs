using System;
using System.Collections.Generic;

using Xamarin.Forms;
using StudentDriver.Services;
using StudentDriver.Helpers;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ImageCircle.Forms.Plugin.Abstractions;

namespace StudentDriver
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			//Just placeholder states to resize the elements
			statePicker.SelectedIndexChanged += StateSelected;
			statePicker.SelectedIndex = 48; // wisconsin
			logOutButton.Clicked += LogOutTapped;
			schoolEntry.Unfocused += SchoolEntryUnFocused;
		}

		protected override async void OnAppearing()
		{
			var user = await App.ServiceController.GetUser();
			studentName.Text = user.FirstName;
			var image = new CircleImage() { Aspect = Aspect.AspectFit };
			profileImage.Source = ImageSource.FromUri(new Uri(user.ImageUrl));
			profileImage = image;
		}


		void StateSelected(object sender, EventArgs e)
		{

		}

		void LogOutTapped(object sender, EventArgs e)
		{
			//TODO Fix UserDialogs call? it's not working?

			Acr.UserDialogs.UserDialogs.Instance.Confirm(new ConfirmConfig
			{
				Title = "Logout",
				Message = "Are you sure you want to logout?",
				CancelText = "No",
				OkText = "Yes",
				OnAction = (bool obj) =>
				{
					if (obj)
					{
						App.ServiceController.Logout();
						App.LoginAction.Invoke();
					}

				},
			});

		}



		async void SchoolEntryUnFocused(object sender, FocusEventArgs e)
		{
			schoolEntry.IsEnabled = false;
			var connectSuccessful = await App.ServiceController.ConnectSchool(schoolEntry.Text);
			await DisplayAlert("Connect To School", connectSuccessful ? "Connection Successful" : "Connection Failed", "OK");
			schoolEntry.IsEnabled = true;
		}
	}
}
