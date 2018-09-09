using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 日志
    /// </summary>
    public class SmartOperationLog
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 系统日志id，主要给系统日志查询使用
        /// </summary>
        public string LogID { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public LogType Type { get; set; }

        /// <summary>
        /// type值
        /// </summary>
        public int TypeValue { get; set; }
        /// <summary>
        /// 日志类型名称
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long? CustomerID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 操作用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 操作用户名称（z主要给系统日志展示使用）
        /// </summary>
        public string LogCreateName { get; set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; set; }
    }
}
