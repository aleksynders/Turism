using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turism
{
    public partial class Hotel
    {
        public int CountTour
        {
            get
            {
                List<HotelOfTour> list = BaseClass.BD.HotelOfTour.Where(x => x.HotelId == Id).ToList();
                return list.Count;
            }
        }
    }
}
