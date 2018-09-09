using Com.IFlyDog.CommonDTO;
using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 会员工作台
    /// </summary>
    public class MemberDeskCustomer
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
        /// 咨询人员
        /// </summary>
        public string ManagerUserName { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
    }

    /// <summary>
    /// 会员工作台查询条件
    /// </summary>
    public class MemberDeskCustomerSelect
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 男女
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 会员生日
        /// </summary>
        public DateTime? BirthdayStart { get; set; }
        /// <summary>
        /// 会员生日
        /// </summary>
        public DateTime? BirthdayEnd { get; set; }
        /// <summary>
        /// 会员类型
        /// </summary>
        public long MemberCategoryID { get; set; }
        /// <summary>
        /// 分享家类型
        /// </summary>
        public long ShareCategoryID { get; set; }
    }

    /// <summary>
    /// 会员工作台
    /// </summary>
    public class MemberDeskBirthdayCustomer
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
        /// 咨询人员
        /// </summary>
        public string ManagerUserName { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        public DateTime Birthday { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Mobile { get; set; }
    }
}
