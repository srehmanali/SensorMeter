using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SensorMeter
{
    public partial class Form1 : Form
    {
        private StringBuilder recievedData = new StringBuilder();

        public Form1()
        {
            InitializeComponent();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
          
            //foreach (string portname in SerialPort.GetPortNames())
            //{
            //    cmbCOMPort.Items.Add(portname);
            //}
            //timer1.Start();
        }
     
       
      
       
        

        private void btnOpenPort_Click(object sender, EventArgs e)
        {
           // //serialPort1.PortName = cmbCOMPort.Text;


           // IPAddress localAdd = IPAddress.Parse(SERVER_IP);
           // TcpListener listener = new TcpListener(localAdd, PORT_NO);
           // //Console.WriteLine("Listening...");
           // OutputWindow.Text = "Listening...\n";
           // listener.Start();
           // TcpClient client = listener.AcceptTcpClient();

           // //---get the incoming data through a network stream---
           // NetworkStream nwStream = client.GetStream();
           // byte[] buffer = new byte[client.ReceiveBufferSize];

           // //---read incoming stream---
           // int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

           // //---convert the data received into a string---
           // string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
           // OutputWindow.Text = dataReceived;
           // //Console.WriteLine("Received : " + dataReceived);
           // client.Close();
           // listener.Stop();
           //// Console.ReadLine();
           // //if (!serialPort1.IsOpen)
           // //    serialPort1.Open();


            TcpListener serverSocket = new TcpListener(8888);
            int requestCount = 0;
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            OutputWindow.Text = " >> Server Started\n";
            clientSocket = serverSocket.AcceptTcpClient();
            OutputWindow.Text = " >> Accept connection from client\n";
            requestCount = 0;

            while ((true))
            {
                try
                {
                    requestCount = requestCount + 1;
                    NetworkStream networkStream = clientSocket.GetStream();
                    byte[] bytesFrom = new byte[10025];
                    networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                    string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                    dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                    OutputWindow.Text = " >> Data from client - " + dataFromClient +"\n";
                    string serverResponse = "Last Message from client" + dataFromClient;
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(serverResponse);
                    networkStream.Write(sendBytes, 0, sendBytes.Length);
                    networkStream.Flush();
                    OutputWindow.Text = " >> " + serverResponse+"\n";
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }

            clientSocket.Close();
            serverSocket.Stop();
            OutputWindow.Text = " >> exit\n";
            Console.ReadLine();
        }
        

       

    }

}
