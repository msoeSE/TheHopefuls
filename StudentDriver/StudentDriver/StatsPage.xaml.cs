using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace StudentDriver
{
	public partial class StatsPage : ContentPage
	{

		public StatsPage ()
		{
			InitializeComponent ();
			daytimeDistanceLabel.GestureRecognizers.Add (new TapGestureRecognizer (daytimeDistanceLabelPressed));
			nighttimeDistanceLabel.GestureRecognizers.Add (new TapGestureRecognizer (nighttimeDistanceLabelPressed));
			totalDistanceLabel.GestureRecognizers.Add (new TapGestureRecognizer (totalDistanceLabelPressed));
			totalTimeLabel.GestureRecognizers.Add (new TapGestureRecognizer (timeLabelPressed));
		}

		void daytimeDistanceLabelPressed (View pressedLabel, object arg2)
		{
			daytimeDistanceLabel.Text = "33%";
		}

		void nighttimeDistanceLabelPressed (View pressedLabel, object arg2)
		{
			nighttimeDistanceLabel.Text = "100%";
		}

		void totalDistanceLabelPressed (View pressedLabel, object arg2)
		{
			Label label = (Label)pressedLabel;
			label.Text = "60%";
		}

		void timeLabelPressed (View arg1, object arg2)
		{
			totalTimeLabel.Text = "Flut shots are fake.";
		}
	}
}
