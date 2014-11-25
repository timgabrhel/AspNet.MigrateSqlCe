using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AspNet.MigrateSqlCe.App.Models
{
    [Table("Categories")]
    public class Category : BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool? Active { get; set; }

        [Ignore]
        public List<Transaction> Transactions { get; set; }
    }
}
