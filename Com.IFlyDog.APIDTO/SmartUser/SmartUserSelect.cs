using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{

    /// <summary>
    /// 用户搜索dto类
    /// </summary>
    public class SmartUserSelect
    {
        /// <summary>
        /// 部门id
        /// </summary>
        public long DeptId { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public long RoleID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账号
        /// 
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 所属医院ID，0查当前医院以及下属所有医院的用户
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 状态0：停用1：使用999：全部
        /// </summary>
        public CommonStatus Status {get;set;}
        /// <summary>
        /// 每页多少数据
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageNum { get; set; }
    }
}
