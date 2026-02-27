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
    /// Логика взаимодействия для TrainersPage.xaml
    /// </summary>
    public partial class TrainersPage : Page
    {
        public TrainersPage()
        {
            InitializeComponent();
            if (App.CurrentUser.RoleId == 3) // Тренер не может управлять тренерами
            {
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
            }
            LoadData();
        }

        private void LoadData()
        {
            var trainers = Connect.db.Trainers.ToList();
            dgTrainers.ItemsSource = trainers;
            lblTotal.Text = $"Всего записей: {trainers.Count}";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditTrainerWindow window = new AddEditTrainerWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgTrainers.SelectedItem == null) return;
            var selected = dgTrainers.SelectedItem as Trainers;
            AddEditTrainerWindow window = new AddEditTrainerWindow(selected);
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTrainers.SelectedItem == null) return;
            var selected = dgTrainers.SelectedItem as Trainers;
            if (MessageBox.Show($"Удалить тренера {selected.LastName} {selected.FirstName}?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Connect.db.Trainers.Remove(selected);
                Connect.db.SaveChanges();
                LoadData();
            }
        }

        private void PerformSearch()
        {
            string search = txtSearch.Text.ToLower();
            var trainers = Connect.db.Trainers.Where(t =>
                t.LastName.ToLower().Contains(search) ||
                t.FirstName.ToLower().Contains(search) ||
                (t.Specialization != null && t.Specialization.ToLower().Contains(search))
            ).ToList();
            dgTrainers.ItemsSource = trainers;
            lblTotal.Text = $"Найдено: {trainers.Count}";
        }

        private void txtSearch_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter) PerformSearch();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            PerformSearch();
        }
    }
}
