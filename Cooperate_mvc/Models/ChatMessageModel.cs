using System;
using System.ComponentModel.DataAnnotations;

namespace Cooperate_mvc.Models
{
    public class ChatMessageModel
    {
        public long Id { get; set; }

        public string Text { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Datetime { get; set; }

        public string SenderLogin { get; set; }

        public string RecipientLogin { get; set; }
    }
}