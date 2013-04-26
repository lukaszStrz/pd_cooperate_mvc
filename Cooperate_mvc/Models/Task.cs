//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Cooperate_mvc.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Task
    {
        public Task()
        {
            this.Comments = new HashSet<Comment>();
        }
    
        public long Task_id { get; set; }
        public string Task_title { get; set; }
        public string Task_description { get; set; }
        public System.DateTime Task_creationDate { get; set; }
        public System.DateTime Task_deadline { get; set; }
        public int User_from { get; set; }
        public int User_to { get; set; }
        public byte TaskStatus_id { get; set; }
        public System.DateTime Task_statusLastChange { get; set; }
        public int User_statusChangedBy { get; set; }
        public long Group_id { get; set; }
    
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual Group Group { get; set; }
        public virtual TaskStatu TaskStatu { get; set; }
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
    }
}