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
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        private int _port = 5000;
        public MainPage()
        {
            InitializeComponent();
        }

        private async void SignIn_Click(object sender, RoutedEventArgs e)
        {
            string loginUser = LoginTextBox.Text;
            string passwordUser = PasswordBox.Password;

            var user = new
            {
                loginUser = loginUser,
                passwordUser = passwordUser
            };

            string json = JsonSerializer.Serialize(user);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

            using HttpClient client = new HttpClient();
            var response = await client.PostAsync($"http://{DataServer.IP_ADDRESS}:{DataServer.PORT}/signin", content);
            string responseContent = await response.Content.ReadAsStringAsync();

            var jsonDoc = JsonDocument.Parse(responseContent);
            var root = jsonDoc.RootElement;

            if (root.GetProperty("status").GetString() == "OK")
            {
                DataUsers.tocken = root.GetProperty("token").GetString();
                DataUsers.id = root.GetProperty("userId").GetInt64();

                MainWindow MainFrame = (MainWindow)Application.Current.MainWindow;
                MainFrame.MainFrame.Navigate(new mess());
            }
            else
            {
                MessageBox.Show("Неверный логин или пароль. Попробуйте снова.");
            }
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainFrame = (MainWindow)Application.Current.MainWindow;
            MainFrame.MainFrame.Navigate(new Registr());
        }
    }
}
