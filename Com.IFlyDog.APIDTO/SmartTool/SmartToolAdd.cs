namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    ///     工具-添加
    /// </summary>
    public class SmartToolAdd
    {
        /// <summary>
        ///     工具名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        ///     操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}