using ShareApp.Models;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ShareApp
{
    public partial class App : Application
    {
        static Page currentpage;
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            if (Application.Current.MainPage.BindingContext is IAppStateAware appStateAwareVm)
                appStateAwareVm.OnResumeApplicationAsync();
        }

        public static void LoadFromSendTo(Stream stream, string ext)
        {
            //Global.sendto = stream;
            //Global.sendtotype = ext;
            if (currentpage?.GetType() == typeof(MainPage))
            {
                MainPage mainpage1 = (MainPage)currentpage;
                //mainpage1.TogglerToolbar(Global.CurrentUrl);
            }
            else
            {
                currentpage?.Navigation.PopModalAsync();
            }
        }

    }
}
