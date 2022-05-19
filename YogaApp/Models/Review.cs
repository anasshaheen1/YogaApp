using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class Review
    {
        public int ReviewId { get; set; }
        public string? ReviewText { get; set; }
        public short? Rating { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? UserId { get; set; }
        public int? InstructorId { get; set; }
        public int? LocationId { get; set; }
        public bool? Valid { get; set; }

        public virtual Instructor? Instructor { get; set; }
        public virtual Location? Location { get; set; }
        public virtual Participant? User { get; set; }
    }
}
