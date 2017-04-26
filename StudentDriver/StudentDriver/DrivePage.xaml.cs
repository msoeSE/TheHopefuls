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
				UpdateDrivingButton();
				try
				{
					if (!isStudentDriving)
					{
						await locator.StopListeningAsync();
						Acr.UserDialogs.UserDialogs.Instance.Loading("Syncing Drives...").Show();
						if (positions.Count != 0)
						{
							await App.ServiceController.AddDrivePoints(positions);
							positions.Clear();
						}
						var allDrivePoints = await App.ServiceController.GetAllDrivePoints();
						var unsyncDrives = await App.ServiceController.GetAllUnsyncedDrives();
						if (allDrivePoints.Count == 0 && unsyncDrives.Count == 0)
						{
							Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to find any saved drives");
							Acr.UserDialogs.UserDialogs.Instance.HideLoading();
							return;
						}
						var didComplete = await App.ServiceController.PostDrivePoints(
							allDrivePoints,
							unsyncDrives);
						if (!didComplete)
						{
							Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to send drivepoints to web service.");
							Acr.UserDialogs.UserDialogs.Instance.HideLoading();
							return;
						}
						if (!await App.ServiceController.DeleteAllDriveData())
						{
							Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to delete local drive data after update");
							Acr.UserDialogs.UserDialogs.Instance.HideLoading();
							return;
						}
						Acr.UserDialogs.UserDialogs.Instance.ShowSuccess("Successfully uploading drive data");
						Acr.UserDialogs.UserDialogs.Instance.HideLoading();
					}
					else
					{
						Acr.UserDialogs.UserDialogs.Instance.ShowLoading("Starting Drive...");
						if (locator.IsGeolocationEnabled && locator.IsGeolocationAvailable)
						{

							var driveId = await App.ServiceController.CreateUnsyncDrive();
							if (driveId == -1)
							{
								Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to create drive, please try again");
								await UpdateDrivingButton();
								return;
							}
							this.unsyncDriveId = driveId;
							var currentLocation = await locator.GetPositionAsync(timeoutMilliseconds: 5000);
							if (currentLocation == null)
							{
								await App.ServiceController.DeleteUnsyncDrive(unsyncDriveId);
								unsyncDriveId = -1;
								Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to get current location, stopping drive");
								await UpdateDrivingButton();
								return;
							}
							var weatherCreated = await App.ServiceController.CreateDriveWeatherData(
								currentLocation.Latitude, currentLocation.Longitude, unsyncDriveId);
							if (weatherCreated == null)
							{
								await App.ServiceController.DeleteUnsyncDrive(unsyncDriveId);
								unsyncDriveId = -1;
								Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to create weather data, stopping drive.");
								await UpdateDrivingButton();
								return;
							}
							else
							{
								UpdateWeatherIcons(weatherCreated.WeatherIcon);
							}
							await locator.StartListeningAsync(1, 5.0, true);

						}
						else
						{
							Acr.UserDialogs.UserDialogs.Instance.ShowError("Unable to use GPS: GPS is not enabled");
							await UpdateDrivingButton();
						}

						Acr.UserDialogs.UserDialogs.Instance.HideLoading();
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
					unsyncDriveId = App.ServiceController.CreateUnsyncDrive().Result;
				}
				Task.Factory.StartNew(async () =>
				{
					await App.ServiceController.AddDrivePoints(positions);
					positions.Clear();
				});
			}
			Device.BeginInvokeOnMainThread(() =>
			{
				avgSpeedLabel.Text = string.Format("{0} MPH", averageSpeed.ToString("F"));
			});
		}


		private async Task UpdateDrivingButton()
		{
			drivingButton.BackgroundColor = isStudentDriving ? AppColors.Third : AppColors.Fourth;
			isStudentDriving = !isStudentDriving;
			if (isStudentDriving)
			{

				timeLabel.Text = "0:00:00";
				avgSpeedLabel.Text = "0.0 MPH";
				averageSpeed = 0.0;

				unsyncDriveId = await App.ServiceController.CreateUnsyncDrive();

			}
			else
			{
				await App.ServiceController.StopUnsyncDrive(unsyncDriveId);
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

		private void UpdateWeatherIcons(string iconName)
		{
			var image = ImageSource.FromFile(string.Format("{0}.png", iconName));
			var timeOfDayImage = DateTime.Now.ToLocalTime().Hour > 18 ? ImageSource.FromFile("day.png") : ImageSource.FromFile("night.png");
			if (image != null && timeOfDayImage != null)
			{
				weatherImage.Source = image;
				timeImage.Source = timeOfDayImage;
			}

		}


	}
}
