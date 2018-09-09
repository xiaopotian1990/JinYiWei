using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 角色状态
    /// </summary>
    public enum RoleStatus
    {
        /// <summary>
        /// 角色停用
        /// </summary>
        [Description("角色停用")]
        Stop=0,
        /// <summary>
        /// 角色使用
        /// </summary>
        [Description("角色使用")]
        Use =1
    }
}
