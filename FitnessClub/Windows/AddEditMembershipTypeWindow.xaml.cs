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
    /// Логика взаимодействия для AddEditMembershipTypeWindow.xaml
    /// </summary>
    public partial class AddEditMembershipTypeWindow : Window
    {
        private MembershipTypes _type;

        public AddEditMembershipTypeWindow(MembershipTypes type = null)
        {
            InitializeComponent();
            _type = type ?? new MembershipTypes();

            if (type != null)
            {
                txtName.Text = type.Name;
                txtDescription.Text = type.Description;
                txtDurationDays.Text = type.DurationDays.ToString();
                txtVisitsCount.Text = type.VisitsCount?.ToString();
                txtPrice.Text = type.Price.ToString("F2");
                chkIsActive.IsChecked = type.IsActive;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                !int.TryParse(txtDurationDays.Text, out int days) || days <= 0 ||
                !decimal.TryParse(txtPrice.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Заполните корректно название, срок действия и цену");
                return;
            }

            _type.Name = txtName.Text.Trim();
            _type.Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim();
            _type.DurationDays = days;
            _type.VisitsCount = int.TryParse(txtVisitsCount.Text, out int visits) ? visits : (int?)null;
            _type.Price = price;
            _type.IsActive = chkIsActive.IsChecked ?? true;

            if (_type.Id == 0)
                Connect.db.MembershipTypes.Add(_type);

            Connect.db.SaveChanges();
            DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e) => DialogResult = false;
    }
}