using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace AspNet.MigrateSqlCe.App.Models
{
    [Table("Images")]
    public class Image : BindableBase
    {
        [PrimaryKey, AutoIncrement]
        public int ImageId { get; set; }
        public string ImagePath { get; set; }
        public string ImageName { get; set; }
        public int TransId { get; set; }
    }
}
