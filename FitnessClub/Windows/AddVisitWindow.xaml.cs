using FitnessClub.Connection;
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

namespace FitnessClub.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddVisitWindow.xaml
    /// </summary>
    public partial class AddVisitWindow : Window
    {
        public AddVisitWindow()
        {
            InitializeComponent();

            // Загрузка клиентов с формированием FullName
            var clients = Connect.db.Clients.ToList()
                .Select(c => new
                {
                    Id = c.Id,
                    FullName = $"{c.LastName} {c.FirstName} {c.MiddleName}".Trim()
                }).ToList();
            cmbClient.ItemsSource = clients;
            cmbClient.DisplayMemberPath = "FullName";
            cmbClient.SelectedValuePath = "Id";

            // Загрузка занятий с добавлением пункта "без занятия"
            var workouts = Connect.db.Workouts.ToList()
                .Select(w => new { Id = (int?)w.Id, Name = w.Name }).ToList();
            workouts.Insert(0, new { Id = (int?)null, Name = "(без занятия)" });
            cmbWorkout.ItemsSource = workouts;
            cmbWorkout.DisplayMemberPath = "Name";
            cmbWorkout.SelectedValuePath = "Id";
            cmbWorkout.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbClient.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента");
                return;
            }

            if (dpDate.SelectedDate == null || !TimeSpan.TryParse(txtTime.Text, out TimeSpan time))
            {
                MessageBox.Show("Укажите корректные дату и время");
                return;
            }

            DateTime visitDateTime = dpDate.SelectedDate.Value.Add(time);

            int? workoutId = cmbWorkout.SelectedValue as int?;

            Visits visit = new Visits
            {
                ClientId = (int)cmbClient.SelectedValue,
                VisitDateTime = visitDateTime,
                WorkoutId = workoutId
            };

            Connect.db.Visits.Add(visit);
            Connect.db.SaveChanges();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
