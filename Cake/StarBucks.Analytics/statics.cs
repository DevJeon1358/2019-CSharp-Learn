using System.Data;

namespace StarBucks.Analytics
{
    public class statics
    {
        private Database.Connection connection;

        public statics()
        {
            connection = new Database.Connection();
            connection.InitConnection();

            // Table init
            InitTable();
        }

        /// <summary>
        /// 테이블을 초기화 합니다.
        /// </summary>
        private void InitTable()
        {
            connection.ExcuteQuery("CREATE TABLE IF NOT EXISTS payment " +
                "( Id INTEGER primary key, category VARCHAR(50) not null, paymentMethod VARCHAR(10) not null, paymentfor VARCHAR(50) not null, paymentAmount INTEGER not null, paymentDate STRING not null );");
        }

        public void AddPayment(string payMenu, string category, payments.paymentMethod payMethod, int payAmount, string payTime)
        {
            connection.ExcuteQuery("INSERT INTO payment (category, paymentMethod, paymentfor, paymentAmount, paymentDate) VALUES ('" + category + "', '" + payMethod + "', '" + payMenu +"', '" + payAmount + "', '" + payTime + "');");   
        }

        public DataSet GetPaymentByCategory(int category)
        {
            return connection.ExcuteQueryAndGetData("SELECT * FROM payment WHERE category =" + category + ";");
        }

        public DataSet GetPaymentByMenu(string paymenu)
        {
            return connection.ExcuteQueryAndGetData("SELECT * FROM payment WHERE paymentfor =" + paymenu + ";");
        }

        //public DataSet getTodayPayment()
        //{
        //    return connection.ExcuteQueryAndGetData("SELECT * FROM payment where paymentDate >=" + DateTime.Today.CompareTo(DateTime.Parse("2019-10-17")) + " && paymentDate <" + DateTime.Today.AddDays(1) + ";");
        //}

        public DataSet GetPayments()
        {
            return connection.ExcuteQueryAndGetData("SELECT * FROM payment;");
        }
    }
}
