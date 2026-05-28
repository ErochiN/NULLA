using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace KWIZY
{
    /// <summary>
    /// Логика взаимодействия для Registr.xaml
    /// </summary>
    public partial class Registr : Page
    {
        private int _port = DataServer.PORT;
        public Registr()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainFrame = (MainWindow)Application.Current.MainWindow;
            MainFrame.MainFrame.Navigate(new MainPage());
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string userName = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string nickname = NicknameTextBox.Text;

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
            {
                MessageBox.Show("Пароли не совпадают. Пожалуйста, введите одинаковые пароли.");
                return;
            }
            string password = PasswordBox.Password;

            string dateOfBirth = DateOfBirthDatePicker.Text;

            var user = new
            {
                userName = userName,
                email = email,
                nickname = nickname,
                password = password,
                dateOfBirth = dateOfBirth
            };

            string json = JsonSerializer.Serialize(user);
            StringContent stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpClient client = new HttpClient();
            var response = await client.PostAsync($"http://{DataServer.IP_ADDRESS}:{DataServer.PORT}/register", stringContent);
        }
    }
}
