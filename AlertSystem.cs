

using System;
using System.Threading;
namespace IntelReader
{
    public class AlertSystem
    {
        public void start(string[] args)
        {
            setup.ImportData(args);
            Console.WriteLine($" Started: {DateTime.Now}");
            ReadLogs.ReadAndCheckNames();
                // Console.WriteLine($"1 second loop {DateTime.Now}");
        }
    }
}
