using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// /咨询条件查询
    /// </summary>
    public class ConsultConditionSelect
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院ID，下拉菜单
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 咨询项目id
        /// </summary>
        public long ConsultChargeID { get; set; }

        /// <summary>
        /// 咨询开始时间
        /// </summary>
        public DateTime? ConsultBeginTime { get; set; }

        /// <summary>
        /// 咨询结束时间
        /// </summary>
        public DateTime? ConsultEndTime { get; set; }

        /// <summary>
        /// 咨询次数条件类型 >=、>、=、<=、<
        /// </summary>
        public string ConsultNumberConditionType { get; set; }
        /// <summary>
        /// 咨询次数
        /// </summary>
        public int? ConsultNumberConditionNum { get; set; }

        /// <summary>
        /// 咨询方式
        /// </summary>
        public long ConsultType { get; set; }

        /// <summary>
        /// 初次咨询开始时间
        /// </summary>
        public DateTime? FirstConsultBeginTime { get; set; }

        /// <summary>
        ///初次咨询结束时间
        /// </summary>
        public DateTime? FirstConsultEndTime { get; set; }

        /// <summary>
        /// 最后咨询开始时间
        /// </summary>
        public DateTime? LastConsultBeginTime { get; set; }

        /// <summary>
        /// 最后咨询结束时间
        /// </summary>
        public DateTime? LastConsultEndTime { get; set; }

        /// <summary>
        /// 咨询内容
        /// </summary>
        public string ConsultContent { get; set; }
    }

    /// <summary>
    /// 临时表
    /// </summary>
    public class ConsultConditionSelectTemp
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 咨询内容
        /// </summary>
        public string Content { get; set; }
    }
}
