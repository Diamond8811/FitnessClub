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
    /// Логика взаимодействия для PaymentsPage.xaml
    /// </summary>
    public partial class PaymentsPage : Page
    {
        public PaymentsPage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            var payments = Connect.db.Payments.ToList();
            dgPayments.ItemsSource = payments;
            lblTotal.Text = $"Всего записей: {payments.Count}";
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddPaymentWindow window = new AddPaymentWindow();
            window.Owner = Window.GetWindow(this);
            if (window.ShowDialog() == true)
                LoadData();
        }
    }
}
