using IntelReader.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace IntelReader
{
    public static class Read
    {
        public static void ReadLog(FileChanged fc)
        {
            var fn = fc.FoundFile;
                var fs = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using (StreamReader sr = new StreamReader(fs))
                {
                    Skip(sr, Convert.ToInt32(fc.Lines));
                    while (sr.Peek() >= 0)
                    {
                        string line = sr.ReadLine().CleanString();
                        var data = line.Split(' ');
                            Console.WriteLine($"Log: {fn} comment: {line}");
                            bool clr = false;
                            foreach (string d in data)
                            {
                                if(d.ToLower() == "clr" || d.ToLower() == "clear" || d.ToLower() == "status")
                                {
                                    clr = true;
                                }
                            }
                            if (!clr)
                            {
                                IEnumerable<string> data6 = data.Where(a => a.Length == 6);
                                foreach (string d in data6)
                                {
                                        Check.checkSystems(d, fn);
                                }
                            }

                    }
                    
                }
                fs.Dispose();
            }
            public static void Skip(StreamReader rd, int skip)
            {
                for(int i = 0; i < skip; i++)
                {
                    var data  = rd.ReadLine();
                }
            }
        }
    }

