using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 打印设置查询dto
    /// </summary>
    public class HospitalPrintInfo
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
        /// 类型  1、项目单 2、还款 3、预收款 4、退预收款 5、退项目
        /// 6、结算 7、划扣 8 、 手术通知 9、 进货入库 10、厂家退货、
        /// 11、调拨  12、盘点 13、领用
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        public string Width { get; set; }
        /// <summary>
        /// 内容，展示到文本编辑器里的
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 参数说明
        /// </summary>
        public string ContentSM { get; set; }
        /// <summary>
        ///字体大小
        /// </summary>
        public string FontSize { get; set; }
        /// <summary>
        /// 字体
        /// </summary>
        public string FontFamily { get; set; }
        /// <summary>
        /// 参数说明
        /// </summary>
        public string PrintExplain { get; set; }
    }
}
