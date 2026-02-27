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
    /// Логика взаимодействия для AddEditClientWindow.xaml
    /// </summary>
    public partial class AddEditClientWindow : Window
    {
        private Clients _currentClient;

        public AddEditClientWindow(Clients client = null)
        {
            InitializeComponent();
            _currentClient = client ?? new Clients();

            if (client != null)
            {
                txtLastName.Text = client.LastName;
                txtFirstName.Text = client.FirstName;
                txtMiddleName.Text = client.MiddleName;
                dpBirthDate.SelectedDate = client.BirthDate;
                if (client.Gender == "M")
                    cmbGender.SelectedIndex = 0;
                else if (client.Gender == "F")
                    cmbGender.SelectedIndex = 1;
                txtPhone.Text = client.Phone;
                txtEmail.Text = client.Email;
                cmbStatus.Text = client.Status;
                txtNotes.Text = client.Notes;
            }
            else
            {
                cmbStatus.SelectedIndex = 0; // Активен
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

            _currentClient.LastName = txtLastName.Text.Trim();
            _currentClient.FirstName = txtFirstName.Text.Trim();
            _currentClient.MiddleName = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? null : txtMiddleName.Text.Trim();
            _currentClient.BirthDate = dpBirthDate.SelectedDate;
            _currentClient.Gender = cmbGender.SelectedIndex == 0 ? "M" : "F";
            _currentClient.Phone = txtPhone.Text.Trim();
            _currentClient.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            _currentClient.Status = cmbStatus.Text;
            _currentClient.Notes = string.IsNullOrWhiteSpace(txtNotes.Text) ? null : txtNotes.Text.Trim();

            if (_currentClient.Id == 0)
            {
                _currentClient.RegistrationDate = DateTime.Now.Date;
                Connect.db.Clients.Add(_currentClient);
            }

            Connect.db.SaveChanges();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
