using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.Util.Command;
using WpfApp1.Util.Extension_Method;
using WpfApp1.View;

namespace WpfApp1.ViewModel
{
    public class MateriaViewModel : BaseViewModel
    {
        #region -   Initialize  -

        private ContactContext Context;
        ObservableCollection<Model.Materail> C_Materail;
        List<int> IDM=new List<int>();

        #region -   Constructor    -
        public MateriaViewModel()
        {

            Context = new ContactContext();
            _CollecMaterail = new ObservableCollection<Model.Materail>();
            _ListItemOfType = new ObservableCollection<CheckedBoxType>();
            _ListBill = new ObservableCollection<Model.Bill>();
            _mymaterails = new ObservableCollection<JustMaterail>();
            FillListItemToType();
            FillMaterial();
            FillJustMaterail();
           // FillBill();
            FillBillMaterail();
        }
        #endregion

        #endregion

        #region -   Command    -
        private ICommand _CommandAddToChake;
        public ICommand CommandAddToChake
        {

            get {
                return _CommandAddToChake = new Command(() =>
                {

                    _CollecMaterail = C_Materail;
                    List<int> IdsType = new List<int>();
                    foreach (var item in ListItemOfType)
                    {
                        if (item.IsChecked)
                            IdsType.Add(item.Id);
                    }


                    ObservableCollection<Model.Materail> NewList = new ObservableCollection<Model.Materail>();
                    foreach (var materail in CollecMaterail)
                    {
                        if (IdsType.Contains(materail.type.Id))
                        {
                            NewList.Add(materail);
                        }
                        _CollecMaterail = NewList;
                        OnPropertyChanged(nameof(CollecMaterail));
                    }

                }); }
            set { _CommandAddToChake = value; }
        }
        private ICommand _CommandDelete;
        public ICommand CommandDelete
        {
            get {
                return _CommandDelete = new CommandT<int>(id =>
            {

                var One = CollecMaterail.Where(item => item.Id == id).FirstOrDefault();
                _CollecMaterail.Remove(One);
                C_Materail.Remove(One);
                Context.Materails.Find(id).Isdelete = true;
                Context.SaveChanges();
                return;

            }); }
            set { _CommandDelete = value; }
        }

        private ICommand _CommandUpdate;
        public ICommand CommandUpdate
        {
            get
            {
                return _CommandUpdate = new CommandT<int>(IdUpdate =>
                  {
                      foreach (var matupdate in _CollecMaterail)
                      {
                          if (matupdate.Id == IdUpdate)
                          {
                              var s = _CollecMaterail.Single(m => m.Id == IdUpdate);
                              Materail IdS = Context.Materails.Single(M => M.Id == IdUpdate);
                              IdS.Name = s.Name;
                              IdS.Price = s.Price;
                              Context.SaveChanges();
                              MessageBox.Show("Done");
                              return;

                          }
                      }
                  }); }
            set { _CommandUpdate = value; }
        }

        private ICommand _CommandAdd;
        public ICommand CommandAdd
        {
            get
            {
                return _CommandAdd = new Command(() =>
                 {
                     Materail s = new Materail();
                     s.Name = _NameMaterial;
                     s.Price = _price;
                     s.Isdelete = false;
                     s.TypeId = _selecttype.Value;
                     Context.Materails.Add(s);
                     Context.SaveChanges();
                     MessageBox.Show("Done");
                     FillMaterial();
                     ClrM();
                     _IsOpenAdd = false;
                     OnPropertyChanged(nameof(IsOpenAdd));
                 }); }
            set { _CommandAdd = value; }
        }
        private ICommand _CommandAddType;
        public ICommand CommandAddType
        {
            get
            {
                return _CommandAddType = new Command(() =>
                {
                   Model.Type SetType = new Model.Type()
                   {
                       Name = _NameType
                   };
                   Context.Types.Add(SetType);

                   Context.SaveChanges();
                   FillListItemToType();
                   OnPropertyChanged(nameof(ListItemOfType));


               });
            }
            set { _CommandAddType = value; }
        }

