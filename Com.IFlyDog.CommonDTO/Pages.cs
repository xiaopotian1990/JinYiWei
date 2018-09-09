using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 分页数据格式
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Pages<T>
    {
        /// <summary>
        /// 每页多少数据
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int PageTotals { get; set; }
        /// <summary>
        /// 数据
        /// </summary>
        public T PageDatas { get; set; }
    }
}
