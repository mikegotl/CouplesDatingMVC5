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
    
    public partial class Blog
    {
        public int BlogID { get; set; }
        public Nullable<short> BlogCategoryID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Nullable<System.DateTime> DatePosted { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public Nullable<long> CreatedBy { get; set; }
    
        public virtual BlogCategory BlogCategory { get; set; }
    }
}
