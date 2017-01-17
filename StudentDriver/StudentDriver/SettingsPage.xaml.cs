using System;
using System.Collections.Generic;

using Xamarin.Forms;
using StudentDriver.Services;
using StudentDriver.Helpers;
using System.Threading.Tasks;

namespace StudentDriver
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
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
			await WebService.GetInstance ().OAuthLogout ();

		}
	}
}
