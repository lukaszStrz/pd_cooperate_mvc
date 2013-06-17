using Microsoft.Web.WebSockets;
using System.Web.Mvc;

namespace Cooperate_mvc
{
    public class GroupsSocket : WebSocketHandler
    {
        private static WebSocketCollection clients = new WebSocketCollection();

        public string Login { get; private set; }

        public override void OnOpen()
        {
            Login = this.WebSocketContext.QueryString["login"];
            //clients.Broadcast(Login + " connected");
            clients.Add(this);
        }

        public static void SendTo(string login, string message)
        {
            foreach (var socket in clients)
            {
                if (((GroupsSocket)socket).Login == login)
                {
                    socket.Send(message);
                }
            }
        }

        public override void OnClose()
        {
            clients.Remove(this);
            //clients.Broadcast(Login + " disconnected");
        }
    }
}