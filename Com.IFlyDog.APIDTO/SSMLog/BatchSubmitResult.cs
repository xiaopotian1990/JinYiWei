using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    public class BatchSubmitResult
    {
        public int Result { get; set; }
        public string Desc { get; set; }
        public List<BatchSubmitResultData> Data { get; set; }
    }

    public class BatchSubmitResultData
    {
        public string Msgid { get; set; }
        public string FailPhones { get; set; }
        public int Status { get; set; }
        public string Desc { get; set; }
    }
}
