using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 未成交报表
    /// </summary>
    public class ReportFailture
    {
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 未成交类型
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 提交医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 提交用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 未成交原因
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// 未成交类型统计报表
    /// </summary>
    public class ReportFailtureCount
    {
        /// <summary>
        /// 未成交类型ID
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 未成交类型
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 百分比
        /// </summary>
        public decimal Per { get; set; }
    }

    /// <summary>
    ///未成交选择
    /// </summary>
    public class ReportFailtureSelect
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
        /// 提交用户ID
        /// </summary>
        public long? CreateUserID { get; set; }
        /// <summary>
        /// 分类ID
        /// </summary>
        public long? CatogoryID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long? CustomerID { get; set; }
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
