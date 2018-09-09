using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户档案详细信息
    /// </summary>
    public class ProfileCustomerDetail
    {
        /// <summary>
        /// 顾客编号
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public string ChannelID { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 备用联系方式
        /// </summary>
        public string MobileBackup { get; set; }
        /// <summary>
        /// 主咨询项目ID
        /// </summary>
        public string ConsultSymptomID { get; set; }
        /// <summary>
        /// 主咨询项目
        /// </summary>
        public string ConsultSymptomName { get; set; }
        /// <summary>
        /// 开发人员
        /// </summary>
        public string ExploitUserName { get; set; }
        /// <summary>
        /// 咨询人员
        /// </summary>
        public string ManagerUserName { get; set; }
        /// <summary>
        /// 推荐店家ID
        /// </summary>
        public string StoreID { get; set; }
        /// <summary>
        /// 推荐店家
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 推荐人ID
        /// </summary>
        public string PromoterID { get; set; }
        /// <summary>
        /// 推荐人
        /// </summary>
        public string PromoterName { get; set; }
        /// <summary>
        /// 地区
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 咨询次数
        /// </summary>
        public int ConsultTimes { get; set; }
        /// <summary>
        /// 初次咨询时间
        /// </summary>
        public DateTime? FirstConsultTime { get; set; }
        /// <summary>
        /// 上次咨询时间
        /// </summary>
        public DateTime? LastConsultTime { get; set; }
        /// <summary>
        /// 出诊医院
        /// </summary>
        public string FirstVisitHospital { get; set; }
        /// <summary>
        /// 初诊时间
        /// </summary>
        public DateTime? FirstVisitTime { get; set; }
        /// <summary>
        /// 上门次数
        /// </summary>
        public int VisitTimes { get; set; }
        /// <summary>
        /// 最近到诊
        /// </summary>
        public DateTime? LastVisitTime { get; set; }
        /// <summary>
        /// 最近访问医院
        /// </summary>
        public string LastVisitHospital { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        /// 微信绑定时间
        /// </summary>
        public DateTime? WechatBindTime { get; set; }
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
        /// <summary>
        /// 头像
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        public string MemberCategoryImage { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        public string MemberCategoryName { get; set; }
        /// <summary>
        /// 分享家等级
        /// </summary>
        public string ShareCategoryImage { get; set; }
        /// <summary>
        /// 分享家等级
        /// </summary>
        public string ShareCategoryName { get; set; }
        /// <summary>
        /// 上门状态
        /// </summary>
        public string ComeType { get; set; }
        /// <summary>
        /// 成交状态
        /// </summary>
        public string DealType { get; set; }
        /// <summary>
        /// 咨询项目
        /// </summary>
        public IEnumerable<string> Symptoms { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public IEnumerable<string> Tags { get; set; }
        /// <summary>
        /// 会员权益
        /// </summary>
        public IEnumerable<string> Equitys { get; set; }
    }
}
