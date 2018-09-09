using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 知识分类启用停用
    /// </summary>
    public class KnowledgeCategoryStopOrUse
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 知识分类ID
        /// </summary>
        public long KnowledgeCategoryID { get; set; }
        /// <summary>
        /// 状态 1：使用；0：停用；2：删除
        /// </summary>
        public CommonStatus OpenStatus { get; set; }
    }
}
