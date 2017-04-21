using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using Xamarin.Forms;
using StudentDriver.Services;
using StudentDriver.Models;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;

namespace StudentDriver
{
	public partial class DrivePage : ContentPage
	{
		private volatile bool isStudentDriving = false;
		//speed and distance in miles (imperial)
		private double averageSpeed;
		private TimeSpan currentTime;
		private DrivePoint firstPoint;
		private List<DrivePoint> positions = new List<DrivePoint>();
		private IGeolocator locator;
		private volatile int unsyncDriveId = -1;

		protected override void OnAppearing()
		{
			base.OnAppearing();

			try
			{
				locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 5;
				locator.PositionChanged += PositionChangedEventListener;
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
				try
				{
					var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Permission.Location);
					if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
					{
						if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Location))
						{
							await DisplayAlert("Please Allow Location", "Allow location services for this application.", "Ok");
						}
						var results = await CrossPermissions.Current.RequestPermissionsAsync(new[] { Plugin.Permissions.Abstractions.Permission.Location });
						status = results[Permission.Location];
					}
					if (status != PermissionStatus.Granted)
					{
						Acr.UserDialogs.UserDialogs.Instance.ShowError("Cannot use GPS, no location permissions given.");
						return;
					}
				}
				catch (Exception exception)
				{
					Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to check permissions");
					return;

				}
				Device.StartTimer(new TimeSpan(0, 0, 1), () =>
				{
					if (isStudentDriving)
					{
						currentTime = currentTime.Add(new TimeSpan(0, 0, 1));
						Device.BeginInvokeOnMainThread(() =>
						{
							timeLabel.Text = currentTime.ToString("g");
						});
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
					if (!isStudentDriving)
					{
						await locator.StopListeningAsync();
						var loading = Acr.UserDialogs.UserDialogs.Instance.Loading("Syncing Drives...");
						loading.PercentComplete = 0;
						loading.Show();
						if (positions.Count != 0)
						{
							await ServiceController.Instance.AddDrivePoints(positions);
							positions.Clear();
						}
						loading.PercentComplete = 40;
						var didComplete = await ServiceController.Instance.PostDrivePoints(
							await ServiceController.Instance.GetAllDrivePoints(), 
							await ServiceController.Instance.GetAllUnsyncedDrives());
						if (!didComplete)
						{
							Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to send drivepoints to web service.");
							loading.Hide();
							return;
						}
						loading.PercentComplete = 80;
						//TODO get all of the drive sessions, and drivepoints (all unsync drives and sessions, not just the current)
						//TODO Setup the drives to push to web backend
						//TODO push to web
						//TODO on success, clear database locally. 
						//TODO on failure, notify the user, do not clear database.
						loading.PercentComplete = 100;
						loading.Hide();

					}
					else
					{
						if (locator.IsGeolocationEnabled && locator.IsGeolocationAvailable)
						{

							var driveId = await ServiceController.Instance.CreateUnsyncDrive();
							var currentLocation = await locator.GetPositionAsync();
							if (driveId == -1)
							{
								Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to create drive, please try again");
								UpdateDrivingButton();
								return;
							}
							this.unsyncDriveId = driveId;
							var weatherCreated = await ServiceController.Instance.CreateDriveWeatherData(
								currentLocation.Latitude, currentLocation.Longitude, unsyncDriveId);
							if (!weatherCreated)
							{
								await ServiceController.Instance.StopUnsyncDrive(unsyncDriveId);
								unsyncDriveId = -1;
								Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to create weather data, stopping drive.");
								UpdateDrivingButton();
								return;
							}
							await locator.StartListeningAsync(1, 5.0, true);

						}
						else
						{
							Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to use GPS: GPS is not enabled");
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

		void PositionChangedEventListener(object sender, PositionEventArgs e)
		{
			var currentPosition = e.Position;
			if (positions.Count < 100) // every 100 points, we will add to the database.
			{
				var drivePoint = new DrivePoint
				{
					UnsyncDriveId = unsyncDriveId,
					Latitude = currentPosition.Latitude,
					Longitude = currentPosition.Longitude,
					PointDateTime = currentPosition.Timestamp.DateTime,
					Speed = currentPosition.Speed,
				};
				if (firstPoint == null) firstPoint = drivePoint;
				positions.Add(drivePoint);
				averageSpeed = Math.Abs(averageSpeed - new double()) < 0.0001 ? ConvertSpeeds(e.Position.Speed) : (0.8 * averageSpeed) + (0.2 * ConvertSpeeds(e.Position.Speed));
			}
			else
			{
				if (unsyncDriveId == -1)
				{
					unsyncDriveId = ServiceController.Instance.CreateUnsyncDrive().Result;
				}
				Task.Factory.StartNew(async () =>
				{
					await ServiceController.Instance.AddDrivePoints(positions);
					positions.Clear();
				});
			}
			Device.BeginInvokeOnMainThread(() =>
			{
				avgSpeedLabel.Text = string.Format("{0} MPH", averageSpeed.ToString("F"));
			});
		}


		private void UpdateDrivingButton()
		{
			drivingButton.BackgroundColor = isStudentDriving ? AppColors.Third : AppColors.Fourth;
			isStudentDriving = !isStudentDriving;
			if (isStudentDriving)
			{
				
				timeLabel.Text = "0:00:00";
				avgSpeedLabel.Text = "0.0 MPH";
				averageSpeed = 0.0;
				unsyncDriveId = ServiceController.Instance.CreateUnsyncDrive().Result;

			}
			else
			{
				ServiceController.Instance.StopUnsyncDrive(unsyncDriveId).RunSynchronously();
				unsyncDriveId = -1;
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
