using Cooperate_mvc.WebSockets;
using Microsoft.Web.WebSockets;
using System.Web;

namespace Cooperate_mvc
{
    public class TasksSocketHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
            {
                context.AcceptWebSocketRequest(new TasksSocket());
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