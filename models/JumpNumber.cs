using System;
using System.Collections.Generic;

namespace IntelReader.models
{
    public class JumpData
    {
        public string targetSystem { get; set; }
        public List<JumpNumber>  jumpNumber { get; set; }
        public List<JumpNumber> special { get; set; }
    }
    
    public class JumpNumber
    {
        public string system;
        public string jumps;
    }
  
    public class LogFileInfo
    {
        public long length;
        public long fileDate;
//        public long lines;
        public bool firstCheck;
        public string fullName;
        public string name;
        public string prefix;
        public Int32 suffix;
        public DateTime Created;
        public DateTime lastWrite;
    }
    public class loglist
    {
        public List<string> ChatFiles { get; set; }

    }
   
}
