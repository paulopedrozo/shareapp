using System;
using System.Collections.Generic;
using Foundation;
using Newtonsoft.Json;
using Social;
using UIKit;

namespace ShareAppShareExtension
{
    public partial class ShareViewController : SLComposeServiceViewController
    {
        public ShareViewController(IntPtr handle) : base(handle)
        {
        }

        public override void DidReceiveMemoryWarning()
        {
            // Releases the view if it doesn't have a superview.
            base.DidReceiveMemoryWarning();

            // Release any cached data, images, etc that aren't in use.
        }

        public override void ViewDidLoad()
        {
        }

        //public override bool ShouldInteractWithUrl(UITextView textView, NSUrl url, NSRange characterRange, UITextItemInteraction interaction)
        //{
        //    return base.ShouldInteractWithUrl(textView, url, characterRange, interaction);
        //}

        public override bool IsContentValid()
        {
            // Do validation of contentText and/or NSExtensionContext attachments here
            return true;
        }

        public override void DidSelectPost()
        {
            // This is called after the user selects Post. Do the upload of contentText and/or NSExtensionContext attachments.           
            NSExtensionItem item = ExtensionContext.InputItems[0];

            if (item.Attachments.Length > 0)
            {
                // *** Aqui precisa receber as imagens ou pdfs. Nesse exemplo está pegando uma URL

                List<string> PathList = new List<string>();
                PathList.Add("Teste");

                // *** Essa parte comentada funciona com compartilhamento de URL
                //foreach (NSItemProvider itemProvider in item.Attachments)
                //{
                //    var type = itemProvider.Description.Split('"');
                //    if (itemProvider.HasItemConformingTo(type[1]))
                //    {
                //        itemProvider.LoadItem(type[1], null, (url, error) =>
                //        {
                //            if (url != null)
                //            {
                //                var a = (NSUrl)url;
                //                PathList.Add(a.AbsoluteString);
                //            }
                //        });
                //    }
                //}

                string path = JsonConvert.SerializeObject(PathList);
                var defs = new NSUserDefaults("group.com.shareapp.ios", NSUserDefaultsType.SuiteName);
                defs.SetString(path, "ListaTeste");

                // **** Chama o projeto IOS para recuperar as imagens/pdfs
                UIApplication.SharedApplication.OpenUrl(new NSUrl("com.shareapp.test://"));

                ExtensionContext.CompleteRequest(new NSExtensionItem[0], null);
            }
        }

        public override void PresentationAnimationDidFinish()
        {
            base.PresentationAnimationDidFinish();
        }

        public override SLComposeSheetConfigurationItem[] GetConfigurationItems()
        {
            // To add configuration options via table cells at the bottom of the sheet, return an array of SLComposeSheetConfigurationItem here.
            return new SLComposeSheetConfigurationItem[0];
        }
    }
}
