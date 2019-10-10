using System;
using System.Data;

namespace StarBucks.Analytics
{
    public class statics
    {
        private Database.Connection connection;

        public statics()
        {
            connection = new Database.Connection();
            connection.initConnection();

            // Table init
            initTable();
        }

        /// <summary>
        /// 테이블을 초기화 합니다.
        /// </summary>
        private void initTable()
        {
            connection.ExcuteQuery("CREATE TABLE IF NOT EXISTS payment " +
                "( Id INTEGER primary key, category INTEGER not null, paymentMethod INTEGER not null, paymentfor VARCHAR(50) not null, paymentAmount INTEGER not null );");
        }

        public void addPayment(String payMenu, int category, int payMethod, int payAmount)
        {
            connection.ExcuteQuery("INSERT INTO payment VALUES (" + category + ", " + payMethod + ", " + payMenu +")," + payAmount + ");");   
        }

        public DataSet getPaymentByCategory(int category)
        {
            return connection.ExcuteQueryAndGetData("SELECT * FROM payment WHERE category =" + category + ";");
        }

        public DataSet getPaymentByMenu(String paymenu)
        {
            return connection.ExcuteQueryAndGetData("SELECT * FROM payment WHERE paymentfor =" + paymenu + ";");
        }
    }
}
