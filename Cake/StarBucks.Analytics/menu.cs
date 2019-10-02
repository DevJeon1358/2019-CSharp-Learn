using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace StarBucks.Analytics
{
    public class menu
    {
        private SQLiteConnection _DatabaseConnection;

        public menu(SQLiteConnection con)
        {
            if(con == null)
            {
                throw new Exception("DB Connection 이 Null입니다.");
            }

            _DatabaseConnection = con;
        }

        private void initTable(SQLiteConnection con)
        {
            con.
        }
    }
}
