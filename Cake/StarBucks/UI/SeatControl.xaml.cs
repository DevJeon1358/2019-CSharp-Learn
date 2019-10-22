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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StarBucks
{
    /// <summary>
    /// Interaction logic for SeatControl.xaml
    /// </summary>
    public partial class SeatControl : UserControl
    {
        private Seat seat = new Seat();

        public SeatControl()
        {
            InitializeComponent();
        }

        public void Setseat(Seat item)
        {
            seat = item;
            tbId.Text = seat.Id.ToString();
            SetSeatDrink(seat);
        }

        public void Refresh()
        {

        }

        public void SetSeatDrink(Seat seat)
        {
            foreach (Drink drink in seat.lstDrink)
            {
                TextBlock tbFood = new TextBlock();
                
                tbFood.Text = drink.Name + " - " + drink.Count.ToString();
                tbFood.FontSize = 16;

                contentGrid.Children.Add(tbFood);
            }
        }

        public int GetSeatId()
        {
            return seat.Id;
        }
    }
}
