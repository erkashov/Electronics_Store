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
    /// Логика взаимодействия для UserForm.xaml
    /// </summary>
    public partial class UserForm : Window
    {
        private MainWindow mv;
        public Пользователь CurrentUser;
        public List<Продажа> SellsList = new List<Продажа>();
        public UserForm(MainWindow main, Пользователь пользователь)
        {
            InitializeComponent();
            CurrentUser = пользователь;
            mv = main;
        }

        private int finishedSum = 0;
        private List<Pos2> pos1s = new List<Pos2>();
        private List<Pos2> pos2s = new List<Pos2>();
        public void filldatagrid()
        {
            finishedSum = 0;
            DG.ItemsSource = pos2s;
            pos1s.Clear();
            int counter = 1;
            foreach (Продажа продажа in SellsList)
            {
                pos1s.Add(new Pos2() { id = counter, countTov = продажа.countProd, manufacture = продажа.Товар.manufacture, name = продажа.Товар.name,
                    price = продажа.Товар.price.ToString(), type = продажа.Товар.type });
                counter++;
                finishedSum += продажа.Товар.price * продажа.countProd;
            }
            DG.ItemsSource = pos1s;
            ItogStoimLabel.Content = $"Итоговая стоимость: {finishedSum}";
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mv.Visibility = Visibility.Visible;
        }

        private void AddTovBut_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditSellForm addOrEditSellForm = new AddOrEditSellForm(this);
            addOrEditSellForm.ShowDialog();
        }

        private void BtnEditP_Click(object sender, RoutedEventArgs e)
        {
            Pos2 pos = (sender as Button).DataContext as Pos2;
            foreach (Продажа продажа in SellsList)
            {
                if (pos.manufacture == продажа.Товар.manufacture && pos.name == продажа.Товар.name)
                {
                    AddOrEditSellForm addOrEditSellForm = new AddOrEditSellForm(this,продажа);
                    addOrEditSellForm.ShowDialog();
                    break;
                }
            }
        }

        private void BtnDelP_Click(object sender, RoutedEventArgs e)
        {
            Pos2 pos = (sender as Button).DataContext as Pos2;
            Продажа currentSell = null;
            foreach (Продажа продажа in SellsList)
            {
                if (pos.manufacture == продажа.Товар.manufacture && pos.name == продажа.Товар.name)
                {
                    currentSell = продажа;
                    break;
                }
            }
            SellsList.Remove(currentSell);
            МагазинЭлектроникиEntities.GetContext().SaveChanges();
            filldatagrid();
        }

        private void AcceptTovBut_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show($"Вы подтверждаете продажу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                for (int i = 0; i < SellsList.Count; i++)
                {
                    МагазинЭлектроникиEntities.GetContext().Пользователь.Find(CurrentUser.id).Продажа.Add(SellsList[i]);
                    МагазинЭлектроникиEntities.GetContext().Продажа.Add(SellsList[i]);
                    МагазинЭлектроникиEntities.GetContext().Товар.Find(SellsList[i].idTovar).number -= SellsList[i].countProd;
                    МагазинЭлектроникиEntities.GetContext().SaveChanges();
                }
                SellsList.Clear();
                filldatagrid();
                MessageBox.Show("Продажа осуществлена успешно!");
            }
        }

        private void HistorySellsBut_Click(object sender, RoutedEventArgs e)
        {
            HistorySellsForm historySellsForm = new HistorySellsForm();
            historySellsForm.ShowDialog();
        }
    }

    class Pos2
    {
        public int id { get; set; }
        public string name { get; set; }
        public string manufacture { get; set; }
        public string price { get; set; }
        public string type { get; set; }
        public int countTov { get; set; }
    }
}
