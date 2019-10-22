using StarBucks.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace StarBucks
{
    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    { 

        public List<Drink> OrderedDrink { get; set; }
        private List<Drink> Drinks = new List<Drink>();

        public OrderControl()
        {
            InitializeComponent();
            this.Loaded += OrderControl_Loaded;
        }

        private void OrderControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.DrinkData.Load();
            OrderedDrink = new List<Drink>();
            initMenu();
            AddListItems();
        }

        private void initMenu()
        {
            Drinks.Clear();
            foreach (var drink in App.DrinkData.listDrink)
            {
                Drinks.Add(drink.Clone());
            }
        }

        private void AddListItems()
        {
            lvDrink.Items.Clear();

            foreach (Drink drink in Drinks)
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

            foreach (Drink drink in Drinks)
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
            List<Drink> categoryDrinkList = new List<Drink>(App.DrinkData.getCategoryList(category));

            foreach (Drink drink in categoryDrinkList)
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
            List<Drink> categoryDrinkList = new List<Drink>(App.DrinkData.getCategoryList(category));

            foreach (Drink drink in categoryDrinkList)
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
            List<Drink> categoryDrinkList = new List<Drink>(App.DrinkData.getCategoryList(category));

            foreach (Drink drink in categoryDrinkList)
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

            int TotalPrice = 0;

            foreach(var item in lvDrink.Items)
            {
                TotalPrice += (item as DrinkControl).GetTotalPrice();
            }

            totalPrice.Text = TotalPrice + "원";

            selectedDrink.ItemsSource = OrderedDrink;
            selectedDrink.Items.Refresh();
        }

        private void PlusMinusDrink(object sender, RoutedEventArgs e)
        {
            var type = ((Button)sender).Name;

            if (type == "plus")
            {
                
            }
            else
            {

            }
        }

        private void cashPay(object sender, RoutedEventArgs e)
        {

        }

        private void cardPay(object sender, RoutedEventArgs e)
        {

        }

        private void BackHome(object sender, RoutedEventArgs e)
        {
            InitOrderControl();
            this.Visibility = Visibility.Collapsed;
        }

        private void InitOrderControl()
        {
            OrderedDrink.Clear();
            initMenu();
            OrderedDrink = new List<Drink>();
            totalPrice.Text = "";
            AddListItems();
        }

        private void AllClear_Click(object sender, RoutedEventArgs e)
        {
            InitOrderControl();
        }
    }
}
