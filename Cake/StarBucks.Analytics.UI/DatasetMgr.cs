using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBucks.Analytics.UI
{
    class DatasetMgr
    {
        private Analytics.statics stat = new Analytics.statics();
        public DataSet getMenuStatics()
        {
            DataSet ds = stat.getPayments();

            ds.Tables[0].Columns.Add("paymentMethodText", Type.GetType("System.String"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows.IndexOf(row) != i && ds.Tables[0].Rows[i].RowState != DataRowState.Deleted)
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
                }
            }

            ds.AcceptChanges();

            return ds;
        }        

        public DataSet getTodayStatics()
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

            return ds;
        }

        public DataSet getCategoryStatics()
        {
            DataSet ds = stat.getPayments();

            ds.Tables[0].Columns.Add("paymentMethodText", Type.GetType("System.String"));

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                for (var i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (ds.Tables[0].Rows.IndexOf(row) != i && ds.Tables[0].Rows[i].RowState != DataRowState.Deleted)
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
                }
            }

            ds.AcceptChanges();

            return ds;
        }
    }
}
