using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户负责用户
    /// </summary>
    public class ProfileOwinnerShip
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProfileOwinnerShip()
        {
            Exploits = new List<ProfileOwinerShipTemp>();
            Managers = new List<ProfileOwinerShipTemp>();
        }
        /// <summary>
        /// 网电开发
        /// </summary>
       public IList<ProfileOwinerShipTemp> Exploits { get; set; }
        /// <summary>
        /// 现场咨询
        /// </summary>
        public IList<ProfileOwinerShipTemp> Managers { get; set; }
    }
    /// <summary>
    /// 具体咨询
    /// </summary>
    public class ProfileOwinerShipTemp
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 咨询人员或者开发人员
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 归属医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 归属类型
        /// </summary>
        public OwnerShipType Type { get; set; }
    }
}
