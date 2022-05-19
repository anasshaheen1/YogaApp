using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class EnrollmentStatusLkp
    {
        public EnrollmentStatusLkp()
        {
            ClassParticipantLinks = new HashSet<ClassParticipantLink>();
        }

        public int EnrollmentStatusId { get; set; }
        public string? EntrollmentStatus { get; set; }

        public virtual ICollection<ClassParticipantLink> ClassParticipantLinks { get; set; }
    }
}
