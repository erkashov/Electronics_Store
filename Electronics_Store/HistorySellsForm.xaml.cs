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

namespace Electronics_Store
{
    /// <summary>
    /// Логика взаимодействия для HistorySellsForm.xaml
    /// </summary>
    public partial class HistorySellsForm : Window
    {
        public HistorySellsForm()
        {
            InitializeComponent();
            var list = BooksShopEntities.GetContext().TovarSales.ToList();
            DG.ItemsSource = list;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
