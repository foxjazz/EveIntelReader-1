using IntelReader.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using RatEaseW;

namespace IntelReader
{
    public static class Check
    {
        public static DiscordSend ds;
        public static void checkSystems(string d, string fn, string prefix)
        {
            string fn1 = fn;
            bool hasNamed = false;
            if(fn1.Contains("intel"))
            {
                fn1 = "";
            }

            var jd = setup.jumpData;
            string log = d.ToUpper();
            if (ds == null)
                ds = new DiscordSend();
            foreach (var jn in jd.jumpNumber)
            {
                if(jn.system == log)
                {
                    ds.SendMessage(d);
                    hasNamed = false;
                    foreach(string fsys in setup.named){
                        if(fsys == jn.system){
                            PlaySound.playAlert(fsys);
                            hasNamed = true;
                        }
                    }
                    if(!hasNamed){
                        PlaySound.playAlert(jn.jumps.Trim());
                    }
                    Debug.WriteLine($"CL: {prefix} : {jn.system} ");
                }
            }
          
            // special alert sounds if neut appears in special systems
            foreach (var jn in jd.special)
            {
                if (jn.system == log)
                {
                    PlaySound.playAlert(jn.jumps);
                    Console.WriteLine($"CL: {prefix} : {jn.system} which is {jn.jumps} away.");
                    ds.SendMessage(d);
                }
            }
        }
    }
}
