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
    
    public partial class tblMembershipPackage
    {
        public tblMembershipPackage()
        {
            this.PaymentPlans = new HashSet<PaymentPlans>();
        }
    
        public int PackageID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
    
        public virtual ICollection<PaymentPlans> PaymentPlans { get; set; }
    }
}