using System;
using System.Collections.Generic;
using System.IO;
using Foundation;
using Newtonsoft.Json;
using ShareApp.Helper;
using UIKit;

namespace ShareApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }

        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            //var fileIOS = new FileIOS();
            //var tempPath = fileIOS.GetFilePath("Teste123.png");

            Stream stream = new MemoryStream();
            string ext = "pdf";
            App.LoadFromSendTo(stream, ext);

            return true;
        }

        public override bool ContinueUserActivity(UIApplication application, NSUserActivity userActivity, UIApplicationRestorationHandler completionHandler)
        {
            return base.ContinueUserActivity(application, userActivity, completionHandler);
        }
    }
}
