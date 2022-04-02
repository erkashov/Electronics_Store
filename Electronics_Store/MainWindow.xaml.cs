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

        /*
        private static МагазинЭлектроникиEntities _context;

        public static МагазинЭлектроникиEntities GetContext()
        {
            if(_context == null)
            {
                _context = new МагазинЭлектроникиEntities();
            }
            return _context;
        }
         */

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Пользователь User in МагазинЭлектроникиEntities.GetContext().Пользователь)
            {
                if (User.login == loginTb.Text && User.password == passwordTb.Text)
                {
                    if (User.role == "Менеджер")
                    {
                        ManagerForm Form = new ManagerForm(this);
                        Form.Visibility = Visibility.Visible;
                    }
                    if (User.role == "Администратор")
                    {
                        AdminForm Form = new AdminForm(this);
                        Form.Visibility = Visibility.Visible;
                    }
                    if (User.role == "Пользователь")
                    {
                        UserForm Form = new UserForm(this);
                        Form.Visibility = Visibility.Visible;
                    }
                    this.Visibility = Visibility.Hidden;
                    loginTb.Text = "";
                    passwordTb.Text = "";
                    return;
                }
            }
            MessageBox.Show("Логин или пароль введен неверно!");
        }
    }
}
