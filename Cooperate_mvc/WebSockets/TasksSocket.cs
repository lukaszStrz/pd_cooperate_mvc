using Microsoft.Web.WebSockets;
using System.Web.Mvc;

namespace Cooperate_mvc.WebSockets
{
    public class TasksSocket : WebSocketHandler
    {
        private static WebSocketCollection clients = new WebSocketCollection();

        /// <summary>
        /// Id tasku
        /// </summary>
        public long Id { get; private set; }

        public override void OnOpen()
        {
            long id;
            if (long.TryParse(this.WebSocketContext.QueryString["id"], out id))
            {
                Id = id;
                //clients.Broadcast(Id.ToString() + " connected");
                clients.Add(this);
            }
        }

        public static void SendTo(long id, string message)
        {
            foreach (var socket in clients)
            {
                if (((TasksSocket)socket).Id == id)
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