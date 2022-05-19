using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class Instructor
    {
        public Instructor()
        {
            LocationInstructorLinks = new HashSet<LocationInstructorLink>();
            Reviews = new HashSet<Review>();
            Transactions = new HashSet<Transaction>();
        }

        public int InstructorId { get; set; }
        public string? Name { get; set; }
        public string? Heading { get; set; }
        public string? Body { get; set; }
        public byte[]? Photo { get; set; }
        public string? ContactInfo { get; set; }
        public DateTime? LastUpdatedDate { get; set; }

        public virtual ICollection<LocationInstructorLink> LocationInstructorLinks { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }

        public int YogaUserId { get; set; }

    }
}
