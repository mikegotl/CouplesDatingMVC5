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
    
    public partial class vwQuestionAnswers
    {
        public long memberID { get; set; }
        public int questionID { get; set; }
        public string questionFull { get; set; }
        public Nullable<short> questionCategoryID { get; set; }
        public Nullable<short> questionImportance { get; set; }
        public string questionChoiceFull { get; set; }
        public Nullable<int> ordinal { get; set; }
        public Nullable<bool> multipleAnswersAllowed { get; set; }
        public int questionChoiceID { get; set; }
    }
}