using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 店家管理查询
    /// </summary>
    public class StoreSelect
    {
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 联系人
        /// </summary>
        public string Linkman { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// 银行账户名
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// 查询当前医院的店家
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string CrateUserID { get; set; }

        /// <summary>
        /// 类型 等于1 的时候是查询当前用户负责的，不传的时候是查询所有的
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 当前分页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
