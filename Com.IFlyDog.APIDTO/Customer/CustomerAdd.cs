using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 顾客添加
    /// </summary>
    public class CustomerAdd
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 男女
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        public long ChannelID { get; set; }
        /// <summary>
        /// 城市ID
        /// </summary>
        public long CityID { get; set; }
        /// <summary>
        /// 开发人员ID
        /// </summary>
        public long CurrentExploitUserID { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 咨询症状ID
        /// </summary>
        public virtual IEnumerable<long> SymptomIDS { get; set; }
        /// <summary>
        /// 沟通方式ID
        /// </summary>
        public long ToolID { get; set; }
        /// <summary>
        /// 咨询内容
        /// </summary>
        public string ConsultContent { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 店家ID
        /// </summary>
        public long StoreID { get; set; }
        /// <summary>
        /// 顾客登记方式
        /// </summary>
        public CustomerRegisterType CustomerRegisterType { get; set; }
    }
}
