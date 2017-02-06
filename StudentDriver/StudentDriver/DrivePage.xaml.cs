using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StudentDriver
{
	public partial class DrivePage : ContentPage
	{
		private bool drivingButtonPressed = false;
		public DrivePage ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar (this, false);
			drivingButton.Clicked += async (object sender, EventArgs e) => {
				drivingButton.BackgroundColor = drivingButtonPressed ? AppColors.Third : AppColors.Fourth;
				drivingButtonPressed = !drivingButtonPressed;
				drivingButton.Text = drivingButtonPressed ? "Stop" : "Start";
			};
		}


	}
}
