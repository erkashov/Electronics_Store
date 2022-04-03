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
            var list = МагазинЭлектроникиEntities.GetContext().Продажа.ToList();
            DG.ItemsSource = list;
            /*foreach(Продажа продажа in )
            {
                DateTime dt = (DateTime)продажа.date;
                DG.Items.Add(new
                {
                    id = продажа.id,
                    //FIO = продажа.Пользователь.fullname + продажа.Пользователь.name + продажа.Пользователь.papaname,
                    name = МагазинЭлектроникиEntities.GetContext().Товар.Find(продажа.idTovar).name,
                    manufacture = МагазинЭлектроникиEntities.GetContext().Товар.Find(продажа.idTovar).manufacture,
                    price = МагазинЭлектроникиEntities.GetContext().Товар.Find(продажа.idTovar).price,
                    type = МагазинЭлектроникиEntities.GetContext().Товар.Find(продажа.idTovar).type,
                    countTov = продажа.countProd,
                    DATA = dt.ToShortDateString()
                });
            }*/
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }
}
