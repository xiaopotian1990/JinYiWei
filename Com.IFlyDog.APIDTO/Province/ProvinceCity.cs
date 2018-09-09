using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 省市自动识别
    /// </summary>
    public class ProvinceCity
    {
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 省ID,-1表示为查询到
        /// </summary>
        public string ProvinceID { get; set; }
        /// <summary>
        /// 市ID,-1表示为查询到
        /// </summary>
        public string CityID { get; set; }
    }
}
