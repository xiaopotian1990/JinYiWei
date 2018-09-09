using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 工作量明细表
    /// </summary>
    public class ReportOperation
    {
        /// <summary>
        /// 划扣时间	
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 客户编号	
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 客户姓名	
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 执行医院	
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 执行用户	
        /// </summary>
        public string Operator { get; set; }
        /// <summary>
        /// 分工
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 报表项目	
        /// </summary>
        public string Item { get; set; }
        /// <summary>
        /// 报表项目组	
        /// </summary>
        public string ItemGroup { get; set; }
        /// <summary>
        /// 项目编号	
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 项目名称	
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 单位	
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
    }

    /// <summary>
    /// 工作量明细表Select
    /// </summary>
    public class ReportOperationSelect
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
        /// 医院ID
        /// </summary>
        public long? HospitalID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long? OperatorID { get; set; }
        /// <summary>
        /// 岗位分工ID
        /// </summary>
        public long? PositionID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public long? ChargeID { get; set; }
        /// <summary>
        /// 报表项目组ID
        /// </summary>
        public long? ItemID { get; set; }
        /// <summary>
        /// 报表项目
        /// </summary>
        public long? ItemGroupID { get; set; }
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
