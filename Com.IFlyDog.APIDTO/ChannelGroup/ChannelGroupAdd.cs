using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 渠道组添加
    /// </summary>
    public class ChannelGroupAdd
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

    /// <summary>
    /// 渠道组映射类
    /// </summary>
    public class ChannelGroupDetail{
        /// <summary>
        /// 渠道组映射id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 渠道分组id
        /// </summary>
        public string GroupID { get; set; }
        /// <summary>
        /// 渠道id
        /// </summary>
        public string ChannelID { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string ChannelName { get; set; }
    }
}
