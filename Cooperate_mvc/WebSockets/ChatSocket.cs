using Microsoft.Web.WebSockets;
using System.Web.Mvc;

namespace Cooperate_mvc.WebSockets
{
    public class ChatSocket : WebSocketHandler
    {
        private static WebSocketCollection clients = new WebSocketCollection();

        public string Login1 { get; private set; }

        public string Login2 { get; private set; }

        public override void OnOpen()
        {
            Login1 = this.WebSocketContext.QueryString[0];
            Login2 = this.WebSocketContext.QueryString[1];

            if (string.IsNullOrWhiteSpace(Login1) || string.IsNullOrWhiteSpace(Login2))
                return;

            clients.Add(this);
        }

        public static void SendTo(string L1, string L2, string message)
        {
            foreach (var socket in clients)
            {
                var s = (ChatSocket)socket;
                if ((s.Login1.Equals(L1) && s.Login2.Equals(L2)) || (s.Login1.Equals(L2) && s.Login2.Equals(L1)))
                {
                    socket.Send(message);
                }
            }
        }

        public override void OnClose()
        {
            clients.Remove(this);
            //clients.Broadcast(Id.ToString() + " disconnected");
        }

    }
}