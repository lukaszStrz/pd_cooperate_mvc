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
    
    public partial class Participation
    {
        public int User_id { get; set; }
        public long Group_id { get; set; }
        public bool Participation_isAdmin { get; set; }
    
        public virtual Group Group { get; set; }
        public virtual User User { get; set; }
    }
}
