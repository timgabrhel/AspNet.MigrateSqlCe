using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AspNet.MigrateSqlCe.App.Models
{
    [Table("Transactions")]
    public class Transaction : BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int TransId { get; set; }
        public DateTime TransDate { get; set; }
        public float TransAmount { get; set; }
        public int TransType { get; set; }
        public int RefNum { get; set; }
        public string Memo { get; set; }
        public int PayeeId { get; set; }
        public int CategoryId { get; set; }
        public int AccountId { get; set; }
        public bool Cleared { get; set; }

        [Ignore]
        public Transaction SourceTrans { get; set; }

        [Ignore]
        public Transaction DestTrans { get; set; }

        [Ignore]
        public List<Image> Images { get; set; }

        [Ignore]
        public Category Category { get; set; }

        [Ignore]
        public Payee Payee { get; set; }
    }
}
