using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 上门条件查询
    /// </summary>
   public class DoorConditionSelect
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院ID，下拉菜单
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 上门次数条件类型 >=、>、=、<=、<
        /// </summary>
        public string DoorConditionType { get; set; }

        /// <summary>
        /// 上门次数
        /// </summary>
        public int? DoorNumber { get; set; }

        /// <summary>
        /// 初次上门开始时间
        /// </summary>
        public DateTime? FirstDoorBeginTime { get; set; }

        /// <summary>
        /// 初次上门结束时间
        /// </summary>
        public DateTime? FirstDoorEndTime { get; set; }

        /// <summary>
        /// 最后上门开始时间
        /// </summary>
        public DateTime? LastDoorBeginTime { get; set; }

        /// <summary>
        /// 最后上门结束时间
        /// </summary>
        public DateTime? LastDoorEndTime { get; set; }
    }
}
