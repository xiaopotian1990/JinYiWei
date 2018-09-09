using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 打印设置枚举
    /// </summary>
    public enum HospitalPrintStatus
    {
        /// <summary>
        /// 项目单
        /// </summary>
        [Description("项目单")]
        menu = 1,

        /// <summary>
        /// 还款
        /// </summary>
        [Description("还款")]
        refund = 2,

        /// <summary>
        /// 预收款
        /// </summary>
        [Description("预收款")]
        advancesReceived = 3,

        /// <summary>
        /// 退预收款
        /// </summary>
        [Description("退预收款")]
        depositRefund = 4,

        /// <summary>
        /// 退项目
        /// </summary>
        [Description("退项目")]
        returnAProject = 5,

        /// <summary>
        /// 结算
        /// </summary>
        [Description("结算")]
        clearing = 6,

        /// <summary>
        /// 划扣
        /// </summary>
        [Description("划扣")]
        draw = 7,

        /// <summary>
        /// 手术通知
        /// </summary>
        [Description("手术通知")]
        operationNotice = 8,

        /// <summary>
        /// 进货入库
        /// </summary>
        [Description("进货入库")]
        purchase = 9,

        /// <summary>
        /// 厂家退货
        /// </summary>
        [Description("厂家退货")]
        smartReturn= 10,

        /// <summary>
        /// 调拨
        /// </summary>
        [Description("调拨")]
        allocate = 11,

        /// <summary>
        /// 盘点
        /// </summary>
        [Description("盘点")]
        check = 12,
        /// <summary>
        /// 领用
        /// </summary>
        [Description("领用")]
        use = 13
    }
}
