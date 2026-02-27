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
    /// Логика взаимодействия для AddEditUserWindow.xaml
    /// </summary>
    public partial class AddEditUserWindow : Window
    {
        private Users _user;
        private bool _isNew;

        public AddEditUserWindow(Users user = null)
        {
            InitializeComponent();
            cmbRole.ItemsSource = Connect.db.Roles.ToList();

            _user = user ?? new Users();
            _isNew = user == null;

            if (!_isNew)
            {
                txtLogin.Text = _user.Login;
                txtFullName.Text = _user.FullName;
                cmbRole.SelectedValue = _user.RoleId;
                chkIsActive.IsChecked = _user.IsActive;
                // Пароль не заполняем при редактировании (оставляем пустым)
                txtPassword.IsEnabled = false;
                txtPasswordConfirm.IsEnabled = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLogin.Text))
            {
                MessageBox.Show("Введите логин");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Введите ФИО");
                return;
            }

            if (cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Выберите роль");
                return;
            }

            // Проверка пароля только для нового пользователя
            if (_isNew)
            {
                if (string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    MessageBox.Show("Введите пароль");
                    return;
                }

                if (txtPassword.Password != txtPasswordConfirm.Password)
                {
                    MessageBox.Show("Пароли не совпадают");
                    return;
                }
            }

            _user.Login = txtLogin.Text.Trim();
            _user.FullName = txtFullName.Text.Trim();
            _user.RoleId = (int)cmbRole.SelectedValue;
            _user.IsActive = chkIsActive.IsChecked ?? true;

            if (_isNew)
            {
                // Без хеширования, сохраняем как есть
                _user.PasswordHash = txtPassword.Password;
                Connect.db.Users.Add(_user);
            }
            else
            {
                // При редактировании пароль не меняем (можно добавить опцию смены пароля отдельно)
                // Если нужно разрешить смену пароля, нужно добавить логику.
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
