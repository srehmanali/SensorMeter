using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensorMeter.Models
{
    public class CompartmentsDTO
    {
        public int CompartmentID { get; set; }
        public string Key { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }

    }
}