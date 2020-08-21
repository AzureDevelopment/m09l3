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
using System.Net;
using System.Diagnostics;

namespace Company.Function
{
    public class FirstFunction
    {
        private readonly ITelemetry _telemetry;

        public FirstFunction(ITelemetry telemetry)
        {
            _telemetry = telemetry;
        }

        [FunctionName("FirstFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            _telemetry.SetUserContext("testId");
            var requestActivity = new Activity("Sample: Function 1 HttpRequest");
            requestActivity.Start();
            _telemetry.SetOperationContext(requestActivity.Id);
            _telemetry.Event(new Event
            {
                Name = "Starting some important operation",
                Context = req.HttpContext,
                Additional = new Dictionary<string, string>
                {
                    ["ts"] = DateTime.Now.ToString()
                },

            });
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("CorellationId", requestActivity.Id);
            DateTime startOfDependencyCall = DateTime.Now;
            HttpResponseMessage result = await httpClient.GetAsync("http://localhost:7071/api/SecondFunction");
            _telemetry.Dependency(new Dependency
            {
                Name = "Fetching something important",
                Request = result.RequestMessage,
                Response = result,
                Duration = (DateTime.Now - startOfDependencyCall).TotalMilliseconds
            });
            var requestOperation = _telemetry.StartOperation(requestActivity);
            _telemetry.StopOperation(requestOperation);
            if (result.StatusCode == HttpStatusCode.OK)
            {
                return new OkObjectResult("Thanks");
            }
            else
            {
                throw new Exception();
            }

        }
    }
}
