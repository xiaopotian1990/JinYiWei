using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 角色查询
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 所属医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 所属医院ID
        /// </summary>
        public string HospitalID { get; set; }
    }

    /// <summary>
    /// 角色详细查询
    /// </summary>
    public class RoleDetail
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        //public RoleDetail()
        //{
        //    MenuRole = new List<MenuRole>();
        //}
        /// <summary>
        /// 角色ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 所属医院ID
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 是否可以接受分诊0:否1：是
        /// </summary>
        public int FZ { get; set; }
        /// <summary>
        /// 是否是医护人员
        /// </summary>
        public int YHRY { get; set; }
        /// <summary>
        /// 是否参与排班
        /// </summary>
        public int CYPB { get; set; }
        /// <summary>
        /// 是否可接受手术预约
        /// </summary>
        public int SSYY { get; set; }
        /// <summary>
        /// 是否允许查看联系方式
        /// </summary>
        public int CKLXFS { get; set; }
        /// <summary>
        /// 是否允许查看药品成本价
        /// </summary>
        public int CKYPCBJ { get; set; }
        /// <summary>
        /// 详细操作ID
        /// </summary>
        public IEnumerable<MenuRole> MenuRole { get; set; }
    }

    /// <summary>
    /// 添加角色相关
    /// </summary>
    public class MenuRole
    {
        /// <summary>
        /// 菜单名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// PID
        /// </summary>
        public string PID { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public int Lev { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public IEnumerable<Child> Children { get; set; }
    }

    /// <summary>
    /// 子菜单
    /// </summary>
    public class Child
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// PID
        /// </summary>
        public string PID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool Checked { get; set; }
        /// <summary>
        /// 层级
        /// </summary>
        public int Lev { get; set; }
        /// <summary>
        /// 子节点
        /// </summary>
        public IEnumerable<Child> Children { get; set; }
    }
}
