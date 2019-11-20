using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Util.Command;

namespace WpfApp1.ViewModel
{
    public class MateriaViewModel : BaseViewModel
    {
        #region -   Initialize  -



        #region -   Constructor    -
        public MateriaViewModel()
        {

            _ListItemOfType = new ObservableCollection<Model.Type>();
            {
                new Model.Type() { Name = "Hr" };

            }

            FillListItemToType();

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

                MessageBox.Show("hi");

            }); }
            set { _CommandAddToChake = value; }
        }



        #endregion

        #region -   Action  -



        #endregion

        #region -   CallTask    -


        #endregion

        #region -   Method  -
        private void FillListItemToType()
        {
            int j = 0;
            for (int i = 0; i < 3; i++)
            {
                _ListItemOfType.Add(new Model.Type()
                {
                    Id = j++
                });
            }
        }


        private void GetAllInfo()
        {

            //for (int i = 0; i < 12; i++){_CollecMaterail.Add(new Model.Materail() {});}
            _ListItemOfType = new ObservableCollection<Model.Type>();
            {
                new Model.Type() { };
            }
        }


        #endregion

        #region -   InnerClass  -
        //public class ListItemType
        //{
        //   public Type Type { get; set; }
        //}


        #endregion

        #region -   Propertie   -

        #region -   List    -
        private ObservableCollection<Model.Type> _ListItemOfType;
        public ObservableCollection<Model.Type> ListItemOfType
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
            set
            {
                _CollecMaterail = value;
            }
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
        private string _TextShow;
        public string TextShow
        {
            get { return _TextShow; }
            set { _TextShow = value; }
        }



        #endregion

        #region -   Date    -




        #endregion

        #region -   Number  -




        #endregion

        #region -   Bool    -



        #endregion

        #endregion


    }
}
