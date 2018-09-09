using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 
    /// </summary>
   public class HospitalInfo
    {
        /// <summary>
        /// 医院id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 医院地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string SortNo { get; set; }
    }
}
