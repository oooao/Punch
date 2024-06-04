using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using people_errandd.Droid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using TimePicker = Xamarin.Forms.TimePicker;

[assembly: ExportRenderer(typeof(TimePicker), typeof(BorderlessTimePickerRenderer))]
namespace people_errandd.Droid
{
    
        public class BorderlessTimePickerRenderer : TimePickerRenderer
        {


            protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
            {
                base.OnElementChanged(e);
            
                if (e.OldElement == null)
                {
                    Control.Background = null;

                    var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
                    layoutParams.SetMargins(0, 0, 0, 0);
                    LayoutParameters = layoutParams;
                    Control.LayoutParameters = layoutParams;
                    Control.SetPadding(0, 0, 0, 0);
                    SetPadding(0, 0, 0, 0);
                }
            }
        }
    
}