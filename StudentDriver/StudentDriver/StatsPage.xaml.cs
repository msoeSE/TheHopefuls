using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;
using StudentDriver.Helpers;
using StudentDriver.Models;
using StudentDriver.Services;

namespace StudentDriver
{
	public partial class StatsPage : ContentPage
	{
		private string daytimeHoursText = "";
		private string nighttimeHoursText = "";
		private string totalHoursText = "";
	    private string inclementHoursText = "";
	    private readonly string userId;

	    public StatsPage(string userId)
	    {
	        this.userId = userId;
	    }

        public StatsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing ()
		{
			base.OnAppearing ();
		    IsBusy = true;
		    int defaultIndex = 48;
            var stateSelected = statePicker.Items[defaultIndex];
            await UpdateDrivingData(stateSelected);


		    daytimeHoursLabel.GestureRecognizers.Add(new TapGestureRecognizer(daytimeHoursLabelPressed));
		    nighttimeHoursLabel.GestureRecognizers.Add(new TapGestureRecognizer(nighttimeHoursLabelPressed));
		    totalHoursLabel.GestureRecognizers.Add(new TapGestureRecognizer(totalHoursLabelPressed));
		    inclementHoursLabel.GestureRecognizers.Add(new TapGestureRecognizer(inclementHoursPressed));
            statePicker.SelectedIndexChanged += StatePicker_SelectedIndexChanged;
		    IsBusy = false;
		}

	    public async Task UpdateDrivingData(string stateSelected)
	    {
            var viewModel = await App.ServiceController.GetAggregatedDrivingData(stateSelected,userId);
            if (viewModel != null)
            {
                await UpdateView(viewModel);
            }
        }

	    public async Task UpdateView(DrivingDataViewModel viewModel)
	    {
            await Task.Run(() => {
                totalHoursProgress.ProgressTo(viewModel.Total.PercentCompletedDouble, 1500, Easing.Linear);
                daytimeHoursProgress.ProgressTo(viewModel.TotalDayTime.PercentCompletedDouble, 1500, Easing.Linear);
                nighttimeHoursProgress.ProgressTo(viewModel.TotalNightTime.PercentCompletedDouble, 1500, Easing.Linear);
                inclementHoursProgress.ProgressTo(viewModel.TotalInclement.PercentCompletedDouble, 1500, Easing.Linear);
            });
            daytimeHoursText = viewModel.TotalDayTime.PercentCompletedString;
            nighttimeHoursText = viewModel.TotalNightTime.PercentCompletedString;
            totalHoursText = viewModel.Total.PercentCompletedString;
            inclementHoursText = viewModel.TotalInclement.PercentCompletedString;

        }

        private async void StatePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var stateSelected = statePicker.Items[statePicker.SelectedIndex];
            await UpdateDrivingData(stateSelected);
        }

		void daytimeHoursLabelPressed (View pressedLabel, object arg2)
		{
			if (daytimeHoursLabel.Text.Equals (daytimeHoursText)) {
				daytimeHoursLabel.Text = "33%";
			} else {
				daytimeHoursLabel.Text = daytimeHoursText;
			}

		}

		void nighttimeHoursLabelPressed (View pressedLabel, object arg2)
		{
			if (nighttimeHoursLabel.Text.Equals (nighttimeHoursText))
				nighttimeHoursLabel.Text = "100%";
			else {
				nighttimeHoursLabel.Text = nighttimeHoursText;
			}
		}

		void totalHoursLabelPressed (View pressedLabel, object arg2)
		{
			Label label = (Label)pressedLabel;
			if (label.Text.Equals (totalHoursText))
				label.Text = "60%";
			else {
				label.Text = totalHoursText;
			}
		}

        void inclementHoursPressed(View pressedLabel, object arg2)
        {
            Label label = (Label)pressedLabel;
            if (label.Text.Equals(totalHoursText))
                label.Text = "60%";
            else
            {
                label.Text = totalHoursText;
            }
        }

    }
}
