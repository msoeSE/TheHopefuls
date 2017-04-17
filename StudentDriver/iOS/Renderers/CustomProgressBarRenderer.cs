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
				var loadingColor = AppColors.Main;
				Control.ProgressTintColor = new UIColor (new nfloat (loadingColor.R), new nfloat (loadingColor.G), new nfloat (loadingColor.B), 1.0f);
			}
		}
	}
}
