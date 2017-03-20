using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;

namespace StudentDriver
{
	public partial class DrivePage : ContentPage
	{
		private bool isStudentDriving = false;
		private ulong pointsCollected = 0;
		//speed and distance in miles (imperial)
		private double currentAverageSpeed = 0.0;
		private double currentDistance = 0.0;
		private Position lastPosition = null;
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
					pointsCollected++;
					if (lastPosition == null)
					{
						lastPosition = eventArg.Position;
						currentAverageSpeed = ConvertSpeeds(eventArg.Position.Speed);
					}
					else
					{
						currentDistance += GeoCodeCalc.CalcDistance(lastPosition.Latitude, lastPosition.Longitude, eventArg.Position.Latitude, eventArg.Position.Longitude);
						//do a weighted average in order to not handle drastic jumps
						currentAverageSpeed = (currentAverageSpeed * 0.95) + (ConvertSpeeds(eventArg.Position.Speed) * 0.05);

					}
					distanceLabel.Text = string.Format("{0} mi", currentDistance.ToString("F"));
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
				UpdateDrivingButton();
				try
				{
					//IMPORTANT distances and speeds coming from the geolocator are in meters, so we 
					//need to do our conversions.
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
				timeLabel.Text = "0 min";
				distanceLabel.Text = "0.0 mi";
				avgSpeedLabel.Text = "0.0 MPH";
				pointsCollected = 0;
				currentAverageSpeed = 0;
				currentDistance = 0;
				lastPosition = null;
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
