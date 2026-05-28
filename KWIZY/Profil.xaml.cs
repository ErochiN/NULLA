using System;
using System.Collections.Generic;
using System.Net.Http;
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
    /// Логика взаимодействия для Profil.xaml
    /// </summary>
    public partial class Profil : Page
    {
        public Profil()
        {
            InitializeComponent();

            GetUserData();
        }

        private async void GetUserData()
        {
            var dataprofil = new
            {
                userId = DataUsers.id,
                token = DataUsers.tocken
            };

            StringContent content = new StringContent(JsonSerializer.Serialize(dataprofil), Encoding.UTF8, "application/json");

            HttpClient httpClient = new HttpClient();
            var response = await httpClient.PostAsync(new Uri($"http://{DataServer.IP_ADDRESS}:{DataServer.PORT}/userdata"), content);

            string jsonString = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonString);
            var root = jsonDoc.RootElement;

            Username.Text = root.GetProperty("username").GetString();
            Nickname.Text = root.GetProperty("nickname").GetString();
        }

        private void message_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainFrame = (MainWindow)Application.Current.MainWindow;
            MainFrame.MainFrame.Navigate(new mess());
        }

        private void frineds_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainFrame = (MainWindow)Application.Current.MainWindow;
            MainFrame.MainFrame.Navigate(new Friends());
        }

        private void profil_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainFrame = (MainWindow)Application.Current.MainWindow;
            MainFrame.MainFrame.Navigate(new Profil());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
