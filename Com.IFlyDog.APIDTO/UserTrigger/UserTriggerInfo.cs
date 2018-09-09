using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 用户通知方案展示
    /// </summary>
   public class UserTriggerInfo
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 方案名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 触发条件只有两种，1是到院，2 是收费
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 客户选择范围
        /// </summary>
        public int CustomerType { get; set; }
        /// <summary>
        /// 客户组id
        /// </summary>
        public string CustomerGroupID { get; set; }

        /// <summary>
        /// 会员类型id
        /// </summary>
        public string MemberCategoryID { get; set; }

        /// <summary>
        /// 分享家id
        /// </summary>
        public string ShareCategoryID { get; set; }

        /// <summary>
        /// 提醒信息
        /// </summary>
        public string Info { get; set; }

        /// <summary>
        /// (暂时还不清楚)
        /// </summary>
        public int AllUsers { get; set; }

        /// <summary>
        /// 客户范围
        /// </summary>
        public string CustomerScope { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 归属开发人员
        /// </summary>
        public int ExploitUserStatus{ get; set; }

        /// <summary>
        /// 归属咨询人员
        /// </summary>
        public int ManagerUserStatus { get; set; }

        /// <summary>
        /// 指定人员集合
        /// </summary>
        public virtual List<AssignUserInfo> AssignUserInfoAdd { get; set; }

        /// <summary>
        /// 指定部门集合
        /// </summary>
        public virtual List<AssignDeptInfo> AssignDeptInfoAdd { get; set; }
    }
}
