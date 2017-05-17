using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using StudentDriver.Helpers;

namespace StudentDriver
{
	public partial class StatsPage : ContentPage
	{
		private string _daytimeHoursText = "";
		private string _nighttimeHoursText = "";
		private string _totalHoursText = "";
		private readonly string _userId;

		public StatsPage(string userId)
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
			_userId = userId;
		}

		public StatsPage()
		{
			InitializeComponent();
			NavigationPage.SetHasNavigationBar(this, false);
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();
			IsBusy = true;
			int defaultIndex = 48;
			var stateSelected = statePicker.Items[defaultIndex];
			statePicker.SelectedIndex = defaultIndex;
			await UpdateDrivingData(stateSelected);
			daytimeHoursLabel.GestureRecognizers.Add(new TapGestureRecognizer(daytimeHoursLabelPressed));
			nighttimeHoursLabel.GestureRecognizers.Add(new TapGestureRecognizer(nighttimeHoursLabelPressed));
			totalHoursLabel.GestureRecognizers.Add(new TapGestureRecognizer(totalHoursLabelPressed));
			statePicker.SelectedIndexChanged += StatePicker_SelectedIndexChanged;
			IsBusy = false;
		}

		public async Task UpdateDrivingData(string stateSelected)
		{
			var userId = await GetUserId();
			var viewModel = await App.ServiceController.GetAggregatedDrivingData(stateSelected, userId);
			if (viewModel != null)
			{
				await UpdateView(viewModel);
			}
		}

		private async Task<string> GetUserId()
		{
			if (!string.IsNullOrEmpty(_userId)) return this._userId;
			var user = await App.ServiceController.GetUser();
			return user.ServerId;
		}

		public async Task UpdateView(DrivingDataViewModel viewModel)
		{
			await Task.Run(() =>
			{
				totalHoursProgress.ProgressTo(viewModel.Total.PercentCompletedDouble, 1500, Easing.Linear);
				daytimeHoursProgress.ProgressTo(viewModel.TotalDayTime.PercentCompletedDouble, 1500, Easing.Linear);
				nighttimeHoursProgress.ProgressTo(viewModel.TotalNightTime.PercentCompletedDouble, 1500, Easing.Linear);
			});
			daytimeHoursLabel.Text = viewModel.TotalDayTime.RatioString;
			nighttimeHoursLabel.Text = viewModel.TotalNightTime.RatioString;
			totalHoursLabel.Text = viewModel.Total.RatioString;
		}

		private async void StatePicker_SelectedIndexChanged(object sender, EventArgs e)
		{
			var stateSelected = statePicker.Items[statePicker.SelectedIndex];
			await UpdateDrivingData(stateSelected);
		}

		void daytimeHoursLabelPressed(View pressedLabel, object arg2)
		{
			if (daytimeHoursLabel.Text.Equals(_daytimeHoursText))
			{
				daytimeHoursLabel.Text = "33%";
			}
			else
			{
				daytimeHoursLabel.Text = _daytimeHoursText;
			}

		}

		void nighttimeHoursLabelPressed(View pressedLabel, object arg2)
		{
			if (nighttimeHoursLabel.Text.Equals(_nighttimeHoursText))
				nighttimeHoursLabel.Text = "100%";
			else
			{
				nighttimeHoursLabel.Text = _nighttimeHoursText;
			}
		}

		void totalHoursLabelPressed(View pressedLabel, object arg2)
		{
			Label label = (Label)pressedLabel;
			if (label.Text.Equals(_totalHoursText))
				label.Text = "60%";
			else
			{
				label.Text = _totalHoursText;
			}
		}
	}
}
