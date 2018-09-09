using Com.IFlyDog.CommonDTO;
using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户管理，选择条件
    /// </summary>
    public class CustomerSelect
    {
        /// <summary>
        /// 操作人所在医院ID
        /// </summary>
        public long HospitalID { get; set; }
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
        /// 手机号
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 男女
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 开发人员
        /// </summary>
        public long ExploitUserID { get; set; }
        /// <summary>
        /// 咨询人员
        /// </summary>
        public long ManagerUserID { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public long ChannelID { get; set; }
        /// <summary>
        /// 主咨询项目
        /// </summary>
        //public long MainSymptomID { get; set; }
        /// <summary>
        /// 咨询项目
        /// </summary>
        public long SymptomID { get; set; }
        /// <summary>
        /// 会员类型
        /// </summary>
        public long MemberCategoryID { get; set; }
        /// <summary>
        /// 分享家类型
        /// </summary>
        public long ShareCategoryID { get; set; }
        /// <summary>
        /// 初诊时间
        /// </summary>
        public DateTime? FirstVisitTimeStart { get; set; }
        /// <summary>
        /// 初诊时间
        /// </summary>
        public DateTime? FirstVisitTimeEnd { get; set; }
        /// <summary>
        /// 最后光临时间
        /// </summary>
        public DateTime? LastVisitTimeStart { get; set; }
        /// <summary>
        /// 最后光临时间
        /// </summary>
        public DateTime? LastVisitTimeEnd { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime? CreateTimeStart { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime? CreateTimeEnd { get; set; }
        /// <summary>
        /// 上次咨询时间
        /// </summary>
        public DateTime? LastConsultTimeStart { get; set; }
        /// <summary>
        /// 上次咨询时间
        /// </summary>
        public DateTime? LastConsultTimeEnd { get; set; }
        /// <summary>
        /// 成交状态
        /// </summary>
        public DealType DealType { get; set; }
        /// <summary>
        /// 上门状态
        /// </summary>
        public ComeType ComeType { get; set; }
        /// <summary>
        /// 微信绑定
        /// </summary>
        public WechatStatus WechatStatus { get; set; }
        /// <summary>
        /// 净收总额 
        /// </summary>
        public decimal? CashStart { get; set; }
        /// <summary>
        /// 净收总额 
        /// </summary>
        public decimal? CashEnd { get; set; }
        /// <summary>
        /// 客户组
        /// </summary>
        //public long GroupID { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public long TagID { get; set; }
        /// <summary>
        /// 店铺
        /// </summary>
        public long StoreID { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime? AppointmentStart { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime? AppointmentEnd { get; set; }
        /// <summary>
        /// 到诊医院
        /// </summary>
        public long VisitHospitalID { get; set; }

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
