using System;
using System.Collections.Generic;
using System.Data;
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
using LiveCharts;
using LiveCharts.Wpf;

namespace StarBucks.Analytics.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        statics stat;
        public MainWindow()
        {
            InitializeComponent();
            stat = new statics();

            stat.addPayment("TEST", "Category #2", payments.paymentMethod.CARD, 10000, string.Format("{0:yyyy-MM-dd hh:mm:ss}", DateTime.Now));
            getMenuStatics();
            getTodayStatics();
            getCategoryStatics();
            initCharts();
        }

        private void getMenuStatics()
        {
            DataSet ds = stat.getPayments();

            ds.Tables[0].Columns.Add("paymentMethodText", Type.GetType("System.String"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows.IndexOf(row) != i)
                    {
                        try
                        {
                            if (row["paymentMethod"].ToString() == "CASH")
                            {
                                row["paymentMethodText"] = "현금";
                            }
                            else
                            {
                                row["paymentMethodText"] = "카드";
                            }

                            if (ds.Tables[0].Rows[i]["paymentMethod"].ToString() == "CASH")
                            {
                                ds.Tables[0].Rows[i]["paymentMethodText"] = "현금";
                            }
                            else
                            {
                                ds.Tables[0].Rows[i]["paymentMethodText"] = "카드";
                            }

                            if (ds.Tables[0].Rows[i]["paymentfor"].ToString() == row["paymentfor"].ToString() && ds.Tables[0].Rows[i]["paymentMethod"].ToString() == row["paymentMethod"].ToString())
                            {
                                ds.Tables[0].Rows[i]["paymentAmount"] = Convert.ToInt64(ds.Tables[0].Rows[i]["paymentAmount"]) + Convert.ToInt64(row["paymentAmount"]);
                                row.Delete();
                                break;
                            }

                            
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }

            this.menu.ItemsSource = ds.Tables[0].DefaultView;
        }

        private void getTodayStatics()
        {
            var todayAmount = 0;
            DataSet ds = stat.getPayments();

            ds.Tables[0].Columns.Add("paymentMethodText", Type.GetType("System.String"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row["paymentMethod"].ToString() == "CASH")
                {
                    row["paymentMethodText"] = "현금";
                }
                else
                {
                    row["paymentMethodText"] = "카드";
                }

                if (DateTime.Today.Date.CompareTo(DateTime.Parse(row["paymentDate"].ToString()).Date) != 0)
                {
                    row.Delete();
                }
                else
                {
                    todayAmount += Convert.ToInt32(row["paymentAmount"]);
                }
            }

            this.todayAmountLabel.Content = "오늘 판매량: " + todayAmount + " 원";
            this.today.ItemsSource = ds.Tables[0].DefaultView;
        }

        private void getCategoryStatics()
        {
            DataSet ds = stat.getPayments();

            ds.Tables[0].Columns.Add("paymentMethodText", Type.GetType("System.String"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows.IndexOf(row) != i)
                    {
                        try
                        {
                            if (row["paymentMethod"].ToString() == "CASH")
                            {
                                row["paymentMethodText"] = "현금";
                            }
                            else
                            {
                                row["paymentMethodText"] = "카드";
                            }

                            if (ds.Tables[0].Rows[i]["paymentMethod"].ToString() == "CASH")
                            {
                                ds.Tables[0].Rows[i]["paymentMethodText"] = "현금";
                            }
                            else
                            {
                                ds.Tables[0].Rows[i]["paymentMethodText"] = "카드";
                            }

                            if (ds.Tables[0].Rows[i]["category"].ToString() == row["category"].ToString() && ds.Tables[0].Rows[i]["paymentMethod"].ToString() == row["paymentMethod"].ToString())
                            {
                                ds.Tables[0].Rows[i]["paymentAmount"] = Convert.ToInt64(ds.Tables[0].Rows[i]["paymentAmount"]) + Convert.ToInt64(row["paymentAmount"]);
                                row.Delete();
                                break;
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }

            this.category.ItemsSource = ds.Tables[0].DefaultView;
        }

        private void initCharts()
        {
            var SeriesCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "판매량",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "카드",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
                new LineSeries
                {
                    Title = "현금",
                    Values = new ChartValues<double> { 4, 6, 5, 2 ,4 }
                },
            };

            DataSet ds = stat.getPayments();
            ds.Tables[0].Columns.Add("paymentMethodText", Type.GetType("System.String"));

            var timeIdxCount = new int[24];
            var cardIdxCount = new int[24];
            var cashIdxCount = new int[24];

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (DateTime.Today.Date.CompareTo(DateTime.Parse(row["paymentDate"].ToString()).Date) != 0)
                {
                    row.Delete();
                }
                else
                {
                    var timeIdx = DateTime.Parse(row["paymentDate"].ToString()).Hour;
                    if (row["paymentMethod"].ToString() == "CASH")
                    {
                        row["paymentMethodText"] = "현금";
                        cashIdxCount[timeIdx] = cashIdxCount[timeIdx] + 1;
                    }
                    else
                    {
                        row["paymentMethodText"] = "카드";
                        cardIdxCount[timeIdx] = cardIdxCount[timeIdx] + 1;
                    }

                    timeIdxCount[timeIdx] = timeIdxCount[timeIdx] + 1;
                }
            }

            SeriesCollection[0].Values = new ChartValues<int>(timeIdxCount);
            SeriesCollection[1].Values = new ChartValues<int>(cardIdxCount);
            SeriesCollection[2].Values = new ChartValues<int>(cashIdxCount);

            var Labels = new[] { "00 시", "01 시", "02 시", "03 시", "04 시", "05 시", "06 시", "07 시", "08 시", "09 시", "10 시", "11 시", "12 시",
                "13 시", "14 시", "15 시", "16 시", "17 시", "18 시", "19 시", "20 시", "21 시", "22 시", "23 시", "24 시" };

            this.today_chart.Series = SeriesCollection;
            this.today_chart_X.Labels = Labels;
        }
    }
}
