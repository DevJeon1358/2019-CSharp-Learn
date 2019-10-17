using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
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

        private Boolean menuBackgroundWorkerFinished = false;
        private Boolean todayBackgroundWorkerFinished = false;
        private Boolean categoryBackgroundWorkerFinished = false;

        public MainWindow()
        {
            InitializeComponent();
            stat = new statics();

            stat.addPayment("TEST", "Category #2", payments.paymentMethod.CARD, 10000, string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now));
            initBaseChart();
        }

        private async Task<DataSet> getMenuStatics()
        {
            DataSet ds = stat.getPayments();

            ds.Tables[0].Columns.Add("paymentMethodText", Type.GetType("System.String"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows.IndexOf(row) != i && ds.Tables[0].Rows[i].RowState != DataRowState.Deleted)
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
                        catch
                        {

                        }
                    }
                }
            }

            ds.AcceptChanges();

            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                this.menu.ItemsSource = ds.Tables[0].DefaultView;
            }));

            return ds;
        }

        private async Task<DataSet> getTodayStatics()
        {
            var todayAmount = 0;
            DataSet ds = stat.getPayments();

            ds.Tables[0].Columns.Add("paymentMethodText", Type.GetType("System.String"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (row.RowState != DataRowState.Deleted)
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
            }

            ds.AcceptChanges();

            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                this.todayAmountLabel.Content = "오늘 판매량: " + todayAmount + " 원";
                this.today.ItemsSource = ds.Tables[0].DefaultView;
            }));

            return ds;
        }

        private async Task<DataSet> getCategoryStatics()
        {
            DataSet ds = stat.getPayments();

            ds.Tables[0].Columns.Add("paymentMethodText", Type.GetType("System.String"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows.IndexOf(row) != i && ds.Tables[0].Rows[i].RowState != DataRowState.Deleted)
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
                        catch
                        {

                        }
                    }
                }
            }

            ds.AcceptChanges();

            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                this.category.ItemsSource = ds.Tables[0].DefaultView;
            }));

            return ds;
        }

        private void initBaseChart()
        {
            var todayCollection = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "판매량",
                    Values = null
                },
                new LineSeries
                {
                    Title = "카드",
                    Values = null
                },
                new LineSeries
                {
                    Title = "현금",
                    Values = null
                },
            };

            var menuCollection = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "판매량",
                    Values = new ChartValues<double>{ },
                },
            };

            var categoryCollection = new SeriesCollection
            {
                new RowSeries
                {
                    Title = "판매량",
                    Values = new ChartValues<double>{ },
                },
            };

            this.today_chart.Series = todayCollection;
            this.menu_chart.Series = menuCollection;
            this.category_chart.Series = categoryCollection;
        }

        private async void initTodayChart(DataSet ds)
        {
            var timeIdxCount = new int[24];
            var cardIdxCount = new int[24];
            var cashIdxCount = new int[24];

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                try
                {
                    var timeIdx = DateTime.Parse(row["paymentDate"].ToString()).Hour;
                    if (row["paymentMethod"].ToString() == "CASH")
                    {
                        cashIdxCount[timeIdx] = cashIdxCount[timeIdx] + 1;
                    }
                    else
                    {
                        cardIdxCount[timeIdx] = cardIdxCount[timeIdx] + 1;
                    }

                    timeIdxCount[timeIdx] = timeIdxCount[timeIdx] + 1;
                }
                catch
                {

                }
            }

            var Labels = new[] { "00 시", "01 시", "02 시", "03 시", "04 시", "05 시", "06 시", "07 시", "08 시", "09 시", "10 시", "11 시", "12 시",
                "13 시", "14 시", "15 시", "16 시", "17 시", "18 시", "19 시", "20 시", "21 시", "22 시", "23 시", "24 시" };

            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                this.today_chart.Series[0].Values = new ChartValues<int>(timeIdxCount);
                this.today_chart.Series[1].Values = new ChartValues<int>(cardIdxCount);
                this.today_chart.Series[2].Values = new ChartValues<int>(cashIdxCount);
                this.today_chart_X.Labels = Labels;
            }));
        }

        private async void initMenuChart(DataSet ds)
        {
            var tempValue = new ChartValues<double> { };

            var LabelsList = new List<String>();

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                try
                {
                    LabelsList.Add(ds.Tables[0].Rows[i]["paymentfor"].ToString() + "(" + ds.Tables[0].Rows[i]["paymentMethodText"].ToString() + ")");
                    tempValue.Add(Convert.ToDouble(ds.Tables[0].Rows[i]["paymentAmount"].ToString()));
                }
                catch
                {

                }
            }

            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                this.menu_chart.Series[0].Values = tempValue;
                this.menu_chart.AxisY[0].Labels = LabelsList.ToArray();
            }));
        }

        private async void initCategoryChart(DataSet ds)
        {
            var tempValues = new ChartValues<double> { };
            var LabelsList = new List<String>();

            for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                try
                {
                    LabelsList.Add(ds.Tables[0].Rows[i]["category"].ToString() + "(" + ds.Tables[0].Rows[i]["paymentMethodText"].ToString() + ")");
                    tempValues.Add(Convert.ToDouble(ds.Tables[0].Rows[i]["paymentAmount"].ToString()));
                }
                catch
                {

                }
            }

            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                this.category_chart.Series[0].Values = tempValues;
                this.category_chart.AxisY[0].Labels = LabelsList.ToArray();
            }));
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            this.Loading.Visibility = Visibility.Visible;
            refreshData();
        }

        private async void refreshData()
        {
            BackgroundWorker menuBackgroundWorker = new BackgroundWorker();
            BackgroundWorker todayBackgroundWorker = new BackgroundWorker();
            BackgroundWorker categoryBackgroundWorker = new BackgroundWorker();

            menuBackgroundWorker.DoWork += MenuBackgroundWorker_DoWork;
            todayBackgroundWorker.DoWork += TodayBackgroundWorker_DoWork;
            categoryBackgroundWorker.DoWork += CategoryBackgroundWorker_DoWork;

            menuBackgroundWorker.RunWorkerCompleted += MenuBackgroundWorker_RunWorkerCompleted;
            todayBackgroundWorker.RunWorkerCompleted += TodayBackgroundWorker_RunWorkerCompleted;
            categoryBackgroundWorker.RunWorkerCompleted += CategoryBackgroundWorker_RunWorkerCompleted;

            menuBackgroundWorker.RunWorkerAsync();
            todayBackgroundWorker.RunWorkerAsync();
            categoryBackgroundWorker.RunWorkerAsync();
        }

        private async void CategoryBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var categoryDataset = await getCategoryStatics();
            initCategoryChart(categoryDataset);
        }

        private async void TodayBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var todayDataset = await getTodayStatics();
            initTodayChart(todayDataset);
        }

        private async void MenuBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            var menuDataset = await getMenuStatics();
            initMenuChart(menuDataset);
        }

        private void CategoryBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (todayBackgroundWorkerFinished == true && menuBackgroundWorkerFinished == true)
            {
                onLoadFinished();
            }

            categoryBackgroundWorkerFinished = true;
        }

        private void TodayBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (categoryBackgroundWorkerFinished == true && menuBackgroundWorkerFinished == true)
            {
                onLoadFinished();
            }

            todayBackgroundWorkerFinished = true;
        }

        private void MenuBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if(categoryBackgroundWorkerFinished == true && todayBackgroundWorkerFinished == true)
            {
                onLoadFinished();
            }

            menuBackgroundWorkerFinished = true;
        }

        private void onLoadFinished()
        {
            categoryBackgroundWorkerFinished = false;
            todayBackgroundWorkerFinished = false;
            menuBackgroundWorkerFinished = false;

            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                this.Loading.Visibility = Visibility.Hidden;
            }));
        }
    }
}
