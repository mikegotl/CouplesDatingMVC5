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
    
    public partial class tblAttributes
    {
        public tblAttributes()
        {
            this.tblMemberAttributes = new HashSet<tblMemberAttributes>();
        }
    
        public long attributeID { get; set; }
        public string attributeName { get; set; }
        public string attributeDescription { get; set; }
    
        public virtual ICollection<tblMemberAttributes> tblMemberAttributes { get; set; }
    }
}