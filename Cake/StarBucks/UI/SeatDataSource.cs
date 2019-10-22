using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StarBucks.Core;

namespace StarBucks
{
    public class SeatDataSource
    {
        private readonly int TABLE_MAX = 6;
        bool isLoaded = false;
        public List<Seat> lstSeat = null;

        public void Load()
        {
            if (isLoaded) return;

            if(lstSeat == null)
            {
                lstSeat = new List<Seat>();
            }

            for (int i = 1; i <= TABLE_MAX; i++)
            {
                Seat seat = new Seat();
                seat.Id = i;
                lstSeat.Add(seat);
            }

            //lstSeat = new List<Seat>()
            //{
            //    new Seat() {Id = 1},
            //    new Seat() {Id = 2},
            //    new Seat() {Id = 3},
            //    new Seat() {Id = 4},
            //    new Seat() {Id = 5},
            //    new Seat() {Id = 6}
            //};

            isLoaded = true;
        }


    }
}
