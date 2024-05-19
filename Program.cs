using Amazon;
using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;
using MimeKit;
using System.Configuration;

namespace CandidateTest
{
    class Program
    {

        /// <summary>
        /// Do not edit this function
        /// </summary>
        static void Main(string[] args)
        {
            try
            {
            bool useDefaultSetting = false;
            string accessKeyId = string.Empty;
            string secretAccessKey = string.Empty;
            string response = string.Empty;
            do
            {
                Console.WriteLine("Do you want to use default settings? (Y/N)");
                response = Console.ReadLine();
            } while (!response.Equals("Y", StringComparison.OrdinalIgnoreCase) && !response.Equals("N", StringComparison.OrdinalIgnoreCase));
            useDefaultSetting = response.Equals("Y", StringComparison.OrdinalIgnoreCase);
            do {
                if (useDefaultSetting)
                {
                    accessKeyId = "xxxx";
                    secretAccessKey = "xxxxx";
                }
                else {
                    Console.WriteLine("Please enter your AccessKeyId:");
                    accessKeyId = Console.ReadLine();
                    Console.WriteLine("Please enter your Secret Access Key:");
                    secretAccessKey = Console.ReadLine();
                }
                //Process Emails
                ProcessEmails pe = new ProcessEmails();
                pe.ProcessAWSEmails(accessKeyId, secretAccessKey);
                //Display Dataset 1
                pe.DisplayDataSet1();
                //Display Dataset 2
                pe.DisplayDataSet2();
                Console.ReadLine();
                Console.WriteLine("Process finished!");

                Console.WriteLine("Do you want to re-connect? (Y/N)");
                response = Console.ReadLine();
                if (response.Equals("N", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                else if (response.Equals("Y", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Do you want to use default settings? (Y/N)");
                    response = Console.ReadLine();
                    useDefaultSetting = response.Equals("Y", StringComparison.OrdinalIgnoreCase);
                }
                else
                {
                    Console.WriteLine("Invalid response.");
                    response = "Y";
                }
            } while (response.Equals("Y", StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                // Handle the exception here
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }     
    }
}
