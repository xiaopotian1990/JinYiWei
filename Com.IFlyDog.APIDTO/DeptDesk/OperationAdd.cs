using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 划扣添加
    /// </summary>
    public class OperationAdd
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OperationAdd()
        {
            ChargesList = new List<ChargesAdd>();
            OperationerList = new List<OperationerAdd>();
        }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 项目列表
        /// </summary>
        public IEnumerable<ChargesAdd> ChargesList { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 人员列表
        /// </summary>
        public IEnumerable<OperationerAdd> OperationerList { get; set; }
    }

    /// <summary>
    /// 划扣更新
    /// </summary>
    public class OperationUpdate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OperationUpdate()
        {
            OperationerList = new List<OperationerAdd>();
        }
        /// <summary>
        /// 划扣记录ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 人员列表
        /// </summary>
        public IEnumerable<OperationerAdd> OperationerList { get; set; }
    }

    /// <summary>
    /// 项目列表
    /// </summary>
    public class ChargesAdd
    {
        /// <summary>
        /// 项目详细ID
        /// </summary>
        public long DetailID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public long ChargeID { get; set; }
        /// <summary>
        /// 划扣数量
        /// </summary>
        public int Num { get; set; }
    }

    /// <summary>
    /// 人员列表添加
    /// </summary>
    public class OperationerAdd
    {
        /// <summary>
        /// 医生ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 职位ID
        /// </summary>
        public long PositionID { get; set; }
    }
}
