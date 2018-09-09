using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO.CallbackGroup
{
    /// <summary>
    /// 回访组修改
    /// </summary>
    public class SmartCallbackGroupUpdate
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 回访组详细DTO
        /// </summary>
        public virtual List<SmartCallbackSetDetailUpdate> CallbackSetDetailUpdate { get; set; }

        /// <summary>
        /// 回访组详细DTO
        /// </summary>
        public class SmartCallbackSetDetailUpdate
        {
            /// <summary>
            /// 回访组ID
            /// </summary>
            public string SetID { get; set; }
            /// <summary>
            /// 备注
            /// </summary>
            public string DetailRemark { get; set; }
            /// <summary>
            /// 天数
            /// </summary>
            public int Days { get; set; }

            /// <summary>
            /// 回访类型名称
            /// </summary>
            public string CategoryName { get; set; }
            /// <summary>
            /// 类型名称
            /// </summary>
            public long CategoryID { get; set; }

        }
    }
}
