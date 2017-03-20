using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using StudentDriver.Models;

namespace StudentDriver
{
	public partial class DrivePage : ContentPage
	{
		private bool isStudentDriving = false;
		//speed and distance in miles (imperial)
		private double currentAverageSpeed = 0.0;
		private TimeSpan currentTime;
		private List<DrivePoint> positions = new List<DrivePoint>();
		private IGeolocator locator;

		protected override void OnAppearing()
		{
			base.OnAppearing();

			try
			{
				locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 5;
				locator.PositionChanged += (object s, PositionEventArgs eventArg) =>
				{
					var currentPosition = eventArg.Position;
					if (positions.Count < 100) // every 100 points, we will add to the database.
					{
						var drivePoint = new DrivePoint();
						drivePoint.Latitude = currentPosition.Latitude;
						drivePoint.Longitude = currentPosition.Longitude;
						drivePoint.PointDateTime = currentPosition.Timestamp.DateTime;
						drivePoint.Speed = (float)currentPosition.Speed;
						positions.Add(drivePoint);
						currentAverageSpeed = ConvertSpeeds(eventArg.Position.Speed);
					}
					else
					{
						// Save to database.


					}
					avgSpeedLabel.Text = string.Format("{0} MPH", currentAverageSpeed.ToString("F1"));
				};
			}
			catch (Exception ex)
			{
				Acr.UserDialogs.UserDialogs.Instance.ShowError("Error: Unable to Start GPS");
				UpdateDrivingButton();
			}

		}
		public DrivePage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);

			drivingButton.Clicked += async (object sender, EventArgs e) =>
			{
				Device.StartTimer(new TimeSpan(0, 0, 1), () =>
				{
					if (isStudentDriving)
					{
						currentTime.Add(new TimeSpan(0, 0, 1));
						if (currentTime.Days > 0)
						{
							timeLabel.Text = string.Format("{0}d {1}h {2}min {3}sec", currentTime.Days, currentTime.Hours, currentTime.Minutes, currentTime.Seconds);
						}
						else if (currentTime.Hours > 0)
						{
							timeLabel.Text = string.Format("{0}h {1}min {2}sec", currentTime.Hours, currentTime.Minutes, currentTime.Seconds);
						}
						else if (currentTime.Minutes > 0)
						{
							timeLabel.Text = string.Format("{0}min {1}sec", currentTime.Minutes, currentTime.Seconds);
						}
						else
						{
							timeLabel.Text = string.Format("{0}sec", currentTime.Seconds);
						}
						return true;
					}
					else
					{
						currentTime = new TimeSpan();
						return false;
					}
				});
				UpdateDrivingButton();
				try
				{
					//IMPORTANT distances and speeds coming from the geolocator are in meters, so we 
					//need to do our conversions.
					Debug.WriteLine(locator.IsGeolocationEnabled);
					if (!isStudentDriving)
					{
						await locator.StopListeningAsync();
					}
					else
					{
						if (locator.IsGeolocationEnabled && locator.IsGeolocationAvailable)
						{
							await locator.StartListeningAsync(1, 5.0, true);
						}
						else
						{
							Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to use GPS: Please make sure GPS is enabled");
							UpdateDrivingButton();
						}
					}

				}
				catch (Exception exception)
				{
					Acr.UserDialogs.UserDialogs.Instance.ShowError("Error: Unable to Start GPS");
					UpdateDrivingButton();
				}


			};
		}

		private void UpdateDrivingButton()
		{
			drivingButton.BackgroundColor = isStudentDriving ? AppColors.Third : AppColors.Fourth;
			isStudentDriving = !isStudentDriving;
			if (isStudentDriving)
			{
				timeLabel.Text = "0 sec";
				avgSpeedLabel.Text = "0.0 MPH";
				currentAverageSpeed = 0;
			}
			drivingButton.Text = isStudentDriving ? "Stop" : "Start";
		}

		//converts the speed in Meters per second to miles per hours
		private double ConvertSpeeds(double speed)
		{
			// 2.236936 is the conversion factor from meters per second to miles per hour
			return speed * 2.236936;
		}


	}
}
