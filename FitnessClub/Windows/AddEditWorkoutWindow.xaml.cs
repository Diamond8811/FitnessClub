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
    /// Логика взаимодействия для AddEditWorkoutWindow.xaml
    /// </summary>
    public partial class AddEditWorkoutWindow : Window
    {
        private Workouts _workout;

        public AddEditWorkoutWindow(Workouts workout = null)
        {
            InitializeComponent();
            cmbTrainer.ItemsSource = Connect.db.Trainers.Where(t => t.IsActive == true).ToList();
            _workout = workout ?? new Workouts();

            if (workout != null)
            {
                txtName.Text = workout.Name;
                txtDescription.Text = workout.Description;
                cmbTrainer.SelectedValue = workout.TrainerId;
                dpDate.SelectedDate = workout.WorkoutDateTime.Date;
                txtTime.Text = workout.WorkoutDateTime.ToString("HH:mm");
                txtDuration.Text = workout.DurationMinutes.ToString();
                txtMaxParticipants.Text = workout.MaxParticipants.ToString();
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                !int.TryParse(txtDuration.Text, out int duration) || duration <= 0 ||
                !int.TryParse(txtMaxParticipants.Text, out int max) || max <= 0 ||
                dpDate.SelectedDate == null ||
                !TimeSpan.TryParse(txtTime.Text, out TimeSpan time))
            {
                MessageBox.Show("Заполните корректно все обязательные поля");
                return;
            }

            DateTime dateTime = dpDate.SelectedDate.Value.Add(time);

            _workout.Name = txtName.Text.Trim();
            _workout.Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim();
            _workout.TrainerId = cmbTrainer.SelectedValue as int?;
            _workout.WorkoutDateTime = dateTime;
            _workout.DurationMinutes = duration;
            _workout.MaxParticipants = max;

            if (_workout.Id == 0)
                Connect.db.Workouts.Add(_workout);

            Connect.db.SaveChanges();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
