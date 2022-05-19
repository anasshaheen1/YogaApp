using System;
using System.Collections.Generic;

namespace YogaApp.Models
{
    public partial class TransactionStatusLkp
    {
        public TransactionStatusLkp()
        {
            Transactions = new HashSet<Transaction>();
        }

        public int TransactionStatusId { get; set; }
        public string? TransactionStatus { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
