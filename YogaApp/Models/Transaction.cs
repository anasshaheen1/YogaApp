using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YogaApp.Models
{
    public partial class Transaction
    {
        public int Tid { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? Amount { get; set; }
        public int? TypeId { get; set; }
        public int? TstatusId { get; set; }
        public int? UserId { get; set; }
        public int? ParticipantId { get; set; }
        public int? LocationId { get; set; }
        public int? InstructorId { get; set; }
        public int? TriggerdBy { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? TriggerDate { get; set; }

        public virtual Instructor? Instructor { get; set; }
        public virtual Participant? Participant { get; set; }
        public virtual TransactionStatusLkp? Tstatus { get; set; }
        public virtual TransactionTypeLkp? Type { get; set; }
    }
}
