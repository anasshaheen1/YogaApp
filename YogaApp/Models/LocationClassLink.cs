using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class LocationClassLink
    {
        public int LocationClassLinkId { get; set; }
        public int? LocationId { get; set; }
        public int? ClassId { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
