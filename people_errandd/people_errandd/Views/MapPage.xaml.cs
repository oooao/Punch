using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Extensions;
using people_errandd.ViewModels;
using Xamarin.Forms.Maps;
using Xamarin.Essentials;

namespace people_errandd.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : Rg.Plugins.Popup.Pages.PopupPage
    {
        public MapPage()
        {
            InitializeComponent();
            Position position = new Position(App.Latitude,App.Longitude);
            MapSpan mapSpan = new MapSpan(position, 0.01, 0.01);
            var map = new Xamarin.Forms.Maps.Map(mapSpan)
            {
                IsShowingUser = true,
                HeightRequest = 50,
                WidthRequest = 250,
                HasScrollEnabled = true,
                HasZoomEnabled = true              
            };
            
            WebView View = new WebView
            {
                Source= "https://uri.amap.com/marker?position="+App.Longitude+","+App.Latitude+"&name=目前位置&callnative=1"
                ,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            Label L=new Label
            {
                Text = "現在位置",
                FontSize = 28,
                TextColor=Color.White,
                HorizontalOptions = LayoutOptions.Center
            };
            StackLayout s = new StackLayout()
            {     
                    Margin = new Thickness(50,200)
            };
            try
            {
                if (geoLocation.LocationNowText.Substring(0, 2) == "中國" || geoLocation.LocationNowText.Substring(geoLocation.LocationNowText.Length - 5, 5) == "China")
                {
                    s.Margin = new Thickness(20, 100);
                    s.Children.Add(View);
                }
                else
                {
                    s.Children.Add(L);
                    s.Children.Add(map);

                }
            }
            catch (Exception)
            {
                s.Children.Add(L);
                s.Children.Add(map);
            }
           
            Content = s;           
        }
    }
}