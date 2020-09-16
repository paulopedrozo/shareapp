using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using System.IO;
using Xamarin.Forms;

namespace ShareApp.Droid
{
    [Activity(Label = "ShareApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"application/pdf")]
    [IntentFilter(new[] { Intent.ActionSend }, Categories = new[] { Intent.CategoryDefault }, DataMimeType = @"image/*")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        protected override void OnNewIntent(Intent intent)
        {
            if (intent.Action == Intent.ActionSend)
            {
                ClipData.Item item = intent.ClipData.GetItemAt(0);

                // Pega o nome do arquivo que está sendo compartilhado
                Android.Database.ICursor returnCursor = ContentResolver.Query(item.Uri, null, null, null, null);
                int index = returnCursor.GetColumnIndex(Android.Provider.OpenableColumns.DisplayName);
                returnCursor.MoveToFirst();
                string ext = Path.GetExtension(returnCursor.GetString(index));

                var filestream = ContentResolver.OpenInputStream(item.Uri);

                App.LoadFromSendTo(filestream, ext);
            }

            base.OnNewIntent(intent);
        }
    }
}