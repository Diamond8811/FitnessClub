using FitnessClub.Pages;
using FitnessClub.Windows;
using System;
using System.Collections.Generic;
using System.Configuration;
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

namespace FitnessClub
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadMenu();
        }

        private void LoadMenu()
        {
            if (App.CurrentUser == null)
            {
                LoginWindow login = new LoginWindow();
                login.Show();
                this.Close();
                return;
            }

            var roleId = App.CurrentUser.RoleId;
            List<string> menuItems = new List<string>();

            menuItems.Add("Клиенты");
            menuItems.Add("Абонементы");
            menuItems.Add("Посещения");
            menuItems.Add("Тренеры");
            menuItems.Add("Занятия");
            menuItems.Add("Оплаты");

            if (roleId == 1) // Администратор
                menuItems.Add("Настройки");

            menuListBox.ItemsSource = menuItems;
            if (menuItems.Count > 0)
                menuListBox.SelectedIndex = 0;
        }

        private void menuListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (menuListBox.SelectedItem == null) return;
            string selected = menuListBox.SelectedItem.ToString();

            switch (selected)
            {
                case "Клиенты":
                    mainFrame.Navigate(new ClientsPage());
                    break;
                case "Абонементы":
                    mainFrame.Navigate(new MembershipsPage());
                    break;
                case "Посещения":
                    mainFrame.Navigate(new VisitsPage());
                    break;
                case "Тренеры":
                    mainFrame.Navigate(new TrainersPage());
                    break;
                case "Занятия":
                    mainFrame.Navigate(new WorkoutsPage());
                    break;
                case "Оплаты":
                    mainFrame.Navigate(new PaymentsPage());
                    break;
                case "Настройки":
                    mainFrame.Navigate(new SettingsPage());
                    break;
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            LoginWindow login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
