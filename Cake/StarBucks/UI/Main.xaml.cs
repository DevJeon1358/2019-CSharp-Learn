using StarBucks.Core;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using System.Threading;

namespace StarBucks.UI
{
    public partial class Main : Window
    {
        private notify notify;
        private bool isOfflineMode = false;
        private string TimestampEndText = string.Empty;

        public Main()
        {
            InitializeComponent();

            if(App.socketController != null)
            {
                notify = new notify();
                notify.listenMessage();

                var socketInstance = App.socketController.GetSocketInstance();
                App.socketController.lostEvent += SocketController_lostEvent;
                App.socketController.connectEvent += SocketController_connectEvent;
                isOfflineMode = false;
                changeOnline.Visibility = Visibility.Collapsed;
                TimestampEndText = "\n최근 접속시간: " + socketInstance.recentTime.ToString();
            }
            else
            {
                isOfflineMode = true;
                changeOnline.Visibility = Visibility.Visible;
            }
            
            this.Loaded += MainWindow_Loaded;
            orderControl.onOrder += OrderControl_onOrder;
        }

        #region Socket Reconnect
        private void SocketController_lostEvent(object sender, Network.lostConnectionArgs e)
        {
            Task.Run(delegate
            {
                // 서버와의 연결이 종료됨
                var socketInstance = App.socketController.GetSocketInstance();
                socketInstance.reconnectAttempt = 1;
                App.socketController.reconnect(socketInstance.remote, socketInstance.port);

                App.Current.Dispatcher.Invoke(new Action(delegate
                {
                    // 재연결 시도 알림
                    notify.Send_Disconnected_Message();

                    TimestampEndText = "\n최종 접속시간: " + socketInstance.finalTime.ToString();
                }));
            });
        }

        private void SocketController_connectEvent(object sender, Network.connectedArgs e)
        {
            Task.Run(delegate
            {
                var socketInstance = App.socketController.GetSocketInstance();
                if(e.connected == true)
                {
                    // 연결됨
                    App.socketController.sendMessage("@" + App.loginID);
                    TimestampEndText = "\n최근 접속시간: " + socketInstance.recentTime.ToString();
                    notify.Send_Reconnected_Message();
                }
                else
                {
                    if (socketInstance.reconnectAttempt > 0)
                    {
                        notify.Send_Reconnecting_Message(socketInstance.reconnectAttempt * 2);
                    }

                    if (socketInstance.reconnectAttempt > 10)
                    {
                        App.Current.Dispatcher.Invoke(new Action(delegate
                        {
                            App.socketController = null;
                            this.changeOnline.Visibility = Visibility.Visible;
                        }));
                    }

                    Thread.Sleep(socketInstance.reconnectAttempt * 2000 + 2000);
                    App.socketController.reconnect(socketInstance.remote, socketInstance.port);
                }
            });
        }
        #endregion

        private void OrderControl_onOrder(object sender, OrderEventArgs args)
        {
            // 주문이 완료됨
            int idx = args.id;
            //Seat 주문내역 갱신?!업데이트?!
            App.SeatData.lstSeat.Where(x => x.Id == idx).FirstOrDefault().lstDrink = args.orderedDrinks;

            lstSeat.Items.Clear();
            AddSeatitems();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            StartTime(); //실시간 시간
            AddSeatitems();
        }

        private void AddSeatitems()
        {
            foreach (Seat seat in App.SeatData.lstSeat)
            {
                SeatControl seatControl = new SeatControl();
                seatControl.Setseat(seat);

                lstSeat.Items.Add(seatControl);
            }
        }

        private void StartTime()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (isOfflineMode)
            {
                dClock.Text = "[오프라인 모드] " + DateTime.Now.ToString();
            }
            else
            {
                dClock.Text = DateTime.Now.ToString() + TimestampEndText;
            }
        }

        private void LstSeat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstSeat.SelectedIndex == -1)
            {
                return;
            }

            SeatControl seatControl = lstSeat.SelectedItem as SeatControl; //선택된 SeatCtrl에 
            int id = seatControl.GetSeatId(); // seat id를 가져오기

            //OrderControl 보이기
            orderControl.Visibility = Visibility.Visible;

            // Seat 번호 order에 넘기기? order에 설정하기?
            orderControl.SetSeatIdOnOrder(id);

            lstSeat.SelectedIndex = -1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                App.analytics.Show();
            }
            catch (Exception)
            {
                App.analytics = new analytics();
                App.analytics.Show();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ChangeOnline_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}

