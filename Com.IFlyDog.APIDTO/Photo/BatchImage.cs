using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 批量图片
    /// </summary>
    public class BatchImage
    {
        /// <summary>
        /// 原始图片
        /// </summary>
        public string BigImage { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string ReducedImage { get; set; }
    }
}
