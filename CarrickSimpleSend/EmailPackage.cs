using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;

namespace CarrickSimpleSend
{
    public class EmailPackage
    {
        public string SenderEmail { get; set; }
        public string SenderPassword { get; set; }
        public string DestinationEmail { get; set; }
        public string FilesPath { get; set; }
        public List<string> Files { get; set; }

        public EmailPackage()
        {
            FilesPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Files = Directory.EnumerateFiles(FilesPath, "*", SearchOption.TopDirectoryOnly)
                    .Select(Path.GetFileName)
                    .ToList();
            Files.RemoveAll(EndsWithExe);
        }

        internal void FinalConfirm()
        {
            Console.Clear();
            Console.WriteLine("******** CONFIRM BEFORE SENDING ********\n");
            Console.WriteLine($"Sending FROM: {SenderEmail}");
            Console.WriteLine($"Sending TO: {DestinationEmail}\n");
            Console.WriteLine($"The following files will be sent:");
            ListFilesToSend();
            Console.WriteLine("\nPress any key to start sending email...\n");
            Console.ReadKey();
        }

        internal void SendAllMail()
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(SenderEmail, SenderPassword),
                EnableSsl = true,
            };

            for (int i = 0; i < Files.Count; i++)
            {
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(SenderEmail),
                    Subject = "",
                    Body = "<h1>Hi. I tried to send you a file, but something went wrong on this end. Sit tight while I fix it.</h1>",
                    IsBodyHtml = true,
                };

                mailMessage.To.Add(DestinationEmail);
                mailMessage.Subject = $"{Files[i]}, {i+1} of {Files.Count}";
                mailMessage.Body = $"<p>Attached is {Files[i]}.</p>" +
                                    $"<p>This is file {i+1} of {Files.Count}.</p>";

                var attachment = new Attachment($"{Files[i]}");
                mailMessage.Attachments.Add(attachment);
                try
                {
                    smtpClient.Send(mailMessage);
                }
                catch (SmtpException e)
                {
                    Console.WriteLine($"{e.Message}");
                    break;
                }
                Console.WriteLine($"Sent {Files[i]}");
            }
            Console.WriteLine("\nDone!\nPress any key to exit...");
        }

        internal void ListFilesToSend()
        {
            foreach (string file in Files)
            {
                Console.WriteLine(file);
            }
        }

        private bool EndsWithExe(string s)
        {
            return s.ToLower().EndsWith(".exe");
        }

        internal void GetSenderPassword()
        {
            SenderPassword = GetInput("Enter password", false);
        }

        internal void GetSenderAddress()
        {
            SenderEmail = GetInput("Enter sender email address");
        }

        private string GetInput(string prompt, bool needConfirm = true)
        {
            bool confirmed = false;
            string userInput = "";

            while (!confirmed)
            {
                Console.Write($"\n{prompt}: ");
                userInput = Console.ReadLine();

                if (needConfirm)
                {
                    Console.WriteLine($"You entered {userInput}\nIs this correct? (y/n)");
                    string confirmation = Console.ReadLine();
                    if (confirmation.ToLower() == "y")
                    {
                        confirmed = true;
                    }
                }
                else
                {
                    confirmed = true;
                }

            }
            return userInput;
        }

        internal void GetDestination()
        {
            DestinationEmail = GetInput("Enter destination email address");
        }
    }
}
