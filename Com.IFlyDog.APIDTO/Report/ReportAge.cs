using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 年龄明细
    /// </summary>
    public class ReportAge
    {
        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 客户姓名	
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 年龄,为空不传	
        /// </summary>
        public int? Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 渠道
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// 咨询项目	
        /// </summary>
        public string SymptomName { get; set; }
        /// <summary>
        /// 会员类型	
        /// </summary>
        public string MemberCategoryName { get; set; }
        /// <summary>
        /// 上门状态	
        /// </summary>
        public ComeType ComeType { get; set; }
        /// <summary>
        /// 成交状态
        /// </summary>
        public DealType DealType { get; set; }
    }

    /// <summary>
    /// 年龄明细 Select
    /// </summary>
    public class ReportAgeSelect
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        public long? ChannelID { get; set; }
        /// <summary>
        /// 症状ID
        /// </summary>
        public long? SymptomID { get; set; }
        /// <summary>
        /// 会员ID
        /// </summary>
        public long? MemberCategoryID { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
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
