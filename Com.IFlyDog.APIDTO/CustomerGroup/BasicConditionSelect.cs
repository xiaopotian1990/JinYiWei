using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 基础条件查询
    /// </summary>
  public class BasicConditionSelect
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 手机
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 男女1：男2：女
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// 渠道id
        /// </summary>
        public long ChannelID { get; set; }

        /// <summary>
        /// 收费项目
        /// </summary>
        public long ChargeID { get; set; }

        /// <summary>
        /// 会员类型id
        /// </summary>
        public long MemberCategoryID { get; set; }

        /// <summary>
        /// 分享家类型id
        /// </summary>
        public long ShareMemberCategoryID { get; set; }

        /// <summary>
        /// 开发人员id
        /// </summary>
        public long DeveloperUserID { get; set; }

        /// <summary>
        /// 咨询人员id
        /// </summary>
        public long ConsultantUserID { get; set; }

        /// <summary>
        /// 上门状态 1：已上门0：未上门
        /// </summary>
        public int DoorStatus { get; set; }

        /// <summary>
        /// 成交状态 1 已成交 0 未成交
        /// </summary>
        public int ClinchStatus{ get; set; }

        /// <summary>
        /// 微信状态 1 已绑定 0 未绑定
        /// </summary>
        public int WeChatStatus{ get; set; }

        /// <summary>
        /// 登记开始时间
        /// </summary>
        public DateTime? RegisterBeginTime { get; set; }

        /// <summary>
        /// 登记结束时间
        /// </summary>
        public DateTime? RegisterEndTime { get; set; }

        /// <summary>
        /// 初诊开始时间
        /// </summary>
        public DateTime? FirstVisitBeginTime { get; set; }

        /// <summary>
        /// 初诊结束时间
        /// </summary>
        public DateTime? FirstVisitEndTime { get; set; }

        /// <summary>
        /// 最后光临开始时间
        /// </summary>
        public DateTime? LastVisitBeginTime { get; set; }

        /// <summary>
        /// 最后光临结束时间
        /// </summary>
        public DateTime? LastVisitEndTime { get; set; }
    }
}
