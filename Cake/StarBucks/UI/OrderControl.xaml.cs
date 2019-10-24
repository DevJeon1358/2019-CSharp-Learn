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

    //public int tableIdx
    //{
    //    get
    //    {
    //        return (Convert.ToInt32(tableId.Text));
    //    }
    //    set
    //    {
    //        tableId.Text = value.ToString();
    //    }
    //}

    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    { 
        public List<Drink> OrderedDrink { get; set; }
        private List<Drink> Drinks = new List<Drink>();
        private statics statics;
        private int Seatid = 0;

        public delegate void OrderHandler(object sender, OrderEventArgs args);
        public event OrderHandler onOrder;

        public OrderControl()
        {
            InitializeComponent();
            this.Loaded += OrderControl_Loaded;
            onOrder?.Invoke(this,new OrderEventArgs());
            statics = new statics();
        }

        private void OrderControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.DrinkData.Load();
            OrderedDrink = new List<Drink>();
            InitMenu();
            AddListItems();
        }

        private void InitMenu()
        {
            Drinks.Clear();
            foreach (var drink in App.DrinkData.listDrink)
            {
                Drinks.Add(drink.Clone());
            }
        }

        public void SetSeatIdOnOrder(int id)
        {
            Seatid = id;
            tableId.Text = Seatid.ToString();
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
        private void Select_Menu(object sender, RoutedEventArgs e)  // 각 메뉴 선택 시
        {
            lvDrink.Items.Clear();
            string category = ((ListBoxItem)sender).Name;
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

            int TotalPrice = SetTotalPrice();

            totalPrice.Text = TotalPrice + "원";

            SelectMenuImage(drink);

            selectedDrink.ItemsSource = OrderedDrink;
            selectedDrink.Items.Refresh();
        }

        private void SelectMenuImage(Drink drink)
        {
            ImageViewer.Source = new BitmapImage(new Uri(drink.ImagePath, UriKind.Relative));
        }

        private int SetTotalPrice()
        {//총액
            int sum = 0;

            foreach (var item in lvDrink.Items)
            {
                sum += (item as DrinkControl).GetTotalPrice();
            }

            return sum;
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

        private string OrderedDrinkListString(List<Drink> OrderedDrink)
        {//주문내역을 string값으로 한번에 반환
            string menuList = "";
            foreach (Drink drink in OrderedDrink)
            {
                menuList += (drink.Name + " X " + drink.Count + "\n");
            }
            return menuList;
        }

        private void addPayment(List<Drink> OrderedDrink, payments.paymentMethod paymentMethod)
        {
            string menuList = OrderedDrinkListString(OrderedDrink);
            if (MessageBox.Show(menuList + "결제하시겠습니까?", SetTotalPrice().ToString() + " 원 ", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
            onOrder.Invoke(this, new OrderEventArgs() { id = this.Seatid, orderedDrinks = new List<Drink>() });
            this.Seatid = 0;
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
            
            onOrder.Invoke(this, new OrderEventArgs() { id = this.Seatid, orderedDrinks = OrderedDrink });
            //InitOrderControl();
            this.Seatid = 0;
            this.Visibility = Visibility.Collapsed;
        }

        private void InitOrderControl()     // 결제 시 or 주문 리스트 전체 삭제 시 사용
        {
            OrderedDrink.Clear();
            selectedDrink.Items.Refresh();
            InitMenu();
            OrderedDrink = new List<Drink>();
            totalPrice.Text = "";
            AddListItems();
        }

        private void outOrderControl()      // 주문하고 뒤로가기 시 사용
        {

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
