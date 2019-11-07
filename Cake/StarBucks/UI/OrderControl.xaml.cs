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
        //public List<Drink> OrderedDrink { get; set; }
        private Seat orderedSeat = new Seat();
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
            // OrderedDrink = new List<Drink>();
            InitMenu();
            AllMenuShow();
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

            orderedSeat = App.SeatData.lstSeat.Where(x => x.Id == Seatid).FirstOrDefault();

            lastOrderTime.Text = orderedSeat.OrderTime;

            selectedDrink.ItemsSource = orderedSeat.lstDrink;
            selectedDrink.Items.Refresh();
            totalPrice.Text = SetTotalPrice() + "원";    // 테이블 나간 후 다시 다른 테이블에 들어갈 때 합계가 올바르게 바뀌기 위해
        }

        private void Select_All(object sender, RoutedEventArgs e)   // 전체 메뉴 선택 시
        {
            AllMenuShow();
        }

        private void AllMenuShow()
        {
            lvDrink.Items.Clear();

            foreach (Drink drink in Drinks)
            {
                DrinkControl drinkControl = new DrinkControl();
                drinkControl.SetItem(drink);    // Clone은 필요없다고 느껴져 지움
                drinkControl.OnMouseDownDrink += OnMouseDowndrink;
                lvDrink.Items.Add(drinkControl);
            }
        }
        private void Select_Menu(object sender, RoutedEventArgs e)  // 각 메뉴 선택 시
        {
            lvDrink.Items.Clear();
            string category = ((ListBoxItem)sender).Name;
            List<Drink> categoryDrinkList = new List<Drink>(App.DrinkData.GetCategoryList(category));

            foreach (Drink drink in categoryDrinkList)
            {
                DrinkControl drinkControl = new DrinkControl();
                drinkControl.SetItem(drink);    // Clone은 필요없다고 느껴져 지움
                drinkControl.OnMouseDownDrink += OnMouseDowndrink;
                lvDrink.Items.Add(drinkControl);
            }
        }

        private void OnMouseDowndrink(Drink drink, Seat seat)   // menu 클릭 시 OrderedDrink 리스트로 추가
        {
            var temp = orderedSeat.lstDrink.Where(x => x.Name == drink.Name).FirstOrDefault();
            //drink.Count++;

            if (temp == null)   // temp가 비었다면 새로 drink 객체를 클론하여 orderedSeat.lstDrink에 추가
            {
                var newItem = drink.Clone();
                newItem.Count++;
                orderedSeat.lstDrink.Add(newItem);
            }
            else                // temp가 안비었다면 count++
            {
                temp.Count++;
            }

            totalPrice.Text = SetTotalPrice() + "원";

            SelectDrinkImage(drink);

            //selectedDrink.ItemsSource = orderedSeat.lstDrink;
            selectedDrink.Items.Refresh();
        }

        private void SelectDrinkImage(Drink drink)
        {
            ImageViewer.Source = new BitmapImage(new Uri(drink.ImagePath, UriKind.Relative));
        }

        private int SetTotalPrice()     // 총액
        {
            int sum = 0;

            foreach (Drink drink in orderedSeat.lstDrink)
            {
                sum += (drink.Price * drink.Count);
            }

            return sum;
        }

        private void CashPay(object sender, RoutedEventArgs e)
        {
            //DB에 주문내역 전달,결제타입은 현금
            AddPayment(orderedSeat.lstDrink, payments.paymentMethod.CASH);
        }

        private void CardPay(object sender, RoutedEventArgs e)
        {
            //DB에 주문내역 전달,결제타입은 카드
            AddPayment(orderedSeat.lstDrink, payments.paymentMethod.CARD);
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

        private void AddPayment(List<Drink> OrderedDrink, payments.paymentMethod paymentMethod)
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
                        statics.AddPayment(drink.Name, drink.Category, paymentMethod, drink.Price, string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                    }
                }

                // Analytics Window의 Data 를 Refresh 함
                App.analytics.RefreshData();

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
        
        private void BackHome(object sender, RoutedEventArgs e) // 주문하고 뒤로가기 시 사용
        {
            if(this.orderedSeat.lstDrink.Count != 0) //주문내역이 없을경우 최근 주문시간은 존재하지않는다.
            {
                this.orderedSeat.OrderTime = DateTime.Now.ToString();
            }
            onOrder.Invoke(this, new OrderEventArgs() { id = this.Seatid, orderedDrinks = orderedSeat.lstDrink });
            selectedDrink.Items.Refresh();
            this.Seatid = 0;
            this.Visibility = Visibility.Collapsed;
        }

        private void InitOrderControl()     // 결제 시 or 주문 리스트 전체 삭제 시 사용
        {
            orderedSeat.lstDrink.Clear();
            lastOrderTime.Text = "";       // 결제 혹은 전체삭제 시 최근 주문시간 삭제
            selectedDrink.Items.Refresh();
            ImageViewer.Source = null;     // 이미지 뷰어 널값처리
            InitMenu();
            totalPrice.Text = "";          // 합계 초기화
            orderedSeat.OrderTime = "";    // 최근 주문시간 초기화
            //AllMenuShow();
            //AddListItems();
        }

        private void AllClear_Click(object sender, RoutedEventArgs e)
        {
            InitOrderControl();
        }

        private void PlusMinusDrink(object sender, RoutedEventArgs e)   // plus minus 버튼 클릭 시 이벤트
        {
            var type = ((Button)sender).Name;
            var drink = ((ListViewItem)selectedDrink.ContainerFromElement(sender as Button)).Content as Drink;

            if (type == "plus")  // button의 name이 plus라면
            {
                drink.Count++;
            }
            else  // button의 namedl minus라면
            {
                if (drink.Count == 1)  // Count가 1 일때 minus 버튼을 누르면 삭제
                {
                    ////selectedDrink.Items
                    //selectedDrink.ItemsSource       Items와 ItemsSource 차이가 궁금함!!
                    RemoveDrink(drink);
                }
                else  // 아닐 때는 카운트 감소
                {
                    drink.Count--;
                }
            }
            SelectedDrinkRefresh(drink);
        }

        private void SelectRemove_Click(object sender, RoutedEventArgs e)   // 주문 메뉴에서 메뉴 하나 지울 때
        {
            var drink = ((ListViewItem)selectedDrink.ContainerFromElement(sender as Button)).Content as Drink;
            RemoveDrink(drink);
        }

        private void RemoveDrink(Drink drink)   // drink 받아서 selectedDrink에서 삭제
        {
            var itemsSource = selectedDrink.ItemsSource as List<Drink>;

            itemsSource.Remove(drink);
            SelectedDrinkRefresh(drink);
            ImageViewer.Source = null;     // 이미지 뷰어 널값처리
        }

        private void SelectedDrinkRefresh(Drink drink)  // 주문 메뉴에서 버튼 클릭 후 값이 변경 되었을 때, 합계, 이미지 등 새로 고침
        {
            SelectDrinkImage(drink);
            totalPrice.Text = SetTotalPrice() + "원";
            selectedDrink.Items.Refresh();
        }
    }
}
