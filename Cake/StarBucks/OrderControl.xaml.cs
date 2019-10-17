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
        public OrderControl()
        {
            InitializeComponent();
        }

        private void OrderControl_Loaded(object sender, RoutedEventArgs e)
        {
            App.DrinkData.Load();

            AddListItems();
        }

        private void AddListItems()
        {
            foreach (Drink drink in App.DrinkData.listDrink)
            {
                DrinkControl drinkControl = new DrinkControl();
                drinkControl.SetItem(drink);

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

                lvDrink.Items.Add(drinkControl);
            }
        }
        private void Select_ColdBrew(object sender, RoutedEventArgs e)  // 콜드브루 선택 시
        {
            lvDrink.Items.Clear();
            string category = "콜드브루";
            App.DrinkData.set(category);

            foreach (Drink drink in App.DrinkData.listTest)
            {
                DrinkControl drinkControl = new DrinkControl();
                drinkControl.SetItem(drink);

                lvDrink.Items.Add(drinkControl);
            }
        }
        private void Select_Espresso(object sender, RoutedEventArgs e)  // 에스프레소 선택 시
        {
            lvDrink.Items.Clear();
            string category = "에스프레소";
            App.DrinkData.set(category);

            foreach (Drink drink in App.DrinkData.listTest)
            {
                DrinkControl drinkControl = new DrinkControl();
                drinkControl.SetItem(drink);

                lvDrink.Items.Add(drinkControl);
            }
        }
        private void Select_Frappuccino(object sender, RoutedEventArgs e)   // 프라푸치노 선택 시
        {
            lvDrink.Items.Clear();
            string category = "프라푸치노";
            App.DrinkData.set(category);

            foreach (Drink drink in App.DrinkData.listTest)
            {
                DrinkControl drinkControl = new DrinkControl();
                drinkControl.SetItem(drink);

                lvDrink.Items.Add(drinkControl);
            }
        }
    }
}
