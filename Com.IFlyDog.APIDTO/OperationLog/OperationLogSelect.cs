using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 系统日志
    /// </summary>
   public class OperationLogSelect
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
        /// 操作用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 操作账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 医院id查询当前医院的日志
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 操作日志类型
        /// </summary>
        public string LogType { get; set; }

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
