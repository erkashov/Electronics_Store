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
    /// Логика взаимодействия для AdminForm.xaml
    /// </summary>
    public partial class AdminForm : Window
    {
        private MainWindow mv;
        public AdminForm(MainWindow main)
        {
            InitializeComponent();
            dataGridt.ItemsSource = МагазинЭлектроникиEntities.GetContext().Товар.ToList();
            mv = main;
            updateTCB();
            filldatagrid();
        }

        private Товар CurrentTovar = null;
        private bool IsEditTov = false;

        private void updateTCB()
        {
            foreach (Товар товар in МагазинЭлектроникиEntities.GetContext().Товар)
            {
                if (manufTPTB.Items.Contains(товар.manufacture))
                    continue;
                manufTPTB.Items.Add(товар.manufacture);
            }
        }

        private List<Pos1> pos1s = new List<Pos1>();
        private List<Pos1> pos2s = new List<Pos1>();
        private void filldatagrid()
        {
            dataGridPos.ItemsSource = pos2s;
            pos1s.Clear();
            foreach (Поставка поставка in МагазинЭлектроникиEntities.GetContext().Поставка)
            {
                DateTime dt = (DateTime)поставка.date;
                pos1s.Add(new Pos1() {id = поставка.id, manufacture = поставка.Товар.manufacture, name = поставка.Товар.name, countTov = поставка.countTov, date = dt.ToShortDateString(), status = поставка.status });
            }
            dataGridPos.ItemsSource = pos1s;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mv.Visibility = Visibility.Visible;
        }

        private void SaveOrCreateBut_Click(object sender, RoutedEventArgs e)
        {
            if (nameTTB.Text != "" && manufactureTTB.Text != "" && typeTCB.Text != "" && priceTTB.Text != "" && numberTTB.Text != "" && warperTTB.Text != "")
            {
                if (Int32.TryParse(priceTTB.Text, out int pr) && Int32.TryParse(numberTTB.Text, out int num))
                {
                    if (IsEditTov)
                    {
                        for (int j = 0; j < МагазинЭлектроникиEntities.GetContext().Товар.ToList().Count; j++)
                        {
                            Товар Tovar = МагазинЭлектроникиEntities.GetContext().Товар.ToList()[j];
                            if (Tovar.id == CurrentTovar.id)
                            {
                                foreach (Товар tov in МагазинЭлектроникиEntities.GetContext().Товар)
                                {
                                    if (tov.name == nameTTB.Text && tov.manufacture == manufactureTTB.Text && CurrentTovar.id != tov.id)
                                    {
                                        MessageBox.Show("Товар данного производителя уже существует");
                                        return;
                                    }
                                }
                                Tovar.name = nameTTB.Text;
                                Tovar.manufacture = manufactureTTB.Text;
                                Tovar.name = nameTTB.Text;
                                Tovar.price = pr;
                                Tovar.type = typeTCB.Text;
                                Tovar.number = num;
                                Tovar.warrantyPeriod = warperTTB.Text;
                                МагазинЭлектроникиEntities.GetContext().SaveChanges();
                                dataGridt.ItemsSource = МагазинЭлектроникиEntities.GetContext().Товар.ToList();
                                MessageBox.Show("Данные успешно изменены!");
                                ResetBut_Click(sender, e);
                                CurrentTovar = null;
                                updateTCB();
                                return;
                            }
                        }
                    }
                    foreach (Товар tov in МагазинЭлектроникиEntities.GetContext().Товар)
                    {
                        if (tov.name == nameTTB.Text && tov.manufacture == manufactureTTB.Text)
                        {
                            MessageBox.Show("Товар данного производителя уже существует");
                            return;
                        }
                    }
                    МагазинЭлектроникиEntities.GetContext().Товар.Add(new Товар()
                    {
                        manufacture = manufactureTTB.Text,
                        name = nameTTB.Text,
                        price = pr,
                        type = typeTCB.Text,
                        number = num,
                        warrantyPeriod = warperTTB.Text
                    });
                    try
                    {
                        МагазинЭлектроникиEntities.GetContext().SaveChanges();
                        MessageBox.Show("Новый товар успешно добавлен!");
                        ResetBut_Click(sender, e);
                        dataGridt.ItemsSource = МагазинЭлектроникиEntities.GetContext().Товар.ToList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    updateTCB();
                    return;
                }
                else
                {
                    MessageBox.Show("Количество и цена должны быть числами!!!");
                    return;
                }
            }
            else
            {
                MessageBox.Show("Не все поля заполнены!!!");
                return;
            }
        }

        private void ResetBut_Click(object sender, RoutedEventArgs e)
        {
            nameTTB.Text = "";
            manufactureTTB.Text = "";
            typeTCB.Text = "";
            priceTTB.Text = "";
            numberTTB.Text = "";
            warperTTB.Text = "";
            IsEditTov = false;
        }

        private void BtnEditT_Click(object sender, RoutedEventArgs e)
        {
            CurrentTovar = (sender as Button).DataContext as Товар;
            manufactureTTB.Text = CurrentTovar.manufacture;
            nameTTB.Text = CurrentTovar.name;
            priceTTB.Text = CurrentTovar.price.ToString();
            typeTCB.Text = CurrentTovar.type;
            numberTTB.Text = CurrentTovar.number.ToString();
            warperTTB.Text = CurrentTovar.warrantyPeriod;
            IsEditTov = true;
        }

        private void BtnDelT_Click(object sender, RoutedEventArgs e)
        {
            CurrentTovar = (sender as Button).DataContext as Товар;
            if (MessageBox.Show($"Вы точно хотите удалить {CurrentTovar.name} производителя {CurrentTovar.manufacture}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                for (int j = 0; j < МагазинЭлектроникиEntities.GetContext().Поставка.ToList().Count; j++)
                {
                    Поставка поставка = МагазинЭлектроникиEntities.GetContext().Поставка.ToList()[j];
                    if (поставка.Товар == CurrentTovar)
                    {
                        МагазинЭлектроникиEntities.GetContext().Поставка.Remove(поставка);
                    }
                }
                МагазинЭлектроникиEntities.GetContext().Товар.Remove(CurrentTovar);
                МагазинЭлектроникиEntities.GetContext().SaveChanges();
                filldatagrid();
                dataGridt.ItemsSource = МагазинЭлектроникиEntities.GetContext().Товар.ToList();
                MessageBox.Show("Товар успешно удален!");
                updateTCB();
            }
        }
        private void manufTPTB_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (manufTPTB.SelectedItem == null)
                return;
            nameTPTB.Text = "";
            nameTPTB.Items.Clear();
            foreach (Товар товар in МагазинЭлектроникиEntities.GetContext().Товар)
            {
                if (manufTPTB.SelectedItem.ToString() == товар.manufacture)
                    nameTPTB.Items.Add(товар.name);

            }
        }
        private bool IsEditPost = false;
        private Поставка currentpost = null;
        private void ResetTP_Click(object sender, RoutedEventArgs e)
        {
            nameTPTB.Items.Clear();
            nameTPTB.Text = "";
            manufTPTB.Text = "";
            dateTPTB.SelectedDate = null;
            StatusTPTB.Text = "";
            numberTPTB.Text = "";
            IsEditPost = false;
        }

        private void BtnEditP_Click(object sender, RoutedEventArgs e)
        {
            Pos1 pos = (sender as Button).DataContext as Pos1;

            foreach (Поставка поставка in МагазинЭлектроникиEntities.GetContext().Поставка)
            {
                if(pos.id == поставка.id)
                {
                    currentpost = поставка;
                    break;
                }

            }

            manufTPTB.Text = currentpost.Товар.manufacture;
            nameTPTB.Items.Clear();
            foreach (Товар товар in МагазинЭлектроникиEntities.GetContext().Товар)
            {
                if (manufTPTB.SelectedItem.ToString() == товар.manufacture)
                    nameTPTB.Items.Add(товар.name);

            }
            nameTPTB.Text = currentpost.Товар.name;
            StatusTPTB.Text = currentpost.status;
            dateTPTB.SelectedDate = currentpost.date;
            numberTPTB.Text = currentpost.countTov.ToString();
            IsEditPost = true;
        }

        private void SaveOrCreateTP_Click(object sender, RoutedEventArgs e)
        {
            if (nameTPTB.Text != "" && manufTPTB.Text != "" && dateTPTB.SelectedDate != null && StatusTPTB.Text != "" && numberTPTB.Text != "")
            {
                if (Int32.TryParse(numberTPTB.Text, out int num))
                {
                    Товар changedTov = null;
                    foreach (Товар товар in МагазинЭлектроникиEntities.GetContext().Товар)
                    {
                        if (manufTPTB.Text == товар.manufacture && nameTPTB.Text == товар.name)
                        {
                            changedTov = товар;
                            break;
                        }
                    }
                    if (IsEditPost)
                    {
                        for (int j = 0; j < МагазинЭлектроникиEntities.GetContext().Поставка.ToList().Count; j++)
                        {
                            Поставка поставка = МагазинЭлектроникиEntities.GetContext().Поставка.ToList()[j];
                            if (поставка.id == currentpost.id)
                            {
                                if(поставка.status == "Получена")
                                {
                                    MessageBox.Show("Поставка отмечена как полученая ее дальнейшее изменение невозможно!!!");
                                    return;
                                }
                                поставка.countTov = num;
                                поставка.date = dateTPTB.SelectedDate;
                                поставка.idTov = changedTov.id;
                                поставка.Товар = changedTov;
                                поставка.status = StatusTPTB.Text;
                                if (StatusTPTB.Text == "Получена")
                                {
                                    if (MessageBox.Show($"Вы точно хотите изменить статус товара {currentpost.Товар.name} производителя {currentpost.Товар.manufacture}, датйо прибытия {((DateTime)currentpost.date).ToShortDateString()} на статус прибыла?(Изменить поставку более будет невозможно)", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                    {
                                        currentpost.Товар.number += currentpost.countTov;
                                    }
                                }
                                МагазинЭлектроникиEntities.GetContext().SaveChanges();
                                dataGridt.ItemsSource = МагазинЭлектроникиEntities.GetContext().Товар.ToList();
                                filldatagrid();
                                MessageBox.Show("Данные успешно изменены!");
                                ResetTP_Click(sender, e);
                                CurrentTovar = null;
                                return;
                            }
                        }
                    }

                    МагазинЭлектроникиEntities.GetContext().Поставка.Add(new Поставка()
                    {
                        countTov = num, date = dateTPTB.SelectedDate, status = StatusTPTB.Text, idTov = changedTov.id, Товар = changedTov
                    }); ;
                    try
                    {
                        МагазинЭлектроникиEntities.GetContext().SaveChanges();
                        MessageBox.Show("Новый поставка успешно добавлена!");
                        filldatagrid();
                        ResetTP_Click(sender, e);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    return;
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
            Pos1 pos = (sender as Button).DataContext as Pos1;

            foreach (Поставка поставка in МагазинЭлектроникиEntities.GetContext().Поставка)
            {
                if (pos.id == поставка.id)
                {
                    currentpost = поставка;
                    break;
                }
            }
            if (MessageBox.Show($"Вы точно хотите удалить {currentpost.Товар.name} производителя {currentpost.Товар.manufacture} статуса {currentpost.status}", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                МагазинЭлектроникиEntities.GetContext().Поставка.Remove(currentpost);
                МагазинЭлектроникиEntities.GetContext().SaveChanges();
                filldatagrid();
                MessageBox.Show("Поставка успешно удалена!");
            }
        }
    }

    class Pos1
    {
        public int id { get; set; }
        public string name { get; set; }
        public string manufacture { get; set; }
        public string date { get; set; }
        public string status { get; set; }
        public int countTov { get; set; }
    }
}