using System;
using System.ComponentModel.DataAnnotations;

namespace Cooperate_mvc.Models
{
    public class TaskStatusPartialModel
    {
        [DataType(DataType.DateTime)]
        public DateTime LastChange { get; set; }

        public string Login { get; set; }
    }
}