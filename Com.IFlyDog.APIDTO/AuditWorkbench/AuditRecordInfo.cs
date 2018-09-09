using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 审核记录展示dto
    /// </summary>
   public class AuditRecordInfo
    {
        /// <summary>
        /// 记录id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 提交医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 提交用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 客户id
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 类型值 1订单折扣 2退项目 3退预收款 4 咨询人员变更 5 开发人员变更
        /// </summary>
        public string TypeValue { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 审核等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 审核状态 1 待审核 2 无需审核 3 审核不通过 4 审核通过 5 未提交
        /// </summary>
        public int AuditStatus { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string Content { get; set; }
    }
}
