using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO.Knowledge
{
    /// <summary>
    /// 知识管理添加
    /// </summary>
    public class KnowledgeAdd
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 知识分类ID
        /// </summary>
        public string CategoryID { get; set; }
        
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string RtfContent { get; set; }
        /// <summary>
        /// 状态0：停用1：使用
        /// </summary>
        public string OpenStatus { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
