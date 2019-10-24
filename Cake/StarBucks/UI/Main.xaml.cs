using StarBucks.Core;
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
using System.ComponentModel;


namespace StarBucks.UI
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>

    //public class MainEventArgs : EventArgs //핸들러로 주문화면에 seat id넘겨주기
    //{
    //    public int id;
    //}

    public partial class Main : Window
    {
        //public delegate void CompleteHandler(object sender, MainEventArgs args);
        //public event CompleteHandler OnComplete; //이벤트 이름 Oncomplete

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
            var item = App.SeatData.lstSeat.Where(x => x.Id == idx).FirstOrDefault();

            item.lstDrink = args.orderedDrinks;

            lstSeat.Items.Clear();
            AddSeatitems();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.SeatData.Load();

            AddSeatitems();
            StartTime();
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
            SeatControl seatControl = lstSeat.SelectedItem as SeatControl; //선택된 SeatCtrl에서 
            int id = seatControl.GetSeatId(); // seat id를 가져오기

            //MainEventArgs args = new MainEventArgs(); //핸들러 선언
            //args.id = id; //핸들러의 id에 seat id 넣기

            ////같다. OnComplete?.Invoke(this, null);
            //if (OnComplete != null) //누군가 핸들러를 등록했다면 
            //{
            //    OnComplete(this, args); //이벤트 발생,(this, 파라미터[seat id])
            //}

            orderControl.Visibility = Visibility.Visible;

            // Table 번호
            orderControl.tableIdx = id;
            var item = App.SeatData.lstSeat.Where(x => x.Id == id).FirstOrDefault();
            orderControl.setOrderList(item.lstDrink);

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

