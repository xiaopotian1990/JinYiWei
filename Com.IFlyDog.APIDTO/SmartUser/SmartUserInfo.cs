using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 用户查询dto
    /// </summary>
    public class SmartUserInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmartUserInfo()
        {
            Roles = new List<RoleInfo>();
        }
        /// <summary>
        /// 用户id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 角色信息
        /// </summary>
        public List<RoleInfo> Roles { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 状态0：停用1：使用
        /// </summary>
        public CommonStatus Status { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 男女1：男2：女
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 所属部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public string DeptID { get; set; }
        /// <summary>
        /// 所属医院ID
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 所属医院
        /// </summary>
        public string HospitalName { get; set; }
       
    }

    /// <summary>
    /// 角色信息
    /// </summary>
    public class RoleInfo
    {
        /// <summary>
        /// 角色
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public string RoleID { get; set; }
        /// <summary>
        /// 是否可以分疹
        /// </summary>
        public int FZ { get; set; }
        /// <summary>
        /// 是否医护人员
        /// </summary>
        public int YHRY { get; set; }
        /// <summary>
        /// 是否参与排班
        /// </summary>
        public int CYPB { get; set; }
        /// <summary>
        /// 手术预约
        /// </summary>
        public int SSYY { get; set; }
    }
}
