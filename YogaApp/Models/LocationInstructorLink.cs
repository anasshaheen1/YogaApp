using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class LocationInstructorLink
    {
        public int Id { get; set; }
        public int? LocationId { get; set; }
        public int? InstructorId { get; set; }
        public int? ApprovalStatusId { get; set; }
        public int? ApprovalBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? RemovalDate { get; set; }

        public virtual ApprovalStatusLkp? ApprovalStatus { get; set; }
        public virtual Instructor? Instructor { get; set; }
        public virtual Location? Location { get; set; }
    }
}
