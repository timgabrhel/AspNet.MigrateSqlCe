using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;

namespace AspNet.MigrateSqlCe.Models
{
    public class CbmDataContext : DataContext
    {
        public Table<Account> Accounts;
        public Table<Category> Categories;
        public Table<Group> Groups;
        public Table<Images> Images;
        public Table<Payee> Payees;
        public Table<Transaction> Transactions;
        public Table<Transfer> Transfers;

        public CbmDataContext(IDbConnection connection) : base(connection)
        {
        }
    }

    [Table(Name = "Groups")]
    public class Group : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public Group()
        {
            _Accounts = new EntitySet<Account>(attach_Accounts, detach_Accounts);
        }

        #region Private Declarations

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs =
            new PropertyChangingEventArgs(String.Empty);

        private readonly EntitySet<Account> _Accounts;
        private List<Account> _AccountsSorted;
        private bool? _Active;

        private int _GroupId;

        private string _GroupName;

        private string _SortId;
        private decimal _TotalBalance;

        #endregion

        #region Columns

        [Column(IsVersion = true)]
        private Binary _version;

        [Column(Storage = "_GroupId", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int GroupId
        {
            get { return _GroupId; }
            set
            {
                if ((_GroupId != value))
                {
                    SendPropertyChanging();
                    _GroupId = value;
                    SendPropertyChanged("GroupId");
                }
            }
        }

        [Column(Storage = "_GroupName", DbType = "nvarchar(30)", CanBeNull = false)]
        public string GroupName
        {
            get { return _GroupName; }
            set
            {
                if ((_GroupName != value))
                {
                    SendPropertyChanging();
                    _GroupName = value;
                    SendPropertyChanged("GroupName");
                }
            }
        }

        [Column(Storage = "_TotalBalance", DbType = "Decimal(19,2)")]
        public decimal TotalBalance
        {
            get
            {
                decimal total = 0;

                foreach (Account account in _Accounts)
                {
                    total += account.Transactions.Sum(t => t.TransAmount);
                }

                return total;

                //decimal deposits = 0;
                //foreach (Account account in this._Accounts)
                //{
                //    deposits += account.Transactions
                //                    .Where(t => t.TransType == (int)TransType.Deposit)
                //                    .Sum(t => t.TransAmount);
                //}

                //decimal withdrawals = 0;
                //foreach (Account account in this._Accounts)
                //{
                //    withdrawals += account.Transactions
                //                    .Where(t => t.TransType == (int)TransType.Withdrawal)
                //                    .Sum(t => t.TransAmount);
                //}

                //return (deposits + withdrawals);
            }
            set { }
        }

        [Column(Storage = "_Active")]
        public bool? Active
        {
            get { return _Active; }
            set
            {
                if ((_Active != value))
                {
                    SendPropertyChanging();
                    _Active = value;
                    SendPropertyChanged("Active");
                }
            }
        }

        [Column(Storage = "_SortId", DbType = "nvarchar(30)", CanBeNull = false)]
        public string SortId
        {
            get { return _SortId; }
            set
            {
                if ((_SortId != value))
                {
                    SendPropertyChanging();
                    _SortId = value;
                    SendPropertyChanged("SortId");
                }
            }
        }

        public List<Account> AccountsSorted
        {
            get { return _Accounts.OrderBy(a => a.SortId).ToList(); }
        }

        #endregion

        #region Relationships

        [Association(Storage = "_Accounts", ThisKey = "GroupId", OtherKey = "GroupId")]
        public EntitySet<Account> Accounts
        {
            get { return _Accounts; }
            set { _Accounts.Assign(value); }
        }

        private void attach_Accounts(Account entity)
        {
            SendPropertyChanging();
            entity.Group = this;
        }

        private void detach_Accounts(Account entity)
        {
            SendPropertyChanging();
            entity.Group = null;
        }

        #endregion

        #region Property Event Handlers

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void SendPropertyChanging()
        {
            if ((PropertyChanging != null))
            {
                PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((PropertyChanged != null))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table(Name = "Accounts")]
    public class Account : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public Account()
        {
            _Transactions = new EntitySet<Transaction>(attach_Transactions, detach_Transactions);
            _Groups = default(EntityRef<Group>);
        }

        #region Private Declarations

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs =
            new PropertyChangingEventArgs(String.Empty);

        private readonly EntitySet<Transaction> _Transactions;
        private decimal _AccountBalance;

        private decimal _AccountBalanceCleared;

        private int _AccountId;

        private string _AccountName;

        private short _AccountType;

        private bool? _Active;

        private int _GroupId;

        private EntityRef<Group> _Groups;

        private string _SortId;

        #endregion

        #region Columns

        [Column(IsVersion = true)]
        private Binary _version;

        [Column(Storage = "_AccountId", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int AccountId
        {
            get { return _AccountId; }
            set
            {
                if ((_AccountId != value))
                {
                    SendPropertyChanging();
                    _AccountId = value;
                    SendPropertyChanged("AccountId");
                }
            }
        }

        [Column(Storage = "_AccountName", DbType = "nvarchar(30)", CanBeNull = false)]
        public string AccountName
        {
            get { return _AccountName; }
            set
            {
                if ((_AccountName != value))
                {
                    SendPropertyChanging();
                    _AccountName = value;
                    SendPropertyChanged("AccountName");
                }
            }
        }

        [Column(Storage = "_AccountType")]
        public short AccountType
        {
            get { return _AccountType; }
            set
            {
                if ((_AccountType != value))
                {
                    SendPropertyChanging();
                    _AccountType = value;
                    SendPropertyChanged("AccountType");
                }
            }
        }

        [Column(Storage = "_AccountBalance", DbType = "Decimal(19,2)")]
        public decimal AccountBalance
        {
            get
            {
                return _Transactions.Sum(t => t.TransAmount);

                //decimal deposits = this._Transactions
                //                                .Where(t => t.TransType == (int)TransType.Deposit)
                //                                .Sum(t => t.TransAmount);

                //decimal withdrawals = this._Transactions
                //                                .Where(t => t.TransType == (int)TransType.Withdrawal)
                //                                .Sum(t => t.TransAmount);
                //return (deposits - withdrawals);
            }
            set { }
        }

        [Column(Storage = "_AccountBalanceCleared", DbType = "Decimal(19,2)")]
        public decimal AccountBalanceCleared
        {
            get
            {
                return _Transactions
                    .Where(t => t.Cleared)
                    .Sum(t => t.TransAmount);

                //decimal deposits = this._Transactions
                //                                .Where(t => t.TransType == (int)TransType.Deposit)
                //                                .Where(t => t.Cleared == true)
                //                                .Sum(t => t.TransAmount);

                //decimal withdrawals = this._Transactions
                //                                .Where(t => t.TransType == (int)TransType.Withdrawal)
                //                                .Where(t => t.Cleared == true)
                //                                .Sum(t => t.TransAmount);
                //return (deposits - withdrawals);
            }
        }

        [Column(Storage = "_Active")]
        public bool? Active
        {
            get { return _Active; }
            set
            {
                if ((_Active != value))
                {
                    SendPropertyChanging();
                    _Active = value;
                    SendPropertyChanged("Active");
                }
            }
        }

        [Column(Storage = "_GroupId")]
        public int GroupId
        {
            get { return _GroupId; }
            set
            {
                if ((_GroupId != value))
                {
                    if (_Groups.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    SendPropertyChanging();
                    _GroupId = value;
                    SendPropertyChanged("GroupId");
                }
            }
        }

        [Column(Storage = "_SortId", CanBeNull = true)]
        public string SortId
        {
            get { return _SortId; }
            set
            {
                if ((_SortId != value))
                {
                    SendPropertyChanging();
                    _SortId = value;
                    SendPropertyChanged("SortId");
                }
            }
        }

        #endregion

        #region Relationships

        [Association(Storage = "_Transactions", ThisKey = "AccountId", OtherKey = "AccountId")]
        public EntitySet<Transaction> Transactions
        {
            get { return _Transactions; }
            set { _Transactions.Assign(value); }
        }

        [Association(Storage = "_Groups", ThisKey = "GroupId", OtherKey = "GroupId", IsForeignKey = true)]
        public Group Group
        {
            get { return _Groups.Entity; }
            set
            {
                Group previousValue = _Groups.Entity;
                if (((previousValue != value)
                     || (_Groups.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _Groups.Entity = null;
                        previousValue.Accounts.Remove(this);
                    }
                    _Groups.Entity = value;
                    if ((value != null))
                    {
                        value.Accounts.Add(this);
                        _GroupId = value.GroupId;
                    }
                    else
                    {
                        _GroupId = default(int);
                    }
                    SendPropertyChanged("Group");
                }
            }
        }

        private void attach_Transactions(Transaction entity)
        {
            SendPropertyChanging();
            entity.Account = this;
        }

        private void detach_Transactions(Transaction entity)
        {
            SendPropertyChanging();
            entity.Account = null;
        }

        #endregion

        #region Property Event Handlers

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void SendPropertyChanging()
        {
            if ((PropertyChanging != null))
            {
                PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((PropertyChanged != null))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        public override string ToString()
        {
            return AccountName;
        }
    }

    [Table(Name = "Transactions")]
    public class Transaction : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public Transaction()
        {
            _Images = new EntitySet<Images>(attach_Images, detach_Images);
            _Categories = default(EntityRef<Category>);
            _Payee = default(EntityRef<Payee>);
            _Accounts = default(EntityRef<Account>);
            _SourceTrans = default(EntityRef<Transfer>);
            _DestTrans = default(EntityRef<Transfer>);
        }

        #region Private Declarations

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs =
            new PropertyChangingEventArgs(String.Empty);

        private readonly EntitySet<Images> _Images;

        private int _AccountId;
        private EntityRef<Account> _Accounts;

        private EntityRef<Category> _Categories;
        private int? _CategoryId;
        private bool _Cleared;
        private EntityRef<Transfer> _DestTrans;
        private string _Memo;

        private EntityRef<Payee> _Payee;
        private int _PayeeId;
        private int? _RefNum;

        private EntityRef<Transfer> _SourceTrans;
        private decimal _TransAmount;
        private DateTime _TransDate;
        private int _TransId;
        private short _TransType;

        #endregion

        #region Columns

        [Column(IsVersion = true)]
        private Binary _version;

        [Column(Storage = "_TransId", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int TransId
        {
            get { return _TransId; }
            set
            {
                if ((_TransId != value))
                {
                    SendPropertyChanging();
                    _TransId = value;
                    SendPropertyChanged("TransId");
                }
            }
        }

        [Column(Storage = "_TransDate")]
        public DateTime TransDate
        {
            get { return _TransDate; }
            set
            {
                if ((_TransDate != value))
                {
                    SendPropertyChanging();
                    _TransDate = value;
                    SendPropertyChanged("TransDate");
                }
            }
        }

        [Column(Storage = "_TransAmount", DbType = "Decimal(19,2)")]
        public decimal TransAmount
        {
            get { return _TransAmount; }
            set
            {
                if ((_TransAmount != value))
                {
                    SendPropertyChanging();
                    _TransAmount = value;
                    SendPropertyChanged("TransAmount");
                }
            }
        }

        [Column(Storage = "_TransType")]
        public short TransType
        {
            get { return _TransType; }
            set
            {
                if ((_TransType != value))
                {
                    SendPropertyChanging();
                    _TransType = value;
                    SendPropertyChanged("TransType");
                }
            }
        }

        [Column(Storage = "_RefNum")]
        public int? RefNum
        {
            get { return _RefNum; }
            set
            {
                if ((_RefNum != value))
                {
                    SendPropertyChanging();
                    _RefNum = value;
                    SendPropertyChanged("RefNum");
                }
            }
        }

        [Column(Storage = "_Memo", DbType = "NVarChar(100)")]
        public string Memo
        {
            get { return _Memo; }
            set
            {
                if ((_Memo != value))
                {
                    SendPropertyChanging();
                    _Memo = value;
                    SendPropertyChanged("Memo");
                }
            }
        }

        [Column(Storage = "_PayeeId")]
        public int PayeeId
        {
            get { return _PayeeId; }
            set
            {
                if ((_PayeeId != value))
                {
                    SendPropertyChanging();
                    _PayeeId = value;
                    SendPropertyChanged("PayeeId");
                }
            }
        }

        [Column(Storage = "_CategoryId")]
        public int? CategoryId
        {
            get { return _CategoryId; }
            set
            {
                if ((_CategoryId != value))
                {
                    if (_Categories.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    SendPropertyChanging();
                    _CategoryId = value;
                    SendPropertyChanged("CategoryId");
                }
            }
        }

        [Column(Storage = "_AccountId")]
        public int AccountId
        {
            get { return _AccountId; }
            set
            {
                if ((_AccountId != value))
                {
                    if (_Accounts.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    SendPropertyChanging();
                    _AccountId = value;
                    SendPropertyChanged("AccountId");
                }
            }
        }

        [Column(Storage = "_Cleared")]
        public bool Cleared
        {
            get { return _Cleared; }
            set
            {
                if ((_Cleared != value))
                {
                    SendPropertyChanging();
                    _Cleared = value;
                    SendPropertyChanged("Cleared");
                }
            }
        }

        [Association(Name = "Transaction_Transfer0", Storage = "_SourceTrans", ThisKey = "TransId",
            OtherKey = "TransferId", IsUnique = true, IsForeignKey = false)]
        public Transfer SourceTrans
        {
            get { return _SourceTrans.Entity; }
            set
            {
                Transfer previousValue = _SourceTrans.Entity;
                if (((previousValue != value)
                     || (_SourceTrans.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _SourceTrans.Entity = null;
                        previousValue.SourceTransactions = null;
                    }
                    _SourceTrans.Entity = value;
                    if ((value != null))
                    {
                        value.SourceTransactions = this;
                    }
                    SendPropertyChanged("SourceTrans");
                }
            }
        }

        [Association(Name = "Transaction_Transfer1", Storage = "_DestTrans", ThisKey = "TransId",
            OtherKey = "TransferId", IsUnique = true, IsForeignKey = false)]
        public Transfer DestTrans
        {
            get { return _DestTrans.Entity; }
            set
            {
                Transfer previousValue = _DestTrans.Entity;
                if (((previousValue != value)
                     || (_DestTrans.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _DestTrans.Entity = null;
                        previousValue.DestTransactions = null;
                    }
                    _DestTrans.Entity = value;
                    if ((value != null))
                    {
                        value.DestTransactions = this;
                    }
                    SendPropertyChanged("DestTrans");
                }
            }
        }

        #endregion

        #region Relationships

        [Association(Storage = "_Images", ThisKey = "TransId", OtherKey = "TransId")]
        public EntitySet<Images> Images
        {
            get { return _Images; }
            set { _Images.Assign(value); }
        }

        [Association(Storage = "_Categories", ThisKey = "CategoryId", OtherKey = "CategoryId", IsForeignKey = true)]
        public Category Category
        {
            get { return _Categories.Entity; }
            set
            {
                Category previousValue = _Categories.Entity;
                if (((previousValue != value)
                     || (_Categories.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _Categories.Entity = null;
                        previousValue.Transactions.Remove(this);
                    }
                    _Categories.Entity = value;
                    if ((value != null))
                    {
                        value.Transactions.Add(this);
                        _CategoryId = value.CategoryId;
                    }
                    else
                    {
                        _CategoryId = default(int?);
                    }
                    SendPropertyChanged("Category");
                }
            }
        }

        [Association(Storage = "_Payee", ThisKey = "PayeeId", OtherKey = "PayeeId", IsForeignKey = true)]
        public Payee Payee
        {
            get { return _Payee.Entity; }
            set
            {
                Payee previousValue = _Payee.Entity;
                if (((previousValue != value)
                     || (_Payee.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _Payee.Entity = null;
                        previousValue.Transactions.Remove(this);
                    }
                    _Payee.Entity = value;
                    if ((value != null))
                    {
                        value.Transactions.Add(this);
                        _PayeeId = value.PayeeId;
                    }
                    else
                    {
                        _PayeeId = default(int);
                    }
                    SendPropertyChanged("Payee");
                }
            }
        }

        [Association(Storage = "_Accounts", ThisKey = "AccountId", OtherKey = "AccountId", IsForeignKey = true)]
        public Account Account
        {
            get { return _Accounts.Entity; }
            set
            {
                Account previousValue = _Accounts.Entity;
                if (((previousValue != value)
                     || (_Accounts.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _Accounts.Entity = null;
                        previousValue.Transactions.Remove(this);
                    }
                    _Accounts.Entity = value;
                    if ((value != null))
                    {
                        value.Transactions.Add(this);
                        _AccountId = value.AccountId;
                    }
                    else
                    {
                        _AccountId = default(int);
                    }
                    SendPropertyChanged("Account");
                }
            }
        }

        private void attach_Images(Images entity)
        {
            SendPropertyChanging();
            entity.Transaction = this;
        }

        private void detach_Images(Images entity)
        {
            SendPropertyChanging();
            entity.Transaction = null;
        }

        #endregion

        #region Property Event Handlers

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void SendPropertyChanging()
        {
            if ((PropertyChanging != null))
            {
                PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((PropertyChanged != null))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table(Name = "Categories")]
    public class Category : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public Category()
        {
            _Transactions = new EntitySet<Transaction>(attach_Transactions, detach_Transactions);
        }

        #region Private Declarations

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs =
            new PropertyChangingEventArgs(String.Empty);

        private readonly EntitySet<Transaction> _Transactions;
        private bool? _Active;

        private int _CategoryId;

        private string _CategoryName;

        #endregion

        #region Columns

        [Column(IsVersion = true)]
        private Binary _version;

        [Column(Storage = "_CategoryId", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int CategoryId
        {
            get { return _CategoryId; }
            set
            {
                if ((_CategoryId != value))
                {
                    SendPropertyChanging();
                    _CategoryId = value;
                    SendPropertyChanged("CategoryId");
                }
            }
        }

        [Column(Storage = "_CategoryName", DbType = "nvarchar(30)", CanBeNull = false)]
        public string CategoryName
        {
            get { return _CategoryName; }
            set
            {
                if ((_CategoryName != value))
                {
                    SendPropertyChanging();
                    _CategoryName = value;
                    SendPropertyChanged("CategoryName");
                }
            }
        }

        [Column(Storage = "_Active")]
        public bool? Active
        {
            get { return _Active; }
            set
            {
                if ((_Active != value))
                {
                    SendPropertyChanging();
                    _Active = value;
                    SendPropertyChanged("Active");
                }
            }
        }

        #endregion

        #region Relationships

        [Association(Storage = "_Transactions", ThisKey = "CategoryId", OtherKey = "CategoryId")]
        public EntitySet<Transaction> Transactions
        {
            get { return _Transactions; }
            set { _Transactions.Assign(value); }
        }

        private void attach_Transactions(Transaction entity)
        {
            SendPropertyChanging();
            entity.Category = this;
        }

        private void detach_Transactions(Transaction entity)
        {
            SendPropertyChanging();
            entity.Category = null;
        }

        #endregion

        #region Property Event Handlers

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void SendPropertyChanging()
        {
            if ((PropertyChanging != null))
            {
                PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((PropertyChanged != null))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table(Name = "Payees")]
    public class Payee : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public Payee()
        {
            _Transactions = new EntitySet<Transaction>(attach_Transactions, detach_Transactions);
        }

        #region Private Declarations

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs =
            new PropertyChangingEventArgs(String.Empty);

        private readonly EntitySet<Transaction> _Transactions;
        private bool? _Active;

        private int _PayeeId;

        private string _PayeeName;

        #endregion

        #region Columns

        [Column(IsVersion = true)]
        private Binary _version;

        [Column(Storage = "_PayeeId", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int PayeeId
        {
            get { return _PayeeId; }
            set
            {
                if ((_PayeeId != value))
                {
                    SendPropertyChanging();
                    _PayeeId = value;
                    SendPropertyChanged("PayeeId");
                }
            }
        }

        [Column(Storage = "_PayeeName", DbType = "nvarchar(30)", CanBeNull = false)]
        public string PayeeName
        {
            get { return _PayeeName; }
            set
            {
                if ((_PayeeName != value))
                {
                    SendPropertyChanging();
                    _PayeeName = value;
                    SendPropertyChanged("PayeeName");
                }
            }
        }

        [Column(Storage = "_Active")]
        public bool? Active
        {
            get { return _Active; }
            set
            {
                if ((_Active != value))
                {
                    SendPropertyChanging();
                    _Active = value;
                    SendPropertyChanged("Active");
                }
            }
        }

        #endregion

        #region Relationships

        [Association(Storage = "_Transactions", ThisKey = "PayeeId", OtherKey = "PayeeId")]
        public EntitySet<Transaction> Transactions
        {
            get { return _Transactions; }
            set { _Transactions.Assign(value); }
        }

        private void attach_Transactions(Transaction entity)
        {
            SendPropertyChanging();
            entity.Payee = this;
        }

        private void detach_Transactions(Transaction entity)
        {
            SendPropertyChanging();
            entity.Payee = null;
        }

        #endregion

        #region Property Event Handlers

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void SendPropertyChanging()
        {
            if ((PropertyChanging != null))
            {
                PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((PropertyChanged != null))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table(Name = "Images")]
    public class Images : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public Images()
        {
            _Transaction = default(EntityRef<Transaction>);
        }

        #region Private Declarations

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs =
            new PropertyChangingEventArgs(String.Empty);

        private string _Image;
        private int _ImageId;

        private string _ImageName;

        private int _TransId;

        private EntityRef<Transaction> _Transaction;

        #endregion

        #region Columns

        [Column(Storage = "_ImageId", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int ImageId
        {
            get { return _ImageId; }
            set
            {
                if ((_ImageId != value))
                {
                    SendPropertyChanging();
                    _ImageId = value;
                    SendPropertyChanged("ImageId");
                }
            }
        }

        [Column(Storage = "_Image", DbType = "NVarchar(100)", CanBeNull = false, UpdateCheck = UpdateCheck.Never)]
        public string Image
        {
            get { return _Image; }
            set
            {
                if ((_Image != value))
                {
                    SendPropertyChanging();
                    _Image = value;
                    SendPropertyChanged("Image");
                }
            }
        }

        [Column(Storage = "_ImageName", DbType = "NVarchar(100)", CanBeNull = false)]
        public string ImageName
        {
            get { return _ImageName; }
            set
            {
                if ((_ImageName != value))
                {
                    SendPropertyChanging();
                    _ImageName = value;
                    SendPropertyChanged("ImageName");
                }
            }
        }

        //public ImageSource ImageSource { get; set; }

        [Column(Storage = "_TransId")]
        public int TransId
        {
            get { return _TransId; }
            set
            {
                if ((_TransId != value))
                {
                    if (_Transaction.HasLoadedOrAssignedValue)
                    {
                        throw new ForeignKeyReferenceAlreadyHasValueException();
                    }
                    SendPropertyChanging();
                    _TransId = value;
                    SendPropertyChanged("TransId");
                }
            }
        }

        #endregion

        #region Relationships

        [Association(Storage = "_Transaction", ThisKey = "TransId", OtherKey = "TransId", IsForeignKey = true)]
        public Transaction Transaction
        {
            get { return _Transaction.Entity; }
            set
            {
                Transaction previousValue = _Transaction.Entity;
                if (((previousValue != value)
                     || (_Transaction.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _Transaction.Entity = null;
                        previousValue.Images.Remove(this);
                    }
                    _Transaction.Entity = value;
                    if ((value != null))
                    {
                        value.Images.Add(this);
                        _ImageId = value.TransId;
                    }
                    else
                    {
                        _ImageId = default(int);
                    }
                    SendPropertyChanged("Transaction");
                }
            }
        }

        #endregion

        #region Property Event Handlers

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void SendPropertyChanging()
        {
            if ((PropertyChanging != null))
            {
                PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((PropertyChanged != null))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }

    [Table(Name = "Transfers")]
    public class Transfer : INotifyPropertyChanging, INotifyPropertyChanged
    {
        public Transfer()
        {
            _SourceTransactions = default(EntityRef<Transaction>);
            _DestTransactions = default(EntityRef<Transaction>);
        }

        #region Private Declarations

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs =
            new PropertyChangingEventArgs(String.Empty);

        private int? _DestTransId;

        private EntityRef<Transaction> _DestTransactions;
        private int? _SourceTransId;
        private EntityRef<Transaction> _SourceTransactions;
        private int _TransferId;

        #endregion

        #region Columns

        [Column(IsVersion = true)]
        private Binary _version;

        [Column(Storage = "_TransferId", IsPrimaryKey = true, IsDbGenerated = true, DbType = "INT NOT NULL Identity",
            CanBeNull = false)]
        public int TransferId
        {
            get { return _TransferId; }
            set
            {
                if ((_TransferId != value))
                {
                    SendPropertyChanging();
                    _TransferId = value;
                    SendPropertyChanged("TransferId");
                }
            }
        }

        [Column(Storage = "_SourceTransId")]
        public int? SourceTransId
        {
            get { return _SourceTransId; }
            set
            {
                if ((_SourceTransId != value))
                {
                    SendPropertyChanging();
                    _SourceTransId = value;
                    SendPropertyChanged("SourceTransId");
                }
            }
        }

        [Column(Storage = "_DestTransId")]
        public int? DestTransId
        {
            get { return _DestTransId; }
            set
            {
                if ((_DestTransId != value))
                {
                    SendPropertyChanging();
                    _DestTransId = value;
                    SendPropertyChanged("DestTransId");
                }
            }
        }

        #endregion

        #region Relationships

        [Association(Name = "Transfer_Transaction0", Storage = "_SourceTransactions", ThisKey = "SourceTransId",
            OtherKey = "TransId", IsForeignKey = true)]
        public Transaction SourceTransactions
        {
            get { return _SourceTransactions.Entity; }
            set
            {
                Transaction previousValue = _SourceTransactions.Entity;
                if (((previousValue != value)
                     || (_SourceTransactions.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _SourceTransactions.Entity = null;
                        previousValue.SourceTrans = null;
                    }
                    _SourceTransactions.Entity = value;
                    if ((value != null))
                    {
                        value.SourceTrans = this;
                    }
                    SendPropertyChanged("SourceTransactions");
                }
            }
        }

        [Association(Name = "Transfer_Transaction1", Storage = "_DestTransactions", ThisKey = "DestTransId",
            OtherKey = "TransId", IsForeignKey = true)]
        public Transaction DestTransactions
        {
            get { return _DestTransactions.Entity; }
            set
            {
                Transaction previousValue = _DestTransactions.Entity;
                if (((previousValue != value)
                     || (_DestTransactions.HasLoadedOrAssignedValue == false)))
                {
                    SendPropertyChanging();
                    if ((previousValue != null))
                    {
                        _DestTransactions.Entity = null;
                        previousValue.DestTrans = null;
                    }
                    _DestTransactions.Entity = value;
                    if ((value != null))
                    {
                        value.DestTrans = this;
                    }
                    SendPropertyChanged("DestTransactions");
                }
            }
        }

        #endregion

        #region Property Event Handlers

        public event PropertyChangedEventHandler PropertyChanged;
        public event PropertyChangingEventHandler PropertyChanging;

        protected virtual void SendPropertyChanging()
        {
            if ((PropertyChanging != null))
            {
                PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((PropertyChanged != null))
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}