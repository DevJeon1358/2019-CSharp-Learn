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
                new Drink() {Name = "나이트로 콜드 브루", Price = 4500, ImagePath = @"Assets/ColdBrew/나이트로 콜드 브루.jpg"},
                new Drink() {Name = "돌체 콜드 브루", Price = 5000, ImagePath = @"Assets/ColdBrew/돌체 콜드 브루.jpg"},
                new Drink() {Name = "바닐라 크림 콜드 브루", Price = 6500, ImagePath = @"Assets/ColdBrew/바닐라 크림 콜드 브루.jpg"},

                new Drink() {Name = "라벤더 카페 브레베", Price = 5500, ImagePath = @"Assets/Espresso/라벤더 카페 브레베.jpg"},
                new Drink() {Name = "블론드 에스프레소 토닉", Price = 6000, ImagePath = @"Assets/Espresso/블론드 에스프레소 토닉.jpg"},
                new Drink() {Name = "아이스 블랙 글레이즈드 라떼", Price = 5000, ImagePath = @"Assets/Espresso/아이스 블랙 글레이즈드 라떼.jpg"},

                new Drink() {Name = "제주 쑥떡 크림 프라푸치노", Price = 7000, ImagePath = @"Assets/Frappuccino/제주 쑥떡 크림 프라푸치노.jpg"},
                new Drink() {Name = "초콜릿 크림 프라푸치노", Price = 4500, ImagePath = @"Assets/Frappuccino/초콜릿 크림 프라푸치노.jpg"},
                new Drink() {Name = "화이트 딸기 크림 프라푸치노", Price = 4500, ImagePath = @"Assets/Frappuccino/화이트 딸기 크림 프라푸치노.jpg"},

                new Drink() {Name = "아이스 커피", Price = 1000, ImagePath = @"Assets/BrewCoffe/아이스 커피.jpg"},
                new Drink() {Name = "오늘의 커피", Price = 55500, ImagePath = @"Assets/BrewCoffe/오늘의 커피.jpg"},

                new Drink() {Name = "딸기 요거트 블렌디드", Price = 7000, ImagePath = @"Assets/Blended/딸기 요거트 블렌디드.jpg"},
                new Drink() {Name = "망고 바나나 블렌디드", Price = 7000, ImagePath = @"Assets/Blended/망고 바나나 블렌디드.jpg"},
                new Drink() {Name = "망고 패션 후르츠 블렌디드", Price = 8500, ImagePath = @"Assets/Blended/망고 패션 후르츠 블렌디드.jpg"},
                new Drink() {Name = "익스트림 티 블렌디드", Price = 10000, ImagePath = @"Assets/Blended/익스트림 티 블렌디드.jpg"},
                new Drink() {Name = "자몽 셔벗 블렌디드", Price = 7000, ImagePath = @"Assets/Blended/자몽 셔벗 블렌디드.jpg"},

                new Drink() {Name = "매직 팝 스플래쉬 피지오", Price = 99900, ImagePath = @"Assets/StarbucksFigio/매직 팝 스플래쉬 피지오.jpg"},
                new Drink() {Name = "블랙 티 레모네이드 피지오", Price = 8000, ImagePath = @"Assets/StarbucksFigio/블랙 티 레모네이드 피지오.jpg"},
                new Drink() {Name = "쿨 라임 피지오", Price = 9900, ImagePath = @"Assets/StarbucksFigio/쿨 라임 피지오.jpg"},
                new Drink() {Name = "패션 탱고 티 레모네이드 피지오", Price = 22000, ImagePath = @"Assets/StarbucksFigio/패션 탱고 티 레모네이드 피지오.jpg"},
                new Drink() {Name = "핑크 자몽 피지오", Price = 33300, ImagePath = @"Assets/StarbucksFigio/핑크 자몽 피지오.jpg"},

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

            for (int i = 9; i < 11; i++)
            {
                listDrink[i].Category = "브루커피";
            }

            for (int i = 11; i < 16; i++)
            {
                listDrink[i].Category = "블렌디드";
            }

            for (int i = 16; i < 21; i++)
            {
                listDrink[i].Category = "피지오";
            }
        }
        public List<Drink> GetCategoryList(string a)
        {
            if (listDrink == null)
                return null;

            //listTest.Clear();
            return listDrink.Where(x => x.Category.Equals(a)).ToList();
        }
    }
}
