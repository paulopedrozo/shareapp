using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ShareApp.iOS.FileIOS))]
namespace ShareApp.iOS
{
    public class FileIOS : IFile
    {
        public string GetFilePath(string filename)
        {
            //            var folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //            return Path.Combine(folder, filename);
            var fileManager = new NSFileManager();
            var appGroupContainer = fileManager.GetContainerUrl("group.com.shareapp.ios");
            var appGroupContainerPath = appGroupContainer.Path;

            string directoryPath = Path.Combine(appGroupContainerPath, "DocPro");
            return Path.Combine(directoryPath, filename);
        }

        public string[] GetFilePaths()
        {
            var fileManager = new NSFileManager();
            var appGroupContainer = fileManager.GetContainerUrl("group.com.shareapp.ios");
            var appGroupContainerPath = appGroupContainer.Path;

            string directoryPath = Path.Combine(appGroupContainerPath, "DocPro");
            return Directory.GetFiles(directoryPath,"*.*");
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