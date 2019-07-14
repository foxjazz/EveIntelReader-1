using System;
using System.Diagnostics;
namespace IntelReader
{

    public class FileChanged : EventArgs
    {
        public string FoundFile { get; }
        public string Prefix {get;}
        public long Length { get; }
        public FileChanged(string fileName, long length, string prefix)
        {
            FoundFile = fileName;
            Length = length;
            Prefix = prefix;

        }
    }
}
