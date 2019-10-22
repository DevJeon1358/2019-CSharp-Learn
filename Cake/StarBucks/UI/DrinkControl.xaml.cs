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
    /// Interaction logic for DrinkControl.xaml
    /// </summary>
    public partial class DrinkControl : UserControl
    {
        public delegate void OnMouseDownHandler(Drink drink, Seat seat);
        public OnMouseDownHandler OnMouseDownDrink;
        private Drink drink = new Drink();
        private Seat seat = new Seat();
        public DrinkControl()
        {
            InitializeComponent();
        }
        public void SetItem(Drink item)
        {
            drink = item;
            tbName.Text = item.Name;
            DrinkImage.Source = new BitmapImage(new Uri(item.ImagePath, UriKind.Relative));
            tbPrice.Text = item.Price.ToString() + "원";
        }

        public int GetTotalPrice()
        {
            return drink.Price * drink.Count;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            OnMouseDownDrink?.Invoke(drink, seat);
        }
    }
}
