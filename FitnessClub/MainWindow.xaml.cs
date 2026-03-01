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
            if (App.CurrentUser?.RoleId != 1)
            {
                btnSettings.Visibility = Visibility.Collapsed;
            }
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new ClientsPage());
        }

        private void btnMemberships_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new MembershipsPage());
        }

        private void btnVisits_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new VisitsPage());
        }

        private void btnTrainers_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new TrainersPage());
        }

        private void btnWorkouts_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new WorkoutsPage());
        }

        private void btnPayments_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new PaymentsPage());
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Navigate(new SettingsPage());
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUser = null;
            LoginWindow login = new LoginWindow();
            login.Show();
            Close();
        }
    }
}
