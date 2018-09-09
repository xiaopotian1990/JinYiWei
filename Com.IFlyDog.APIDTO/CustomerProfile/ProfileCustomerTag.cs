using System.Collections.Generic;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户档案顾客标签
    /// </summary>
    public class ProfileCustomerTag
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProfileCustomerTag()
        {
            Tags = new List<ProfileTag>();
        }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 标签列表
        /// </summary>
        public virtual List<ProfileTag> Tags { get; set; }
    }

    /// <summary>
    /// 客户档案顾客标签
    /// </summary>
    public class ProfileCustomerTagAdd
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 标签列表
        /// </summary>
        public virtual IEnumerable<long> Tags { get; set; }
    }
}
