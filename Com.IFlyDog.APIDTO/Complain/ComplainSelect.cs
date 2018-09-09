using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 查询dto
    /// </summary>
   public class ComplainSelect
    {
        /// <summary>
        /// 查询当前医院
        /// </summary>
        public long HospitalID { get; set; }

        /// <summary>
        /// 投诉客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 处理用户名称
        /// </summary>
        public string FinishUserName { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

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
