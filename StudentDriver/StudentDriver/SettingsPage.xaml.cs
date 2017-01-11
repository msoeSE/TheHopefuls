using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StudentDriver
{
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
			//Just placeholder states to resize the elements
			statePicker.Items.Add ("Wisconsin");
			nightModePicker.Items.Add ("Off");
			nightModePicker.Items.Add ("On");
			nightModePicker.Items.Add ("Auto");
			nightModePicker.SelectedIndex = 0;
			nightModePicker.SelectedIndexChanged += NightModeSelected;
			statePicker.SelectedIndexChanged += StateSelected;
			logOutButton.Clicked += LogOutTapped;
		}

		void StateSelected (object sender, EventArgs e)
		{

		}

		void NightModeSelected (object sender, EventArgs e)
		{

		}

		void LogOutTapped (object sender, EventArgs e)
		{

		}
	}
}
