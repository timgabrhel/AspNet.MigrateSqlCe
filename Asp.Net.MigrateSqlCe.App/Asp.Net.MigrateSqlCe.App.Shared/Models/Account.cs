using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AspNet.MigrateSqlCe.App.Models
{
    [Table("Accounts")]
    public class Account : BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int AccountId { get; set; }
        public string AccountName { get; set; }
        public int AccountType { get; set; }
        public float AccountBalance { get; set; }
        public float AccountBalanceCleared { get; set; }
        public bool? Active { get; set; }
        public int GroupId { get; set; }
        public int? SortId { get; set; }

        [Ignore]
        public List<Transaction> Transactions { get; set; }

        [Ignore]
        public Group Group { get; set; }
    }
}
