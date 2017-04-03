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
		private bool isStudentDriving = false;
		//speed and distance in miles (imperial)
		private double averageSpeed;
		private TimeSpan currentTime;
		private DrivePoint firstPoint;
		private List<DrivePoint> positions = new List<DrivePoint>();
		private IGeolocator locator;
		private UnsyncDrive drive;
		private int unsyncDriveId = -1;

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
						drivePoint.UnsyncDriveId = unsyncDriveId;
						drivePoint.Latitude = currentPosition.Latitude;
						drivePoint.Longitude = currentPosition.Longitude;
						drivePoint.PointDateTime = currentPosition.Timestamp.DateTime;
						drivePoint.Speed = currentPosition.Speed;
						if (firstPoint == null) firstPoint = drivePoint;
						positions.Add(drivePoint);
						averageSpeed = Math.Abs(averageSpeed - new double()) < 0.0001 ? ConvertSpeeds(eventArg.Position.Speed) : (0.8 * averageSpeed) + (0.2 * ConvertSpeeds(eventArg.Position.Speed));
					}
					else
					{
						Task.Factory.StartNew(async () =>
						{
							var database = SQLiteDatabase.GetInstance();
							if (drive == null)
							{
								drive.StartDateTime = firstPoint.PointDateTime;
							}

							await database.AddDrivePoints(positions);
							positions.Clear();
						});
					}
					Device.BeginInvokeOnMainThread(() =>
					{
						avgSpeedLabel.Text = string.Format("{0} MPH", averageSpeed.ToString("F"));
					});

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
					//IMPORTANT distances and speeds coming from the geolocator are in meters, so we 
					//need to do our conversions.
					if (!isStudentDriving)
					{
						await locator.StopListeningAsync();
						var database = SQLiteDatabase.GetInstance();
						var loading = Acr.UserDialogs.UserDialogs.Instance.Loading("Syncing Drives...");
						loading.Show();
						//TODO Check if we have positions to add, then add them.
						if (positions.Count != 0)
						{
							positions.Clear();
						}
						//TODO get all of the drive sessions, and drivepoints (all unsync drives and sessions, not just the current)
						//TODO Setup the drives to push to web backend
						//TODO push to web
						//TODO on success, clear database locally. 
						//TODO on failure, notify the user, do not clear database.

					}
					else
					{
						if (locator.IsGeolocationEnabled && locator.IsGeolocationAvailable)
						{
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

		private void UpdateDrivingButton()
		{
			drivingButton.BackgroundColor = isStudentDriving ? AppColors.Third : AppColors.Fourth;
			isStudentDriving = !isStudentDriving;
			if (isStudentDriving)
			{
				var database = SQLiteDatabase.GetInstance();
				timeLabel.Text = "0:00:00";
				avgSpeedLabel.Text = "0.0 MPH";
				averageSpeed = 0.0;
				unsyncDriveId = database.StartAsyncDrive().Result;

			}
			else
			{
				var database = SQLiteDatabase.GetInstance();
				database.StopAsyncDrive(unsyncDriveId).RunSynchronously();
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
