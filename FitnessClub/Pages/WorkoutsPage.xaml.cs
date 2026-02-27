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
    /// Логика взаимодействия для WorkoutsPage.xaml
    /// </summary>
    public partial class WorkoutsPage : Page
    {
        public WorkoutsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var workouts = Connect.db.Workouts.ToList();
            dgWorkouts.ItemsSource = workouts;
            lblTotal.Text = $"Всего записей: {workouts.Count}";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditWorkoutWindow window = new AddEditWorkoutWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgWorkouts.SelectedItem == null) return;
            var selected = dgWorkouts.SelectedItem as Workouts;
            AddEditWorkoutWindow window = new AddEditWorkoutWindow(selected);
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
                LoadData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgWorkouts.SelectedItem == null) return;
            var selected = dgWorkouts.SelectedItem as Workouts;
            if (MessageBox.Show($"Удалить занятие '{selected.Name}'?", "Подтверждение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Connect.db.Workouts.Remove(selected);
                Connect.db.SaveChanges();
                LoadData();
            }
        }
    }
}
