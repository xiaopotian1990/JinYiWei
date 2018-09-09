using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改渠道组
    /// </summary>
   public class ChannelGroupUpdate
    {
        /// <summary>
        /// 渠道组id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 渠道组名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 渠道组序号
        /// </summary>
        public string SortNo { get; set; }
        /// <summary>
        /// 渠道组备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建用户id
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 渠道组映射详细dto
        /// </summary>
        public virtual List<ChannelGroupDetail> ChannelGroupDetailAdd { get; set; }
    }
}
