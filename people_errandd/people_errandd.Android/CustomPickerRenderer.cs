using Android.Graphics;
using Android.Graphics.Drawables;
using AndroidX.Core.Content;
using people_errandd.Controls;
using people_errandd.Droid;
using System.Runtime.Remoting.Contexts;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(Picker), typeof(CustomPickerRenderer))]
namespace people_errandd.Droid
{
    public class CustomPickerRenderer : PickerRenderer
    {
       

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Background = null;
            }
        }
    }
}