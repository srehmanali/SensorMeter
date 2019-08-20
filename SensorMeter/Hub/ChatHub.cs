using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;

namespace SensorMeter
{
    public class ChatHub: Hub
    {
        public void LetsChat(string Cl_Name, string Cl_Message)
        {
            Clients.All.NewMessage(Cl_Name, Cl_Message);
        }
        public void SendUpdate(int sensorId, string data)
        {
            Clients.All.UpdateChart(sensorId, data);
        }

        public override Task OnConnected(){
            SendUpdate(1, "Testing");
            return base.OnConnected();
        }

        //public override Task OnDisconnected(bool stopCalled){
        //}

        //public override Task OnReconnected(){
        //}

    }
}