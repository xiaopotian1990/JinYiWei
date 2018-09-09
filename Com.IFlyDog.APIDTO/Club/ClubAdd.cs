using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 单项目管理添加dto
    /// </summary>
   public class ClubAdd
    {
        /// <summary>
        /// 单项目id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 单项目管理名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public string ChargeID { get; set; }

        /// <summary>
        /// 1：对一类项目有效2：对单个项目有效
        /// </summary>
        public string ScopeLimit { get; set; }

        /// <summary>
        /// 项目分类
        /// </summary>
        public string ChargeCategoryID { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public string HospitalID { get; set; }
    }
}
