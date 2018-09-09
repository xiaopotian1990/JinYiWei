using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 审核用户信息
    /// </summary>
   public class AuditUserInfo
    {

        /// <summary>
        /// 当前用户所属审核级别
        /// </summary>
        public int UserLevel { get; set; }
        /// <summary>
        /// 此审核规则总的审核级别
        /// </summary>
        public int SumLevel { get; set; }

        /// <summary>
        /// 当前审核用户id
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 当前类型的所有能审核的用户信息
        /// </summary>
        public List<AuditUserData> AuditUserData { get; set; }
    }

    /// <summary>
    /// 审核用户信息
    /// </summary>
    public class AuditUserData {

        /// <summary>
        /// 审核用户id
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 审核用户名称 （部门+用户名）
        /// </summary>
        public string UserNameInfo { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 审核详情
        /// </summary>
        public string AuditInfoDetail { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        public string Status { get; set; }

    }
}
