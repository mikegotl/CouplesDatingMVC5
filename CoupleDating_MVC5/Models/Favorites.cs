//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoupleDating_MVC5.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Favorites
    {
        public int ID { get; set; }
        public Nullable<long> FavoriteMemberID { get; set; }
        public Nullable<long> MemberID { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public Nullable<bool> Enabled { get; set; }
    
        public virtual Member Member { get; set; }
        public virtual Member Member1 { get; set; }
    }
}
