namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 知识分类修改
    /// </summary>
    public class KnowledgeCategoryUpdate
    {
        /// <summary>
        /// id
        /// 
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态0：停用1：使用
        /// </summary>
        public string OpenStatus { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
