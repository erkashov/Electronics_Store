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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Electronics_Store
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Пользователь user = МагазинЭлектроникиEntities.GetContext().Пользователь.Where(p => p.login == loginTb.Text && p.password == passwordTb.Text).FirstOrDefault();
            /*foreach (Пользователь User in МагазинЭлектроникиEntities.GetContext().Пользователь)
            {*/
            if (user != null)
            {
                if (user.role == "Менеджер")
                {
                    ManagerForm Form = new ManagerForm(this);
                    Form.Visibility = Visibility.Visible;
                }
                if (user.role == "Администратор")
                {
                    AdminForm Form = new AdminForm(this);
                    Form.Visibility = Visibility.Visible;
                }
                if (user.role == "Пользователь")
                {
                    UserForm Form = new UserForm(this, user);
                    Form.Visibility = Visibility.Visible;
                }
                this.Visibility = Visibility.Hidden;
                loginTb.Text = "";
                passwordTb.Text = "";
            }
            else MessageBox.Show("Логин или пароль введен неверно!");
        }
    }
}
