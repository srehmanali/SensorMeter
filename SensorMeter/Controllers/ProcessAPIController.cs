using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SensorMeter.Controllers
{
    public class ProcessAPIController : ApiController
    {
        [HttpGet]
        public string Start(int id) {
            Domain.Client.StartListening(id);
            return "Started.";
        }
        [HttpGet]
        public string Stop(int id)
        {
            Domain.Client.StopReading(id);
            return "Stopped.";
        }

    }
}
