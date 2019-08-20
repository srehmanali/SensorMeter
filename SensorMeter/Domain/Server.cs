using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using System.Web;
using SensorMeter.Domain;

namespace SensorMeter.Domain
{
    public class Server
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        static TcpClient client;

        static void Main(string[] args)
        {
            IPAddress localAdd = IPAddress.Parse(SERVER_IP);
            TcpListener listener = new TcpListener(localAdd, PORT_NO);

            Console.WriteLine("Listening...");
            listener.Start();

            //---incoming client connected---
            client = listener.AcceptTcpClient();


            Timer t = new Timer(1000);
            t.Elapsed += t_Elapsed;
            t.AutoReset = true;
            t.Start();
            Console.ReadKey();
        }

        static void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            Random random = new Random();
            Message msg = new Message()
            {
                SensorId = 1,
                Key = "A1",
                Pressure = random.Next(1, 10),
                Time = DateTime.Now
            };
            //---get the incoming data through a network stream---
            NetworkStream nwStream = client.GetStream();
            //byte[] buffer = new byte[client.ReceiveBufferSize];
            var buffer = msg.GetBytes();
            ////---read incoming stream---
            //int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

            ////---convert the data received into a string---
            //string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            //Console.WriteLine("Received : " + dataReceived);

            //---write back the text to the client---
            Console.WriteLine("Sending : " + msg.ToString());
            nwStream.Write(buffer, 0, buffer.Length);

        }
        
    }
}