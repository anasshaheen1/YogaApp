using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class LocationAdministratorLink
    {
        public int AdministratorId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? CreatedDate { get; set; }

        public virtual Location? Location { get; set; }
    }
}
