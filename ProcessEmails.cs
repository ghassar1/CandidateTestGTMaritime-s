using System;
using Amazon;
using Amazon.S3;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.S3.Model;
using MimeKit;

namespace CandidateTest
{
    internal class ProcessEmails
    {
        Dictionary<string, MimeMessage> processedData;
        public ProcessEmails()
        {
            processedData = new Dictionary<string, MimeMessage>();
        }
        public Dictionary<string, MimeMessage> ProcessAWSEmails(string accessKeyId, string secretAccessKey)
        {
            using (var s3Client = new AmazonS3Client(accessKeyId, secretAccessKey, RegionEndpoint.EUWest1))
            {
                var objectList = s3Client.ListObjects("junior-developer-candidate-test-gtmaritime");
                foreach (var s3Object in objectList.S3Objects)
                {
                    using (var retrievedObject = s3Client.GetObject(new GetObjectRequest
                    {
                        BucketName = s3Object.BucketName,
                        Key = s3Object.Key
                    }))
                    {
                        MimeMessage message = MimeMessage.Load(retrievedObject.ResponseStream);
                        processedData.Add(retrievedObject.Key, message);
                    }
                }
            }
            return processedData;
        }
        public void DisplayDataSet1()
        {
            foreach (var processData in processedData)
            {
                Console.WriteLine("Email:" + processData.Key + "\n");
                Console.WriteLine("Subject: " + processData.Value.Subject + "\n");
                Console.WriteLine("Date: " + processData.Value.Date + "\n");
                Console.WriteLine("From: " + processData.Value.From + "\n");
                Console.WriteLine("To: " + processData.Value.To + "\n");
                Console.WriteLine("Cc:" + processData.Value.Cc + "\n");
                Console.WriteLine("------------------------------------------------------------------------------------------" + "\n");
            }
        }
        public void DisplayDataSet2()
        {
            var To = processedData.Where(e => e.Value.To != null && e.Value.To.Count != 0).Sum(t => t.Value.To.Count);
            var CC = processedData.Where(e => e.Value.Cc != null && e.Value.Cc.Count != 0).Sum(c => c.Value.Cc.Count);
            Console.WriteLine("Last Year: " + processedData.Where(e => e.Value.Date.Year < DateTime.Now.Year).Count());
            Console.WriteLine("This Year: " + processedData.Where(e => e.Value.Date.Year == DateTime.Now.Year).Count());
            Console.WriteLine("This Month: " + processedData.Where(e => e.Value.Date.Month == DateTime.Now.Month).Count());
            Console.WriteLine("This Week: " + processedData.Where(e => e.Value.Date >= DateTime.Now.AddDays(-(int)DateTime.Now.DayOfWeek).Date
            && e.Value.Date <= DateTime.Now.AddDays(6 - (int)DateTime.Now.DayOfWeek).Date).Count());
            Console.WriteLine("Total Recipients: " + (To + CC));
            Console.WriteLine("To Count: " + To);
            Console.WriteLine("CC Count: " + CC);
            Console.WriteLine("Subject Contains Confusion: " + processedData.Where(e => e.Value.Subject.ToLower().Contains("confusion")).Count());
            Console.WriteLine("Message Contains Header x-gt-settings: " + processedData.Sum(e => e.Value.Headers.Count(f => f.Field.ToLower().Contains("x-gt-settings"))));
            Console.WriteLine("Message Contains Three Musketeers: " + processedData.Where(e => e.Value.TextBody.Contains("Athos,") || e.Value.TextBody.Contains("Aramis")
            || e.Value.TextBody.Contains("Porthos)")).Count());
            Console.WriteLine("Sender is from Space.Corp: " + processedData.Where(e => e.Value.Sender != null && e.Value.Sender.Address.Contains("space.corp")).Count());
            Console.WriteLine("Errors: " + "o");
        }
    }

}
