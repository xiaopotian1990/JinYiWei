using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户档案更新
    /// </summary>
    public class ProfileCustomerUpdate
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        public long ChannelID { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 备用联系方式
        /// </summary>
        public string MobileBackup { get; set; }
        /// <summary>
        /// 咨询项目
        /// </summary>
        public long CurrentConsultSymptomID { get; set; }
        /// <summary>
        /// 推荐店家
        /// </summary>
        public long StoreID { get; set; }
    }

    /// <summary>
    /// 客户信息
    /// </summary>
    public class ProfileCustomerInfoUpdate
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public long CityID { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom1 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom2 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom3 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom4 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom5 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom6 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom7 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom8 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom9 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom10 { get; set; }
    }

    /// <summary>
    /// 客户信息
    /// </summary>
    public class ProfileCustomerInfo
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 省ID
        /// </summary>
        public string ProvinceID { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public string CityID { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// QQ
        /// </summary>
        public string QQ { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public string WeChat { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom1Name { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom1 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom2Name { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom2 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom3Name { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom3 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom4Name { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom4 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom5Name { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom5 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom6Name { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom6 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom7Name { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom7 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom8Name { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom8 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom9Name { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom9 { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom10Name { get; set; }
        /// <summary>
        /// 自定义
        /// </summary>
        public string Custom10 { get; set; }
    }
}
