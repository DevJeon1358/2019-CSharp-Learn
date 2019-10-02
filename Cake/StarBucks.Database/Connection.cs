using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

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
        /// 연결된 SQL Connection에 Query 를 수행합니다.
        /// </summary>
        /// <example>ExcuteQuery("select * from table;")</example>
        /// <param name="sql">SQL Query</param>
        /// <returns>Effected Row Count</returns>
        public int ExcuteQuery(String sql)
        {
            SQLiteCommand sqlCommand = new SQLiteCommand(sql, CurrentConnection);

            return sqlCommand.ExecuteNonQuery();
        }
    }
}
