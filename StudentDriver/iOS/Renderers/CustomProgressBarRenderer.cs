using System;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms;
using StudentDriver.iOS;
using CoreGraphics;
using UIKit;
[assembly: ExportRenderer (typeof (ProgressBar), typeof (CustomProgressBarRenderer))]
namespace StudentDriver.iOS
{
	public class CustomProgressBarRenderer : ProgressBarRenderer
	{
		protected override void OnElementChanged (ElementChangedEventArgs<ProgressBar> e)
		{
			base.OnElementChanged (e);
			if (Control != null) {
				Control.ProgressTintColor = UIColor.Orange;
			}
		}
	}
}
