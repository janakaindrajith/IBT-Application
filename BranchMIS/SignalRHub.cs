using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;

namespace BranchMIS
{
    [SignalR.Hubs.HubName("signalRHub")]
    public class SignalRHub : SignalR.Hubs.Hub
    {
        public void Broadcast(string message)
        {
            this.Clients.showMessage(message);
        }


        public string SayHello()
        {
            //Context property can be used to retreive HTTP attributes like User
            return "Hello " + Context.User.Identity.Name;
        }


        public void Subscribe(string category)
        {
            this.AddToGroup(category);
        }


        public void Publish(string category, string message)
        {

            this.Clients[category].showMessage(message);
        }
    }
}