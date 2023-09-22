using System;
using System.IO;
using FunctionBlobTrigger.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FunctionBlobTrigger
{
    public class ReenbitTriggerFunction
    {
        private readonly IConfigurationRoot config;
        public ReenbitTriggerFunction()
        {
            config = new ConfigRoot().Config;
        }
        [FunctionName("ReenbitTriggerFunction")]
        public void Run([BlobTrigger("files/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, ILogger log)
        {

            if (name != null)
            {
                try
                {
                    var index = name.IndexOf('#');
                    var email = name.Substring(0, index);
                    var fileName = name.Substring(index+1);
                    var subject = "Confirmation";
                    var body = "The file is successfully uploaded!";
                    ISender sender = new EmailSender();
                    sender.Send(email, subject, body, config);
                    log.LogInformation($"C# Blob trigger function Processed blob\n Name:{fileName} \n Size: {myBlob.Length} Bytes");
                }
                catch(Exception ex)
                {
                    log.LogError(ex.Message);
                }
            }

            
        }
    }
}
