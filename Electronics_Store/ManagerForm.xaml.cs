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
    /// Логика взаимодействия для ManagerForm.xaml
    /// </summary>
    public partial class ManagerForm : Window
    {
        public ManagerForm(MainWindow mainWindow)
        {
            InitializeComponent();
            dataGrid.ItemsSource = МагазинЭлектроникиEntities.GetContext().Пользователь.ToList();
            mw = mainWindow;
        }

        private MainWindow mw;
        private bool isChangeMode = false;
        private Пользователь CurrentUser;
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = (sender as Button).DataContext as Пользователь;
            FullnameTB.Text = CurrentUser.fullname;
            NameTB.Text = CurrentUser.name;
            PapanameTB.Text = CurrentUser.papaname;
            PhoneTB.Text = CurrentUser.phone;
            PasswordTB.Text = CurrentUser.password;
            LoginTB.Text = CurrentUser.login;
            EmailTB.Text = CurrentUser.email;
            RoleCB.Text = CurrentUser.role;
            isChangeMode = true;
        }

        private void BtnDel_Click(object sender, RoutedEventArgs e)
        {
            CurrentUser = (sender as Button).DataContext as Пользователь;
            if (MessageBox.Show($"Вы точно хотите удалить {CurrentUser.fullname} {CurrentUser.name} {CurrentUser.papaname}","Внимание",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                МагазинЭлектроникиEntities.GetContext().Пользователь.Remove(CurrentUser);
                МагазинЭлектроникиEntities.GetContext().SaveChanges();
               dataGrid.ItemsSource = МагазинЭлектроникиEntities.GetContext().Пользователь.ToList();
                MessageBox.Show("Пользователь успешно удален!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(FullnameTB.Text!="" && NameTB.Text != "" &&  PapanameTB.Text != "" && PhoneTB.Text!="" && Int64.TryParse(PhoneTB.Text, out long i) && PasswordTB.Text !="" && LoginTB.Text != "" && EmailTB.Text != "" && RoleCB.Text != "")
            {
                if(FullnameTB.Text.All(char.IsLetter) && NameTB.Text.All(char.IsLetter) && PapanameTB.Text.All(char.IsLetter))
                {
                    if (isChangeMode)
                    {
                        for (int j = 0; j< МагазинЭлектроникиEntities.GetContext().Пользователь.ToList().Count; j++)
                        {
                            Пользователь user = МагазинЭлектроникиEntities.GetContext().Пользователь.ToList()[j];
                            if (user.id == CurrentUser.id)
                            {
                                user.name = NameTB.Text;
                                user.fullname = FullnameTB.Text;
                                user.email = EmailTB.Text;
                                user.login = LoginTB.Text;
                                user.papaname = PapanameTB.Text;
                                user.password = PasswordTB.Text;
                                user.phone = PhoneTB.Text;
                                user.role = RoleCB.Text;
                                МагазинЭлектроникиEntities.GetContext().SaveChanges();
                                dataGrid.ItemsSource = МагазинЭлектроникиEntities.GetContext().Пользователь.ToList();
                                MessageBox.Show("Данные успешно изменены!");
                                Button_Click_1(sender, e);
                                CurrentUser = null;
                                return;
                            }
                        }
                    }
                    foreach (Пользователь user in МагазинЭлектроникиEntities.GetContext().Пользователь)
                    {
                        if(user.name == NameTB.Text && user.papaname == PapanameTB.Text && user.fullname == FullnameTB.Text)
                        {
                            MessageBox.Show("Пользователь с таким ФИО уже существует");
                            return;
                        }
                        if(user.login == LoginTB.Text || user.password == PasswordTB.Text)
                        {
                            MessageBox.Show("Пользователь с таким логином или паролем уже существует");
                            return;
                        }
                    }
                    МагазинЭлектроникиEntities.GetContext().Пользователь.Add(new Пользователь() {name = NameTB.Text, fullname = FullnameTB.Text, 
                        email = EmailTB.Text, login = LoginTB.Text, papaname = PapanameTB.Text, password = PasswordTB.Text, phone = PhoneTB.Text, role = RoleCB.Text });
                    try
                    {
                        МагазинЭлектроникиEntities.GetContext().SaveChanges();
                        MessageBox.Show("Новый пользователь успешно добавлен!");
                        Button_Click_1(sender, e);
                        dataGrid.ItemsSource = МагазинЭлектроникиEntities.GetContext().Пользователь.ToList();
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    return;
                }
                else
                {
                    MessageBox.Show("ФИО не должно содержать цифры!!!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены!!!");
                return;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            FullnameTB.Text = "";
            NameTB.Text = "";
            PapanameTB.Text = "";
            PhoneTB.Text = "";
            PasswordTB.Text = "";
            LoginTB.Text = "";
            EmailTB.Text = "";
            RoleCB.Text = "";
            isChangeMode = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mw.Visibility = Visibility.Visible;
        }
    }
}
