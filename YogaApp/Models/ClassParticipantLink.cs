using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class ClassParticipantLink
    {
        public int ClassParticipantId { get; set; }
        public int? ClassId { get; set; }
        public int? ParticipantId { get; set; }
        public int? EnrollmentStatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public int? LastUpdatedBy { get; set; }

        public virtual Course? Class { get; set; }
        public virtual EnrollmentStatusLkp? EnrollmentStatus { get; set; }
        public virtual Participant? Participant { get; set; }
    }
}
