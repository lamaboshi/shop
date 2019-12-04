using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for Windowmatira.xaml
    /// </summary>
    public partial class Windowmatira : Window
    {
        public Windowmatira()
        {
            InitializeComponent();

        }

        private void AddTBtn_Click(object sender, RoutedEventArgs e)
        {
            Windowtype windowtype = new Windowtype();
            windowtype.ShowDialog();

        }
        private void GoToBill(object sender, RoutedEventArgs e)
        {
            WindowBill windowBill = new WindowBill();
            windowBill.ShowDialog();
        }

        private void AddMat_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
