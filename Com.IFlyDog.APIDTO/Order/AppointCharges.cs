using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 预约时已购买项目
    /// </summary>
    public class AppointCharges
    {
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime OrderTime { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int RestNum { get; set; }
        /// <summary>
        /// 分类
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
    }
}
