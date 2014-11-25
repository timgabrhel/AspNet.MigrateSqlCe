using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AspNet.MigrateSqlCe.App.Models
{
    [Table("Payees")]
    public class Payee : BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int PayeeId { get; set; }
        public string PayeeName { get; set; }
        public bool? Active { get; set; }

        [Ignore]
        public List<Transaction> Transactions { get; set; }
    }
}
