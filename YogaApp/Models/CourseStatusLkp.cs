using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class CourseStatusLkp
    {
        public CourseStatusLkp()
        {
            Courses = new HashSet<Course>();
        }

        public int StatusId { get; set; }
        public string? Status { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
    }
}
