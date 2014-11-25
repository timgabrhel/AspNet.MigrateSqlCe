using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AspNet.MigrateSqlCe.App.Models
{
    public class Transfer : BindableBase
    {
        public int TransferId { get; set; }

        public int? SourceTransId { get; set; }

        public int? DestTransId { get; set; }

        [Ignore]
        public Transaction SourceTrans { get; set; }

        [Ignore]
        public Transaction DestTrans { get; set; }
    }
}
