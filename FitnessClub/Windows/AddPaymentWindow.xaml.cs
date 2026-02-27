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
    /// Логика взаимодействия для AddPaymentWindow.xaml
    /// </summary>
    public partial class AddPaymentWindow : Window
    {
        public AddPaymentWindow()
        {
            InitializeComponent();
            cmbClient.ItemsSource = Connect.db.Clients.ToList();
            cmbMethod.SelectedIndex = 0;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (cmbClient.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента");
                return;
            }
            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Введите корректную сумму");
                return;
            }
            if (dpDate.SelectedDate == null)
            {
                MessageBox.Show("Выберите дату");
                return;
            }

            Payments payment = new Payments
            {
                ClientId = (int)cmbClient.SelectedValue,
                PaymentDate = dpDate.SelectedDate.Value,
                Amount = amount,
                PaymentMethod = (cmbMethod.SelectedItem as ComboBoxItem)?.Content.ToString(),
                ClientMembershipId = int.TryParse(txtMembershipId.Text, out int mid) ? mid : (int?)null,
                Notes = null
            };

            Connect.db.Payments.Add(payment);
            Connect.db.SaveChanges();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}
