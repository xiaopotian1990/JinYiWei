namespace Com.IFlyDog.Common
{
    public static class RedisPreKey
    {
        /// <summary>
        /// 查询医院下属所有医院
        /// </summary>
        //public const string Hospital = "Hospital:";
        /// <summary>
        /// 角色菜单
        /// </summary>
        public const string RolesMenu = "Role:RoleMenu:";
        /// <summary>
        /// 医院所以角色
        /// </summary>
        //public const string RolesOfHospital = "Role:HospitalID:";
        /// <summary>
        /// Token
        /// </summary>
        public const string ToKen = "Token:";

        /// <summary>
        /// 缓存整个系统的用户信息
        /// </summary>
        public const string UserInfo = "User:Info";

        /// <summary>
        /// 登录返回信息
        /// </summary>
        public const string UserLogin = "User:Login:";

        #region 下拉菜单
        /// <summary>
        /// 下拉菜单
        /// </summary>
        public const string Category = "Select:";
        /// <summary>
        /// 下拉菜单
        /// </summary>
        //public const string CategoryHtmlSelect = "HtmlSelect:";
        #endregion

        /// <summary>
        /// 顾客ID
        /// </summary>
        public const string CustomerID = "CustmerID";

        /// <summary>
        /// 分诊人员
        /// </summary>
        public const string FZUser = "User:FZ:";

        /// <summary>
        /// 排班人员
        /// </summary>
        public const string CYPBUser = "User:PB:";

        /// <summary>
        /// 自定义字段
        /// </summary>
        public const string Option = "Option";

        /// <summary>
        /// 微信系统设置
        /// </summary>
        public const string WXOption = "WXOption";

        /// <summary>
        /// 收银记录ID
        /// </summary>
        public const string CashierID = "CashierID";
    }
}
