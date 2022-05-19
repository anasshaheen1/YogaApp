using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class TransactionTypeLkp
    {
        public TransactionTypeLkp()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int TransactionTypeId { get; set; }
        public string? TransactionType { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
