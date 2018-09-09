using Com.IFlyDog.CommonDTO;
using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户管理
    /// </summary>
    public class CustomerManager
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
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 会员等级
        /// </summary>
        public string MemberCategoryImage { get; set; }
        /// <summary>
        /// 分享家等级
        /// </summary>
        public string ShareCategoryImage { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// 咨询项目
        /// </summary>
        public string Symptom { get; set; }
        /// <summary>
        /// 开发人员
        /// </summary>
        public string ExploitUserName { get; set; }
        /// <summary>
        /// 咨询人员
        /// </summary>
        public string ManagerUserName { get; set; }
        /// <summary>
        /// 上门状态
        /// </summary>
        public ComeType ComeType { get; set; }
        /// <summary>
        /// 成交状态
        /// </summary>
        public DealType DealType { get; set; }
        /// <summary>
        /// 初诊日期
        /// </summary>
        public DateTime? FirstVistiTime { get; set; }
        /// <summary>
        /// 初诊医院
        /// </summary>
        public string FirstVisitHospital { get; set; }
        /// <summary>
        /// 最后光临时间
        /// </summary>
        public DateTime? LastVisitTime { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// 登记医院
        /// </summary>
        public string CreateHospital { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 推荐店家
        /// </summary>
        public string Store { get; set; }
        /// <summary>
        /// 预约日期
        /// </summary>
        public DateTime? AppointmentDate { get; set; }
        /// <summary>
        /// 预约医院
        /// </summary>
        public string AppointmentHospital { get; set; }
        /// <summary>
        /// 推荐人ID
        /// </summary>
        public string PromoterID { get; set; }
        /// <summary>
        /// 推荐人
        /// </summary>
        public string PromoterName { get; set; }
    }
}
