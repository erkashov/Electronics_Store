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
    /// Логика взаимодействия для AddOrEditSellForm.xaml
    /// </summary>
    public partial class AddOrEditSellForm : Window
    {
        private UserForm userForm;
        public AddOrEditSellForm(UserForm UF)
        {
            userForm = UF;
            InitializeComponent();
            foreach(Товар товар in МагазинЭлектроникиEntities.GetContext().Товар)
            {
                if (!TypeTovCB.Items.Contains(товар.type))
                {
                    TypeTovCB.Items.Add(товар.type);
                }
            }
        }

        private Продажа curprod;
        private bool editmode = false;
        public AddOrEditSellForm(UserForm UF, Продажа продажа)
        {
            InitializeComponent();
            editmode = true;
            userForm = UF;
            curprod = продажа;
            SaveBut.IsEnabled = true;
            ItogStoimLabel.Content = "Изменение товара";
            foreach (Товар товар in МагазинЭлектроникиEntities.GetContext().Товар)
            {
                if (!TypeTovCB.Items.Contains(товар.type))
                {
                    TypeTovCB.Items.Add(товар.type);
                }
            }
            TypeTovCB.Text = продажа.Товар.type;
            NameTovCB.Text = продажа.Товар.name;
            ManufTovCB.Text = продажа.Товар.manufacture;
            CountTovTB.Text = продажа.countProd.ToString();
        }

        private void TypeTovCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NameTovCB.Items.Clear();
            PriceForOneTB.Text = "";
            PriceForSeveralTB.Text = "";
            ManufTovCB.Items.Clear();
            CountTovTB.Text = "";
            CountAtStorageLab.Content = "Количество товаров (остаток на складе: )";
            CountTovTB.IsEnabled = false;
            SaveBut.IsEnabled = false;
            foreach (Товар товар in МагазинЭлектроникиEntities.GetContext().Товар)
            {
                if (TypeTovCB.SelectedItem == null)
                    return;
                if (TypeTovCB.SelectedItem.ToString() == товар.type && !NameTovCB.Items.Contains(товар.name))
                {
                    NameTovCB.Items.Add(товар.name);
                }
            }
        }

        private void NameTovCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
            PriceForOneTB.Text = "";
            PriceForSeveralTB.Text = "";
            ManufTovCB.Items.Clear();
            CountTovTB.Text = "";
            CountAtStorageLab.Content = "Количество товаров (остаток на складе: )";
            CountTovTB.IsEnabled = false;
            SaveBut.IsEnabled = false;
            foreach (Товар товар in МагазинЭлектроникиEntities.GetContext().Товар)
            {
                if (NameTovCB.SelectedItem == null)
                    return;
                if (NameTovCB.SelectedItem.ToString() == товар.name && !ManufTovCB.Items.Contains(товар.manufacture))
                {
                    ManufTovCB.Items.Add(товар.manufacture);
                }
            }
        }
        private Товар CurrentTov = null;
        private void ManufTovCB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PriceForOneTB.Text = "";
            PriceForSeveralTB.Text = "";
            CountTovTB.Text = "";
            CountTovTB.IsEnabled = true;
            SaveBut.IsEnabled = false;
            foreach (Товар товар in МагазинЭлектроникиEntities.GetContext().Товар)
            {
                if (TypeTovCB.SelectedItem == null || NameTovCB.SelectedItem == null || ManufTovCB.SelectedItem == null)
                    return;
                if (TypeTovCB.SelectedItem.ToString() == товар.type && NameTovCB.SelectedItem.ToString() == товар.name && ManufTovCB.SelectedItem.ToString() == товар.manufacture)
                {
                    CurrentTov = товар;
                    if (!editmode)
                    {
                        foreach (Продажа продажа in userForm.SellsList)
                        {
                            if (продажа.Товар == CurrentTov)
                            {
                                negr += продажа.countProd;
                            }
                        }
                    }
                    CountAtStorageLab.Content = $"Количество товаров (остаток на складе: {товар.number-negr})";
                    PriceForOneTB.Text = товар.price.ToString();
                    return;
                }
            }
        }
        private int negr;
        private void CountTovTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CountTovTB.Text == "")
                return;
            if(Int32.TryParse(CountTovTB.Text, out int count))
            {
                if(count > 0)
                {
                    if(count <= (CurrentTov.number-negr))
                    {
                        PriceForSeveralTB.Text = (count * CurrentTov.price).ToString();
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

        private void SaveBut_Click(object sender, RoutedEventArgs e)
        {
            if (editmode)
            {
                for (int i = 0; i < userForm.SellsList.Count; i++)
                {
                    if (curprod == userForm.SellsList[i])
                    {
                        userForm.SellsList[i] = new Продажа() { countProd = Int32.Parse(CountTovTB.Text), date = DateTime.Now, Товар = CurrentTov, idTovar = CurrentTov.id, idUser = userForm.CurrentUser.id, Пользователь = userForm.CurrentUser };
                        userForm.filldatagrid();
                        this.Close();
                        return;
                    }
                }
            }
            for(int i = 0; i<userForm.SellsList.Count; i++)
            {
                if(CurrentTov == userForm.SellsList[i].Товар)
                {
                    userForm.SellsList[i].countProd += Int32.Parse(CountTovTB.Text);
                    userForm.filldatagrid();
                    this.Close();
                    return;
                }
            }
            userForm.SellsList.Add(new Продажа() { countProd = Int32.Parse(CountTovTB.Text), date = DateTime.Now, Товар = CurrentTov, idTovar = CurrentTov.id, idUser = userForm.CurrentUser.id, Пользователь = userForm.CurrentUser });
            userForm.filldatagrid();
            this.Close();
        }
    }
}
