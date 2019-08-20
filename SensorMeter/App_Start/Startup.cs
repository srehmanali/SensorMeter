using System;
using Microsoft.Owin;
using Owin;


[assembly: OwinStartup(typeof(SensorMeter.Startup))]

namespace SensorMeter
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
        }
    }
}
