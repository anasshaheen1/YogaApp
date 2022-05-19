using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class ApprovalStatusLkp
    {
        public ApprovalStatusLkp()
        {
            LocationInstructorLinks = new HashSet<LocationInstructorLink>();
        }

        public int ApprovalStatusId { get; set; }
        public string? ApprovalStatus { get; set; }

        public virtual ICollection<LocationInstructorLink> LocationInstructorLinks { get; set; }
    }
}
