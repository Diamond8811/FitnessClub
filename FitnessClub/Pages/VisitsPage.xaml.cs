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
    /// Логика взаимодействия для VisitsPage.xaml
    /// </summary>
    public partial class VisitsPage : Page
    {
        public VisitsPage()
        {
            InitializeComponent();
            // При необходимости можно скрыть кнопку для тренеров:
            // if (App.CurrentUser.RoleId == 3) btnAddVisit.Visibility = Visibility.Collapsed;
            LoadData();
        }

        private void LoadData()
        {
            var visits = Connect.db.Visits
                .OrderByDescending(v => v.VisitDateTime)
                .ToList();
            dgVisits.ItemsSource = visits;
            lblTotal.Text = $"Всего записей: {visits.Count}";
        }

        private void btnAddVisit_Click(object sender, RoutedEventArgs e)
        {
            AddVisitWindow window = new AddVisitWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void PerformSearch()
        {
            string search = txtSearch.Text.ToLower();
            var visits = Connect.db.Visits
                .Where(v =>
                    v.Clients.LastName.ToLower().Contains(search) ||
                    v.Clients.FirstName.ToLower().Contains(search) ||
                    (v.Clients.MiddleName != null && v.Clients.MiddleName.ToLower().Contains(search))
                )
                .OrderByDescending(v => v.VisitDateTime)
                .ToList();
            dgVisits.ItemsSource = visits;
            lblTotal.Text = $"Найдено: {visits.Count}";
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
    }
}
