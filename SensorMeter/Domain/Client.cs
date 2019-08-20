using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.IO;
using Microsoft.AspNet.SignalR;
using System.Net;
using System.Timers;

namespace SensorMeter.Domain
{
    public class Client
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "182.158.103.101";
        const int MESSAGE_LEN = 100;
        static byte[] bytesToRead;
        static List<int> Sensors = new List<int>();
        static bool listnerStarted = false;
        static TcpListener listener = new TcpListener(IPAddress.Parse(SERVER_IP), PORT_NO);



        public static void StartListening(int sensorId = 0)
        {
            if (sensorId != 0 && !Sensors.Contains(sensorId))
            {
                Sensors.Add(sensorId);
            }
            try
            {
                if (!listnerStarted) {
                    listener.Start();
                    listnerStarted = true;
                }
                listener.BeginAcceptTcpClient(new AsyncCallback(ListenCallback), listener);
            }
            catch
            {
                if (Sensors.Count > 0)
                {
                    StartListening();
                }
            }
        }
        public static void ListenCallback(IAsyncResult ar)
        {
            var listner = (TcpListener)ar.AsyncState;
            var client = listner.EndAcceptTcpClient(ar);
            try
            {
                bytesToRead = new byte[client.ReceiveBufferSize];
                NetworkStream nwStream = client.GetStream();
                var result = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                var msg = new Message(bytesToRead);
                if (bytesToRead[0] > 0)
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                    hubContext.Clients.All.UpdateChart(1, ASCIIEncoding.ASCII.GetString(bytesToRead));
                }
                if (Sensors.Contains(msg.SensorId))
                {
                    if (msg.Insert() > 0)
                    {
                        Console.WriteLine("Inserted into database successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Insert into database failed.");
                    }
                }
                
            }
            catch
            {
                client.Close();
            }
            finally
            {
                if (Sensors.Count > 0)
                {
                    StartListening();
                }
            }
        }
        public static void StopReading(int sensorId)
        {
            Sensors.Remove(sensorId);
        }
    }
}