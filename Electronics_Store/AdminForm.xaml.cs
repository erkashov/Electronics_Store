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
    /// Логика взаимодействия для AdminForm.xaml
    /// </summary>
    public partial class AdminForm : Window
    {
        private MainWindow mv;
        private Tovar CurrentTov;
        public Delivery CurrDel;
        public List<TovarDel> tovarDels = new List<TovarDel>();
        private List<string> statuses = new List<string>() { "Принято", "В пути", "Новая" };
        public AdminForm(MainWindow main)
        {
            InitializeComponent();
            dataGridt.ItemsSource = BooksShopEntities.GetContext().Tovars.ToList();
            typeTCB.ItemsSource = BooksShopEntities.GetContext().Tovars.Select(p => p.category).Distinct().ToList(); 
            StatusTPTB.ItemsSource = statuses;
            dataGridPos.ItemsSource = BooksShopEntities.GetContext().Deliveries.ToList();
            mv = main;
            CurrentTov = new Tovar() {name = "", category="", author="", number=0, price=0 };
            CurrDel = new Delivery() { countTov = 0, date = DateTime.Now, status = "Новая", TovarDels = tovarDels };
            stackTov.DataContext = CurrentTov;
            stackdel.DataContext = CurrDel;
            
            filldatagrid();
        }
        private bool IsEditTov = false;

        private void filldatagrid()
        {
            dataGridt.ItemsSource = BooksShopEntities.GetContext().Tovars.ToList();
            typeTCB.ItemsSource = BooksShopEntities.GetContext().Tovars.Select(p => p.category).Distinct().ToList();
            dataGridPos.ItemsSource = BooksShopEntities.GetContext().Deliveries.ToList();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mv.Visibility = Visibility.Visible;
        }

        private void SaveOrCreateBut_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(priceTTB.Text, out int pr) && Int32.TryParse(numberTTB.Text, out int num))
            {
                CurrentTov.category = typeTCB.Text;
                string mes = "";
                if (IsEditTov)
                {
                    mes = ChangeTov(CurrentTov);
                    IsEditTov = false;
                }
                else
                {
                    mes = AddTovar(CurrentTov);
                }                
                MessageBox.Show(mes);
                filldatagrid();
                if(mes == "Успешно") CurrentTov = new Tovar() { name = "", category = "", author = "", number = 0, price = 0 };
                stackTov.DataContext = null;
                stackTov.DataContext = CurrentTov;
            }
            else
            {
                MessageBox.Show("Количество и цена должны быть числами!!!");
                return;
            }
        }

        public static string AddTovar(Tovar t)
        {
            if (t == null) return "Укажите товар";
            if (t.name == "" || t.category == "" || t.author == "" || t.number == 0 || t.price == 0) return "Заполните все поля";
            if (BooksShopEntities.GetContext().Tovars.Where(p => p.name == t.name && p.author == t.author && p.category == p.category).FirstOrDefault() != null) return "Данный товар уже существует";
            try
            {
                BooksShopEntities.GetContext().Tovars.Add(t);
                BooksShopEntities.GetContext().SaveChanges();
            }
            catch (Exception e)
            {
                return e.InnerException.InnerException.Message;
            }
            return "Успешно";
        }

        public static string ChangeTov(Tovar t)
        {
            if (t == null) return "Укажите товар";
            if (t.name == "" || t.category == "" || t.author == "" || t.number == 0 || t.price == 0) return "Заполните все поля";
            try
            {
                Tovar temp = BooksShopEntities.GetContext().Tovars.Find(t.id);
                if (temp == null) return "Товара с таким id не найдено";
                temp.author = t.author;
                temp.name = t.name;
                temp.number = t.number;
                temp.price = t.price;
                BooksShopEntities.GetContext().SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                return ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage;
            }
            catch (Exception e)
            {
                return e.Message;
            }
            return "Успешно";
        }

        public static string RemoveTov(Tovar t)
        {
            if (t == null) return "Укажите товар";
            try
            {
                BooksShopEntities.GetContext().Tovars.Remove(t);
                BooksShopEntities.GetContext().SaveChanges();
                return "Успешно";

            }
            catch (Exception ex)
            {
                return ex.InnerException.InnerException.Message;
            }
        }
        private void ResetBut_Click(object sender, RoutedEventArgs e)
        {
            CurrentTov = new Tovar() { name = "", category = "", author = "", number = 0, price = 0 };
            stackTov.DataContext = null;
            stackTov.DataContext = CurrentTov;
            IsEditTov = false;
        }

        private void BtnEditT_Click(object sender, RoutedEventArgs e)
        {
            CurrentTov = dataGridt.SelectedItem as Tovar;
            stackTov.DataContext = null;
            stackTov.DataContext = CurrentTov;
            IsEditTov = true;
        }

        private void BtnDelT_Click(object sender, RoutedEventArgs e)
        {
            CurrentTov = dataGridt.SelectedItem as Tovar;
            if (MessageBox.Show($"Вы точно хотите удалить {CurrentTov.name} автора {CurrentTov.author}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RemoveTov(CurrentTov);
                filldatagrid();
                MessageBox.Show("Товар успешно удален!");
            }
        }

        private bool IsEditPost = false;
        private void ResetTP_Click(object sender, RoutedEventArgs e)
        {
            stackdel.DataContext = null;
            CurrDel = new Delivery() { countTov = 0, date = DateTime.Now, status = "Новая" };
            tovarDels = new List<TovarDel>();
            stackdel.DataContext = CurrDel;
            IsEditPost = false;
        }

        private void BtnEditP_Click(object sender, RoutedEventArgs e)
        {
            CurrDel = dataGridPos.SelectedItem as Delivery;
            tovarDels = CurrDel.TovarDels.ToList();
            if (CurrDel == null) return;
            stackdel.DataContext = null;
            stackdel.DataContext = CurrDel;
            tovarDels = CurrDel.TovarDels.ToList();
            IsEditPost = true;
        }

        private void SaveOrCreateTP_Click(object sender, RoutedEventArgs e)
        {
            if (dateTPTB.SelectedDate != null && StatusTPTB.Text != "" && numberTPTB.Text != "" )
            {
                if (Int32.TryParse(numberTPTB.Text, out int num))
                {
                    if (IsEditPost)
                    {

                        Delivery temp = BooksShopEntities.GetContext().Deliveries.Find(CurrDel.id);
                        temp = CurrDel;
                        temp.TovarDels = tovarDels;
                        BooksShopEntities.GetContext().SaveChanges();
                        ResetTP_Click(null, null);
                        dataGridPos.ItemsSource = BooksShopEntities.GetContext().Deliveries.ToList();
                        return;
                    }
                    try
                    {
                        CurrDel.countTov = tovarDels.Count;
                        BooksShopEntities.GetContext().Deliveries.Add(CurrDel);
                        BooksShopEntities.GetContext().SaveChanges();
                        BooksShopEntities.GetContext().TovarDels.AddRange(tovarDels);
                        BooksShopEntities.GetContext().SaveChanges();
                        ResetTP_Click(null, null);
                        dataGridPos.ItemsSource = BooksShopEntities.GetContext().Deliveries.ToList();

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
                    MessageBox.Show("Количество должно быть числом!!!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены!!!");
                return;
            }
        }

        private void BtnDelP_Click(object sender, RoutedEventArgs e)
        {
            BooksShopEntities.GetContext().Deliveries.Remove(dataGridPos.SelectedItem as Delivery);
            BooksShopEntities.GetContext().SaveChanges();
            dataGridPos.ItemsSource = BooksShopEntities.GetContext().Deliveries.ToList();
            /*currentpost = dataGridPos.SelectedItem as Поставка;
            if (currentpost == null) return;
            if (MessageBox.Show($"Вы точно хотите удалить {currentpost.Товар.name} производителя {currentpost.Товар.manufacture} статуса {currentpost.status}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                МагазинЭлектроникиEntities.GetContext().Поставка.Remove(currentpost);
                МагазинЭлектроникиEntities.GetContext().SaveChanges();
                filldatagrid();
                MessageBox.Show("Поставка успешно удалена!");
            }*/
        }

        private void btAddTovars_Click(object sender, RoutedEventArgs e)
        {
            UserForm user = new UserForm(this);
            user.ShowDialog();
            CurrDel.countTov = tovarDels.Count;
            stackdel.DataContext = null;
            stackdel.DataContext = CurrDel;
        }
    }
}