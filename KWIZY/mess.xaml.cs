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
using System.Windows.Threading;


namespace KWIZY
{
    /// <summary>
    /// Логика взаимодействия для mess.xaml
    /// </summary>
    /// 

    class Message
    {
        public long Id { get; set; }
        public long FriendId { get; set; }
        public long UserId { get; set; }
        public string Messages { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public partial class mess : Page
    {
        private ObservableCollection<AllUsers> userFriendsList = new ObservableCollection<AllUsers>();
        private ObservableCollection<Message> Messages = new ObservableCollection<Message>();
        private DispatcherTimer timer = new DispatcherTimer();

        AllUsers activeChat = new AllUsers();

        public mess()
        {
            InitializeComponent();
            GetFriend();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();

        }

        private async void Timer_Tick(object? sender, EventArgs e)
        {
            if (activeChat.Id != 0)
            {
                UpdateMessages();
            }
        }

        private async void GetFriend()
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

            FrendListBox.ItemsSource = userFriendsList;
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

        private void Profil_Click(object sender, RoutedEventArgs e)
        {
            MainWindow MainFrame = (MainWindow)Application.Current.MainWindow;
            MainFrame.MainFrame.Navigate(new Profil());
        }

        private async void puch_massage_buttom(object sender, RoutedEventArgs e)
        {
            var content = new
            {
                userId = DataUsers.id,
                friendsId = activeChat.Id,
                token = DataUsers.tocken,
                message = MessageTextBox.Text
            };

            StringContent stringContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

            HttpClient client = new HttpClient();
            var response = await client.PostAsync(new Uri($"http://{DataServer.IP_ADDRESS}:{DataServer.PORT}/sendmessage"), stringContent);

            UpdateMessages();
        }

        private async void AddFriendButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            activeChat = (AllUsers)button.DataContext;

            UpdateMessages();

        }

        private async void UpdateMessages()
        {
            var content = new
            {
                userId = DataUsers.id,
                friendsId = activeChat.Id,
                token = DataUsers.tocken
            };
            StringContent stringContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");
            
            HttpClient client = new HttpClient();
            var response = await client.PostAsync(new Uri($"http://{DataServer.IP_ADDRESS}:{DataServer.PORT}/getmessages"), stringContent);
            
            string jsonString = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(jsonString);
            var root = jsonDoc.RootElement;
            
            Messages.Clear();
            foreach (var message in root.EnumerateArray())
            {
                Messages.Add(new Message
                {
                    Id = message.GetProperty("Id").GetInt64(),
                    FriendId = message.GetProperty("FriendId").GetInt64(),
                    UserId = message.GetProperty("UserId").GetInt64(),
                    Messages = message.GetProperty("Message").GetString(),
                    Timestamp = message.GetProperty("Timestamp").GetDateTime()
                });
            }
            MessagesListBox.ItemsSource = Messages;
        }
    }
}
