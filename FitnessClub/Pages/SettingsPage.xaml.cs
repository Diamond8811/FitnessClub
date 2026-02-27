using FitnessClub.Connection;
using FitnessClub.Windows;
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

namespace FitnessClub.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            dgUsers.ItemsSource = Connect.db.Users.ToList();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            AddEditUserWindow window = new AddEditUserWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true) LoadUsers();
        }

        private void btnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem == null) return;
            var selected = dgUsers.SelectedItem as Users;
            AddEditUserWindow window = new AddEditUserWindow(selected);
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true) LoadUsers();
        }

        private void btnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (dgUsers.SelectedItem == null) return;
            var selected = dgUsers.SelectedItem as Users;
            if (MessageBox.Show($"Удалить пользователя {selected.Login}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Connect.db.Users.Remove(selected);
                Connect.db.SaveChanges();
                LoadUsers();
            }
        }
    }
}
