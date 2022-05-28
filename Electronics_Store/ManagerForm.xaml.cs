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
    /// Логика взаимодействия для ManagerForm.xaml
    /// </summary>
    public partial class ManagerForm : Window
    {
        private List<string> roles = new List<string>() { "Администратор", "Пользователь", "Менеджер" };
        public ManagerForm(MainWindow mainWindow)
        {
            InitializeComponent();
            dataGrid.ItemsSource = BooksShopEntities.GetContext().Users.ToList();
            RoleCB.ItemsSource = roles;
            mw = mainWindow;
        }

        private MainWindow mw;
        private bool isChangeMode = false;
        private User CurrentUser;

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = dataGrid.SelectedItem as User;
            stackuser.DataContext = CurrentUser;
            isChangeMode = true;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = (sender as Button).DataContext as User;
            if (MessageBox.Show($"Вы точно хотите удалить {CurrentUser.fullname} {CurrentUser.name} {CurrentUser.papaname}","Внимание",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                RemoveUser(CurrentUser);
               dataGrid.ItemsSource = BooksShopEntities.GetContext().Users.ToList();
                MessageBox.Show("Пользователь успешно удален!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (isChangeMode)
            {
                MessageBox.Show(Change(CurrentUser));
                Button_Click_1(null, null);
                dataGrid.ItemsSource = BooksShopEntities.GetContext().Users.ToList();
            }
            else
            {
                User new_user = new User(FullnameTB.Text, NameTB.Text, PapanameTB.Text, LoginTB.Text, PasswordTB.Text, PhoneTB.Text, EmailTB.Text);
                MessageBox.Show(Reg(new_user));
                Button_Click_1(null, null);
                dataGrid.ItemsSource = BooksShopEntities.GetContext().Users.ToList();

            }
        }

        public static string Reg(User u)
        {
            if (u == null) return "Укажите пользователя";
            if (u.login == "" || u.password == "" || u.role == "" || u.fullname == "" || u.name == "" || u.phone == "") return "Не все поля заполнены";
            if (!u.name.All(char.IsLetter) || !u.fullname.All(char.IsLetter) || !u.papaname.All(char.IsLetter)) return "ФИО не должно содержать цифры";
            if (BooksShopEntities.GetContext().Users.Where(p => p.login == u.login).FirstOrDefault() != null) return "Пользователь с таким логином существует";
            if (BooksShopEntities.GetContext().Users.Where(p => p.phone == u.phone).FirstOrDefault() != null) return "Пользователь с таким телефоном существует";
            try
            {
                BooksShopEntities.GetContext().Users.Add(u);
                BooksShopEntities.GetContext().SaveChanges();
                return "Успешно";
            }
            catch (DbEntityValidationException ex)
            {
                return ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage;
            }
            catch (Exception e)
            {
                return e.InnerException.InnerException.Message;
            }
        }

        public static string Change(User u)
        {
            if (u == null) return "Укажите пользователя";
            if (u.login == "" || u.password == "" || u.role == "" || u.fullname == "" || u.name == "" || u.phone == "") return "Не все поля заполнены";
            if (!u.name.All(char.IsLetter) || !u.fullname.All(char.IsLetter) || !u.papaname.All(char.IsLetter)) return "ФИО не должно содержать цифры";
            try
            {
                User temp = BooksShopEntities.GetContext().Users.Where(p => p.login == u.login).FirstOrDefault();
                if (temp == null) return "Пользователя с таким логином не существует";
                if (BooksShopEntities.GetContext().Users.Where(p => p.phone == u.phone && p.id != temp.id).Count() > 0) return "Пользователь с таким номером уже существует";
                temp = u;
                BooksShopEntities.GetContext().SaveChanges();
                return "Успешно";
            }
            catch (DbEntityValidationException ex)
            {
                return ex.EntityValidationErrors.First().ValidationErrors.First().ErrorMessage;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public static string RemoveUser(User user)
        {
            if (user == null) return "Укажите пользователя";
            try
            {
                User temp = BooksShopEntities.GetContext().Users.Where(p => p.login == user.login).FirstOrDefault();
                if (temp == null) return "Пользователя с таким логином не существует";
                BooksShopEntities.GetContext().Users.Remove(temp);
                BooksShopEntities.GetContext().SaveChanges();
                return "Успешно";
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CurrentUser = new User("", "", "", "", "", "", "");
            stackuser.DataContext = null;
            stackuser.DataContext = CurrentUser;
            isChangeMode = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mw.Visibility = Visibility.Visible;
        }

        private void HistorySellsBut_Click(object sender, RoutedEventArgs e)
        {
            HistorySellsForm historySellsForm = new HistorySellsForm();
            historySellsForm.ShowDialog();
        }
    }
}
