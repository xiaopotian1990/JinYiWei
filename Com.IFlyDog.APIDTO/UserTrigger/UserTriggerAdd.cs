using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加客户通知
    /// </summary>
   public class UserTriggerAdd
    {
        /// <summary>
        /// id
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 触发条件类型 只有两种，1是到院，2 是收费
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 客户分组类型  0 全体， 1会员类型 2 客户组3 分享家
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
        /// 分享家
        /// </summary>
        public string ShareCategoryID { get; set; }
        /// <summary>
        /// 弹窗提醒信息
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int AllUsers { get; set; }
        /// <summary>
        /// 归属开发人员
        /// </summary>
        public int ExploitUserStatus { get; set; }
        /// <summary>
        /// 归属咨询人员
        /// </summary>
        public int ManagerUserStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 指定人员集合
        /// </summary>
        public virtual List<AssignUserInfo> AssignUserInfoAdd { get; set; }

        /// <summary>
        /// 指定部门集合
        /// </summary>
        public virtual List<AssignDeptInfo> AssignDeptInfoAdd { get; set; }
    }

    /// <summary>
    /// 指定人员
    /// </summary>
    public class AssignUserInfo {

        /// <summary>
        /// 人员id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 人员名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 指定部门
    /// </summary>
    public class AssignDeptInfo {
        /// <summary>
        /// 部门id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string Name { get; set; }
    }
}
