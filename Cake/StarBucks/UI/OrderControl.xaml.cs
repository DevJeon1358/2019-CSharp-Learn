using StarBucks.Core;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using StarBucks.Analytics;
using System;
using StarBucks.UI;

namespace StarBucks
{
    public class OrderEventArgs : EventArgs
    {
        public int id;
        public List<Drink> orderedDrinks;
    }

    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    { 
        public List<Drink> OrderedDrink { get; set; }
        private List<Drink> Drinks = new List<Drink>();
        private statics statics;

        public int tableIdx { get; set; }

        public delegate void OrderHandler(object sender, OrderEventArgs args);
        public event OrderHandler onOrder;

        public OrderControl()
        {
            InitializeComponent();
            this.Loaded += OrderControl_Loaded;
            statics = new statics();
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

        private void Select_All(object sender, RoutedEventArgs e)   // 전체 메뉴 선택 시
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

        private void OnMouseDowndrink(Drink drink, Seat seat)   // menu 클릭 시 OrderedDrink 리스트로 추가
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

        private void PlusMinusDrink(object sender, RoutedEventArgs e)   // plus minus 버튼 클릭 시 이벤트
        {
            var type = ((Button)sender).Name;



            //if (type == "plus")
            //{
                
            //}
            //else
            //{

            //}
        }

        private void cashPay(object sender, RoutedEventArgs e)
        {
            //DB에 주문내역 전달,결제타입은 현금
            addPayment(OrderedDrink, payments.paymentMethod.CASH);
        }

        private void cardPay(object sender, RoutedEventArgs e)
        {
            //DB에 주문내역 전달,결제타입은 카드
            addPayment(OrderedDrink, payments.paymentMethod.CARD);
        }

        private void addPayment(List<Drink> OrderedDrink, payments.paymentMethod paymentMethod)
        {
            if (MessageBox.Show("결제하시겠습니까?", "안내", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // DB에 값 전달(이름,카테고리,결제타입,결제금액,결제시간)
                foreach (Drink drink in OrderedDrink) 
                {
                    for(int i= 0; i < drink.Count; i++)
                    {
                        //To-Do Connect Database using Starbucks.Analytics
                        statics.addPayment(drink.Name, drink.Category, paymentMethod, drink.Price, string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                    }
                }

                // Analytics Window의 Data 를 Refresh 함
                App.analytics.refreshData();

                BackHome();
            }
        }

        private void BackHome()     // 결제 시
        {
            onOrder.Invoke(this, new OrderEventArgs() { id = this.tableIdx, orderedDrinks = new List<Drink>() });
            this.tableIdx = 0;
            InitOrderControl();
            this.Visibility = Visibility.Collapsed;
        }
        public void setOrderList(List<Drink> drinks)
        {
            this.OrderedDrink = drinks;
            selectedDrink.Items.Refresh();
        }

        private void BackHome(object sender, RoutedEventArgs e)
        {
            onOrder.Invoke(this, new OrderEventArgs() { id = this.tableIdx, orderedDrinks = OrderedDrink });
            InitOrderControl();
            this.tableIdx = 0;
            this.Visibility = Visibility.Collapsed;
        }

        private void InitOrderControl()
        {
            OrderedDrink.Clear();
            selectedDrink.Items.Refresh();
            initMenu();
            OrderedDrink = new List<Drink>();
            totalPrice.Text = "";
            AddListItems();
        }

        private void AllClear_Click(object sender, RoutedEventArgs e)
        {
            InitOrderControl();
        }

        private void SelectClear_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
