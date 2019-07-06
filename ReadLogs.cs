using IntelReader.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace IntelReader
{
    static class ReadLogs
    {
        
        public static int FileNumber = 0;
        public static void ReadAndCheckNames()
        {
            FileMonitor.fileChanged += EventFileChanged;
            var monitoringFiles = new FileMonitor();
            monitoringFiles.looksFor = config.logFileNames.ToList();
            string folder = config.baseEveFolder.Trim();
            if (!folder.Contains("Chatlogs"))
                folder = Path.Combine(folder, @"Chatlogs");
            monitoringFiles.startPath = folder;
            monitoringFiles.PopulateLogPool();
            //var files = directory.GetFiles()
            //  .Where(file => file.LastWriteTime >= from_date && file.LastWriteTime <= to_date);
            bool doContinue = true;
            while(doContinue){
                var key = Console.Read();
                if(key.ToString() == "q"){
                    doContinue = false;
                }
                monitoringFiles.Off();
                Thread.Sleep(100);
            }
            //Initial read, then next read.
          

        }

        static void EventFileChanged(object sender, FileChanged args)
        {
                Read.ReadLog(args);
        }
        
       
    }
}
