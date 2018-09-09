using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 今日新登记顾客
    /// </summary>
    public class CustomerCreateToday
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 登记人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public string ChannelName { get; set; }    
        /// <summary>
        /// 咨询项目
        /// </summary>
        public string ConsultSymptom { get; set; }
    }
}
