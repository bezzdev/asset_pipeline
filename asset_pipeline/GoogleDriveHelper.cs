using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace asset_pipeline
{
    public static class GoogleDriveHelper
    {
        public static void RunGoogleDrive(string args)
        {
            Console.WriteLine("Starting google drive... ");
            ProcessStartInfo googleDrive = new ProcessStartInfo("C:\\Program Files\\Google\\Drive\\googledrivesync.exe");
            googleDrive.Arguments = args;

            Process runningGDrive = Process.Start(googleDrive);
            Thread.Sleep(1000 * 10);

            Console.WriteLine("Press any key to close google drive");
            Console.ReadKey();
            runningGDrive.Close();
        }
    }
}
