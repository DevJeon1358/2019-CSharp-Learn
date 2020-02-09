using StarBucks.Core;
using System.Windows.Controls;

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
