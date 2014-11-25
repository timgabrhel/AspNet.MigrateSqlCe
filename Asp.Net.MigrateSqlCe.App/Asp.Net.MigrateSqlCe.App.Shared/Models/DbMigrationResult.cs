using System;
using System.Collections.Generic;
using System.Text;

namespace AspNet.MigrateSqlCe.App.Models
{
    public class DbMigrationResult
    {
        public List<Account> Accounts { get; set; }

        public List<Category> Categories { get; set; }

        public List<Group> Groups { get; set; }

        public List<Payee> Payees { get; set; }

        public List<Transaction> Transactions { get; set; }

        public List<Transfer> Transfers { get; set; }

        public List<Image> Images { get; set; } 
    }
}
