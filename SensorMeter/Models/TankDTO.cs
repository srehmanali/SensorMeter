using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SensorMeter.Models
{
    public class TankDTO
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumCompartment { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifyDate { get; set; }
        public virtual ICollection<CompartmentsDTO> Keys { get; set; }
    }
}