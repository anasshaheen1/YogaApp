using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class Location
    {
        public Location()
        {
            Courses = new HashSet<Course>();
            LocationAdministratorLinks = new HashSet<LocationAdministratorLink>();
            LocationInstructorLinks = new HashSet<LocationInstructorLink>();
            Reviews = new HashSet<Review>();
        }

        public int LocationId { get; set; }
        public string? Description { get; set; }
        public string? OpeningTimes { get; set; }
        public string? ContactInfo { get; set; }
        public string? AddressPostcode { get; set; }
        public string? AddressFull { get; set; }
        public int? LastModifiedBy { get; set; }

        public DateTime? LastModifiedDate { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<LocationAdministratorLink> LocationAdministratorLinks { get; set; }
        public virtual ICollection<LocationInstructorLink> LocationInstructorLinks { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }

        public int YogaUserId { get; set; }

    }
}
