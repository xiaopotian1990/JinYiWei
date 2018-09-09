using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 系统设置枚举
    /// </summary>
    public enum Option
    {
        /// <summary>
        /// 客户自定义字段1
        /// </summary>
        Customer1 = 1,
        /// <summary>
        /// 客户自定义字段2
        /// </summary>
        Customer2 = 2,
        /// <summary>
        /// 客户自定义字段3
        /// </summary>
        Customer3 = 3,
        /// <summary>
        /// 客户自定义字段4
        /// </summary>
        Customer4 = 4,
        /// <summary>
        /// 客户自定义字段5
        /// </summary>
        Customer5 = 5,
        /// <summary>
        /// 客户自定义字段6
        /// </summary>
        Customer6 = 6,
        /// <summary>
        /// 客户自定义字段7
        /// </summary>
        Customer7 = 7,
        /// <summary>
        /// 客户自定义字段8
        /// </summary>
        Customer8 = 8,
        /// <summary>
        /// 客户自定义字段9
        /// </summary>
        Customer9 = 9,
        /// <summary>
        /// 客户自定义字段10
        /// </summary>
        Customer10 = 10,
        /// <summary>
        /// 用户咨询模板
        /// </summary>
        ContentTemplate = 11,
        /// <summary>
        /// 预约开始时间
        /// </summary>
        MakeBeginTime = 13,
        /// <summary>
        /// 预约结束时间
        /// </summary>
        MakeEndTime = 14,
        /// <summary>
        /// 预约时间间隔
        /// </summary>
        MakeTimeInterval = 15,

        /// <summary>
        /// 积分数量
        /// </summary>
        IntegralNum = 16,
        /// <summary>
        /// 预收款成交设置code
        /// </summary>
        AdvanceSettingsCode = 17,
        /// <summary>
        /// 是否允许欠款code
        /// </summary>
        AllowArrearsCode = 18,
        /// <summary>
        /// 隐私保护值code
        /// </summary>
        PrivacyProtectionCode=19,
        /// <summary>
        /// 挂号是否开启code
        /// </summary>
        RegistrationCode=20,
        /// <summary>
        /// 挂号收费项目code
        /// </summary>
        RegistrationChargeCode=21,
        /// <summary>
        /// 等候诊断code
        /// </summary>
        WaitingDiagnosisCode=22,
        /// <summary>
        /// 有效期预警
        /// </summary>
        yxqyj=23
    }

    /// <summary>
    /// 预收款成交设置
    /// </summary>
    public enum AdvanceSettings
    {
        /// <summary>
        /// 购买时
        /// </summary>
        Buy = 1,
        /// <summary>
        /// 使用时
        /// </summary>
        Use = 0
    }

    /// <summary>
    /// 是否允许欠款
    /// </summary>
    public enum AllowArrears
    {
        /// <summary>
        /// 是
        /// </summary>
        Yes = 1,
        /// <summary>
        /// 否
        /// </summary>
        No = 0
    }

    /// <summary>
    /// 隐私保护
    /// </summary>
    public enum PrivacyProtection
    {
        /// <summary>
        /// 全部隐藏
        /// </summary>
        AllHide=1,
        /// <summary>
        /// 部分隐藏
        /// </summary>
        PartHide=0
    }

    /// <summary>
    /// 挂号
    /// </summary>
    public enum Registration {
        /// <summary>
        /// 开启
        /// </summary>
        Open=1,
        /// <summary>
        /// 关闭
        /// </summary>
        Close=0
    }

    /// <summary>
    /// 等候诊断
    /// </summary>
    public enum WaitingDiagnosis {
        /// <summary>
        /// 开启
        /// </summary>
        Open = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        Close = 0
    }
}
