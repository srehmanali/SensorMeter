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
    public class ClientBack
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "182.158.103.101";
        const int MESSAGE_LEN = 100;
        static byte[] bytesToRead;
        static List<int> Sensors = new List<int>();
        static TcpClient client;





        public static void StartListening(int sensorId = 0)
        {
            //if (Sensors.Contains(sensorId)) return;
            if (sensorId != 0 && !Sensors.Contains(sensorId))
            {
                Sensors.Add(sensorId);
            }

            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);
            try
            {
                //try { listener.Stop(); }
                //catch { }
                listener.Start();
                listener.BeginAcceptTcpClient(new AsyncCallback(ListenCallback), listener);
            }
            catch
            {
                listener.Stop();
                if (Sensors.Count > 0) {
                    ClientBack.client = null;
                    StartListening();
                }
            }
        }
        public static void ListenCallback(IAsyncResult AR)
        {
            bytesToRead = new byte[client.ReceiveBufferSize];
            NetworkStream nwStream = client.GetStream();
            var result = nwStream.BeginRead(bytesToRead, 0, client.ReceiveBufferSize, new AsyncCallback(ReadCallback), client);
            nwStream.EndRead(result);
        }
        public static void ReadCallback(IAsyncResult AR)
        {
            var msg = new Message(bytesToRead);
            Console.WriteLine("Received at " + PORT_NO + " : " + msg.ToString());
            //var msg = new Message(msgStr);
            if (bytesToRead[0] > 0) { 
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

            TcpClient client = (TcpClient)AR.AsyncState;
            if (Sensors.Count > 0)
            {
                try
                {
                    //bytesToRead = new byte[client.ReceiveBufferSize];
                    //NetworkStream nwStream = client.GetStream();
                    //var result = nwStream.BeginRead(bytesToRead, 0, 1, new AsyncCallback(ReadCallback), client);
                    //nwStream.EndRead(result);
                    StartListening();
                }
                catch (Exception ex)
                {
                    
                    //client.Close();
                    if (ex is IOException)
                    {
                        StartListening();
                        //throw;
                    }
                }
            }
            else {
                client.Close();
                ClientBack.client = null;
            }
        }
        public static void StopReading(int sensorId)
        {
            Sensors.Remove(sensorId);
        }
    }
}