using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AspNet.MigrateSqlCe.App.Models
{
    [Table("Groups")]
    public class Group : BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public float TotalBalance { get; set; }
        public bool? Active { get; set; }
        public int? SortId { get; set; }

        [Ignore]
        public List<Account> Accounts { get; set; }
    }
}
