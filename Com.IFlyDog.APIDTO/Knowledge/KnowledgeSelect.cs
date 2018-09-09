using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO.Knowledge
{
    /// <summary>
    /// 知识管理查询
    /// </summary>
    public class KnowledgeSelect
    {
        /// <summary>
        /// 知识分类ID
        /// </summary>
        public string CategoryID { get; set; }
       
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }     

        /// <summary>
        /// 当前分页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
