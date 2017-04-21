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
using Microsoft.AspNet.SignalR.Client;
using SignalRChat.ServiceModel;

namespace SignalRChat.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string HOST = "http://localhost:27611/";
        private readonly string HUB_NAME = "ChatHub";
        private readonly string QS_USERNAME = "username";

        private HubConnection _connection;
        private IHubProxy _hub;
        private readonly IDictionary<string, string> _queryStringData;
        private string _username;

        public MainWindow()
        {
            InitializeComponent();
            _queryStringData = new Dictionary<string, string>();
        }

        private async void InitConnection(string username)
        {
            _queryStringData[QS_USERNAME] = username;
            _connection = new HubConnection(HOST, _queryStringData);
            _hub = _connection.CreateHubProxy(HUB_NAME);

            if (_hub == null)
            {
                throw new NullReferenceException("You must define a hub");
            }

            _hub.On<ChatMessage>("BroadcastMessage", OnBroadcastMessage);
            _hub.On<ChatMessage>("DirectMessage", OnDirectMessage);

            await _connection.Start();
        }

        #region Events

        #region Commands
        private async void btnSendBroadcast_Click(object sender, RoutedEventArgs e)
        {
            var message = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Message cannot be empty");
                return;
            }

            var request = new ChatMessage
            {
                FromUsername = _username,
                Message = message
            };

            await _hub.Invoke("SendBroadcastMessage", request);
            await ClearMessage();
        }

        private async void btnSendDirect_Click(object sender, RoutedEventArgs e)
        {
            var message = txtMessage.Text.Trim();
            var sendTo = txtSendTo.Text.Trim();

            if (string.IsNullOrEmpty(message))
            {
                MessageBox.Show("Message cannot be empty");
                return;
            }

            if (string.IsNullOrEmpty(sendTo))
            {
                MessageBox.Show("You must provide a Send To username for Direct Messages");
                return;
            }

            var request = new ChatMessage
            {
                FromUsername = _username,
                Message = message
            };

            await _hub.Invoke("SendDirectMessage", sendTo, request);
            await ClearMessage();
            await ClearSendTo();
        }

        #endregion

        #region Window

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var usernameDialog = new UsernameDialog { Owner = this };
            usernameDialog.ShowDialog();
            _username = usernameDialog.Username;

            await SetUsername(_username);

            InitConnection(_username);
        }

        #endregion

        #region SignalR

        private async void OnBroadcastMessage(ChatMessage message)
        {
            await WriteToConsole(message, false);
        }

        private async void OnDirectMessage(ChatMessage message)
        {
            await WriteToConsole(message, true);
        }

        #endregion

        #endregion

        #region Dispatchers

        private async Task SetUsername(string username)
        {
            if (!Dispatcher.CheckAccess())
            {
                await Dispatcher.InvokeAsync(() => SetUsername(username));
            }
            else
            {
                txtUsername.Text = username;
            }
        }

        private async Task ClearSendTo()
        {
            if (!Dispatcher.CheckAccess())
            {
                await Dispatcher.InvokeAsync(ClearSendTo);
            }
            else
            {
                txtSendTo.Text = string.Empty;
            }
        }

        private async Task ClearMessage()
        {
            if (!Dispatcher.CheckAccess())
            {
                await Dispatcher.InvokeAsync(ClearMessage);
            }
            else
            {
                txtMessage.Text = string.Empty;
                txtMessage.Focus();
            }
        }

        private async Task WriteToConsole(ChatMessage message, bool isDirectMessage)
        {
            if (!Dispatcher.CheckAccess())
            {
                await Dispatcher.InvokeAsync(() => WriteToConsole(message, isDirectMessage));
            }
            else
            {
                var prefix = (isDirectMessage) ? "[DM]" : "[+]";
                var output = $"{prefix} {message.FromUsername}: {message.Message}";
                listBox.Items.Add(output);
            }
        }

        #endregion

    }
}
