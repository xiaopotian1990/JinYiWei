using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 顾客档案上门情况
    /// </summary>
    public class ProfileVisitCase
    {
        /// <summary>
        /// 上门记录
        /// </summary>
        public virtual IEnumerable<ProfileVisit> VisitList { get; set; }

        /// <summary>
        /// 分疹记录
        /// </summary>
        public virtual IEnumerable<ProfileTriage> TriageList { get; set; }

        /// <summary>
        /// 部门接待
        /// </summary>
        public virtual IEnumerable<ProfileDeptVisit> DeptVisitList { get; set; }

        /// <summary>
        /// 住院记录
        /// </summary>
        public virtual IEnumerable<ProfileInpatient> InpatientList { get; set; }
    }
    /// <summary>
    /// 客户档案上门记录
    /// </summary>
    public class ProfileVisit
    {
        /// <summary>
        /// 上门ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 上门时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public VisitType VisitType { get; set; }
        /// <summary>
        /// 新老客
        /// </summary>
        public CustomerType CustomerType { get; set; }
        /// <summary>
        /// 分诊用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 上门医院
        /// </summary>
        public string HospitalName { get; set; }
    }

    /// <summary>
    /// 分疹记录
    /// </summary>
    public class ProfileTriage
    {
        /// <summary>
        /// 分诊记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 分诊医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 分诊人员
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 分诊时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 接诊人员
        /// </summary>
        public string AssignUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 部门接待
    /// </summary>
    public class ProfileDeptVisit
    {
        /// <summary>
        /// 上门ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 上门时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 上门医院
        /// </summary>
        public string HospitalName { get; set; }
    }

    /// <summary>
    ///住院记录
    /// </summary>
    public class ProfileInpatient
    {
        /// <summary>
        /// 住院ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 住院时间
        /// </summary>
        public DateTime InTime { get; set; }
        /// <summary>
        /// 出院时间
        /// </summary>
        public DateTime OutTime { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 住院状态0：住院中；1：已出院
        /// </summary>
        public InpatientStatus Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}

