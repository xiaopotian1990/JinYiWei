namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 单位管理DTO类
    /// </summary>
    public class SmartUnitInfo
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 单位管理id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 单位管理名称
        /// </summary>
        public string Name { get; set; }
    }
}
