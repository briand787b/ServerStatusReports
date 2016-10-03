using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerStatusReporter
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            switch (args[0])
            {
                case "-H":
                case "--help":
                    // Display a list of options that help the user use the program.
                    Console.Write("\nHelp page for FreeDiskSpace Program\n\nOptions:\n\t-H\t\tHelp page\n\t--help\t\tHelp page\n\t-r\t\tExecute program to report disk spaces\n\n");
                    break;

                case "-r":
                    // Report the findings to the supplied email address (args[1]).
                    ReturnDiskSpace();
                    break;

                default:
                    Console.Write("\"" + args[0] + "\"" + " command does not exist");
                    return;
            }
        }

        private static void ReturnDiskSpace()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                Console.WriteLine("{0} / {1}", (drive.AvailableFreeSpace / 1073741824.0), (drive.TotalSize / 1073741824.0));
            }
        }
    }
}