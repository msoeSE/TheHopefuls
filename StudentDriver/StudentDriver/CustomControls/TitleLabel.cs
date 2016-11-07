using System;
using Xamarin.Forms;
namespace StudentDriver
{
	public class TitleLabel : Label
	{
		public TitleLabel ()
		{
			this.FontAttributes = FontAttributes.Bold;
			this.FontSize = Device.GetNamedSize (NamedSize.Large, typeof (Label));
		}
	}
}
