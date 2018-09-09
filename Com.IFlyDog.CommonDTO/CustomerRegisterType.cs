using System.ComponentModel;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 顾客登记方式
    /// </summary>
    public enum CustomerRegisterType
    {
        /// <summary>
        /// 网电登记
        /// </summary>
        [Description("网电登记")]
        Exploit = 1,
        /// <summary>
        /// 前台接待
        /// </summary>
        [Description("前台接待")]
        ForeGround = 2,
        /// <summary>
        /// 市场登记
        /// </summary>
        [Description("市场登记")]
        Market = 3,
        /// <summary>
        /// 微信注册
        /// </summary>
        [Description("微信注册")]
        WechatRegedit = 4,
        /// <summary>
        /// 手动推荐
        /// </summary>
        [Description("手动推荐")]
        ManualPromote =5,
        /// <summary>
        /// 分享注册
        /// </summary>
        [Description("分享注册")]
        Share=6
    }
}
