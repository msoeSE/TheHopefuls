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
			statePicker.SelectedIndexChanged += StateSelected;
			logOutButton.Clicked += LogOutTapped;
		}

		void StateSelected (object sender, EventArgs e)
		{

		}

		void LogOutTapped (object sender, EventArgs e)
		{

		}
	}
}
