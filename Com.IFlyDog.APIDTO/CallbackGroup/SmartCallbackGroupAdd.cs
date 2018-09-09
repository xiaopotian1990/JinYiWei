using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.IFlyDog.APIDTO.CallbackGroup
{
    /// <summary>
    /// 回访组设置添加
    /// </summary>
    public class SmartCallbackGroupAdd
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态 1：使用；0：停用；2：删除
        /// </summary>
        public CallbackGroupStatusType Status { get; set; }
     
        /// <summary>
        /// 回访组详细DTO
        /// </summary>
        public virtual List<SmartCallbackSetDetailAdd> CallbackSetDetailAdd { get; set; }

    }

    /// <summary>
    /// 回访组详细DTO
    /// </summary>
    public class SmartCallbackSetDetailAdd
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 天数
        /// </summary>
        public int Days { get; set; }

      
        /// <summary>
        /// 类型名称id
        /// </summary>
        public long CategoryID { get; set; }


        /// <summary>
        /// 类型名称
        /// </summary>
        public long CategoryName { get; set; }
    }
}
