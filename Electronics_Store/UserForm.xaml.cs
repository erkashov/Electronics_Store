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
        public AdminForm ad;
        public User CurrentUser;
        public Sell CurrSell;
        public List<TovarSale> SellsList = new List<TovarSale>();
        private bool isPost = false;
        public UserForm(MainWindow main, User user)
        {
            InitializeComponent();
            CurrentUser = user;
            CurrSell = new Sell();
            mv = main;
        }

        public UserForm(AdminForm main)
        {
            InitializeComponent();
            this.isPost = true;
            AddTovBut.Content = "Добавить";
            AcceptTovBut.Content = "Подтвердить";
            HistorySellsBut.Visibility = Visibility.Hidden;
            ad = main;
            filldatagrid();
        }

        public void filldatagrid()
        {
            if (!isPost)
            {
                DG.ItemsSource = null;
                DG.ItemsSource = SellsList;
                ItogStoimLabel.Content = $"Итоговая стоимость: {SellsList.Sum(P => P.count * P.Tovar.price)}";
            }
            else
            {
                DG.ItemsSource = null;
                DG.ItemsSource = ad.tovarDels;
                ItogStoimLabel.Content = $"Итоговое кол-во: {ad.tovarDels.Sum(p=>p.count)}";
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(!isPost) mv.Visibility = Visibility.Visible;
        }

        private void AddTovBut_Click(object sender, RoutedEventArgs e)
        {
            AddOrEditSellForm addOrEditSellForm = new AddOrEditSellForm(this, isPost);
            addOrEditSellForm.ShowDialog();
            filldatagrid();
        }

        private void BtnEditP_Click(object sender, RoutedEventArgs e)
        {
            if (DG.SelectedItem == null) return;
            if (!isPost)
            {
                AddOrEditSellForm addOrEditSellForm = new AddOrEditSellForm(this, DG.SelectedItem as TovarSale);
                addOrEditSellForm.ShowDialog();
            }
        }

        private void BtnDelP_Click(object sender, RoutedEventArgs e)
        {
            if (DG.SelectedItem == null) return;
            if (!isPost)
            {
                SellsList.Remove(DG.SelectedItem as TovarSale);
                //BooksShopEntities.GetContext().TovarSales.Remove(DG.SelectedItem as TovarSale);
            }
            else
            {
                BooksShopEntities.GetContext().TovarDels.Remove(DG.SelectedItem as TovarDel);
                ad.tovarDels.Remove(DG.SelectedItem as TovarDel);
                BooksShopEntities.GetContext().SaveChanges();
                DG.ItemsSource = null;
                DG.ItemsSource = ad.tovarDels;
            }
            BooksShopEntities.GetContext().SaveChanges();
            filldatagrid();
        }

        private void AcceptTovBut_Click(object sender, RoutedEventArgs e)
        {
            if (!isPost)
            {
                if (MessageBox.Show($"Вы подтверждаете продажу?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    CurrSell.countProd = SellsList.Sum(p => p.count);
                    CurrSell.date = DateTime.Now;
                    CurrSell.idUser = CurrentUser.id;
                    CurrSell = BooksShopEntities.GetContext().Sells.Add(CurrSell);
                    BooksShopEntities.GetContext().SaveChanges();
                    BooksShopEntities.GetContext().TovarSales.AddRange(SellsList);
                    BooksShopEntities.GetContext().SaveChanges();
                    SellsList.Clear();
                    filldatagrid();
                    MessageBox.Show("Продажа осуществлена успешно!");
                }
            }
            else
            {
                this.Close();
            }
        }

        private void HistorySellsBut_Click(object sender, RoutedEventArgs e)
        {
            HistorySellsForm historySellsForm = new HistorySellsForm();
            historySellsForm.ShowDialog();
        }
    }
}
