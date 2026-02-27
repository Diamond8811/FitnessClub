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
    /// Логика взаимодействия для AddEditTrainerWindow.xaml
    /// </summary>
    public partial class AddEditTrainerWindow : Window
    {
        private Trainers _trainer;

        public AddEditTrainerWindow(Trainers trainer = null)
        {
            InitializeComponent();
            _trainer = trainer ?? new Trainers();

            if (trainer != null)
            {
                txtLastName.Text = trainer.LastName;
                txtFirstName.Text = trainer.FirstName;
                txtMiddleName.Text = trainer.MiddleName;
                txtSpecialization.Text = trainer.Specialization;
                txtPhone.Text = trainer.Phone;
                txtEmail.Text = trainer.Email;
                dpHireDate.SelectedDate = trainer.HireDate;
                chkIsActive.IsChecked = trainer.IsActive;
            }
            else
            {
                dpHireDate.SelectedDate = DateTime.Today;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Заполните фамилию, имя и телефон");
                return;
            }

            _trainer.LastName = txtLastName.Text.Trim();
            _trainer.FirstName = txtFirstName.Text.Trim();
            _trainer.MiddleName = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? null : txtMiddleName.Text.Trim();
            _trainer.Specialization = string.IsNullOrWhiteSpace(txtSpecialization.Text) ? null : txtSpecialization.Text.Trim();
            _trainer.Phone = txtPhone.Text.Trim();
            _trainer.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            _trainer.HireDate = dpHireDate.SelectedDate ?? DateTime.Today;
            _trainer.IsActive = chkIsActive.IsChecked ?? true;

            if (_trainer.Id == 0)
                Connect.db.Trainers.Add(_trainer);

            Connect.db.SaveChanges();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
