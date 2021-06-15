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
            UpdateDataFiles(directory.GetFiles());
            dir = new DirectoryInfo(startPath);
            threadMonitor = true;
            ThreadPool.QueueUserWorkItem(StartMonitor);
        }

        public void UpdateLogPool()
        {
            var directory = new DirectoryInfo(startPath);
            UpdateDataFiles(directory.GetFiles());
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

            DateTime dtkeep = DateTime.Now;
            while(threadMonitor){
                Thread.Sleep(500);
                foreach(var di in dir.GetFiles()){
                    if(monitorFiles.Exists(a => a.fullName == di.FullName) ){
                        var lfi = monitorFiles.FirstOrDefault(a => a.fullName == di.FullName);
                        lfi.Created = di.CreationTimeUtc;
                        long currentLength;
                        using ( var fs = new FileStream(lfi.fullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            long flCandidate = Utils.GetFileLength(lfi.fullName);
                            currentLength = flCandidate > fs.Length ? flCandidate : fs.Length;
                        }
                        if(lfi.firstCheck && lfi.length != currentLength){
                            // TODO activate event
                            Debug.WriteLine($"file changed: {lfi.prefix}");
                            var fc = new FileChanged(lfi.fullName, lfi.length, lfi.prefix);
                            Debug.WriteLine($"Reading: {lfi.name} lines: {lfi.length}  newlines{currentLength - lfi.length}  fullName: {lfi.fullName}");
                            lfi.length = currentLength;
                            
                            //await.Task.Run(Read.ReadLog(fc));
                            // Read.ReadLog(fc);
                            OnFileChanged(fc);
                            
                        }
                        lfi.length = currentLength;
                        lfi.firstCheck = true;
                        lfi.lastWrite = di.LastWriteTimeUtc;
                        lfi.prefix =  GetFilePrefix(di.Name);

                        if(lfi.lastWrite < DateTime.UtcNow.AddDays(-1)){
                            monitorFiles.Remove(lfi);
                            UpdateLogPool();
                        }
                    }

                    
                }

                if (DateTime.Now.AddMinutes(-5) > dtkeep)
                {
                    dtkeep = DateTime.Now;
                    UpdateLogPool();
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
        private long GetFileDate(string fn)
        {
            int f, dot;
            //dot = fn.LastIndexOf("_");
            var d = fn.Split('_');
            string sf;
            if (d.Length > 0)
                sf = d[1];
            else
            {
                return 0;
            }
            // f = fn.Substring(0,dot).LastIndexOf("_");
            // f++;
            // string rstring = fn.Substring(f, dot - f);
            long day;
            bool pass = long.TryParse(sf + d[2],out day);
            if (!pass)
                return 0;
            return day + 1000000;
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

        
        private void UpdateDataFiles(FileInfo[] datafiles){
            var candidatFiles = new List<LogFileInfo>();
            
            var dateInt = DateTime.Now.GetYMDT();
            dateInt -= 2;
            foreach (var fi in datafiles)
                    {
                    var fileDate = GetFileDate(fi.FullName);
                    if(dateInt > fileDate)
                        continue;
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
            foreach(var m in monitorFiles)
            {
                Debug.WriteLine($"NEW monitorFiles: {m.fullName}");
            }

        }
        public void CheckChangedLogPool()
        {
            foreach(var m in monitorFiles){
                Debug.WriteLine($"monitorFiles: {m.fullName}");
            }

            var directory = new DirectoryInfo(startPath);
            DateTime fromDate = DateTime.UtcNow.AddHours(-24);
            var datafiles = directory.GetFiles();
            
        
          

        }

        private void CheckCandidate(LogFileInfo lfi)
        {
            if(monitorFiles == null)
                monitorFiles = new List<LogFileInfo>();
            var ismon = monitorFiles.FirstOrDefault(a => a.fullName == lfi.fullName);
            if(ismon == null){
                monitorFiles.Add(lfi);
            }
            var needToRemove = monitorFiles.FirstOrDefault(a =>  GetFilePrefix(a.fullName) == GetFilePrefix(lfi.fullName) && a.fullName != lfi.fullName);
            if(needToRemove != null)
                monitorFiles.Remove(needToRemove);
        }
    }
}
