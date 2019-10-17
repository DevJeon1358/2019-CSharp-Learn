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
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {

        public List<Drink> OrderedDrink { get; set; }       

        public OrderControl()
        {
            InitializeComponent();
            this.Loaded += OrderControl_Loaded;
        }

        private void OrderControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.DrinkData.Load();
            OrderedDrink = new List<Drink>();

            AddListItems();
        }

        private void AddListItems()
        {
            foreach (Drink drink in App.DrinkData.listDrink)
            {
                DrinkControl drinkControl = new DrinkControl();
                drinkControl.SetItem(drink);
                drinkControl.OnMouseDownDrink += OnMouseDowndrink;
                lvDrink.Items.Add(drinkControl);
            }
        }

        private void Select_All(object sender, RoutedEventArgs e)   // 전체 메뉴 선택시
        {
            lvDrink.Items.Clear();

            foreach (Drink drink in App.DrinkData.listDrink)
            {
                DrinkControl drinkControl = new DrinkControl();
                drinkControl.SetItem(drink);
                drinkControl.OnMouseDownDrink += OnMouseDowndrink;
                lvDrink.Items.Add(drinkControl);
            }
        }
        private void Select_ColdBrew(object sender, RoutedEventArgs e)  // 콜드브루 선택 시
        {
            lvDrink.Items.Clear();
            string category = "콜드브루";
            App.DrinkData.Set(category);

            foreach (Drink drink in App.DrinkData.listTest)
            {
                DrinkControl drinkControl = new DrinkControl();
                drinkControl.SetItem(drink);
                drinkControl.OnMouseDownDrink += OnMouseDowndrink;
                lvDrink.Items.Add(drinkControl);
            }
        }
        private void Select_Espresso(object sender, RoutedEventArgs e)  // 에스프레소 선택 시
        {
            lvDrink.Items.Clear();
            string category = "에스프레소";
            App.DrinkData.Set(category);

            foreach (Drink drink in App.DrinkData.listTest)
            {
                DrinkControl drinkControl = new DrinkControl();
                drinkControl.SetItem(drink);
                drinkControl.OnMouseDownDrink += OnMouseDowndrink;
                lvDrink.Items.Add(drinkControl);
            }
        }
        private void Select_Frappuccino(object sender, RoutedEventArgs e)   // 프라푸치노 선택 시
        {
            lvDrink.Items.Clear();
            string category = "프라푸치노";
            App.DrinkData.Set(category);

            foreach (Drink drink in App.DrinkData.listTest)
            {
                DrinkControl drinkControl = new DrinkControl();
                drinkControl.SetItem(drink);
                drinkControl.OnMouseDownDrink += OnMouseDowndrink;
                lvDrink.Items.Add(drinkControl);
            }
        }

        private void OnMouseDowndrink(Drink drink, Seat seat)
        {
            var temp = OrderedDrink.Where(x => x.Name == drink.Name).FirstOrDefault();
            drink.Count++;

            if (temp == null)
            {
                OrderedDrink.Add(drink);
                seat.lstDrink.Add(drink);
            }

            totalPrice.Text = seat.Total + "원";

            selectedDrink.ItemsSource = OrderedDrink;
            selectedDrink.Items.Refresh();
        }

        private void PlusDrink(object sender, RoutedEventArgs e)
        {
            DrinkControl drinkControl = new DrinkControl();
            drinkControl.OnMouseDownDrink += OnMouseDowndrink;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {

        }
    }
}
