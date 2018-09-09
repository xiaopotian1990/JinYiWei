using Com.IFlyDog.CommonDTO;
using System.Collections;
using System.Collections.Generic;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 会员卡添加
    /// </summary>
    public class MemberCategoryAdd
    {
        /// <summary>
        /// 会员名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 到达金额多少自动升级
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 会员权益详细dto
        /// </summary>
        public virtual List<MemberCategoryEquityAdd> MemberCategoryEquityAdd { get; set; }
    }

    /// <summary>
    ///  会员卡修改
    /// </summary>
    public class MemberCategoryUpdate
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 到达金额多少自动升级
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 会员权益详细dto
        /// </summary>
        public virtual List<MemberCategoryEquityAdd> MemberCategoryEquityAdd { get; set; }
    }

    /// <summary>
    /// 会员卡删除
    /// </summary>
    public class MemberCategoryDelete
    {
        /// <summary>
        ///     银行卡ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        ///     操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

    }

    /// <summary>
    /// 会员卡信息
    /// </summary>
    public class MemberCategory
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 会员名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 到达金额多少自动升级
        /// </summary>
        public int Amount { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 最大级别
        /// </summary>
        public string MaxLevle { get; set; }
        /// <summary>
        /// 最大金额
        /// </summary>
        public string MaxAmount { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 会员权益详细dto
        /// </summary>
        public virtual List<MemberCategoryEquityAdd> MemberCategoryEquityAdd { get; set; }
    }

    /// <summary>
    /// 会员权益添加
    /// </summary>
    public class MemberCategoryEquityAdd
    {

        /// <summary>
        /// 权益ID
        /// </summary>
        public string EquityID { get; set; }

        /// <summary>
        /// 会员权益名称
        /// </summary>
        public string EquityName { get; set; }

        /// <summary>
        /// 权益说明
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 会员权益
    /// </summary>
    public class MemberCategoryEquity
    {
        /// <summary>
        /// 会员ID
        /// </summary>
        public string CategoryID { get; set; }
        public IEnumerable<MemberCategoryEquityTemp> Equitys { get; set; }
    }

    /// <summary>
    /// 权益详细
    /// </summary>
    public class MemberCategoryEquityTemp
    {
        /// <summary>
        /// 权益ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 权益名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}