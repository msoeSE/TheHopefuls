using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace StudentDriver
{
	public partial class StatsPage : ContentPage
	{
		private string daytimeDistanceText = "";
		private string nighttimeDistnaceText = "";
		private string totalDistanceText = "";
		private string totalTimeText = "";

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			await Task.Run (() => {
				totalDistanceProgress.ProgressTo (0.6, 1500, Easing.Linear);
				daytimeDistanceProgress.ProgressTo (0.333, 1500, Easing.Linear);
				nighttimeDistanceProgress.ProgressTo (1.0, 1500, Easing.Linear);
			});


		}
		public StatsPage ()
		{
			InitializeComponent ();
			daytimeDistanceText = daytimeDistanceLabel.Text;
			nighttimeDistnaceText = nighttimeDistanceLabel.Text;
			totalDistanceText = totalDistanceLabel.Text;
			totalTimeText = totalTimeLabel.Text;
			daytimeDistanceLabel.GestureRecognizers.Add (new TapGestureRecognizer (daytimeDistanceLabelPressed));
			nighttimeDistanceLabel.GestureRecognizers.Add (new TapGestureRecognizer (nighttimeDistanceLabelPressed));
			totalDistanceLabel.GestureRecognizers.Add (new TapGestureRecognizer (totalDistanceLabelPressed));
			totalTimeLabel.GestureRecognizers.Add (new TapGestureRecognizer (timeLabelPressed));
		}

		void daytimeDistanceLabelPressed (View pressedLabel, object arg2)
		{
			if (daytimeDistanceLabel.Text.Equals (daytimeDistanceText)) {
				daytimeDistanceLabel.Text = "33%";
			} else {
				daytimeDistanceLabel.Text = daytimeDistanceText;
			}

		}

		void nighttimeDistanceLabelPressed (View pressedLabel, object arg2)
		{
			if (nighttimeDistanceLabel.Text.Equals (nighttimeDistnaceText))
				nighttimeDistanceLabel.Text = "100%";
			else {
				nighttimeDistanceLabel.Text = nighttimeDistnaceText;
			}
		}

		void totalDistanceLabelPressed (View pressedLabel, object arg2)
		{
			Label label = (Label)pressedLabel;
			if (label.Text.Equals (totalDistanceText))
				label.Text = "60%";
			else {
				label.Text = totalDistanceText;
			}
		}

		void timeLabelPressed (View arg1, object arg2)
		{
			if (totalTimeLabel.Text.Equals (totalTimeText))
				totalTimeLabel.Text = "Flut shots are fake.";
			else {
				totalTimeLabel.Text = totalTimeText;
			}
		}
	}
}
