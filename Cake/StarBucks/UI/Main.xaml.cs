using StarBucks.Core;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace StarBucks.UI
{
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            orderControl.onOrder += OrderControl_onOrder;
        }

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
            dClock.Text = DateTime.Now.ToString();
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
    }
}

