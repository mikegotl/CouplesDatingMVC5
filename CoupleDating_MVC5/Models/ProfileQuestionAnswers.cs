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
    
    public partial class ProfileQuestionAnswers
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProfileQuestionAnswers()
        {
            this.UserProfileAnswers = new HashSet<UserProfileAnswers>();
        }
    
        public int Id { get; set; }
        public string Answer { get; set; }
        public Nullable<int> QuestionID { get; set; }
    
        public virtual ProfileQuestions ProfileQuestions { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserProfileAnswers> UserProfileAnswers { get; set; }
    }
}
