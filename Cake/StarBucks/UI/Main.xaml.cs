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

namespace StarBucks.UI
{
    /// <summary>
    /// Interaction logic for Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            App.SeatData.Load();

            AddSeatitems();
            StartTime();
        }

        private void AddSeatitems()
        {
           foreach(Seat seat in App.SeatData.lstSeat)
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
    }
}
