using System.Linq;
using people_errandd.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;


namespace people_errandd.iOS
{
	public class UnselectedTabColorEffect : TabbedPage
	{
		public UnselectedTabColorEffect()
		{
			var tabOne = new ContentPage
			{
				Title = "首頁",

			};

			var tabTwo = new ContentPage
			{
				Title = "公告",

			};
			var tabThree = new ContentPage
			{
				Title = "行事曆",

			};

			UnselectedTabColor = Color.FromHex("#555555");
			SelectedTabColor = Color.FromHex("#5C76B1");

			Children.Add(tabOne);
			Children.Add(tabTwo);
			Children.Add(tabThree);
		}
	}
}