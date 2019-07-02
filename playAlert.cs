
//using NAudio.Wave;
using System.IO;

namespace IntelReader
{
    public static class PlaySound
    {
        

        public static void playAlert(string n)
        {

            if (n == "0")
                return;
            string OSV = System.Environment.OSVersion.ToString();
            string fn;
            if (n != "1")
            {
                fn = Directory.GetCurrentDirectory() + $@"\sounds\{n}.wav";
            }
            else
            {
                fn = Directory.GetCurrentDirectory() + $@"\sounds\neuts{n}jump.mp3";
            }
            if (File.Exists(fn))
            {
                // TODO  change this for other platforms
                if (OSV.Contains("Windows")){
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
