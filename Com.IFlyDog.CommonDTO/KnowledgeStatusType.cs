using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
        /// <summary>
        /// 知识管理 1：使用；0：停用；2：删除
        /// </summary>
    public enum KnowledgeStatusType
    {
        /// <summary>
        /// 停用
        /// </summary>
        [Description("知识管理停用")]
        Stop = 0,
        /// <summary>
        /// 正常或者使用
        /// </summary>
        [Description("知识管理使用")]
        Normal = 1,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("知识管理删除")]
        Delete = 2
    }
}
