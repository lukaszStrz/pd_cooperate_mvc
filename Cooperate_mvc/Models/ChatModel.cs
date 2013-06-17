using System.Collections.Generic;

namespace Cooperate_mvc.Models
{
    public class ChatModel
    {
        public string UserLogin { get; set; }

        public List<ChatMessageModel> Messages { get; set; }
    }
}