using System;
using System.IO;

namespace ServerStatusReporter
{
    using System.Runtime.InteropServices;

    internal class Program
    {
        private static bool isVerbose;

        // private static DriveInfo[] drives;

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
                    Console.Write("Verbose mode is temporarily unavailable!");
                    isVerbose = true;
                    ReturnDiskSpace();
                    // Use email client.
                    break;

                default:
                    Console.Write("\"" + args[0] + "\"" + " command does not exist");
                    return;
            }
        }

        private static void ReturnDiskSpace()
        {
            string reportHeader = "Server\t\tDrive\tFree(GB)\t\tTotal(GB)\t\tUpdates";
            string reportBody = String.Empty;
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                reportBody += "\n" + Environment.MachineName + ":\t" + drive.Name + "\t" + (drive.AvailableFreeSpace / 1073741824.0) + "\t" + (drive.TotalSize / 1073741824.0) + "\t0";
            }

            Console.WriteLine(reportHeader + reportBody);
        }
    }
}