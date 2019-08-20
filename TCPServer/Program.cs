using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Timers;
using BaseLib;

namespace TCPServer
{
    class Program
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "182.158.103.101";
        static TcpClient client;
        static TcpListener listener;
        static IPAddress localAdd;

        static void Main(string[] args)
        {
            StartListening();
            Console.ReadKey();
        }
        static void StartListening() {
            localAdd = IPAddress.Parse(SERVER_IP);
            listener = new TcpListener(localAdd, PORT_NO);

            Console.WriteLine("Listening...");
            listener.Start();

            //---incoming client connected---
            client = listener.AcceptTcpClient();


            Timer t = new Timer(5000);
            t.Elapsed += t_Elapsed;
            t.AutoReset = true;
            t.Start();
        }
        static void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (client.Connected) { 
                Random random = new Random();
                Message msg = new Message()
                {
                    SensorId = 1,
                    Key = "A1",
                    Pressure = random.Next(1, 10),
                    Time = DateTime.Now
                };
            
                NetworkStream nwStream = client.GetStream();
                var buffer = msg.GetBytes();
            
                Console.WriteLine("Sending : " + msg.ToString());
                nwStream.Write(buffer, 0, buffer.Length);
            }
            else
            {
                var t = (Timer)sender;
                t.Enabled = false;
                t.Close();
                listener.Stop();
                StartListening();

            }
        }
        
    }
}
