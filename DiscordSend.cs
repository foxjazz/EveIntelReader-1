using System;

using System.Net;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections.Specialized;
using System.IO;
using Renci.SshNet;
using Renci.SshNet.Sftp;
using System.Diagnostics;
using IntelReader;

namespace RatEaseW
{
    public class DiscordSend
    {
        
        WebClient wc;
        private string _hook;
        public DiscordSend()
        {
            _hook = setup.discordHook;
            wc = new WebClient();
            
        }

        public void SendMessage(string system)
        {
            try
            {
                var nvc = new NameValueCollection();
                nvc.Add("username", "Intel");
                nvc.Add("content", $"Reported by EveIntel {system}");
                wc.UploadValues(_hook, nvc);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
            }
        }
       

    }
    
}

