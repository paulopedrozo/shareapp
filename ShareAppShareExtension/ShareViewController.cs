using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using CoreFoundation;
using Foundation;
using MobileCoreServices;
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
            if (ExtensionContext.InputItems != null)
            {
                NSExtensionItem item = ExtensionContext.InputItems[0];

                if (item.Attachments != null && item.Attachments.Length > 0)
                {
                    var fileManager = new NSFileManager();
                    var appGroupContainer = fileManager.GetContainerUrl("group.com.shareapp.ios");
                    var appGroupContainerPath = appGroupContainer.Path;
                    string directoryPath = Path.Combine(appGroupContainerPath, "DocPro");
                    Directory.Delete(directoryPath, true);
                    fileManager.CreateDirectory(directoryPath, true, null);
                    int count = 0;

                    foreach (NSItemProvider itemProvider in item.Attachments)
                    {
                        if (itemProvider.HasItemConformingTo(UTType.Image))
                        {
                            string tempPath = string.Empty;

                            try
                            {
                                itemProvider.LoadItem(UTType.Image, null, (NSObject itemObject, NSError error) =>
                                {
                                    NSData imgData = null;

                                    if (itemObject is UIImage)
                                    {
                                        using (imgData = ((UIImage)itemObject).AsPNG())
                                        {
                                            byte[] myByteArray = new byte[imgData.Length];
                                            Marshal.Copy(imgData.Bytes, myByteArray, 0, Convert.ToInt32(imgData.Length));

                                            var fileIOS = new FileIOS();
                                            fileIOS.WriteAllBytes("Teste123.png", myByteArray);
                                            tempPath = fileIOS.GetFilePath("Teste123.png");
                                        }
                                    }
                                    else if (itemObject is NSUrl)
                                    {
                                        NSUrl nsurl = (NSUrl)itemObject;
                                        var nsdata = NSData.FromUrl(nsurl);
                                        if (nsdata != null)
                                        {
                                            var data = UIImage.LoadFromData(nsdata);

                                            string pngFilename = Path.Combine(directoryPath, $"Img{count}.png");
                                            count++;

                                            using (imgData = data.AsPNG())
                                            {
                                                //NSError err = null;
                                                //bool isSaved = imgData.Save(pngFilename, false, out err);
                                                bool isFileCreated = fileManager.CreateFile(pngFilename, imgData, (NSDictionary)null);
                                            }
                                        }
                                        //using (imgData = (Data).AsPNG())
                                        //{
                                        //    byte[] myByteArray = new byte[imgData.Length];
                                        //    Marshal.Copy(imgData.Bytes, myByteArray, 0, Convert.ToInt32(imgData.Length));

                                        //    var fileIOS = new FileIOS();
                                        //    fileIOS.WriteAllBytes("Teste123.png", myByteArray);
                                        //}
                                    }

                                });

                                //var alert2 = UIAlertController.Create("Funfou", tempPath, UIAlertControllerStyle.Alert);
                                //PresentViewController(alert2, true, () =>
                                //{
                                //    DispatchQueue.MainQueue.DispatchAfter(new DispatchTime(DispatchTime.Now, 5000000000), () =>
                                //    {
                                //        // Inform the host that we're done, so it un-blocks its UI. Note: Alternatively you could call super's -didSelectPost, which will similarly complete the extension context.
                                //    });
                                //});

                                UIApplication.SharedApplication.OpenUrl(new NSUrl("com.shareapp.test://"));
                            }
                            catch (Exception)
                            {
                            }
                        }
                    }
                }

                ExtensionContext.CompleteRequest(ExtensionContext.InputItems, null);
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
    public class FileIOS
    {
        public string GetFilePath(string filename)
        {
            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            return Path.Combine(folder, filename);
        }

        public bool Exists(string filename)
        {
            string filepath = GetFilePath(filename);
            return File.Exists(filepath);
        }

        public void WriteAllBytes(string fileName, byte[] conteudoArquivo)
        {
            string filePath = string.Empty;

            try
            {
                filePath = GetFilePath(fileName);
                File.WriteAllBytes(filePath, conteudoArquivo);
            }
            catch (Exception exception)
            {
                throw new Exception("Ocorreu um erro ao salvar o arquivo " + filePath, exception);
            }

        }
    }
}
