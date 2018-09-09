using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户识别信息
    /// </summary>
    public class CustomerIdentifyInfo
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string MemberCategoryName { get; set; }
        /// <summary>
        /// 会员图标
        /// </summary>
        public string MemberIcon { get; set; }
        /// <summary>
        /// 分享家会员名称
        /// </summary>
        public string ShareCategoryName { get; set; }
        /// <summary>
        /// 分享家图标
        /// </summary>
        public string ShareIcon { get; set; }
        /// <summary>
        /// 预约码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// 上门
        /// </summary>
        public string Come { get; set; }
        /// <summary>
        /// 成交
        /// </summary>
        public string Cash { get; set; }
        /// <summary>
        /// 登记医院
        /// </summary>
        public string CreateHospital { get; set; }
        /// <summary>
        /// 初诊医院
        /// </summary>
        public string FirstVisitHospital { get; set; }
    }
}