        private ICommand _CommandOpenAdd;
        public ICommand CommandOpenAdd
        {
            get { return _CommandOpenAdd = new Command(() =>
              {
                  FillListItemToType();
                  _IsOpenAdd = true;

                  OnPropertyChanged(nameof(IsOpenAdd));
              }); }
            set { _CommandOpenAdd = value; }
        }
        private ICommand _CommandCloseAdd;
        public ICommand CommandCloseAdd
        {
            get
            {
                return _CommandCloseAdd = new Command(() =>
                {
                    ClrM();
                    _IsOpenAdd = false;
                    FillListItemToType();

                    OnPropertyChanged(nameof(IsOpenAdd));
                });
            }
            set { _CommandCloseAdd = value; }
        }
        private ICommand _CommandGoType;
        public ICommand CommandGoType
        {
            get
            {
                return _CommandGoType = new Command(() =>
                {
                    _IsOpenAdd = false;
                    OnPropertyChanged(nameof(IsOpenAdd));
                    Windowtype windowtype = new Windowtype();
                    windowtype.ShowDialog();

                });
            }
            set { _CommandGoType = value; }
        }
        private ICommand _Commandplusb;
        public ICommand Commandplusb
        {
            get
            {
                return _Commandplusb = new CommandT<int>(MaterialId =>
                {
                    IDM.Add(MaterialId);
                    _itembuy += 1;
                    OnPropertyChanged(nameof(itembuy));

          
                });
            }
            set { _Commandplusb = value; }
        }

        private ICommand _CommandToBill;
        public ICommand CommandToBill
        {
            get
            {
                return _CommandToBill = new Command(() =>
                {
                    var getListMaterail = _CollecMaterail.Where(m => IDM.Contains(m.Id));
                    _mymaterails.Clear();
                    _CountBill = _ListBill.Select(s => s.Number).Max() + 1;
                    OnPropertyChanged(nameof(CountBill));
                    foreach (var item in getListMaterail)
                    {
                        var SetJustMaterail = new JustMaterail()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Price = item.Price,
                           
                        };
                        _mymaterails.Add(SetJustMaterail);
                        OnPropertyChanged(nameof(mymaterails));
 
                    }
                    foreach (var it in IDM)
                        {
           
                        if (IDM.Contains(it))
                            {
                            var lm = _mymaterails.SingleOrDefault(m => m.Id == it);
                                lm.Qantity += 1;
                            lm.Price = lm.Price * lm.Qantity;
                            var d = _ListBillMaterail.FirstOrDefault(m => m.MaterailId == it);
                            d.Bill.Totile = _mymaterails.Sum(n => n.Price );
                            _totlPrice = d.Bill.Totile;
                            OnPropertyChanged(nameof(totlPrice));
                            _ListBillMaterail = _ListBillMaterail.ToObservableCollection();
                            OnPropertyChanged(nameof(ListBillMaterail));
                        }
                        }




                    WindowBill windowBill = new WindowBill() {
                        DataContext = this,
                    };
                    windowBill.ShowDialog();
                    _itembuy = 0;
                    OnPropertyChanged(nameof(itembuy));
                    IDM.Clear();
                    // _mymaterails.ToList().AddRange(
                    //     _CollecMaterail.Where(m=>IDM.Contains(m.Id)).Select(m=>new JustMaterail() { 

                    //     })
                    //     );

                });
            }
            set { _CommandToBill = value; }
        }
        private ICommand _CommandAddBill;
        public ICommand CommandAddBill
        {
            get
            {
                return _CommandAddBill = new Command(() =>
                {
                    var SetBill = new Bill()
                    {
                        DateOut=DateTime.Now,
                        Number = _CountBill,
                        Name = _NameBill,
                        Totile = _mymaterails.Sum(m=>m.Price),
                        billMaterails= _mymaterails.Select(m=> new BillMaterail()
                        {
                            MaterailId=m.Id,
                            Count=(int)m.Qantity,
                        }).ToList()
                    };
                    Context.Bills.Add(SetBill);
                    Context.SaveChanges();
                    ClrB();
                    mymaterails.Clear();

                });
            }
            set { _CommandAddBill = value; }
        }
        private ICommand _CommandGoBill;
        public ICommand CommandGoBill
        {
            get
            {
                return _CommandGoBill = new Command(() =>
                {
                    FillBill();
                    WindowViewBill window = new WindowViewBill();
                    window.Show();

                });
            }
            set { _CommandGoBill = value; }
        }
        private ICommand _CommandSearch;
        public ICommand CommandSearch
        {
            get
            {
                return _CommandSearch = new Command(() =>
                {
                          var search=from p in Context.Bills 
                                 where p.Name.Contains(_NameBill)
                                select p;
                    _ListBill = search.ToObservableCollection();
                    OnPropertyChanged(nameof(ListBill));
                });
            }
            set { _CommandSearch = value; }
        }
        private ICommand _CommandDeleteBill;
        public ICommand CommandDeleteBill
        {
            get
            {
                return _CommandDeleteBill = new CommandT<int>(id =>
                {
                    var Dbill = _ListBill.Where(item => item.Id == id).FirstOrDefault();
                    _ListBill.Remove(Dbill);
                    Context.Bills.Find(id).Isdelete = true;
                    Context.SaveChanges();
                });
            }
            set { _CommandDeleteBill = value; }
        }
        private ICommand _CommandRest;
        public ICommand CommandRest
        {
            get
            {
                return _CommandRest = new Command(() =>
                {
                    _itembuy = 0;
                    IDM.Clear();
                    OnPropertyChanged(nameof(itembuy));

                });
            }
            set { _CommandRest = value; }
        }
        private ICommand _CommandPlusOne;
        public ICommand CommandPlusOne
        {
            get
            {
                return _CommandPlusOne = new CommandT<int>(num =>
                {
                    FillJustMaterail();
                    var s = _mymaterails.Single(m=>m.Id==num);  
                    s.Qantity = s.Qantity + 1;
                    var v = _CollecMaterail.FirstOrDefault(m => m.Id == num);
                    s.Price = v.Price * s.Qantity;
                    var d = _ListBillMaterail.FirstOrDefault(m => m.MaterailId == num);
                    d.Bill.Totile = _mymaterails.Sum(n => n.Price);
                    _totlPrice = d.Bill.Totile;
                    OnPropertyChanged(nameof(totlPrice));
                    _mymaterails = _mymaterails.ToObservableCollection();
                    OnPropertyChanged(nameof(mymaterails));
                    _ListBillMaterail = _ListBillMaterail.ToObservableCollection();
                    OnPropertyChanged(nameof(ListBillMaterail));

                });
            }
            set { _CommandPlusOne = value; }
        }
        private ICommand _CommandMinusOne;
        public ICommand CommandMinusOne
        {
            get
            {
                return _CommandMinusOne = new CommandT<int>(num =>
                {
                FillJustMaterail();
                    var s = _mymaterails.Single(m => m.Id == num);
                    s.Qantity = s.Qantity - 1;
                    var v = _CollecMaterail.FirstOrDefault(m => m.Id == num);
                    s.Price = v.Price * s.Qantity;
                     var d = _ListBillMaterail.FirstOrDefault(m=>m.MaterailId==num);
                    d.Bill.Totile =_mymaterails.Sum(n=>n.Price);
                    _totlPrice = d.Bill.Totile;
                    OnPropertyChanged(nameof(totlPrice));
                    _mymaterails = _mymaterails.ToObservableCollection();
                    OnPropertyChanged(nameof(mymaterails));
                    _ListBillMaterail = _ListBillMaterail.ToObservableCollection();
                    OnPropertyChanged(nameof(ListBillMaterail));

                });
            }
            set { _CommandMinusOne = value; }
        }
        private ICommand _Commandclosbill;
        public ICommand Commandclosbill
        {
            get
            {
                return _Commandclosbill = new Command(() =>
                {
                    _IsOpenBill = false;
                    OnPropertyChanged(nameof(IsOpenBill));

                });
            }
            set { _Commandclosbill = value; }
        }
        private ICommand _CommandShowAllB;
        public ICommand CommandShowAllB
        {
            get
            {
                return _CommandShowAllB = new CommandT<int>(id =>
                { 
                    var se = _ListBillMaterail.Where(e => e.BillId == id).Select(n => new JustMaterail()
                  { Id=n.Id,
                  Name=n.materail.Name,
                  Price=n.materail.Price,
                  Qantity=n.Count});
                    _mymaterails = se.ToObservableCollection();
                    OnPropertyChanged(nameof(mymaterails));
                    _IsOpenBill = true;
                    OnPropertyChanged(nameof(IsOpenBill));

                });
            }

            set { _CommandShowAllB = value; }
        }
        #endregion

