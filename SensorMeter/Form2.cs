using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SensorMeter
{
    public partial class Form2 : Form
    {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            msg("Client Started");
            clientSocket.Connect("127.0.0.1", 8888);
            label1.Text = "Client Socket Program - Server Connected ...";
        }

       

        public void msg(string mesg)
        {
            textBox1.Text = textBox1.Text + Environment.NewLine + " >> " + mesg;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = System.Text.Encoding.ASCII.GetBytes(textBox2.Text + "$");
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();

            byte[] inStream = new byte[10025];
            serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
            string returndata = System.Text.Encoding.ASCII.GetString(inStream);
            msg(returndata);
            textBox2.Text = "";
            textBox2.Focus();
        } 
    }
}
