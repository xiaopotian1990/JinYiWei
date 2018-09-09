using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户归属权列表展示
    /// </summary>
   public class OwnerShipInfo
    {
        /// <summary>
        /// 归属权id(主键)
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 用户id
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 用户状态0：停用1：使用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 开发人员数量
        /// </summary>
        public int DeveloperCount { get; set; }

        /// <summary>
        /// 咨询人员数量
        /// </summary>
        public int ConsultantCount { get; set; }
    }
}
