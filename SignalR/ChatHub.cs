using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalR
{
  
    public class ChatHub:Hub
    {
        private static Dictionary<string, string> Chatters = new Dictionary<string, string>();
        public void JoinChat(string name)
        {
            Chatters.Add(Context.ConnectionId, name);
            Clients.All.DisplayMsg(name + " just joined.");
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            Clients.All.DisplayMsg(Chatters[Context.ConnectionId] + " has left.");
            Chatters.Remove(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
        public void BroadcastMsg(string msg)
        {

            Clients.All.DisplayMsg(Chatters[Context.ConnectionId] +": " + msg);
        }
    }
}