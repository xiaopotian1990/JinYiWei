using System.ComponentModel;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 下拉菜单枚举
    /// </summary>
    public enum SelectType
    {
        /// <summary>
        /// 渠道
        /// </summary>
        [Description("/Channel/GetSelect")]
        Channel = 0,
        /// <summary>
        /// 症状
        /// </summary>
        [Description("/Symptom/GetSelect")]
        Symptom = 1,
        /// <summary>
        /// 回访类型
        /// </summary>
        [Description("/CallbackCategory/GetSelect")]
        CallbackCategory = 2,
        /// <summary>
        /// 未成交类型
        /// </summary>
        [Description("/FailtureCategory/GetSelect")]
        FailtureCategory = 3,
        /// <summary>
        /// 投诉类型
        /// </summary>
        [Description("/ComplainCategory/GetSelect")]
        ComplainCategory = 4,
        /// <summary>
        /// 单位
        /// </summary>
        [Description("/SmartUnit/GetSelect")]
        Unit = 5,
        /// <summary>
        /// 银行卡
        /// </summary>
        [Description("/CardCategory/GetSelect")]
        CardCategory = 6,
        /// <summary>
        /// 工具
        /// </summary>
        [Description("/SmartTool/GetSelect")]
        Tool = 7,
        /// <summary>
        /// 关系
        /// </summary>
        [Description("/Relation/GetSelect")]
        Relation = 8,
        /// <summary>
        /// 岗位分工
        /// </summary>
        [Description("/Position/GetSelect")]
        Position = 9,
        /// <summary>
        /// 病例模板
        /// </summary>
        [Description("/CaseTemplate/GetSelect")]
        CaseTemplate = 10,
        /// <summary>
        /// 医院
        /// </summary>
        [Description("/Hospital/GetSelect")]
        Hospital = 11,
        /// <summary>
        /// 项目分类
        /// </summary>
        [Description("/ChargeCategory/GetSelect")]
        ChargeCategory = 12,
        /// <summary>
        /// 知识分类
        /// </summary>
        [Description("/KnowledgeCategory/GetSelect")]
        KnowledgeCategory = 13,
        /// <summary>
        /// 部门
        /// </summary>
        [Description("/Dept/GetSelect")]
        Dept = 14,
        /// <summary>
        /// 医院所有仓库
        /// </summary>
        [Description("/SmartWarehouse/GetSelect")]
        Warehouse = 15,
        /// <summary>
        /// 用户所管辖的仓库
        /// </summary>
        [Description("/SmartWarehouse/GetSelectByUserID")]
        WarehouseOfUser = 16,
        /// <summary>
        /// 供应商
        /// </summary>
        [Description("/SmartSupplier/GetSelect")]
        Supplier = 17,
        /// <summary>
        /// 药物品分类
        /// </summary>
        [Description("/SmartProductCategory/GetSelect")]
        ProductCategory = 18,
        /// <summary>
        /// 省
        /// </summary>
        [Description("/Province/GetSelect")]
        Province = 19,
        /// <summary>
        /// 治疗部门
        /// </summary>
        [Description("/Dept/GetTreatDeptSelect")]
        TreatDept = 20,

        /// <summary>
        /// 班次管理
        /// </summary>
        [Description("/SmartShiftCategory/GetSelect")]
        ShiftCategory = 21,

        /// <summary>
        /// 店铺
        /// </summary>
        [Description("/Store/GetSelect")]
        StoreCategory = 22,
        /// <summary>
        /// 会员等级
        /// </summary>
        [Description("/MemberCategory/GetSelect")]
        MemberCategory =23,
        /// <summary>
        /// 分享家等级
        /// </summary>
        [Description("/ShareCategory/GetSelect")]
        ShareCategory = 24,
        /// <summary>
        /// 标签
        /// </summary>
        [Description("/Tag/GetSelect")]
        TagCategory = 25,
        /// <summary>
        /// 所有医院
        /// </summary>
        [Description("/Hospital/GetSelect")]
        ALLHospital = 26,
        /// <summary>
        /// 报表项目组
        /// </summary>
        [Description("/ItemGroup/GetSelect")]
        ItemGroup=27,
        /// <summary>
        /// 报表项目
        /// </summary>
        [Description("/SmartItem/GetSelect")]
        Item =28,
        /// <summary>
        /// 券类型
        /// </summary>
        [Description("/CouponCategory/GetSelect")]
        CouponCategory = 29,
        /// <summary>
        /// 预收款项目
        /// </summary>
        [Description("/DepositCharge/GetSelect")]
        DepositCharge = 30,
    }
}
