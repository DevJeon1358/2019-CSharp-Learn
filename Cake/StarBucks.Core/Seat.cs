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

        private List<Drink> _lstDrink = new List<Drink>();

        public List<Drink> lstDrink
        {
            get
            {
                return _lstDrink;
            }
            set
            {
                _lstDrink = value;
            }
        }

        public string OrderTime { get; set; }
    }
}