        #region -   Action  -



        #endregion

        #region -   CallTask    -


        #endregion

        #region -   Method  -
        private void FillListItemToType()
        { 
            _ListItemOfType = Context.Types.Select(t => new CheckedBoxType()
            {
                Id = t.Id,
                Name = t.Name,
                IsChecked = true,
            }).ToObservableCollection();
            OnPropertyChanged(nameof(ListItemOfType));
        }
        private void FillMaterial()
        {
   
            _CollecMaterail = Context.Materails.
                Include(nameof(Model.Type)).
                Where(m => !m.Isdelete).
                ToObservableCollection();
            OnPropertyChanged(nameof(CollecMaterail));


            C_Materail = _CollecMaterail.ToObservableCollection();
        }
        private void FillBill()
        {
            _ListBill = Context.Bills.
                Where(m => !m.Isdelete).ToObservableCollection();
            OnPropertyChanged(nameof(ListBill));
        }
        private void FillBillMaterail()
        {

            _ListBillMaterail = Context.billMaterails.
                Where(m => !m.Isdelete).ToObservableCollection();
            OnPropertyChanged(nameof(ListBillMaterail));

        }
        private void FillJustMaterail ()
        {
            //_mymaterails = Context.Materails.Select(t => new JustMaterail()
            //{
            //    Id = t.Id,
            //    Name = t.Name,
            //    Price = t.Price,
            //    Qantity = _NumbMaterial
            //}).ToObservableCollection();
            //OnPropertyChanged(nameof(mymaterails));
        }
        private void ClrM()
        {
            _NameMaterial = "";
            _price = 0;
            _nameType = "";
            OnPropertyChanged(nameof(NameMaterial));
            OnPropertyChanged(nameof(price));
            OnPropertyChanged(nameof(nameType));
        }
        private void ClrB()
        {
            _NameBill = "";
            OnPropertyChanged(nameof(NameBill));
            _CountBill = 0;
            OnPropertyChanged(nameof(CountBill));
            _totlPrice = 0;
            OnPropertyChanged(nameof(totlPrice));
        }
        #endregion

