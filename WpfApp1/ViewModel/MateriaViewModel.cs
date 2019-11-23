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

namespace WpfApp1.ViewModel
{
    public class MateriaViewModel : BaseViewModel
    {
        #region -   Initialize  -

        private ContactContext Context;
        ObservableCollection<Model.Materail> C_Materail;

        #region -   Constructor    -
        public MateriaViewModel()
        {
            Context = new ContactContext();

            _CollecMaterail = new ObservableCollection<Model.Materail>();
            ////  الي كرمال عبي داتا للmodel
            //_ListTypeForMy = new ObservableCollection<Model.Type>();
            //كرمال عبي الداتا من لل class
            _ListItemOfType = new ObservableCollection<CheckedBoxType>();

            //GetAllInfoFromType();
            //GetAllInfoFromMatrial();
            FillListItemToType();
            FillMaterial();
            //FillStor();

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
                //foreach (var item in CollecMaterail.ToList())
                //{
                //    if (item.Id == id)
                //    {
                //        _CollecMaterail.Remove(item);
                //        C_Materail.Remove(item);
                //        Context.Materails.Find(id).Isdelete=true;
                //        Context.SaveChanges();
                //        return;
                //    }
                //}

                var One = CollecMaterail.Where(item=>item.Id==id).FirstOrDefault();
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
                             // IdS.type.Name = _nameType;
                              Context.SaveChanges();
                              
                              return;
                          
                          }
                      }
                  });}
            set{_CommandUpdate=value;}
        }

        private ICommand _CommandAdd;
        public ICommand CommandAdd
        {
            get
            {

                return _CommandAdd = new Command(() =>
                 {
                     _CollecMaterail.Add(new Materail() { Name = namein, Isdelete = false });
                     _ListItemOfType.Add(new CheckedBoxType() {Name=nameTypein });
                     Add(_CollecMaterail);
                 });

            }
            set { _CommandAdd = value; }
        }



        private ICommand _CommandOpenAdd;
        public ICommand CommandOpenAdd
        {
            get { return _CommandOpenAdd=new Command(() =>
            {
                _IsOpenAdd = true;
                OnPropertyChanged(nameof(IsOpenAdd));
            }); }
            set { _CommandOpenAdd = value; }
        }


        #endregion

        #region -   Action  -



        #endregion

        #region -   CallTask    -


        #endregion

        #region -   Method  -
        private void FillListItemToType()/////كرمال عبي ال list تيعت ال class
        {

            _ListItemOfType = Context.Types.Select(t => new CheckedBoxType()
            {
                Id = t.Id,
                Name = t.Name,
                IsChecked = true,
            }).
                ToObservableCollection();

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
        private void Add(ObservableCollection<Model.Materail> add)
        {

           // Context.Materails.FirstOrDefault(_CollecMaterail);
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


        #endregion

        #region -   Propertie   -

        #region -   List    -
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
        private ObservableCollection<Model.Store> _ListStore;
        public ObservableCollection<Model.Store> ListStore
        {
            get { return _ListStore; }
            set
            {
                _ListStore = value;
            }
        }


        #endregion

        #region -   Object  -





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
        private string _nameTypein;
        public string nameTypein
        {
            get { return _nameTypein; }
            set { _nameTypein = value; }
        }


        private string _NameMaterial;
        public string NameMaterial
        {
            get { return _NameMaterial; }
            set { _NameMaterial = value; }
        }


        #endregion

        #region -   Date    -




        #endregion

        #region -   Number  -


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
