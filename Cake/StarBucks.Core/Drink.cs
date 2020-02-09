using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarBucks.Core
{
    public class Drink
    {
        public String Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public String ImagePath { get; set; }
        public String Category { get; set; }

        public Drink Clone()
        {
            var retval = new Drink();
            retval.Category = this.Category;
            retval.Price = this.Price;
            retval.Count = this.Count;
            retval.ImagePath = this.ImagePath;
            retval.Name = this.Name;

            return retval;
        }
    }
}
