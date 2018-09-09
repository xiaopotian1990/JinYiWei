namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 查询病例模板dto
    /// </summary>
    public class CaseTemplateInfo
    {
        /// <summary>
        /// 病例模板id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 病例模板标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 病例模板内容
        /// </summary>
        public string RtfContent { get; set; }
        /// <summary>
        /// 状态0：停用1：使用
        /// </summary>
        public string OpenStatus { get; set; }
    }
}
