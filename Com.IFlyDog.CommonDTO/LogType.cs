using System.ComponentModel;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    ///     日志类型
    /// </summary>
    public enum LogType
    {
        /// <summary>
        ///     登录
        /// </summary>
        [Description("登录")]
        Login = 0,

        #region 部门相关

        /// <summary>
        ///     部门添加
        /// </summary>
        [Description("部门添加")]
        DeptAdd = 1,

        /// <summary>
        ///     部门修改
        /// </summary>
        [Description("部门修改")]
        DeptUpdate = 2,

        /// <summary>
        ///     部门停用或者启用
        /// </summary>
        [Description("部门停用或者启用")]
        DeptStopOrUse = 3,

        #endregion

        #region 渠道相关

        /// <summary>
        ///     渠道添加
        /// </summary>
        [Description("渠道添加")]
        ChannelAdd = 4,

        /// <summary>
        ///     渠道修改
        /// </summary>
        [Description("渠道修改")]
        ChannelUpdate = 5,

        /// <summary>
        ///     渠道停用或者启用
        /// </summary>
        [Description("渠道停用或者启用")]
        ChannelStopOrUse = 6,

        #endregion

        #region 症状相关

        /// <summary>
        ///     症状添加
        /// </summary>
        [Description("症状添加")]
        SymptomAdd = 7,

        /// <summary>
        ///     症状修改
        /// </summary>
        [Description("症状修改")]
        SymptomUpdate = 8,

        /// <summary>
        ///     症状停用或者启用
        /// </summary>
        [Description("症状停用或者启用")]
        SymptomStopOrUse = 9,

        #endregion

        #region 未成交类型相关

        /// <summary>
        ///     未成交类型添加
        /// </summary>
        [Description("未成交类型添加")]
        FailtureCategoryAdd = 10,

        /// <summary>
        ///     症状修改
        /// </summary>
        [Description("未成交类型修改")]
        FailtureCategoryUpdate = 11,

        /// <summary>
        ///     症状停用或者启用
        /// </summary>
        [Description("未成交类型停用或者启用")]
        FailtureCategoryStopOrUse = 12,

        #endregion

        #region 投诉类型相关

        /// <summary>
        ///     未成交类型添加
        /// </summary>
        [Description("投诉类型添加")]
        ComplainCategoryAdd = 13,

        /// <summary>
        ///     症状修改
        /// </summary>
        [Description("投诉类型修改")]
        ComplainCategoryUpdate = 14,

        /// <summary>
        ///     症状停用或者启用
        /// </summary>
        [Description("投诉类型停用或者启用")]
        ComplainCategoryStopOrUse = 15,

        #endregion

        #region 知识分类相关

        /// <summary>
        ///     知识分类添加
        /// </summary>
        [Description("知识分类添加")]
        KnowledgeCategoryAdd = 16,

        /// <summary>
        ///     知识分类修改
        /// </summary>
        [Description("知识分类修改")]
        KnowledgeCategoryUpdate = 17,

        /// <summary>
        ///     知识分类停用或者启用
        /// </summary>
        [Description("知识分类停用或者启用")]
        KnowledgeCategoryStopOrUse = 18,

        #endregion

        #region 知识管理相关

        /// <summary>
        ///     知识管理添加
        /// </summary>
        [Description("知识管理添加")]
        KnowledgeAdd = 19,

        /// <summary>
        ///     知识管理修改
        /// </summary>
        [Description("知识管理修改")]
        KnowledgeUpdate = 20,

        /// <summary>
        ///     知识管理停用或者启用
        /// </summary>
        [Description("知识管理停用或者启用")]
        KnowledgeStopOrUse = 21,

        #endregion

        #region 回访类型设置相关

        /// <summary>
        ///     知识管理添加
        /// </summary>
        [Description("回访类型添加")]
        CallbackCategoryAdd = 22,

        /// <summary>
        ///     知识管理修改
        /// </summary>
        [Description("回访类型修改")]
        CallbackCategoryUpdate = 23,

        /// <summary>
        ///     知识管理停用或者启用
        /// </summary>
        [Description("回访类型停用或者启用")]
        CallbackCategoryStopOrUse = 24,

        #endregion

        #region 回访组设置

        /// <summary>
        ///     知识管理添加
        /// </summary>
        [Description("回访组添加")]
        CallbackGroupAdd = 25,

        /// <summary>
        ///     知识管理修改
        /// </summary>
        [Description("回访组修改")]
        CallbackGroupUpdate = 26,

        /// <summary>
        ///     知识管理停用或者启用
        /// </summary>
        [Description("回访组停用或者启用")]
        CallbackGroupStopOrUse = 27,

        #endregion

        #region 班次管理相关

        /// <summary>
        ///     班次添加
        /// </summary>
        [Description("班次添加")]
        SmartShiftCategoryAdd = 28,

        /// <summary>
        ///     班次修改
        /// </summary>
        [Description("班次修改")]
        SmartShiftCategoryUpdate = 29,

        /// <summary>
        ///     班次停用或者启用
        /// </summary>
        [Description("班次停用或者启用")]
        SmartShiftCategoryDispose = 30,

        #endregion

        #region 单位管理相关

        /// <summary>
        ///     单位管理添加
        /// </summary>
        [Description("单位添加")]
        SmartUnitAdd = 31,

        /// <summary>
        ///     单位管理修改
        /// </summary>
        [Description("单位修改")]
        SmartUnitUpdate = 32,

        /// <summary>
        ///     单位管理删除
        /// </summary>
        [Description("单位删除")]
        SmartUnitDelete = 33,

        #endregion

        #region 药物品管理相关

        /// <summary>
        ///     药物品管理添加
        /// </summary>
        [Description("药物品添加")]
        SmartProductCategoryAdd = 34,

        /// <summary>
        ///     药物品管理修改
        /// </summary>
        [Description("药物品修改")]
        SmartProductCategoryUpdate = 35,

        /// <summary>
        ///     药物品管理删除
        /// </summary>
        [Description("药物品删除")]
        SmartProductCategoryDelete = 36,

        #endregion

        #region 供应商管理相关

        /// <summary>
        ///     供应商管理添加
        /// </summary>
        [Description("供应商添加")]
        SmartSupplierAdd = 37,

        /// <summary>
        ///     供应商管理修改
        /// </summary>
        [Description("供应商修改")]
        SmartSupplierUpdate = 38,

        /// <summary>
        ///     供应商管理删除
        /// </summary>
        [Description("供应商删除")]
        SmartSupplierDelete = 39,

        #endregion

        #region 仓库管理相关

        /// <summary>
        /// 仓库管理添加
        /// </summary>
        [Description("仓库添加")]
        SmartWarehouseAdd = 49,

        /// <summary>
        ///     仓库管理修改
        /// </summary>
        [Description("仓库修改")]
        SmartWarehouseUpdate = 50,

        /// <summary>
        ///     仓库管理删除
        /// </summary>
        [Description("仓库删除")]
        SmartWarehouseDelete = 51,

        #endregion

        #region 药物品管理相关

        /// <summary>
        /// 药物品管理添加
        /// </summary>
        [Description("药物品添加")]
        SmartProductAdd = 49,

        /// <summary>
        ///     药物品管理修改
        /// </summary>
        [Description("药物品修改")]
        SmartProductUpdate = 50,

        /// <summary>
        ///     药物品管理删除
        /// </summary>
        [Description("药物品删除")]
        SmartProductDispose = 51,

        #endregion

        #region 角色管理

        /// <summary>
        ///     角色添加
        /// </summary>
        [Description("角色添加")]
        RoleAdd = 30,

        /// <summary>
        ///     部门修改
        /// </summary>
        [Description("角色修改")]
        RoleUpdate = 31,

        /// <summary>
        ///     角色删除
        /// </summary>
        [Description("角色删除")]
        RoleDelete = 32,

        #endregion

        #region 银行卡相关

        /// <summary>
        ///     银行卡添加
        /// </summary>
        [Description("银行卡添加")]
        CardCategoryAdd = 33,

        /// <summary>
        ///     银行卡修改
        /// </summary>
        [Description("银行卡修改")]
        CardCategoryUpdate = 34,

        /// <summary>
        ///     银行卡停用或者启用
        /// </summary>
        [Description("银行卡停用或者启用")]
        CardCategoryStopOrUse = 35,

        #endregion

        #region 工具类

        /// <summary>
        ///     银行卡添加
        /// </summary>
        [Description("工具添加")]
        SmartToolAdd = 36,

        /// <summary>
        ///     银行卡修改
        /// </summary>
        [Description("工具修改")]
        SmartToolUpdate = 37,

        /// <summary>
        ///     银行卡停用或者启用
        /// </summary>
        [Description("工具停用或者启用")]
        SmartToolStopOrUse = 38,

        #endregion

        #region 顾客标签相关
        /// <summary>
        /// 顾客标签添加
        /// </summary>

        [Description("顾客标签添加")]
        TagAdd = 39,
        /// <summary>
        /// 顾客标签修改
        /// </summary>
        [Description("顾客标签修改")]
        TagUpdate = 40,
        /// <summary>
        /// 顾客标签停用或者启用
        /// </summary>
        [Description("顾客标签停用或者启用")]
        TagStopOrUse = 41,
        #endregion

        #region 会员卡相关
        /// <summary>
        /// 会员卡添加
        /// </summary>
        [Description("会员卡添加")]
        MemberCategoryAdd = 42,
        /// <summary>
        /// 会员卡修改
        /// </summary>
        [Description("会员卡修改")]
        MemberCategoryUpdate = 43,
        /// <summary>
        /// 会员卡删除
        /// </summary>
        [Description("会员卡删除")]
        MemberCategoryDelete = 44,
        /// <summary>
        /// 设置会员权益
        /// </summary>
        [Description("设置会员权益")]
        MemberCategoryEquitySet = 45,
        #endregion

        #region 权益相关
        /// <summary>
        /// 权益添加
        /// </summary>
        [Description("权益添加")]
        EquityAdd = 46,
        /// <summary>
        /// 权益修改
        /// </summary>
        [Description("权益修改")]
        EquityUpdate = 47,
        /// <summary>
        /// 权益使用停用
        /// </summary>
        [Description("权益使用停用")]
        EquityStopOrUse = 48,
        #endregion

        #region 进货信息相关
        /// <summary>
        /// 进货信息添加
        /// </summary>
        [Description("进货信息添加")]
        SmartPurchaseAdd = 49,
        /// <summary>
        /// 进货信息修改
        /// </summary>
        [Description("进货信息修改")]
        SmartPurchaseUpdate = 50,
        /// <summary>
        /// 进货信息删除
        /// </summary>
        [Description("进货信息删除")]
        SmartPurchaseDelte = 51,
        #endregion

        #region 退货信息相关
        /// <summary>
        /// 退货信息添加
        /// </summary>
        [Description("退货信息添加")]
        SmartReturnAdd = 52,
        /// <summary>
        /// 退货信息修改
        /// </summary>
        [Description("退货信息修改")]
        SmartReturnUpdate = 53,
        /// <summary>
        /// 退货信息删除
        /// </summary>
        [Description("退货信息删除")]
        SmartReturnDelte = 54,
        #endregion

        #region 库存管理相关
        /// <summary>
        /// 添加库存s
        /// </summary>
        [Description("添加库存")]
        SmartStockAdd = 55,
        /// <summary>
        /// 修改库存
        /// </summary>
        [Description("修改库存")]
        SmartStockUpdate = 56,
        #endregion

        #region 科室领用管理相关
        /// <summary>
        /// 添加科室领用
        /// </summary>
        [Description("添加科室领用")]
        UseAdd = 57,
        /// <summary>
        /// 修改科室领用
        /// </summary>
        [Description("修改可是领用")]
        UseUpdate = 58,
        #endregion      

        #region 盘盈盘亏管理相关
        /// <summary>
        /// 添加科室领用
        /// </summary>
        [Description("添加盘盈盘亏")]
        CheckAdd = 59,
        /// <summary>
        /// 修改科室领用
        /// </summary>
        [Description("修改盘盈盘亏")]
        CheckUpdate = 60,
        #endregion

        #region 采购发票管理相关
        /// <summary>
        /// 添加采购发票
        /// </summary>
        [Description("添加采购发票")]
        InvoiceAdd = 59,
        /// <summary>
        /// 删除采购发票
        /// </summary>
        [Description("删除采购发票")]
        InvoiceDelete = 60,
        #endregion

        #region 项目分类管理相关
        /// <summary>
        /// 添加项目分类
        /// </summary>
        [Description("添加项目分类")]
        ChargeCategoryAdd = 61,
        /// <summary>
        /// 删除项目分类
        /// </summary>
        [Description("删除项目分类")]
        ChargeCategoryDelete = 62,

        /// <summary>
        /// 修改项目分类
        /// </summary>
        [Description("修改项目分类")]
        ChargeCategoryUpdate = 63,
        #endregion


        #region 收费项目管理相关
        /// <summary>
        /// 收费项目添加
        /// </summary>
        [Description("添加收费项目")]
        ChargeAdd = 64,

        /// <summary>
        /// 收费项目修改
        /// </summary>
        [Description("修改收费项目")]
        ChargeUpdate = 65,
        #endregion

        #region 用户相关
        /// <summary>
        /// 用户添加
        /// </summary>
        [Description("用户添加")]
        UserAdd = 66,
        /// <summary>
        /// 用户修改
        /// </summary>
        [Description("用户修改")]
        UserUpdate = 67,
        /// <summary>
        /// 用户使用停用
        /// </summary>
        [Description("用户使用停用")]
        UserStopOrUse = 68,
        /// <summary>
        /// 用户密码重置
        /// </summary>
        [Description("用户密码重置")]
        UserPasswordReset = 69,
        /// <summary>
        /// 客户权限修改
        /// </summary>
        [Description("客户权限修改")]
        UserCustomerPermissionUpdate = 70,
        /// <summary>
        /// 客户回访权限修改
        /// </summary>
        [Description("客户回访权限修改")]
        UserCustomerCallBackPermissionUpdate = 71,
        #endregion


        #region 代金卷类型管理
        /// <summary>
        /// 代金卷类型添加
        /// </summary>
        [Description("代金卷类型添加")]
        CouponCategoryAdd = 70,
        /// <summary>
        /// 代金卷类型修改
        /// </summary>
        [Description("代金卷类型修改")]
        CouponCategoryUpdate = 71,
        /// <summary>
        /// 代金卷类型删除
        /// </summary>
        [Description("代金卷类型删除")]
        CouponCategoryDelete = 72,
        #endregion



        #region 预收款类型管理
        /// <summary>
        /// 预收款类型添加
        /// </summary>
        [Description("预收款类型添加")]
        DepositChargeAdd = 73,
        /// <summary>
        /// 预收款类型修改
        /// </summary>
        [Description("预收款类型修改")]
        DepositChargeUpdate = 74,
        /// <summary>
        /// 预收款类型删除
        /// </summary>
        [Description("预收款类型删除")]
        DepositChargeDelete = 74,
        #endregion
        #region 关系管理
        /// <summary>
        /// 关系添加
        /// </summary>
        [Description("关系添加")]
        RelationAdd = 75,
        /// <summary>
        /// 关系修改
        /// </summary>
        [Description("关系修改")]
        RelationUpdate = 76,
        /// <summary>
        /// 关系删除
        /// </summary>
        [Description("关系删除")]
        RelationDelete = 77,
        #endregion


        #region 岗位分工
        /// <summary>
        /// 岗位分工添加
        /// </summary>
        [Description("岗位分工添加")]
        PositionAdd = 78,
        /// <summary>
        /// 岗位分工修改
        /// </summary>
        [Description("岗位分工修改")]
        PositionUpdate = 79,
        #endregion


        #region 标签组管理
        /// <summary>
        /// 标签组添加
        /// </summary>
        [Description("标签组添加")]
        TagGroupAdd = 80,
        /// <summary>
        /// 标签组修改
        /// </summary>
        [Description("标签组修改")]
        TagGroupUpdate = 81,
        /// <summary>
        /// 标签组删除
        /// </summary>
        [Description("标签组删除")]
        TagGroupDelete = 82,
        #endregion

        #region 病例模板
        /// <summary>
        ///病例模板添加
        /// </summary>
        [Description("病例模板添加")]
        CaseTemplateAdd = 83,
        /// <summary>
        /// 病例模板修改
        /// </summary>
        [Description("病例模板修改")]
        CaseTemplateUpdate = 84,
        #endregion



        #region 套餐管理
        /// <summary>
        ///添加套餐
        /// </summary>
        [Description("添加套餐")]
        ChargeSetAdd = 84,
        /// <summary>
        /// 修改套餐
        /// </summary>
        [Description("修改套餐")]
        ChargeSetUpdate = 85,
        #endregion


        #region 单项目管理
        /// <summary>
        ///单项目管理添加
        /// </summary>
        [Description("单项目管理添加")]
        ClubAdd = 86,
        /// <summary>
        /// 单项目管理删除
        /// </summary>
        [Description("单项目管理修改")]
        ClubDelete = 87,
        #endregion


        #region 项目折扣管理
        /// <summary>
        ///项目折扣添加
        /// </summary>
        [Description("项目折扣添加")]
        ChargeDiscountAdd = 88,
        /// <summary>
        /// 项目折扣修改
        /// </summary>
        [Description("项目折扣修改")]
        ChargeDiscountUpdate = 89,
        #endregion


        #region 用户折扣管理
        /// <summary>
        ///用户折扣添加
        /// </summary>
        [Description("用户折扣添加")]
        UserDiscountAdd = 90,
        /// <summary>
        /// 用户折扣修改
        /// </summary>
        [Description("用户折扣修改")]
        UserDiscountUpdate = 91,
        #endregion



        #region 系统设置管理
        /// <summary>
        ///系统设置添加
        /// </summary>
        [Description("系统设置添加")]
        OptionAdd = 92,
        /// <summary>
        /// 系统设置修改
        /// </summary>
        [Description("系统设置修改")]
        OptionUpdate = 93,
        #endregion

        #region 顾客相关
        /// <summary>
        /// 顾客添加
        /// </summary>
        [Description("顾客添加")]
        CustomerAdd = 94,
        /// <summary>
        /// 顾客修改
        /// </summary>
        [Description("顾客修改")]
        CustomerUpdate = 95,
        #endregion

        #region 分享家相关
        /// <summary>
        /// 分享家添加
        /// </summary>
        [Description("分享家添加")]
        ShareCategoryAdd = 96,
        /// <summary>
        /// 分享家修改
        /// </summary>
        [Description("分享家修改")]
        ShareCategoryUpdate = 97,

        /// <summary>
        /// 分享家删除
        /// </summary>
        [Description("分享家删除")]
        ShareCategoryDelete = 98,
        #endregion

        #region 打印设置管理
        /// <summary>
        /// 打印设置修改
        /// </summary>
        [Description("打印设置修改")]
        HospitalPrintUpdate = 99,
        #endregion



        #region 排班管理相关
        /// <summary>
        /// 添加排班
        /// </summary>
        [Description("添加排班")]
        ShiftAdd = 100,
        /// <summary>
        /// 修改排班
        /// </summary>
        [Description("修改排班")]
        ShiftUpdate = 101,

        /// <summary>
        /// 删除排班
        /// </summary>
        [Description("删除排班")]
        ShiftDelete = 102,
        #endregion

        #region 系统设置相关
        /// <summary>
        /// 修改预收款成交设置
        /// </summary>
        [Description("修改预收款成交设置")]
        UpdateAdvanceSettings = 103,
        /// <summary>
        /// 修改是否允许欠款
        /// </summary>
        [Description("修改是否允许欠款")]
        UpdateAllowArrears = 104,

        /// <summary>
        /// 修改咨询模板
        /// </summary>
        [Description("修改咨询模板")]
        UpdateContentTemplate = 105,

        /// <summary>
        /// 修改客户自定义字段
        /// </summary>
        [Description("修改客户自定义字段")]
        UpdateCustomer = 106,

        /// <summary>
        /// 修改积分比例
        /// </summary>
        [Description("修改积分比例")]
        UpdateIntegralNum = 107,

        /// <summary>
        /// 修改预约设置
        /// </summary>
        [Description("修改预约设置")]
        UpdateMakeTime = 108,

        /// <summary>
        /// 修改隐私保护
        /// </summary>
        [Description("修改隐私保护")]
        UpdatePrivacyProtection = 108,

        /// <summary>
        /// 修改挂号
        /// </summary>
        [Description("修改挂号")]
        UpdateRegistration = 109,

        /// <summary>
        /// 修改等候是否开启
        /// </summary>
        [Description("修改等候是否开启")]
        UpdateWaitingDiagnosis = 110,

        #endregion



        #region 店铺管理相关
        /// <summary>
        /// 添加店铺
        /// </summary>
        [Description("添加店铺")]
        StoreAdd = 111,
        /// <summary>
        /// 修改店铺
        /// </summary>
        [Description("修改店铺")]
        StoreUpdate = 112,

        /// <summary>
        /// 删除店铺
        /// </summary>
        [Description("删除店铺")]
        StoreDelete = 113,
        #endregion


        #region 店铺负责人管理相关
        /// <summary>
        /// 添加店铺负责人
        /// </summary>
        [Description("添加店铺负责人")]
        StoreManagerAdd = 114,
        /// <summary>
        /// 删除店铺负责人
        /// </summary>
        [Description("删除店铺负责人")]
        StoreManagerDelete = 115,
        #endregion

        #region 床位相关

        /// <summary>
        /// 床位添加
        /// </summary>
        [Description("床位添加")]
        BedAdd = 116,

        /// <summary>
        /// 床位修改
        /// </summary>
        [Description("床位修改")]
        BedUpdate = 117,

        /// <summary>
        /// 床位停用或者启用
        /// </summary>
        [Description("床位停用或者启用")]
        BedStopOrUse = 118,

        #endregion



        #region 回款记录相关
        /// <summary>
        /// 添加回款记录
        /// </summary>
        [Description("添加回款记录")]
        SaleBackAdd = 119,
        /// <summary>
        /// 修改回款记录
        /// </summary>
        [Description("修改回款记录")]
        SaleBackUpdate = 120,

        /// <summary>
        /// 删除回款记录
        /// </summary>
        [Description("删除回款记录")]
        SaleBackDelete = 121,
        #endregion

        #region 卷活动管理相关
        /// <summary>
        /// 添加卷活动
        /// </summary>
        [Description("添加卷活动")]
        CouponActivityAdd = 122,
        /// <summary>
        /// 修改卷活动
        /// </summary>
        [Description("修改卷活动")]
        CouponActivityUpdate = 123,

        /// <summary>
        /// 删除卷活动
        /// </summary>
        [Description("删除卷活动")]
        CouponActivityDelete = 124,
        #endregion


        #region 卷活动详细管理相关
        /// <summary>
        /// 添加卷活动详情
        /// </summary>
        [Description("添加卷活动详情")]
        CouponActivityDetailAdd = 125,
        /// <summary>
        /// 修改卷活动详情
        /// </summary>
        [Description("修改卷活动详情")]
        CouponActivityDetailUpdate = 126,

        /// <summary>
        /// 删除卷活动详情
        /// </summary>
        [Description("删除卷活动详情")]
        CouponActivityDetailDelete = 127,
        #endregion


        #region 渠道组管理相关
        /// <summary>
        /// 添加渠道组
        /// </summary>
        [Description("添加渠道组")]
        ChannelGroupAdd = 128,
        /// <summary>
        /// 修改渠道组
        /// </summary>
        [Description("修改渠道组")]
        ChannelGroupUpdate = 129,

        /// <summary>
        /// 删除渠道组
        /// </summary>
        [Description("删除渠道组")]
        ChannelGroupDelete = 130,
        #endregion



        #region 报表项目组管理相关
        /// <summary>
        /// 添加报表项目组
        /// </summary>
        [Description("添加报表项目组")]
        ItemGroupAdd = 131,
        /// <summary>
        /// 修改报表项目组
        /// </summary>
        [Description("修改报表项目组")]
        ItemGroupUpdate = 132,

        /// <summary>
        /// 删除报表项目组
        /// </summary>
        [Description("删除报表项目组")]
        ItemGroupDelete = 133,
        #endregion


        #region 报表项目管理相关
        /// <summary>
        /// 添加报表项目
        /// </summary>
        [Description("添加报表项目")]
        SmartItemAdd = 134,
        /// <summary>
        /// 修改报表项目
        /// </summary>
        [Description("修改报表项目")]
        SmartItemUpdate = 135,

        /// <summary>
        /// 删除报表项目组
        /// </summary>
        [Description("删除报表项目组")]
        SmartItemDelete = 136,
        #endregion


        #region 审核规则相关
        /// <summary>
        /// 添加审核规则
        /// </summary>
        [Description("添加审核规则")]
        AuditRuleAdd = 137,
        /// <summary>
        /// 启用停用审核规则
        /// </summary>
        [Description("启用停用审核规则")]
        AuditRuleState = 138,

        /// <summary>
        /// 编辑审核规则
        /// </summary>
        [Description("编辑审核规则")]
        AuditRuleUpdate = 139,
        #endregion

        #region 用户通知相关
        /// <summary>
        /// 添加用户通知
        /// </summary>
        [Description("添加用户通知")]
        UserTriggerAdd = 140,
        /// <summary>
        /// 编辑用户通知
        /// </summary>
        [Description("编辑用户通知")]
        UserTriggerUpdate = 141,

        /// <summary>
        /// 删除用户通知
        /// </summary>
        [Description("删除用户通知")]
        UserTriggerDelete = 142,
        #endregion



        #region 客户分组相关
        /// <summary>
        /// 添加客户分组
        /// </summary>
        [Description("添加客户分组")]
        CustomerGroupAdd = 143,
        /// <summary>
        /// 编辑客户分组
        /// </summary>
        [Description("编辑客户分组")]
        CustomerGroupUpdate = 144,

        /// <summary>
        /// 删除客户分组
        /// </summary>
        [Description("删除客户分组")]
        CustomerGroupDelete = 145,
        /// <summary>
        /// 添加客户组用户
        /// </summary>
        [Description("添加客户组用户")]
        CustomerGroupDetailAdd = 146,

        /// <summary>
        /// 删除全部客户组所属客户
        /// </summary>
        [Description("删除全部客户组所属客户")]
        CustomerGroupDetailDelete = 147,

        /// <summary>
        /// 添加客户结果集
        /// </summary>
        [Description("添加客户结果集")]
        CustomerFilterAdd = 148,

        /// <summary>
        /// 添加客户结果集详情
        /// </summary>
        [Description("添加客户结果集详情")]
        CustomerFilterDetailAdd = 149,

        /// <summary>
        /// 合并客户组
        /// </summary>
        [Description("合并客户组")]
        MergeCustomer = 150,

        /// <summary>
        /// 筛选完成结果集后保存筛选结果
        /// </summary>
        [Description("筛选完成客户组客户后保存筛选结果")]
        CustomerFilterFiltrateAdd = 151,
        #endregion


        #region 客户归属权相关
        /// <summary>
        /// 单个添加咨询人员客户归属权
        /// </summary>
        [Description("单个添加咨询人员客户归属权")]
        SingleConsultantUserUpdateAdd = 152,
        /// <summary>
        /// 单个添加开发人员客户归属权
        /// </summary>
        [Description("单个添加开发人员客户归属权")]
        SingleDeveLoperUserUpdateAdd = 153,

        /// <summary>
        /// 批量设置咨询人员
        /// </summary>
        [Description("批量设置咨询人员")]
        BatchConsultantUserAdd = 154,

        /// <summary>
        /// 批量设置开发人员
        /// </summary>
        [Description("批量设置开发人员")]
        BatchDeveloperUserAdd = 155,
        #endregion

        #region 代金券相关
        /// <summary>
        /// 手工添加券
        /// </summary>
        [Description("手工添加券")]
        CouponManualAdd = 156,
        /// <summary>
        /// 手工扣减券
        /// </summary>
        [Description("手工扣减券")]
        CouponManualDelete = 157,
        #endregion



    
        #region 微信系统设置相关
        /// <summary>
        /// 新增级别提点
        /// </summary>
        [Description("新增级别提点")]
        WXOptionUpdatePromoteLevelAdd =158,
        /// <summary>
        /// 修改微信系统设置佣金提成
        /// </summary>
        [Description("修改微信系统设置佣金提成")]
        WXOptionOpenCommission =159,

        /// <summary>
        /// 微信系统设置修改不提点折扣小于
        /// </summary>
        [Description("微信系统设置修改不提点折扣小于")]
        WXOptionUpdateNoDiscount = 160,
        /// <summary>
        /// 微信系统设置修改推荐时限
        /// </summary>
        [Description("微信系统设置修改推荐时限")]
        WXOptionUpdateRecommenDay =161,
        /// <summary>
        /// 更新被推荐用户送券
        /// </summary>
        [Description("更新被推荐用户送券")]
        WXOptionUpdateUserSendVolume =162,
        /// <summary>
        /// 微信系统设置默认渠道
        /// </summary>
        [Description("微信系统设置默认渠道")]
        WXOptionDefaultChannel =171,
        /// <summary>
        /// 微信系统设置特殊渠道
        /// </summary>
        [Description("微信系统设置特殊渠道")]
        WXOptionSpecialChannel =172,
        #endregion



        #region 客户添加开发/咨询人员变更相关
        /// <summary>
        /// 添加/编辑咨询人员
        /// </summary>
        [Description("添加/编辑咨询人员")]
        CustomerConsultanAdd = 163,
        /// <summary>
        /// 添加/ 编辑开发人员
        /// </summary>
        [Description("添加/ 编辑开发人员")]
        CustomerDeveloperAdd = 164,
        #endregion

        #region 客户病例模板
        /// <summary>
        /// 客户病例模板
        /// </summary>
        [Description("添加客户病例模板")]
        CustomerMedicalRecordAdd = 165,
        /// <summary>
        /// 删除客户病历模板
        /// </summary>
        [Description("删除客户病历模板")]
        CustomerMedicalRecordDelete = 166,
        /// <summary>
        /// 修改客户病历模板
        /// </summary>
        [Description("修改客户病历模板")]
        CustomerMedicalRecordUpdate =167,
        #endregion


        #region 审核工作台
        /// <summary>
        /// 审核订单
        /// </summary>
        [Description("审核订单")]
        AuditOperationAdd =168,
        #endregion

        #region 我的审核申请
        /// <summary>
        /// 删除我的审核申请
        /// </summary>
        [Description("删除我的审核申请")]
        AuditApplyDelete =169,
        #endregion

        #region 投诉处理
        /// <summary>
        /// 处理投诉
        /// </summary>
        [Description("处理投诉")]
        ComplainUpdate = 170,
        #endregion
        
    }
}