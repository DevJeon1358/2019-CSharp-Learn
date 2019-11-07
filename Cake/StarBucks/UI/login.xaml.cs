using StarBucks.Network;
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
using System.Windows.Threading;

namespace StarBucks.UI
{
    /// <summary>
    /// Interaction logic for login.xaml
    /// </summary>
    public partial class login : Window
    {
        static socket networkClient;
        Main main;

        public login()
        {
            main = new Main();
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.id.IsEnabled = false;
            this.server.IsEnabled = false;
            networkClient = new socket(this.server.Text.Split(':')[0], Convert.ToInt32(this.server.Text.Split(':')[1]));
            App.socketController = networkClient;

            networkClient.messageEvent += NetworkClient_messageEvent;
            try
            {
                networkClient.sendMessage("@" + this.id.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("서버 주소를 확인하세요.");
            }
            
        }

        private void NetworkClient_messageEvent(object sender, messageArgs e)
        {
            if(e.message.Equals("200"))
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    App.loginID = this.id.Text;
                    main.Show();
                    this.Close();
                }));
            }
            else
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    this.id.IsEnabled = true;
                    this.server.IsEnabled = true;
                }));
                
                MessageBox.Show("아이디를 확인하세요.");
            }
        }
    }
}
