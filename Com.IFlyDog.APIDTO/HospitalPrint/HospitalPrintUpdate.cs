using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改打印设置dto
    /// </summary>
   public class HospitalPrintUpdate
    {
        /// <summary>
        /// 打印设置id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 类型 
        /// </summary>
        public HospitalPrintStatus Type { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public string Width { get; set; }
        /// <summary>
        /// 内容，展示到文本编辑器里的
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        ///字体大小
        /// </summary>
        public string FontSize { get; set; }
        /// <summary>
        /// 字体
        /// </summary>
        public string FontFamily { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
