using System.Net.Http;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Company.Function
{
    public class SecondFunction
    {
        private readonly ITelemetry _telemetry;

        public SecondFunction(ITelemetry telemetry)
        {
            _telemetry = telemetry;
        }

        [FunctionName("SecondFunction")]
        [return: Queue("myqueue-items")]
        public string Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string operationId = req.Headers["CorellationId"];
            _telemetry.SetUserContext("testId");
            _telemetry.SetOperationContext(operationId);
            _telemetry.Event(new Event
            {
                Name = "Starting some important operation in second function",
                Context = req.HttpContext,
                Additional = new Dictionary<string, string>
                {
                    ["ts"] = DateTime.Now.ToString()
                }
            });
            return operationId;

        }
    }
}
