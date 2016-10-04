﻿using System;
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
                    // Report the findings to the supplied email address (args[1])
                    _mailTo = args[1];
                    ReturnDiskSpace();
                    Console.WriteLine("\n" + _mailTo);
                    // Use email client.
                    break;

                case "-vr":
                case "-rv":
                    // Run the program in verbose mode (output all activity to console).
                    Console.Write("Verbose mode is temporarily unavailable!\n\n");
                    isVerbose = true;
                    _mailTo = args[1];
                    ReturnDiskSpace();
                    Console.WriteLine("\n" + args[1]);
                    // Use email client.
                    break;

                default:
                    Console.Write("\"" + args[0] + "\"" + " argument does not exist");
                    return;
            }
        }

        private static void ReturnDiskSpace()
        {
            const string ReportHeader = "Server\t\tDrive\tFree(GB)\t\tTotal(GB)\t\tUpdates";
            string reportBody = String.Empty;
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (var drive in drives)
            {
                reportBody += "\n" + Environment.MachineName + ":\t" + drive.Name + "\t" + (drive.AvailableFreeSpace / 1073741824.0) + "\t" + (drive.TotalSize / 1073741824.0) + "\t0";
            }

            Console.WriteLine(ReportHeader + reportBody);

            if (isVerbose)
            {
                EmailToTarget(ReportHeader + reportBody);
            }
        }

        private static void EmailToTarget(string body)
        {
            // Command line argument must the the SMTP host.
            MailMessage msg = new MailMessage();

            msg.From = new MailAddress("briand787b@gmail.com");
            msg.To.Add("briand787b@gmail.com");
            msg.Subject = "test";
            msg.Body = "Test Content";
            //msg.Priority = MailPriority.High;


            using (SmtpClient client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.UseDefaultCredentials = false;
                client.Credentials = new NetworkCredential("briand787b@gmail.com", Credentials.Password);
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;

                client.Send(msg);
            }
        }
    }
}