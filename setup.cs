using IntelReader.models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace IntelReader
{
    public static class setup
    {
        //Latest Jun 12
        public static JumpData jumpData;
        public static string[] named;
        public static string discordHook;
        
        public static void ImportData(string[] args)

        {
            populateNotificationData();
            string basefolder = Environment.CurrentDirectory;
            string configF = "";

            configF = Path.Combine(basefolder, "config\\config.txt");
            if (!File.Exists(configF))
            {
                Console.WriteLine("config file in config folder must be present.  Please check the Readme");
            }

            Console.WriteLine("configLocation:" + configF);
            using (StreamReader sr = new StreamReader(configF))
            {
                for (int i = 0; i < 3; i++)
                {
                    string[] data = sr.ReadLine().Replace(" ","").Split(',');
                    setData(data);
                }

            }

            jumpData = new JumpData();
            using (StreamReader sr = new StreamReader(Path.Combine(basefolder,   "db/data.csv")))
            {
                if (jumpData == null)
                {
                    jumpData = new JumpData();

                }
                if (jumpData.jumpNumber == null)
                {
                    jumpData.jumpNumber = new List<JumpNumber>();
                }

                string[] data;
                bool onTarget = false;
                data = sr.ReadLine().Replace(" ","").Split(',');
                while (data[0] != "target" || data[1].Replace("\"", "") != config.target)
                {
                    data = sr.ReadLine().Replace(" ","").Split(',');
                }
                if (data[0] == "target" && data[1].Replace("\"", "") == config.target)
                {
                   
                    while (!onTarget)
                    {
                        data = sr.ReadLine().Replace(" ","").Split(',');
                        if (data != null && data.Length > 0 && data[0] == "target" || data == null)
                            break;

                        var jn = new JumpNumber();
                        jn.system = data[0].Replace("\"", "").Trim();
                        if (data.Length > 1)
                        {
                            jn.jumps = data[1].Trim();
                            jumpData.jumpNumber.Add(jn);
                        }
                    }
                }
                foreach (var jn in jumpData.jumpNumber)
                {
                    Console.WriteLine($"{jn.system} : {jn.jumps}");
                }

            }

            named = new[] {""};
            if (File.Exists(Path.Combine(basefolder, "db\\named.csv")))
            {
                using (StreamReader sr = new StreamReader("db/named.csv"))
                {
                    named = sr.ReadLine().Split(',');
                }
            }

            if (jumpData.special == null)
            {
                jumpData.special = new List<JumpNumber>();
            }
            if (File.Exists(Path.Combine(basefolder, "db\\special.csv")))
            {
                using (StreamReader sr = new StreamReader(Path.Combine(basefolder, "db\\special.csv")))
                {

                    string[] data;
                    bool onTarget = false;
                    while (!onTarget)
                    {
                        data = sr.ReadLine()?.Split(',');
                        if (data != null && data.Length > 0 && data[0] == "target" || data == null)
                            break;

                        var jn = new JumpNumber();
                        jn.system = data[0].Replace("\"", "");
                        if (data.Length > 1)
                        {
                            jn.jumps = data[1];
                            jumpData.special.Add(jn);
                        }
                    }

                    foreach (var jn in jumpData.special)
                    {
                        Console.WriteLine($"special: {jn.system} : {jn.jumps}");

                    }

                }
            }

            Console.WriteLine($"target: {config.target}  {DateTime.Now}");

        }
        public static void setData(string[] data)
        {
            if (data[0] == "chatFolder")
                config.baseEveFolder = data[1].Replace("\"", ""); ;
            if (data[0] == "targetSystem")
                config.target = data[1].Replace("\"", "");
            if (data[0] == "LogFiles")
                config.logFileNames = data;

        }
        private static void populateNotificationData()
        {
            var d = Directory.GetCurrentDirectory();
            if (File.Exists($@"{d}\config\discord.txt"))
            {
                StreamReader sr = new StreamReader(@"config\Discord.txt");
                while (true)
                {
                    var read = sr.ReadLine();
                    if (read == null)
                        break;
                    string[] strarray = read.Split(',');
                    switch (strarray[0])
                    {
                        case "webhook":
                            discordHook = strarray[1];
                            break;
                    }
                }
                sr.Dispose();

            }
           


           
        }

    }
}
