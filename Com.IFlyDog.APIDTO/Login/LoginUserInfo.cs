using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 登陆后返回的信息
    /// </summary>
    public class LoginUserInfo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string Mobile { get; set; }
        // <summary>
        // 部门名称
        // </summary>
        // public string DeptName { get; set; }
        // <summary>
        //部门ID
        // </summary>
        //public long DeptID { get; set; }

        /// <summary>
        /// 所属医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 所属医院名称
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public CommonStatus Status { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public List<LeftMenu> Menus { get; set; }
        /// <summary>
        /// 角色权限
        /// </summary>
        public IEnumerable<string> Actions { get; set; }
    }

    /// <summary>
    /// 左侧菜单
    /// </summary>
    public class LeftMenu
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 子菜单
        /// </summary>
        public List<LeftMenuChild> children { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string href { get; set; }
    }

    /// <summary>
    /// 子菜单
    /// </summary>
    public class LeftMenuChild
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }
        /// <summary>
        /// 链接
        /// </summary>
        public string href { get; set; }
    }


    public class MenuTemp
    {
        public long ID { get; set; }
        public long MenuID { get; set; }
        public string MenuName { get; set; }
        public string Icon { get; set; }
        public string URL { get; set; }
        public int Sort { get; set; }
        public long PID { get; set; }
        public string ParentMenuName { get; set; }
        public int PSort { get; set; }
        public string PIcon { get; set; }
        public string ControllerAction { get; set; }
        public string ActionName { get; set; }
    }
}
