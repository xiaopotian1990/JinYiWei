using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO.CallbackGroup
{
    /// <summary>
    /// 回访组设置
    /// </summary>
    public class SmartCallbackGroup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmartCallbackGroup()
        {
            CallbackSetDetailGet = new List<SmartCallbackSetDetail>();
        }
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
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
        public List<SmartCallbackSetDetail> CallbackSetDetailGet { get; set; }
    }

    /// <summary>
    /// 回访组详细DTO
    /// </summary>
    public class SmartCallbackSetDetail
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
        public int DetailSetDays { get; set; }

        /// <summary>
        /// 回访类型名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 类型名称id
        /// </summary>
        public string CategoryID { get; set; }

    }
}
