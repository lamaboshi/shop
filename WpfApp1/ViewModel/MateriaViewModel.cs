using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
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
            _NumbMaterial=1;

            _CollecMaterail = new ObservableCollection<Model.Materail>();
            _ListItemOfType = new ObservableCollection<CheckedBoxType>();
            _ListBill = new ObservableCollection<Model.Bill>();
            _mymaterails = new ObservableCollection<JustMaterail>();
            FillListItemToType();
            FillMaterial();
            FillJustMaterail();
            FillBill();
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
                   MessageBox.Show(SetType.Id.ToString());
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
                    _IsOpenAdd = false;
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
                    
                    foreach (var item in getListMaterail)
                    {
                        var SetJustMaterail = new JustMaterail()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            Price = item.Price,
                            Qantity = 1
                        };
                        _mymaterails.Add(SetJustMaterail);
                    }
                    OnPropertyChanged(nameof(mymaterails));

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

                    ////TODO IDM.Clear();

                    // ObservableCollection<Model.Materail> NewOne = new ObservableCollection<Model.Materail>();

                    // foreach (var item in _CollecMaterail)
                    // {
                    //     if (IDM.Contains(item.Id))
                    //     {
                    //         NewOne.Add(item);

                    //     }
                    //     var newm = _mymaterails.Where(t => t.Id == item.Id).First();
                    //     var s = NewOne.Where(m => m.Id == item.Id).First();
                    //     newm.Name = s.Name;
                    //     newm.Price = s.Price;
                    //     newm.Price = _NumbMaterial;
                    //     OnPropertyChanged(nameof(mymaterails));
                    //     _itembuy = 0;
                    // }
                    // //////////////////////////////////BAD WAY///////////////////////////////////////////////////
                    // //_mymaterails = Context.Materails.Include(nameof(Model.Type)).Where(m => !m.Isdelete).ToObservableCollection();
                    // //OnPropertyChanged(nameof(mymaterails));
                    // //ObservableCollection<Model.Materail> NewOne= new ObservableCollection<Model.Materail>();

                    // //foreach (var item in _mymaterails)
                    // //{
                    // //    if (IDM.Contains(item.Id))
                    // //    {
                    // //        Materail FMI = Context.Materails.Single(M => M.Id == item.Id);
                    // //        NewOne.Add(FMI);
                    // //        MessageBox.Show("s");
                    // //    }

                    // //}
                    // //_mymaterails = NewOne;
                    // //OnPropertyChanged(nameof(mymaterails));
                    // WindowBill windowBill = new WindowBill();
                    // windowBill.Show();
                    // _CountBill += 1;
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
                            Count=m.Qantity,
                        }).ToList()
                    };

                
                    Context.Bills.Add(SetBill);
                    Context.SaveChanges();
                    mymaterails.Clear();
                 //   FillBill();

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
                    var search = _ListBill.Where(item => item.Name == _NameBill).FirstOrDefault();

                });
            }
            set { _CommandSearch = value; }
        }
        private ICommand _CommandDeleteBill;
        public ICommand CommandDeleteBill
        {
            get
            {
                return _CommandDeleteBill = new CommandT<int>(delete =>
                {
                    var Dbill = _ListBill.Where(item => item.Id == delete).FirstOrDefault();
                    _ListBill.Remove(Dbill);
                    Context.Bills.Find(delete).Isdelete = true;
                    Context.SaveChanges();
                });
            }
            set { _CommandDeleteBill = value; }
        }
        private ICommand _Commandrest;
        public ICommand Commandrest
        {
            get
            {
                return _Commandrest = new Command(() =>
                {
                    _itembuy = 0;
                    IDM= new List<int>();
                });
            }
            set { _CommandDeleteBill = value; }
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


            //C_Materail = _ListBill.ToObservableCollection();
        }
        private void FillBillMaterail()
        {

            _ListBillMaterail = Context.billMaterails.
                Where(m => !m.Isdelete).ToObservableCollection();
            OnPropertyChanged(nameof(ListBillMaterail));


            //C_Materail = _ListBill.ToObservableCollection();
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

        public class JustMaterail
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public double Price { get; set; }

            private int _Qantity;
            public int Qantity
            {
                get { return _Qantity; }
                set { _Qantity = value; }
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

        private string _NameMaterial;
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
        private double _price;
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
        private int _totlPrice;
        public int totlPrice
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


        #endregion

        #endregion


    }
}
