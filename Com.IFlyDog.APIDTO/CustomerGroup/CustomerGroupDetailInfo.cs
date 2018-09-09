using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    ///展示管理分组
    /// </summary>
   public class CustomerGroupDetailInfo
    {
        /// <summary>
        /// 管理客户分组
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 男女1：男2：女
        /// </summary>
        public int Gender { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// 主咨询项目
        /// </summary>
        public string ConsultCharge { get; set; }
        /// <summary>
        /// 会员类型
        /// </summary>
        public string MemberCategoryName { get; set; }
        /// <summary>
        /// 分享家类型
        /// </summary>
        public string ShareMemberCategoryName { get; set; }
        /// <summary>
        /// 开发人员
        /// </summary>
        public string ExploitName { get; set; }
        /// <summary>
        /// 咨询人员
        /// </summary>
        public string ManagerName { get; set; }
    }
}
