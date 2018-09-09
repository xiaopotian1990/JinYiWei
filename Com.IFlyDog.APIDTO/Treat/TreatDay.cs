using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    public class TreatDay
    {
        public TreatDay()
        {
            HeaderList = new List<string>();
        }
        public IList<string> HeaderList { get; set; }

    }
}
