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
    public class FileMonitor{
        private List<LogFileInfo> monitorFiles;
        public bool threadMonitor { get; set; }
        public static event EventHandler<FileChanged> fileChanged;
        public List<string> looksFor;
        public string startPath;
        DirectoryInfo dir;
        public FileMonitor(){
            monitorFiles = new List<LogFileInfo>();
            looksFor = new List<string>();
            threadMonitor = false;
        }
        public void PopulateLogPool(){
             var directory = new DirectoryInfo(startPath);
                DateTime fromDate = DateTime.UtcNow.AddHours(-24);
                var datafiles = directory.GetFiles();
                var logFileInfo = new List<LogFileInfo>();
                foreach (var fi in datafiles)
                {
                    if (Utils.HasInFileName(fi.Name.ToLower(), config.logFileNames) && fi.LastWriteTimeUtc >= fromDate)
                    {
                       
                        
                        AddFile(fi.FullName, fi.LastWriteTimeUtc, GetSuffix(fi.FullName));
                    }
                }

        }
        public void Off(){
            threadMonitor = false;
        }
        public void AddFile(string fn, DateTime lastWrite, Int32 suffix){
            if(dir == null){
                dir = new DirectoryInfo(startPath);
                if(!threadMonitor){
                    threadMonitor = true;
                    ThreadPool.QueueUserWorkItem(StartMonitor);
                }
            }
            var exists = monitorFiles.Exists(a => a.fullName == fn);
            if(!exists){
/*
                if (monitorFiles.Exists(a => a.lastWrite < lastWrite && a.name == GetFilePrefix(fn) && a.suffix < suffix))
                {
                    var j = monitorFiles.FirstOrDefault(a => a.lastWrite < lastWrite && a.name == GetFilePrefix(fn) && a.suffix < suffix);
                    monitorFiles.Remove(j);
                }
*/
                if (monitorFiles.Exists(a => a.name == GetFilePrefix(fn) && a.suffix < suffix))
                {
                    var j = monitorFiles.FirstOrDefault(a =>   a.name == GetFilePrefix(fn) && a.suffix < suffix);
                    monitorFiles.Remove(j);
                }
                monitorFiles.Add(new LogFileInfo(){lastWrite = lastWrite, fullName = fn, name=GetFilePrefix(fn), firstCheck = false, lines=0, suffix = suffix});
            }
        }
        
        protected virtual void OnFileChanged(FileChanged e)
        {
            EventHandler<FileChanged> handler = fileChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private void StartMonitor(Object stateInfo){
            foreach (var fi in monitorFiles)
            {
                Console.WriteLine($"Monitoring: {fi.name}");
            }

            while(threadMonitor){
                Thread.Sleep(500);
                foreach(var di in dir.GetFiles()){
                    if(monitorFiles.Exists(a => a.fullName == di.FullName) ){
                        var lfi = monitorFiles.FirstOrDefault(a => a.fullName == di.FullName);
                        lfi.Created = di.CreationTimeUtc;
                        long currentLineCount;
                        using ( var fs = new FileStream(lfi.fullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)){
                            using (StreamReader sr = new StreamReader(fs))
                            {
                                currentLineCount = sr.ReadLineCount();
                            }

                        }
                        if(lfi.firstCheck && lfi.lines != currentLineCount){
                            // TODO activate event
                            Debug.WriteLine($"file changed: {lfi.name}");
                            var fc = new FileChanged(lfi.fullName, lfi.lines);
                            Debug.WriteLine($"Reading: {lfi.name} lines: {lfi.lines}  newlines{currentLineCount - lfi.lines}  fullName: {lfi.fullName}");
                            lfi.lines = currentLineCount;
                            Read.ReadLog(fc);
                            
                        }
                        lfi.lines = currentLineCount;
                        lfi.firstCheck = true;
                        lfi.lastWrite = di.LastWriteTimeUtc;
                        lfi.prefix =  GetFilePrefix(di.Name);

                        if(lfi.lastWrite < DateTime.UtcNow.AddDays(-1)){
                            monitorFiles.Remove(lfi);
                        }
                    }
                    
                }
            }
        }
        private string GetPath(string fn)    {
            int i = fn.LastIndexOf("\\");
            return fn.Substring(0,i);
        }

        private Int32 GetSuffix(string fn)
        {
            int f, dot;
            dot = fn.LastIndexOf(".");
            f = fn.LastIndexOf("_");
            f++;
            string rstring = fn.Substring(f, dot - f);
            return Convert.ToInt32(rstring);
        }
        private string GetFilePrefix(string fq)
        {
            int first, len;
            first = fq.LastIndexOf("\\");
            first++;
            len = fq.Substring(first).IndexOf("_");
            return fq.Substring(first, len);
        }
    }
}