        #region -   InnerClass  -
        public class CheckedBoxType
        {
            public int Id { get; set; }
            public string Name { get; set; }
            private bool _IsChecked;
            public bool IsChecked
            {
                get { return _IsChecked; }
                set { _IsChecked = value; }
            }
        }

        public class JustMaterail: BaseViewModel
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }

            private int _Qantity;
            public int Qantity
            {
                get { return _Qantity; }
                set { _Qantity = value;
                    //OnPropertyChanged();
                }
            }
        }


        #endregion

        #region -   Propertie   -

        #region -   List    -

        private ObservableCollection<JustMaterail> _mymaterails;
        public ObservableCollection<JustMaterail> mymaterails
        {
            get { return _mymaterails; }
            set
            {
                _mymaterails = value;
            }
        }


        private ObservableCollection<CheckedBoxType> _ListItemOfType;
        public ObservableCollection<CheckedBoxType> ListItemOfType
        {
            get { return _ListItemOfType; }
            set
            {
                _ListItemOfType = value;
            }
        }

        private ObservableCollection<Model.Materail> _CollecMaterail;
        public ObservableCollection<Model.Materail> CollecMaterail
        {
            get { return _CollecMaterail; }
            set{ _CollecMaterail = value;}
        }
        

        private ObservableCollection<Model.Bill> _ListBill;
        public ObservableCollection<Model.Bill> ListBill
        {
            get { return _ListBill; }
            set
            {
                _ListBill = value;
            }
        }
        private ObservableCollection<Model.BillMaterail> _ListBillMaterail;
        public ObservableCollection<Model.BillMaterail> ListBillMaterail
        {
            get { return _ListBillMaterail; }
            set
            {
                _ListBillMaterail = value;
            }
        }
        #endregion

        #region -   Object  -
        private int? _selecttype;
        public int? selecttype
        {
            get { return _selecttype; }
            set { _selecttype = value; }
        }
        #endregion

        #region -   String  -
        private string _name;
        public string name
        {
            get { return _name; }
            set { _name = value; }
        }


        private string _nameType;
        public string nameType
        {
            get { return _nameType; }
            set { _nameType = value; }
        }
        private string _namein;
        public string namein
        {
            get { return _namein; }
            set { _namein = value; }
        }

        private string _NameMaterial="";
        public string NameMaterial
        {
            get { return _NameMaterial; }
            set { _NameMaterial = value; }
        }

        private string _NameType;
        public string NameType
        {
            get { return _NameType; }
            set { _NameType = value; }
        }
        private string _NameBill;
        public string NameBill
        {
            get { return _NameBill; }
            set { _NameBill = value; }
        }

        #endregion

        #region -   Date    -

        private DateTime _OutBill;
        public DateTime OutBill
        {
            get { return _OutBill; }
            set { _OutBill = value; }
        }


        #endregion

        #region -   Number  -
        private double _price=0;
        public double price
        {
            get { return _price; }
            set { _price = value; }
        }

        private double _itembuy;
        public double itembuy
        {
            get { return _itembuy; }
            set { _itembuy = value; }
        }
        private int _CountBill;
        public int CountBill
        {
            get { return _CountBill; }
            set { _CountBill = value; }
        }
        private int _NumbMaterial;
        public int NumbMaterial
        {
            get { return _NumbMaterial; }
            set { _NumbMaterial = value; }
        }
        private double _totlPrice;
        public double totlPrice
        {
            get { return _totlPrice; }
            set { _totlPrice = value; }
        }
        #endregion

        #region -   Bool    -

        private bool _IsOpenAdd;
        public bool IsOpenAdd
        {
            get { return _IsOpenAdd; }
            set { _IsOpenAdd = value; }
        }
        private bool _IsOpenBill;
        public bool IsOpenBill
        {
            get { return _IsOpenBill; }
            set { _IsOpenBill = value; }
        }

        #endregion

        #endregion


    }
}
