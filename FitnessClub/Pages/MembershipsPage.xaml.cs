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
    /// Логика взаимодействия для MembershipsPage.xaml
    /// </summary>
    public partial class MembershipsPage : Page
    {
        public MembershipsPage()
        {
            InitializeComponent();
            LoadTypes();
            LoadClientMemberships();
        }
        private void LoadTypes()
        {
            dgTypes.ItemsSource = Connect.db.MembershipTypes.ToList();
        }

        private void LoadClientMemberships()
        {
            dgClientMemberships.ItemsSource = Connect.db.ClientMemberships.ToList();
        }

        // Типы абонементов
        private void btnAddType_Click(object sender, RoutedEventArgs e)
        {
            AddEditMembershipTypeWindow window = new AddEditMembershipTypeWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true) LoadTypes();
        }

        private void btnEditType_Click(object sender, RoutedEventArgs e)
        {
            if (dgTypes.SelectedItem == null) return;
            var selected = dgTypes.SelectedItem as MembershipTypes;
            AddEditMembershipTypeWindow window = new AddEditMembershipTypeWindow(selected);
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true) LoadTypes();
        }

        private void btnDeleteType_Click(object sender, RoutedEventArgs e)
        {
            if (dgTypes.SelectedItem == null) return;
            var selected = dgTypes.SelectedItem as MembershipTypes;
            if (MessageBox.Show($"Удалить тип абонемента '{selected.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Connect.db.MembershipTypes.Remove(selected);
                Connect.db.SaveChanges();
                LoadTypes();
            }
        }

        // Абонементы клиентов
        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            SellMembershipWindow window = new SellMembershipWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true) LoadClientMemberships();
        }

        private void btnFreeze_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientMemberships.SelectedItem == null) return;
            var selected = dgClientMemberships.SelectedItem as ClientMemberships;
            // Простая заморозка: меняем статус (в реальности нужно окно с датами)
            selected.Status = "Заморожен";
            Connect.db.SaveChanges();
            LoadClientMemberships();
        }

        private void btnUnfreeze_Click(object sender, RoutedEventArgs e)
        {
            if (dgClientMemberships.SelectedItem == null)
            {
                MessageBox.Show("Выберите абонемент для разморозки");
                return;
            }

            var selected = dgClientMemberships.SelectedItem as ClientMemberships;
            if (selected.Status != "Заморожен")
            {
                MessageBox.Show("Можно разморозить только абонемент со статусом 'Заморожен'");
                return;
            }

            selected.Status = "Активен";
            Connect.db.SaveChanges();
            LoadClientMemberships();
        }
    }
}
