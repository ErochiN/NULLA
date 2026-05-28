using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для Friends.xaml
    /// </summary>
    /// 
    class AllUsers
    {
        public long Id;
        public string NickName { get; set; }
        public string UserName { get; set; }
    }
    public partial class Friends : Page
    {
        private ObservableCollection<AllUsers> friendsList = new ObservableCollection<AllUsers>();
        private ObservableCollection<AllUsers> userFriendsList = new ObservableCollection<AllUsers>();

        public Friends()
        {
            InitializeComponent();
        }

        private void Profil_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainFrame = (MainWindow)Application.Current.MainWindow;
            MainFrame.MainFrame.Navigate(new Profil());
        }

        private void friends_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainFrame = (MainWindow)Application.Current.MainWindow;
            MainFrame.MainFrame.Navigate(new Friends());
        }

        private void massage_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainFrame = (MainWindow)Application.Current.MainWindow;
            MainFrame.MainFrame.Navigate(new mess());
        }

        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();

            var content = new
            {
                searchquery = SearchTextBox.Text
            };

            string jsonContent = JsonSerializer.Serialize(content);
            StringContent httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(new Uri($"http://{DataServer.IP_ADDRESS}:{DataServer.PORT}/searchusers"), httpContent);

            string jsonString  = await response.Content.ReadAsStringAsync();

            var jsonDoc = JsonDocument.Parse(jsonString);
            var root = jsonDoc.RootElement;

            friendsList.Clear();

            foreach (var user in root.EnumerateArray())
            {
                friendsList.Add(new AllUsers
                {
                    Id = user.GetProperty("UserId").GetInt32(),
                    NickName = user.GetProperty("NickName").GetString(),
                    UserName = user.GetProperty("UserName").GetString()
                });
            }

            UserListBox.ItemsSource = friendsList;
        }

        private async void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            AllUsers allUsers = (AllUsers)button.DataContext;

            var content = new
            {
                userOneId = DataUsers.id,
                userTwoId = allUsers.Id,
                token = DataUsers.tocken
            };

            StringContent stringContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            var response = await client.PostAsync(new Uri($"http://{DataServer.IP_ADDRESS}:{DataServer.PORT}/addfriend"), stringContent);
        }

        private async void GetFriendsButton_Click(object sender, RoutedEventArgs e)
        {
            var content = new
            {
                userId = DataUsers.id,
                token = DataUsers.tocken
            };

            StringContent stringContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            var response = await client.PostAsync(new Uri($"http://{DataServer.IP_ADDRESS}:{DataServer.PORT}/getfriends"), stringContent);

            string jsonString = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonString);
            var root = jsonDoc.RootElement;

            userFriendsList.Clear();

            foreach (var user in root.EnumerateArray())
            {
                userFriendsList.Add(new AllUsers
                {
                    Id = user.GetProperty("UserId").GetInt32(),
                    NickName = user.GetProperty("NickName").GetString(),
                    UserName = user.GetProperty("UserName").GetString()
                });
            }

            FriendsListBox.ItemsSource = userFriendsList;
        }
    }
}
