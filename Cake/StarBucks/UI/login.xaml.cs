using StarBucks.Network;
using System;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Threading.Tasks;

namespace StarBucks.UI
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        static socket networkClient;
        Main main;
        notify notify;

        public login()
        {
            InitializeComponent();
            App.login = this;
            notify = new notify(this, ToastNotifications.Position.Corner.TopLeft);
        }

        #region Events
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Task.Run(delegate
            {
                App.Current.Dispatcher.Invoke(new Action(delegate
                {
                    App.loginID = null;
                    this.id.IsEnabled = false;
                    this.server.IsEnabled = false;
                    this.login_btn.IsEnabled = false;
                }));


                try
                {
                    App.Current.Dispatcher.Invoke(new Action(delegate
                    {
                            // Dispose
                            if (App.socketController != null)
                        {
                            App.socketController.Dispose();
                        }

                        networkClient = new socket(this.server.Text.Split(':')[0], Convert.ToInt32(this.server.Text.Split(':')[1]));
                        App.socketController = networkClient;

                        networkClient.messageEvent += NetworkClient_messageEvent;
                        networkClient.sendMessage("@" + this.id.Text);

                        RegistryKey reg = Registry.CurrentUser;
                        reg.CreateSubKey("DIOSK").SetValue("serverIP", this.server.Text);
                    }));
                }
                catch (Exception ex)
                {
                    notify.showError("서버 연결에 실패하였습니다. 인터넷 연결 및 서버 주소를 확인하세요.");
                }

                App.Current.Dispatcher.Invoke(new Action(delegate
                {
                    this.id.IsEnabled = true;
                    this.server.IsEnabled = true;
                    this.login_btn.IsEnabled = true;
                }));
            });
        }

        private void NetworkClient_messageEvent(object sender, messageArgs e)
        {
            if (App.loginID == null || App.loginID == string.Empty)
            {
                if (e.message.Equals("200"))
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        main = new Main();
                        App.main = main;
                        App.loginID = this.id.Text;
                        main.Show();
                        this.Hide();
                    }));
                }
                else
                {
                    this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        this.id.IsEnabled = true;
                        this.server.IsEnabled = true;
                    }));

                    notify.showError("아이디를 확인하세요.");
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            // 자원 정리
            notify.Dispose();
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // 저장된 서버 IP를 불러옴
            RegistryKey reg = Registry.CurrentUser;
            reg = reg.OpenSubKey("DIOSK");

            if (reg != null)
            {
                Object val = reg.GetValue("serverIP");
                if (val != null)
                {
                    this.server.Text = val.ToString();
                }
            }
        }
        #endregion

        #region PlaceHolder Setting
        private string id_placeHolder = "ID";
        private string server_placeHolder = "서버주소";
        private void Id_GotFocus(object sender, RoutedEventArgs e)
        {
            if (id.Text == id_placeHolder)
            {
                id.Text = "";
            }
        }

        private void Id_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(id.Text))
            {
                id.Text = id_placeHolder;
            }
        }

        private void Server_GotFocus(object sender, RoutedEventArgs e)
        {
            if (server.Text == server_placeHolder)
            {
                server.Text = "";
            }
        }

        private void Server_LostFocus(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(server.Text))
            {
                server.Text = server_placeHolder;
            }
        }
        #endregion

        #region Offline Startup
        private void Offline_login_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // 오프라인 판매 등록
            main = new Main();
            App.main = main;
            main.Show();
            this.Hide();
        }
        #endregion
    }
}
