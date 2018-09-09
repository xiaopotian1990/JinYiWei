using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户组筛选后保存结果
    /// </summary>
   public class CustomerFilterFiltrateAdd
    {
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

        /// <summary>
        ///刷选出来的客户集合数据
        /// </summary>
        public virtual List<FiltrateCustormInfo> FiltrateCustormInfoAdd { get; set; }
    }

    /// <summary>
    /// 刷选出来的客户集合数据
    /// </summary>
    public class FiltrateCustormInfo {
        /// <summary>
        /// 客户id
        /// </summary>
        public long CustormID { get; set; }
    }
}
