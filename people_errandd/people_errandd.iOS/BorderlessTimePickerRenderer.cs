using Foundation;
using people_errandd.iOS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using TimePicker = Xamarin.Forms.TimePicker;

[assembly: ExportRenderer(typeof(TimePicker), typeof(BorderlessTimePickerRenderer))]
namespace people_errandd.iOS
{ 
    public class BorderlessTimePickerRenderer : TimePickerRenderer
    {

        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;
                Control.Layer.CornerRadius = 5;
                //Control.TextColor = UIColor.Black;
            }
        }

    }
}