using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using AspNet.MigrateSqlCe.App.Models;
using SQLite;

namespace AspNet.MigrateSqlCe.App
{
    public static class DbHelper
    {
        public static string DatabaseName = "checkbook.db";


        public static async Task<bool> DoesDbExist()
        {
            bool dbexist = true;
            try
            {
                var storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(DatabaseName);
            }
            catch
            {
                dbexist = false;
            }

            return dbexist;
        }

        public static async Task CreateDatabase()
        {
            var conn = new SQLiteAsyncConnection(DatabaseName);
            await conn.CreateTableAsync<Group>();
            await conn.CreateTableAsync<Account>();
            await conn.CreateTableAsync<Payee>();
            await conn.CreateTableAsync<Category>();
            await conn.CreateTableAsync<Transaction>();
            await conn.CreateTableAsync<Image>();
        }
    }
}
