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
    /// Логика взаимодействия для SellMembershipWindow.xaml
    /// </summary>
    public partial class SellMembershipWindow : Window
    {
        public SellMembershipWindow()
        {
            InitializeComponent();

            var clients = Connect.db.Clients.ToList()
                .Select(c => new { Id = c.Id, FullName = $"{c.LastName} {c.FirstName} {c.MiddleName}".Trim() }).ToList();
            cmbClient.ItemsSource = clients;
            cmbClient.DisplayMemberPath = "FullName";
            cmbClient.SelectedValuePath = "Id";

            cmbType.ItemsSource = Connect.db.MembershipTypes.Where(m => m.IsActive == true).ToList();
            cmbType.DisplayMemberPath = "Name";
            cmbType.SelectedValuePath = "Id";

            dpStart.SelectedDate = DateTime.Today;
        }

        private void btnSell_Click(object sender, RoutedEventArgs e)
        {
            if (cmbClient.SelectedItem == null || cmbType.SelectedItem == null)
            {
                MessageBox.Show("Выберите клиента и тип абонемента");
                return;
            }

            int clientId = (int)cmbClient.SelectedValue;
            var type = cmbType.SelectedItem as MembershipTypes;
            DateTime start = dpStart.SelectedDate ?? DateTime.Today;
            DateTime end = start.AddDays(type.DurationDays);

            ClientMemberships cm = new ClientMemberships
            {
                ClientId = clientId,
                MembershipTypeId = type.Id,
                StartDate = start,
                EndDate = end,
                RemainingVisits = type.VisitsCount,
                Status = "Активен"
            };
            Connect.db.ClientMemberships.Add(cm);

            Payments payment = new Payments
            {
                ClientId = clientId,
                PaymentDate = DateTime.Now,
                Amount = type.Price,
                PaymentMethod = "Наличные",
                ClientMemberships = cm,
                Notes = "Оплата абонемента"
            };
            Connect.db.Payments.Add(payment);

            Connect.db.SaveChanges();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
