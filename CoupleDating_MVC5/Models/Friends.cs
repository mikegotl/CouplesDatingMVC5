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
    
    public partial class Friends
    {
        public int ID { get; set; }
        public Nullable<long> Member1ID { get; set; }
        public Nullable<long> Member2ID { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<bool> Accepted { get; set; }
        public Nullable<System.DateTime> AcceptedDate { get; set; }
        public Nullable<bool> Denied { get; set; }
        public Nullable<System.DateTime> DeniedDate { get; set; }
        public Nullable<System.DateTime> LastCommunication { get; set; }
    
        public virtual Member Member { get; set; }
        public virtual Member Member1 { get; set; }
    }
}