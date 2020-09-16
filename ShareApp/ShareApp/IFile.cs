using System;
using System.Collections.Generic;
using System.Text;

namespace ShareApp
{
    public interface IFile
    {
        string GetFilePath(string filename);
        bool Exists(string filename);
        void WriteAllBytes(string fileName, byte[] conteudoArquivo);
    }
}
