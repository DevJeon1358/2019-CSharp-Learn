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
using StarBucks.Analytics;

namespace StarBucks
{
    /// <summary>
    /// Interaction logic for OrderControl.xaml
    /// </summary>
    public partial class OrderControl : UserControl
    {
        public List<Drink> OrderedDrink { get; set; }
        private statics statics;

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

        private void Cash(object sender, RoutedEventArgs e) //현금결제 시 오오 감사합니당!! ㅋㅋㅋㅋ 오키오키,각각 보내줘야되네 그냥 할게 너가 많이 해줬으니깐 해홀랳ㅎㅎ 엉엉 고마워 
        {
            //DB에 주문내역 전달,결제타입은 현금
            addPayment(OrderedDrink, payments.paymentMethod.CASH);
        }

        private void Card(object sender, RoutedEventArgs e) //카드결제 시 대단해여!! 이거 매뉴 하나 하나씩 보내줘야함 라때 2개 => 라때, 라때 총 2개 전송 :)
        {
            //DB에 주문내역 전달,결제타입은 카드
            addPayment(OrderedDrink, payments.paymentMethod.CARD);
        }

        private void addPayment(List<Drink> OrderedDrink, payments.paymentMethod paymentMethod)
        {
            if (MessageBox.Show("결제하시겠습니까?", "안내", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                // DB에 값 전달(이름,카테고리,결제타입,amout?,결제시간)
                foreach (Drink drink in OrderedDrink) 
                {
                    for(int i= 0; i < drink.Count; i++)
                    {
                        //To-Do Connect Database using Starbucks.Analytics
                        statics.addPayment(drink.Name, drink.Category, paymentMethod, drink.Price, string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
                    }
                }
            }

            // Analytics Window의 Data 를 Refresh 함
            App.analytics.refreshData();
        }
    }
}
