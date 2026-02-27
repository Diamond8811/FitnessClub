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
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        public ClientsPage()
        {
            InitializeComponent();
            if (App.CurrentUser.RoleId == 3) // Тренер
            {
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
            }
            LoadData();
        }
        private void LoadData()
        {
            var clients = Connect.db.Clients.ToList();
            dgClients.ItemsSource = clients;
            lblTotal.Text = clients.Count.ToString();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditClientWindow window = new AddEditClientWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для редактирования");
                return;
            }
            Clients selected = dgClients.SelectedItem as Clients;
            AddEditClientWindow window = new AddEditClientWindow(selected);
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента для удаления");
                return;
            }
            Clients selected = dgClients.SelectedItem as Clients;
            if (MessageBox.Show($"Удалить клиента {selected.LastName} {selected.FirstName}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                if (Connect.db.ClientMemberships.Any(cm => cm.ClientId == selected.Id))
                {
                    MessageBox.Show("Нельзя удалить клиента, у которого есть абонементы.");
                    return;
                }
                Connect.db.Clients.Remove(selected);
                Connect.db.SaveChanges();
                LoadData();
            }
        }

        private void txtSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                PerformSearch();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }

        private void PerformSearch()
        {
            string search = txtSearch.Text.ToLower();
            var clients = Connect.db.Clients.Where(c =>
                c.LastName.ToLower().Contains(search) ||
                c.FirstName.ToLower().Contains(search) ||
                (c.Phone != null && c.Phone.Contains(search)) ||
                (c.Email != null && c.Email.ToLower().Contains(search))
            ).ToList();
            dgClients.ItemsSource = clients;
            lblTotal.Text = clients.Count.ToString();
        }
    }
}
