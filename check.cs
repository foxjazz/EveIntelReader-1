using IntelReader.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace IntelReader
{
    public static class Check
    {
        public static void checkSystems(string d, string fn)
        {
            string fn1 = fn;
            bool hasNamed;
            if(fn1.Contains("intel"))
            {
                fn1 = "";
            }

            var jd = setup.jumpData;
            string log = d.ToUpper();
            foreach(var jn in jd.jumpNumber)
            {
                if(jn.system == log)
                {
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
                    
                    Console.WriteLine($"CL: {fn} : {jn.system} which is {jn.jumps} away.");
                }
            }

            // special alert sounds if neut appears in special systems
            foreach (var jn in jd.special)
            {
                if (jn.system == log)
                {
                    PlaySound.playAlert(jn.jumps);
                    Console.WriteLine($"CL: {fn} : {jn.system} which is {jn.jumps} away.");
                }
            }
        }
    }
}
