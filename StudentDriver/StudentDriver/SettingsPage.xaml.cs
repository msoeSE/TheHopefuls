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
		public SettingsPage ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar (this, false);
			//Just placeholder states to resize the elements
		    
			statePicker.Items.Add ("Wisconsin");
			statePicker.SelectedIndexChanged += StateSelected;
			logOutButton.Clicked += LogOutTapped;
		}

	    protected override async void OnAppearing()
	    {
	        var user =  await SQLiteDatabase.GetInstance().GetUser();
	        studentName.Text = user.FirstName;
	        var image = new CircleImage() {Aspect = Aspect.AspectFit};
            profileImage.Source = ImageSource.FromUri(new Uri(user.ImageUrl));
	        profileImage = image;
	    }


        void StateSelected (object sender, EventArgs e)
		{

		}

		void LogOutTapped (object sender, EventArgs e)
		{
			//TODO Fix UserDialogs call? it's not working?
            WebService.Instance.Logout();
            App.LoginAction.Invoke();
		}
	}
}
