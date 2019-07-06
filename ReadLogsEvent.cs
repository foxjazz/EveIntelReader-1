using System;
using System.Diagnostics;
namespace IntelReader
{

    public class FileChanged : EventArgs
    {
        public string FoundFile { get; }
        public long Lines { get; }
        public FileChanged(string fileName, long lines)
        {
            FoundFile = fileName;
            Lines = lines;

        }
    }
}
