using IntelReader.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;



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
                if (Utils.HasInFileName(fi.Name.ToLower(), config.logFileNames))
                {
                    var lfi = monitorFiles.FirstOrDefault(a => a.prefix == GetFilePrefix( fi.FullName));
                    if(lfi != null){
                        if(lfi.prefix == GetFilePrefix(fi.FullName) && ( lfi.fileDate < GetFileDate(fi.FullName) || lfi.suffix < GetSuffix(fi.FullName))){
                            monitorFiles.Remove(lfi);
                            monitorFiles.Add( new LogFileInfo(){fullName = fi.FullName, prefix = GetFilePrefix(fi.FullName),
                                lastWrite = fi.LastWriteTimeUtc, suffix = GetSuffix(fi.FullName), Created = fi.CreationTimeUtc, fileDate = GetFileDate(fi.FullName)});
                        }
                    }
                    else{
                            monitorFiles.Add( new LogFileInfo(){fullName = fi.FullName, prefix = GetFilePrefix(fi.FullName),
                                lastWrite = fi.LastWriteTimeUtc, suffix = GetSuffix(fi.FullName), Created = fi.CreationTimeUtc, fileDate = GetFileDate(fi.FullName)});                        
                    }
                }
              
            }
            
            dir = new DirectoryInfo(startPath);
            threadMonitor = true;
            ThreadPool.QueueUserWorkItem(StartMonitor);
        }
        public void Off(){
            threadMonitor = false;
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
                Console.WriteLine($"Monitoring: {fi.fullName}");
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
                            var fc = new FileChanged(lfi.fullName, lfi.lines, lfi.prefix);
                            Debug.WriteLine($"Reading: {lfi.name} lines: {lfi.lines}  newlines{currentLineCount - lfi.lines}  fullName: {lfi.fullName}");
                            lfi.lines = currentLineCount;
                            
                            //await.Task.Run(Read.ReadLog(fc));
                            // Read.ReadLog(fc);
                            OnFileChanged(fc);
                            
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
        private Int32 GetFileDate(string fn)
        {
            int f, dot;
            dot = fn.LastIndexOf("_");
            f = fn.Substring(0,dot).LastIndexOf("_");
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
            string rtn = fq.Substring(first, len);
            return rtn;
        }

        public void CheckChangedLogPool()
        {
            foreach(var m in monitorFiles){
                Debug.WriteLine($"monitorFiles: {m.fullName}");
            }
            var candidatFiles = new List<LogFileInfo>();
            var directory = new DirectoryInfo(startPath);
            DateTime fromDate = DateTime.UtcNow.AddHours(-24);
            var datafiles = directory.GetFiles();
            
            foreach (var fi in datafiles)
            {
               if (Utils.HasInFileName(fi.Name.ToLower(), config.logFileNames))
                {
                    var lfi = candidatFiles.FirstOrDefault(a => a.prefix == GetFilePrefix( fi.FullName));
                    if(lfi != null){
                        if(lfi.prefix == GetFilePrefix(fi.FullName) && ( lfi.fileDate < GetFileDate(fi.FullName) || lfi.suffix < GetSuffix(fi.FullName))){
                            candidatFiles.Remove(lfi);
                            candidatFiles.Add( new LogFileInfo(){fullName = fi.FullName, prefix = GetFilePrefix(fi.FullName),
                                lastWrite = fi.LastWriteTimeUtc, suffix = GetSuffix(fi.FullName), Created = fi.CreationTimeUtc, fileDate = GetFileDate(fi.FullName)});
                        }
                    }
                    else{
                            candidatFiles.Add( new LogFileInfo(){fullName = fi.FullName, prefix = GetFilePrefix(fi.FullName),
                                lastWrite = fi.LastWriteTimeUtc, suffix = GetSuffix(fi.FullName), Created = fi.CreationTimeUtc, fileDate = GetFileDate(fi.FullName)});                        
                    }
                }
              
            }

            foreach (var can in candidatFiles)
            {
                CheckCandidate(can);
            }
            foreach(var m in monitorFiles){
                Debug.WriteLine($"NEW monitorFiles: {m.fullName}");
            }

        }

        private void CheckCandidate(LogFileInfo lfi)
        {
            LogFileInfo dmon = null;
            foreach (var mon in monitorFiles)
            {
                if (lfi.fullName != mon.fullName)
                {
                    dmon = mon;
                    monitorFiles.Add(lfi);
                }

                break;
            }
            if(dmon != null)
                monitorFiles.Remove(dmon);
        }
    }
}
