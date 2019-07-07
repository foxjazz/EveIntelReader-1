using System;
using System.Diagnostics;
namespace IntelReader
{

    public class FileChanged : EventArgs
    {
        public string FoundFile { get; }
        public string Prefix {get;}
        public long Lines { get; }
        public FileChanged(string fileName, long lines, string prefix)
        {
            FoundFile = fileName;
            Lines = lines;
            Prefix = prefix;

        }
    }
}
