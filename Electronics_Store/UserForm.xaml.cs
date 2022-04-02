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
        public UserForm(MainWindow main)
        {
            InitializeComponent();
            mv = main;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mv.Visibility = Visibility.Visible;
        }
    }
}
