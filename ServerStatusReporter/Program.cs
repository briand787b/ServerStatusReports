using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace ServerStatusReporter
{
    using System.Runtime.InteropServices;

    internal class Program
    {
        private static bool isVerbose;

        private static string _mailTo;

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Error:  No switch was given to program.  Please see \"--help\" \n");
                return;
            }

            switch (args[0])
            {
                case "-H":
                case "--help":
                    // Display a list of options that help the user use the program.
                    Console.Write("\nHelp page for FreeDiskSpace Program\n\nOptions:\n\t-H\t\t\tHelp page\n\t--help\t\t\tHelp page\n\t-r {recipient}\t\tSend report via email\n\t-v\t\t\tRun program in verbose mode.\n\n");
                    break;

                case "-v":
                    // Display the drive space to the console, but do not email anything
                    Console.Write("Running in verbose mode!\n\n");
                    Console.WriteLine("Warning!  No email will be sent with current command line arguments\n");
                    isVerbose = true;
                    _mailTo = String.Empty;
                    ReturnDiskSpace();
                    Console.WriteLine("\nProgram completed!\n\n");
                    break;

                case "-r":
                    // Report the findings to the supplied email address (args[1])
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Error!  No send-to email address was specified.\n");
                    }

                    _mailTo = args[1];
                    ReturnDiskSpace();
                    Console.WriteLine("\n" + _mailTo);
                    // Use email client.
                    break;

                case "-vr":
                case "-rv":
                    // Run the program in verbose mode (output all activity to console).
                    if (args.Length < 2)
                    {
                        Console.WriteLine("Error!  No send-to email address was specified.\n");
                        return;
                    }

                    Console.Write("Running in verbose mode!\n\n");
                    isVerbose = true;
                    _mailTo = args[1];
                    ReturnDiskSpace();
                    Console.WriteLine("\nThis report has been sent to " + args[1]);
                    break;

                default:
                    Console.Write("\"" + args[0] + "\"" + " argument does not exist");
                    return;
            }
        }

        private static void ReturnDiskSpace()
        {
            if (isVerbose)
            {
                Console.WriteLine("Checking disk space...\n");
            }

            const string ReportHeader = "Server\t\tDrive\tFree(GB)\t\tTotal(GB)\t\tUpdates";
            string reportBody = String.Empty;
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                reportBody += "\n" + Environment.MachineName + ":\t" + drive.Name + "\t" + (drive.AvailableFreeSpace / 1073741824.0) + "\t" + (drive.TotalSize / 1073741824.0) + "\t0";
            }

            if (isVerbose)
            {
                Console.WriteLine(ReportHeader + reportBody);
            }

            if (!string.IsNullOrWhiteSpace(_mailTo))
            {
                EmailToTarget(ReportHeader + reportBody);
            }
        }

        private static void EmailToTarget(string body)
        {
            // Command line argument must the the SMTP host.
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("briand787b@gmail.com");
            msg.To.Add(_mailTo);
            msg.Subject = "test";
            msg.Body = body;

            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("briand787b@gmail.com", Credentials.Password());
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(msg);
            }
        }
    }
}