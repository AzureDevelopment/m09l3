using System.Net.Http;
using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Company.Function
{
    public class ThirdFunction
    {
        private readonly ITelemetry _telemetry;

        public ThirdFunction(ITelemetry telemetry)
        {
            _telemetry = telemetry;
        }

        [FunctionName("ThirdFunction")]
        public void Run(
            [QueueTrigger("myqueue-items")] string operationId,
            ILogger log)
        {
            _telemetry.SetOperationContext(operationId);
            _telemetry.Event(new Event
            {
                Name = "Starting some important operation in third function",
                Additional = new Dictionary<string, string>
                {
                    ["ts"] = DateTime.Now.ToString()
                },

            });

            if (new Random().Next(0, 2) != 0)
                _telemetry.Event(new Event
                {
                    Name = "Ending some important operation in third function operation succeed",
                    Additional = new Dictionary<string, string>
                    {
                        ["ts"] = DateTime.Now.ToString()
                    },

                });

        }
    }
}
