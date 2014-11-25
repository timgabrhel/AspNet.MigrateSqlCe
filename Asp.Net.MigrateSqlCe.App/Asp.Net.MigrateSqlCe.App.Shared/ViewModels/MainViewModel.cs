using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Windows.Devices.Sensors;
using AspNet.MigrateSqlCe.App.Models;
using SQLite;

namespace AspNet.MigrateSqlCe.App.ViewModels
{
    public class MainViewModel : BindableBase
    {
        private List<Account> _accounts;

        public List<Account> Accounts
        {
            get { return _accounts; }
            set { SetProperty(ref _accounts, value); }
        }

        public ICommand LoadedCommand { get; private set; }

        public MainViewModel()
        {
            LoadedCommand = new RelayCommand(Loaded);
        }

        private async void Loaded()
        {
            var conn = new SQLiteAsyncConnection(DbHelper.DatabaseName);
            Accounts = await conn.Table<Account>().ToListAsync();
        }
    }
}
