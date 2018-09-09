using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 审核规则设置
    /// </summary>
   public class AuditRuleInfo
    {
        /// <summary>
        /// 审核规则id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        ///  类型 1订单折扣 2退项目 3退预收款 4 咨询人员变更 5 开发人员变更
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public string Discount { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public long HospitalID { get; set; }

        /// <summary>
        /// 审核用户
        /// </summary>
        public virtual List<AuditUser> AuditUserInfoAdd { get; set; }
    }

    /// <summary>
    /// 审核用户
    /// </summary>
    public class AuditUser {
      
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 审核用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
    }
}
