using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 顾客关系
    /// </summary>
    public class ProfileRelationAdd
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
        /// 关联的顾客ID
        /// </summary>
        public long RelationCustomerID { get; set; }
        /// <summary>
        /// 关系分类
        /// </summary>
        public long RelationID { get; set; }
    }

    /// <summary>
    /// 客户档案顾客关系
    /// </summary>
    public class ProfileRelation
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 关系
        /// </summary>
        public string RelationName { get; set; }
        /// <summary>
        /// 关联的顾客
        /// </summary>
        public string RelationCustomerName { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
