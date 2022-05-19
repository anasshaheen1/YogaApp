using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class Participant
    {
        public Participant()
        {
            ClassParticipantLinks = new HashSet<ClassParticipantLink>();
            Reviews = new HashSet<Review>();
            Transactions = new HashSet<Transaction>();
        }

        public int ParticipantId { get; set; }
        public string? Name { get; set; }
        public string? AboutMe { get; set; }
        public DateTime? LastUpdatedOn { get; set; }

        public int YogaUserId { get; set; }

        public virtual ICollection<ClassParticipantLink> ClassParticipantLinks { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
