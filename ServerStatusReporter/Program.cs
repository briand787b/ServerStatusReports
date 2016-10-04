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
        private static bool isVerbose;

        private static string mailTo;

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                ReturnDiskSpace();
                return;
            }

            switch (args[0])
            {
                case "-H":
                case "--help":
                    // Display a list of options that help the user use the program.
                    Console.Write("\nHelp page for FreeDiskSpace Program\n\nOptions:\n\t-H\t\t\tHelp page\n\t--help\t\t\tHelp page\n\t-r {recipient}\t\tSend report via email\n\t-v\t\t\tRun program in verbose mode.\n\n");
                    break;

                case "-r":
                    // Report the findings to the supplied email address (args[1]).
                    ReturnDiskSpace();
                    // Use email client.
                    break;

                case "-vr":
                case "-rv":
                    // Run the program in verbose mode (output all activity to console).
                    isVerbose = true;
                    mailTo = args[1];
                    ReturnDiskSpace();
                    Console.WriteLine("\n" + args[1]);
                    // Use email client.
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