using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{   /// <summary>
    /// 系统常量
    /// </summary>
    public class GlobalConstants
    {
        public static string ModeAdd = "Add";
        public static string ModeUpdate = "Update";

        //超级管理员账号
        public static int SuperAccount =1;
        //空咨询人员账号
        public static int BlankManagerAccount = 2;

        public static string StatusOn = "启用";
        public static string StatusOff = "禁用";
        public static string Version = "1.00";

        //时间
        public static DateTime MinDateTime = new DateTime(1900, 1, 1);
        public static DateTime MaxDateTime = new DateTime(2099, 12, 31);

        public static string Title = "锦医卫医院经营管理系统";
        public static string Copyright = "2014-2015 &copy; 北京锦医卫科技有限公司.";

        //默认单页数量
        public static int DefaultPageSize = 20;
        //最大页数
        public static int MaxPageSize = 99999999;


        public static string DateFormat = "yyyy-MM-dd";
        public static string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
    }
}
