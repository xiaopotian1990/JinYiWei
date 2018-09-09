using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回款记录查询
    /// </summary>
   public class SaleBackSelect
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { get; set; }
       /// <summary>
       /// 结束时间
       /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// d店铺名称
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 医院id
        /// </summary>
        public string HospitalID { get; set; }

        /// <summary>
        /// 操作用户id
        /// </summary>
        public string CreateUserID { get; set; }

        /// <summary>
        /// 当前分页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
