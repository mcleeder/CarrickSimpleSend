using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CarrickSimpleSend
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Carrick Simple Send.\nThis program will send all the files in the current directory to the given destination one file at a time.");
            Console.WriteLine("Currently only Gmail senders are supported.\n");

            EmailPackage Package = new EmailPackage();

            Package.GetDestination();

            Package.GetSenderAddress();

            Package.GetSenderPassword();

            Package.FinalConfirm();

            Package.SendAllMail();

            Console.ReadKey();
        }


    }
}
