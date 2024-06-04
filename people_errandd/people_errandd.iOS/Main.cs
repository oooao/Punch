using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;

namespace people_errandd.iOS
{
    public class Application
    {
        public static object Current { get; internal set; }

        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            
            UIApplication.Main(args, null, "AppDelegate");
        }


    }
}
