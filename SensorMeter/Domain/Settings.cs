using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace SensorMeter.Domain
{
    public class Settings
    {
        public int GetPort()
        {
            int Port = 5000;
            return Port;
        }

        public static string GetIP()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("Local IP Address Not Found!");
        }

        public static string GetResponce()
        { 
            string status ="Stable";
            WebClient wc = new WebClient();
            status = wc.DownloadString("");
            return status;
        }
    }
}