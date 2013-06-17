using Cooperate_mvc.WebSockets;
using Microsoft.Web.WebSockets;
using System.Web;

namespace Cooperate_mvc
{
    /// <summary>
    /// Summary description for ChatSocketHandler
    /// </summary>
    public class ChatSocketHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(new ChatSocket());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}