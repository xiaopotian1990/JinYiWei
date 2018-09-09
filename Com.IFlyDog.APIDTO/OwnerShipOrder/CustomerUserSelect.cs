using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 用户点击添加开发/咨询人员按钮查询当前开发咨询人员信息
    /// </summary>
   public class CustomerUserSelect
    {
        /// <summary>
        /// 1 开发 2 咨询
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 数据id，如果等于空，说明，还没有咨询或者开发人员,如果有数据则说明已存在开发变更人员
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 当前医院
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 客户人员
        /// </summary>
        public long CustomerID { get; set; }
    }
}
