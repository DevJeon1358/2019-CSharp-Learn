using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBucks.Core
{
    public class Seat
    {
        public int Id { get; set; }

        public List<Drink> lstDrink = new List<Drink>();

        public string OrderTime { get; set; }

        public int Total
        {
            get
            {
                int retval = 0;

                foreach(var item in lstDrink)
                {
                    retval += item.Count * item.Price;
                }

                return retval;
            }
        }
    }
}
