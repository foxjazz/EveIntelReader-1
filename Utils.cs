﻿
using IntelReader.models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using Renci.SshNet;


namespace IntelReader
{
    public static class Utils
    {
        public static bool HasInFileName(string fn, string[] names){
            foreach( string n in names){
                if(fn.Contains(n.ToLower()))
                return true;;
            }
            return false;
        }
        public static string CleanString(this string input)
        {
            var replacements = new[] { "@", "*", "\"", "&", "^", "%", "$", "#", "!", "=", "(", ")", "[", "]", "{", "}", "?", ".", ",", "'", "/" }; 
            var output = new StringBuilder(input); 
            foreach (var r in replacements) output.Replace(r, string.Empty);
            return output.ToString();
        }
        public static string GetFilePrefix(string fq)
        {
            int first, len;
            first = fq.LastIndexOf("\\");
            first++;
            len = fq.Substring(first).IndexOf("_");
            return fq.Substring(first, len);
        }

        public static IEnumerable<LogFileInfo> GetActiveFiles(List<LogFileInfo> lfn)
        {
            var rlfn = new List<LogFileInfo>();
            var data = new List<string>();
            foreach(var d in lfn)
            {
                if(!data.Contains(d.prefix))
                    data.Add(d.prefix);
            }

            foreach(var d in data)
            {
                LogFileInfo latest = null;
                foreach(var lf in lfn)
                {
                    if(d == lf.prefix)
                    {
                        if(latest == null)
                            latest = lf;
                        if (latest.lastWrite < lf.lastWrite)
                            latest = lf;
                    }
                }
                rlfn.Add(latest);
            }
            return rlfn;

        }
        public static bool CheckExists(List<LogFileInfo> lfn, string name)
        {
            
            
            foreach (var d in lfn)
            {
                if (d.name == name)
                    return true;
            }
            return false;

        }
        /* public static long ReadLineCount(this StreamReader stream)
        {
            long cnt = 0;
            while (stream.ReadLine() != null) cnt++;
            return cnt;
        } */
        /*public static Int32 GetYMD(this DateTime dt){
            string year = DateTime.Now.Year.ToString();
            string month = DateTime.Now.Month.ToString();
            if(month.Length == 1)
                month = "0" + month;
            string day = DateTime.Now.Day.ToString();
            if(day.Length == 1)
                day = "0" + day;
            return Convert.ToInt32(year + month + day);
        }
        */

        public static long GetYMDT(this DateTime dt)
        {
            string d = DateTime.Now.ToString("yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            return  Convert.ToInt64(d);
        }
        public static long GetFileLength(string fn)
        {
            FileInfo fi = new FileInfo(fn);
            return fi.Length;
            

        }
    }
}
