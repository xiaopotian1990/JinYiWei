using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 今日划扣
    /// </summary>
    public class OperationToday
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OperationToday()
        {
            OperatorList = new List<Operator>();
        }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 划扣记录ID
        /// </summary>
        public string OperationID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 划扣数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 划扣时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 执行医生列表
        /// </summary>
        public IList<Operator> OperatorList { get; set; }
        /// <summary>
        /// 操作医院
        /// </summary>
        public string HospitalName { get; set; }
    }

    /// <summary>
    /// 执行医生
    /// </summary>
    public class Operator
    {
        /// <summary>
        /// 医生ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 医生
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 分工ID
        /// </summary>
        public string PositionID { get; set; }
        /// <summary>
        /// 分工
        /// </summary>
        public string PositionName { get; set; }
    }
}
