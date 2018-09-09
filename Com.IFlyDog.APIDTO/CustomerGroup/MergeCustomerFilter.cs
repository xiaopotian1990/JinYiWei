using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 合并客户组
    /// </summary>
   public class MergeCustomerFilter
    {
        /// <summary>
        /// 客户组1
        /// </summary>
        public long CustomerFilterIDOne { get; set; }

        /// <summary>
        /// 客户组2
        /// </summary>
        public long CustomerFilterIDTwo { get; set; }

        /// <summary>
        /// 合并类型 1同时在客户组1和客户组2， 2 在客户组1或客户组2 ， 3 在客户组1但不在客户组2
        /// </summary>
        public int MergeType { get; set; }

        /// <summary>
        /// 保存结果 1 覆盖现有客户组  2 保存到新客户组
        /// </summary>
        public int SaveResult { get; set; }

        /// <summary>
        /// 如果选择的是1，则需要传客户组id
        /// </summary>
        public long GroupID { get; set; }

        /// <summary>
        /// 如果选择的2 则需要传新客户组名称
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public long HospitalID { get; set; }
    }
}
