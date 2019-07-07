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
            string useSetFolder = config.baseEveFolder.Trim();
            if (!useSetFolder.Contains("Chatlogs"))
                useSetFolder = Path.Combine(useSetFolder, @"Chatlogs");
            string root = Directory.GetParent(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)).FullName;
            if ( Environment.OSVersion.Version.Major >= 6 ) {
                root = Directory.GetParent(root).ToString();
            }
            string eveRoot = root + @"\Documents\EVE\logs\Chatlogs";
            if(Directory.Exists(eveRoot))
                monitoringFiles.startPath = eveRoot;
            else
            {
                monitoringFiles.startPath = useSetFolder;
            }
            Console.Write($"ChatLogs Path: {monitoringFiles.startPath}");
            monitoringFiles.PopulateLogPool();
            //var files = directory.GetFiles()
            //  .Where(file => file.LastWriteTime >= from_date && file.LastWriteTime <= to_date);
            bool doContinue = true;
            int cnter = 0;
            while(doContinue){
                var key = Console.Read();
                if(key.ToString() == "q"){
                    doContinue = false;
                }
                monitoringFiles.Off();
                Thread.Sleep(100);
                cnter++;
                if (cnter > 50)
                {
                    monitoringFiles.CheckChangedLogPool();
                    cnter = 0;
                }
            }
            //Initial read, then next read.
          

        }

        static void EventFileChanged(object sender, FileChanged args)
        {
                Read.ReadLog(args);
        }
        
       
    }
}
