namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 岗位分工查询dto
    /// </summary>
    public class PositionInfo
    {
        /// <summary>
        ///主键
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
        public string Status { get; set; }
    }
}
