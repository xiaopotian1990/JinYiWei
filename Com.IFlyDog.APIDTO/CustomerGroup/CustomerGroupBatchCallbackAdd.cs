using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户组批量添加回访
    /// </summary>
  public  class CustomerGroupBatchCallbackAdd
    {
        /// <summary>
        /// 回访类型id
        /// </summary>
        public long CallbackCategoryID { get; set; }

        /// <summary>
        /// 回访日期
        /// </summary>
        public DateTime CallbackTime { get; set; }

        /// <summary>
        /// 提醒内容
        /// </summary>
        public string Info { get; set; }
        /// <summary>
        /// 回访人员
        /// </summary>
        public long CallbackUserID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 客户组ID
        /// </summary>
        public long CustomerGroupID { get; set; }
    }

    /// <summary>
    /// 批量短信
    /// </summary>
    public class BatchSSM
    {
       /// <summary>
       /// 组ID
       /// </summary>
        public long GroupID { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
    /// <summary>
    /// 临时表
    /// </summary>
    public class SSMTemp
    {
        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
    }
}
