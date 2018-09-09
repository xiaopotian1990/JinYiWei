using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 仓库管理新增dto
    /// </summary>
  public  class SmartWarehouseAdd
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 仓库管理id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 仓库名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 仓库备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 所属部门id
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 单位类型1：库存单位2：使用单位
        /// </summary>
        public string UnitType { get; set; }
        /// <summary>
        /// 所属医院ID
        /// </summary>
        public string HospitalID { get; set; }

        /// <summary>
        /// 仓库管理员详细DTO
        /// </summary>
        public virtual List<SmartWarehouseManagerAdd> SmartWarehouseSetDetail { get; set; }
    }
}
