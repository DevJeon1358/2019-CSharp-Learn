using System;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace StarBucks.Database
{
    public class Connection
    {
        private SQLiteConnection _CurrentConnection;

        public SQLiteConnection CurrentConnection
        {
            set
            {
                if(_CurrentConnection == null)
                {
                    _CurrentConnection = value;
                }
                else
                {
                    throw new ConnectException("DB 연결을 임의로 SET 할 수 없습니다.");
                }
            }
            get
            {
                return _CurrentConnection;
            }
        }

        /// <summary>
        /// SQL Connection을 초기화합니다.
        /// </summary>
        /// <returns>Current Connection</returns>
        public SQLiteConnection initConnection()
        {
            String filePath = Directory.GetCurrentDirectory() + @"\Database.sqlite";
            if (!File.Exists(filePath))
            {
                SQLiteConnection.CreateFile(filePath);
            }

            CurrentConnection = new SQLiteConnection("Data Source=" + filePath);
            CurrentConnection.Open();
            return CurrentConnection;
        }

        /// <summary>
        /// 연결된 SQL Connection에 Query를 수행합니다.
        /// </summary>
        /// <example>ExcuteQuery("select * from table;")</example>
        /// <param name="sql">SQL Query</param>
        /// <returns>Effected Row Count</returns>
        public int ExcuteQuery(String sql)
        {
            if(CurrentConnection == null)
            {
                throw new ConnectException("DB 연결이 초기화 되지 않았습니다.");
            }

            SQLiteCommand sqlCommand = new SQLiteCommand(sql, CurrentConnection);

            return sqlCommand.ExecuteNonQuery();
        }

        /// <summary>
        /// 연결된 SQL Connection에 Query를 수행하고 결과 데이터를 불러옵니다.
        /// </summary>
        /// <example>ExcuteQueryAndGetData("select * from table;")</example>
        /// <param name="sql">SQL Query</param>
        /// <returns>Query result as Dataset</returns>
        public DataSet ExcuteQueryAndGetData(String sql)
        {
            if (CurrentConnection == null)
            {
                throw new ConnectException("DB 연결이 초기화 되지 않았습니다.");
            }

            DataSet ds = new DataSet();
            SQLiteDataAdapter adpt = new SQLiteDataAdapter(sql, CurrentConnection);

            adpt.Fill(ds);
            
            return ds;
        }
    }
}
