using StarBucks.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBucks
{
    public class DrinkDataSource
    {
        public List<Drink> listDrink = null;
        public List<Drink> listTest = new List<Drink>();

        public void Load()
        {
            listDrink = new List<Drink>()
            {
                new Drink() {Name = "나이트로 콜드 브루", Price = 5500, ImagePath = @"Assets/ColdBrew/나이트로 콜드 브루.jpg"},
                new Drink() {Name = "돌체 콜드 브루", Price = 6000, ImagePath = @"Assets/ColdBrew/돌체 콜드 브루.jpg"},
                new Drink() {Name = "바닐라 크림 콜드 브루", Price = 5500, ImagePath = @"Assets/ColdBrew/바닐라 크림 콜드 브루.jpg"},

                new Drink() {Name = "라벤더 카페 브레베", Price = 5500, ImagePath = @"Assets/Espresso/라벤더 카페 브레베.jpg"},
                new Drink() {Name = "블론드 에스프레소 토닉", Price = 6000, ImagePath = @"Assets/Espresso/블론드 에스프레소 토닉.jpg"},
                new Drink() {Name = "아이스 블랙 글레이즈드 라떼", Price = 5500, ImagePath = @"Assets/Espresso/아이스 블랙 글레이즈드 라떼.jpg"},

                new Drink() {Name = "제주 쑥떡 크림 프라푸치노", Price = 5500, ImagePath = @"Assets/Frappuccino/제주 쑥떡 크림 프라푸치노.jpg"},
                new Drink() {Name = "초콜릿 크림 프라푸치노", Price = 6000, ImagePath = @"Assets/Frappuccino/초콜릿 크림 프라푸치노.jpg"},
                new Drink() {Name = "화이트 딸기 크림 프라푸치노", Price = 5500, ImagePath = @"Assets/Frappuccino/화이트 딸기 크림 프라푸치노.jpg"},
            };
            for (int i = 0; i < 3; i++)
            {
                listDrink[i].Category = "콜드브루";
            }

            for (int i = 3; i < 6; i++)
            {
                listDrink[i].Category = "에스프레소";
            }

            for (int i = 6; i < 9; i++)
            {
                listDrink[i].Category = "프라푸치노";
            }
        }
        public List<Drink> getCategoryList(string a)
        {
            if (listDrink == null)
                return null;

            //listTest.Clear();
            return listDrink.Where(x => x.Category.Equals(a)).ToList();
        }
    }
}
