using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalR.Models;

namespace SignalR
{
    public class SignalHub:Hub
    {
        static List<string> MessageQueue = new List<string>();
        static List<string> ActiveWindowsClients = new List<string>();

        public override Task OnConnected()
        {
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            UnRegisterPCClient();
            return base.OnDisconnected(stopCalled);
        }

        // explicitly called from Windows clients, so that only Windows clients are added to ActiveWindowsClients
        public void RegisterPCClient(string make, string model)
        {
            if(!ActiveWindowsClients.Any(x=>x == Context.ConnectionId))
                ActiveWindowsClients.Add(Context.ConnectionId);

            SaveComputerInfo(make, model);
        }
        public void UnRegisterPCClient()
        {
            //called by Window service client when the service is stopped
            if (ActiveWindowsClients.Any(x => x == Context.ConnectionId))
                ActiveWindowsClients.Remove(Context.ConnectionId);
        }
        
        private void SaveComputerInfo(string make, string model)
        {
            //save to database through entity framework
            using (var ctx = new Context())
            {
              WindowsClient windowsCleint = new WindowsClient() { Make = make, Model = model, TimeAdded = DateTime.Now.ToUniversalTime() };

                ctx.WindowsClients.Add(windowsCleint);

                ctx.SaveChanges();
            }
        }

        public void Relay(string msg)
        {
            //call SaveMessage method on Windows clients
            Clients.Clients(ActiveWindowsClients).SaveMessage(msg);
            //Clients.All.SaveMessage(msg); // this works as well, even though the web client doesn't have SaveMessage defined. I thought it would error out, but it didn't.
            Clients.Caller.displayResponse(ActiveWindowsClients.Count);
        }
    }
}