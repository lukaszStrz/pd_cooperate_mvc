using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cooperate_mvc.Models
{
    public class PostModel
    {
        public long Id { get; set; }

        public string Text { get; set; }

        public string Author_login { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Datetime { get; set; }
    }
}