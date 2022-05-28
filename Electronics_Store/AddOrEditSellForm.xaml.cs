using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
    /// Логика взаимодействия для AddOrEditSellForm.xaml
    /// </summary>
    public partial class AddOrEditSellForm : Window
    {
        private UserForm userForm;
        private bool isPost = false;
        public AddOrEditSellForm(UserForm UF, bool post = false)
        {
            userForm = UF;
            isPost = post;
            InitializeComponent();
            TypeTovCB.ItemsSource = BooksShopEntities.GetContext().Tovars.Select(p => p.category).Distinct().ToList();
        }

        private TovarSale CurTovar;
        private TovarDel CurTovarDel;
        private bool editmode = false;
        public AddOrEditSellForm(UserForm UF, TovarSale tovar)
        {
            InitializeComponent();
            editmode = true;
            userForm = UF;
            CurTovar = tovar;
            SaveBut.IsEnabled = true;
            TypeTovCB.ItemsSource = BooksShopEntities.GetContext().Tovars.Select(p => p.category).Distinct().ToList();
            NameTovCB.ItemsSource = BooksShopEntities.GetContext().Tovars.Where(p => p.category == tovar.Tovar.category).ToList();
            ManufTovCB.ItemsSource = BooksShopEntities.GetContext().Tovars.Where(p => p.author == tovar.Tovar.author).ToList();
            ItogStoimLabel.Content = "Изменение товара";
            this.DataContext = tovar;
            
        }

        private void TypeTovCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PriceForOneTB.Text = "";
            PriceForSeveralTB.Text = "";
            CountTovTB.Text = "";
            CountAtStorageLab.Content = "Количество товаров (остаток на складе: )";
            CountTovTB.IsEnabled = false;
            SaveBut.IsEnabled = false;
            NameTovCB.ItemsSource = null;
            NameTovCB.ItemsSource = BooksShopEntities.GetContext().Tovars.Where(p => p.category == TypeTovCB.SelectedItem.ToString()).ToList();
        }

        private void NameTovCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PriceForOneTB.Text = "";
            PriceForSeveralTB.Text = "";
            CountTovTB.Text = "";
            CountAtStorageLab.Content = "Количество товаров (остаток на складе: )";
            CountTovTB.IsEnabled = false;
            SaveBut.IsEnabled = false;
            Tovar selected = NameTovCB.SelectedItem as Tovar;
            ManufTovCB.ItemsSource = BooksShopEntities.GetContext().Tovars.Where(p => p.author == selected.author).ToList();
        }

        private void ManufTovCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            negr = 0;
            PriceForOneTB.Text = "";
            PriceForSeveralTB.Text = "";
            CountTovTB.Text = "";
            CountTovTB.IsEnabled = true;
            SaveBut.IsEnabled = false;
            if (ManufTovCB.SelectedItem == null) return;
            Tovar selected = ManufTovCB.SelectedItem as Tovar;
            CountAtStorageLab.Content = $"Количество товаров (остаток на складе: {selected.number})";
            PriceForOneTB.Text = selected.price.ToString();
        }
        private int negr;
        private void CountTovTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!isPost)
            {
                if (CountTovTB.Text == "")
                    return;
                if (ManufTovCB.SelectedItem as Tovar == null) return;
                if (Int32.TryParse(CountTovTB.Text, out int count))
                {
                    if (count > 0)
                    {
                        if (count <= ((ManufTovCB.SelectedItem as Tovar).number - negr))
                        {
                            PriceForSeveralTB.Text = (count * (ManufTovCB.SelectedItem as Tovar).price).ToString();
                            SaveBut.IsEnabled = true;
                            return;
                        }
                        else
                        {
                            MessageBox.Show("Товара недостаточно на складе!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Нельзя сформировать продажу с числом товаров 0 или менее!");
                    }
                }
                else
                {
                    MessageBox.Show("Количество должно быть числом!");
                    CountTovTB.Text = "";
                }
                SaveBut.IsEnabled = false;
            }
            else
            {
                SaveBut.IsEnabled = true;
            }
        }

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            if (ManufTovCB.SelectedItem as Tovar == null) return;
            if (!isPost)
            {
                if (editmode)
                {
                    if (ManufTovCB.SelectedItem == null) return;
                    try
                    {
                        TovarSale temp = BooksShopEntities.GetContext().TovarSales.Find(CurTovar.id);
                        temp = CurTovar;
                        BooksShopEntities.GetContext().SaveChanges();
                        this.Close();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        MessageBox.Show(ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    userForm.SellsList.Add(new TovarSale()
                    {
                        Sell = userForm.CurrSell,
                        Tovar = (ManufTovCB.SelectedItem as Tovar),
                        count = Convert.ToInt32(CountTovTB.Text),
                        idTovar = (ManufTovCB.SelectedItem as Tovar).id
                    });
                    userForm.filldatagrid();
                    this.Close();
                }
            }
            else
            {
                if (editmode)
                {
                    if (ManufTovCB.SelectedItem == null) return;
                    try
                    {
                        TovarDel temp = BooksShopEntities.GetContext().TovarDels.Find(CurTovarDel.id);
                        temp = CurTovarDel;
                        BooksShopEntities.GetContext().SaveChanges();
                        this.Close();
                    }
                    catch (DbEntityValidationException ex)
                    {
                        MessageBox.Show(ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    userForm.ad.tovarDels.Add(new TovarDel()
                    {
                        Delivery = userForm.ad.CurrDel,
                        Tovar = (ManufTovCB.SelectedItem as Tovar),
                        count = Convert.ToInt32(CountTovTB.Text)
                    });
                    userForm.filldatagrid();
                    this.Close();
                }
            }
        }
    }
}
