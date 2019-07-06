
using System;
using System.IO;

namespace IntelReader
{
    public static class PlaySound
    {
        
        public static DateTime LastPlayed;

        public static void playAlert(string n)
        {

            if (n == "0")
                return;
            // only play something if a second has passted since laste message
            if(LastPlayed == null)
                LastPlayed = DateTime.Now.AddSeconds(-1);
            if(LastPlayed < DateTime.Now)
                return;
            LastPlayed = DateTime.Now;


            string OSV = System.Environment.OSVersion.ToString();
            string fn = "";
            if (n != "1")
            {
                fn = Directory.GetCurrentDirectory() + $@"\sounds\{n}.wav";
            }
            if (n == "1")
            {
                fn = Directory.GetCurrentDirectory() + $@"\sounds\neuts1jump.wav";
            }
            foreach(string fsys in setup.named){
                if(fsys == n){
                    fn = Directory.GetCurrentDirectory() + $@"\sounds\{fsys}.wav";
                }
            }

            if (File.Exists(fn))
            {
                // TODO  change this for other platforms
                if (OSV.Contains("Windows")){
                    Console.WriteLine($"Playing audio: {fn}");
                    System.Diagnostics.Process.Start(@"powershell", $@"-c (New-Object Media.SoundPlayer '{fn}').PlaySync();");
                }
                else
                {
                    System.Diagnostics.Process.Start(@"audioplay", "." + fn);
                }
            }

            //var waveFileReader = new WaveFileReader(Directory.GetCurrentDirectory() + $@"\sounds\{n}.wav");
            //WaveOutEvent player = new WaveOutEvent();
            //player.Init(waveFileReader);
            //player.PlaySync();
          
        }
    }
}
