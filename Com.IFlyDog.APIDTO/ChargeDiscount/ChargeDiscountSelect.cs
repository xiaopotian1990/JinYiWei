using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 查询项目折扣dto
    /// </summary>
   public class ChargeDiscountSelect
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// 当前医院
        /// </summary>
        public long HospitalID { get; set; }

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
