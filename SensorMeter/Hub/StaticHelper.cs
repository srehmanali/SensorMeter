using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SensorMeter.Domain;

namespace SensorMeter
{
    public class StaticHelper
    {
        public static DependencyHelper dh;
        static StaticHelper() {
            //dh = new DependencyHelper();
        }
        public static void InitializeDependency() {
            dh = new DependencyHelper();            
        }
    }
}