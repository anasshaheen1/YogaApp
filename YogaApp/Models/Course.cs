using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YogaApp.Models
{
    public partial class Course
    {
        public Course()
        {
            ClassParticipantLinks = new HashSet<ClassParticipantLink>();
        }

        public int CourseId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? RequiredExpertise { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")] 
        public DateTime? StartDate { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? EndDate { get; set; }
        public TimeSpan? StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public int? SessionCount { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? PricePerSession { get; set; }
        public int? Capacity { get; set; }
        public int? LocationId { get; set; }
        public int? InstructorId { get; set; }
        public int? StatusId { get; set; }
        public int? PreviousOfferingCourseId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }

        public virtual Location? Location { get; set; }
        public virtual CourseStatusLkp? Status { get; set; }
        public virtual ICollection<ClassParticipantLink> ClassParticipantLinks { get; set; }
    }
}
