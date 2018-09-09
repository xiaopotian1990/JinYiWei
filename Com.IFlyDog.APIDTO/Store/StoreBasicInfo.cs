using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 店家基础信息
    /// </summary>
   public class StoreBasicInfo
    {
        /// <summary>
        /// 店家id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 店家名称
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
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 银行账户名
        /// </summary>
        public string OwnerName { get; set; }
        /// <summary>
        /// 开户行
        /// </summary>
        public string Bank { get; set; }
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 店家客户数量
        /// </summary>
        public int CustormerNum { get; set; }
        /// <summary>
        /// 佣金总额
        /// </summary>
        public string CommissionNum { get; set; }
        /// <summary>
        /// 回款总额
        /// </summary>
        public string SaleBackNum { get; set; }

        /// <summary>
        /// 店家负责人信息
        /// </summary>
        public List<StoreManager> StoreManagerDateil { get; set; }
    }

    /// <summary>
    /// 店家负责人信息
    /// </summary>
    public class StoreManager {
        /// <summary>
        /// 店家负责人id
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 负责人名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 负责人手机号
        /// </summary>
        public string Phone { get; set; }
    }
}
