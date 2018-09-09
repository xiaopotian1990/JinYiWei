using Com.IFlyDog.APIDTO;
using Com.JinYiWei.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.IFlyDog.FlyDogWeb.Helper
{
    /// <summary>
    /// ID帮助类
    /// </summary>
    public class IDHelper
    {
        private static Session session = new Session();

        /// <summary>
        /// 获取用户ID
        /// </summary>
        /// <returns></returns>
        public static long GetUserID()
        {
            //var temp = session["UserID"];
            //return temp == null ? 0 : long.Parse(temp.ToString());
            return 1;
        }

        /// <summary>
        /// 获取医院ID
        /// </summary>
        /// <returns></returns>
        public static long GetHospitalID()
        {
            //var temp = session["HospitalID"];
            //return temp == null ? 0 : long.Parse(temp.ToString());
            return 1;
        }

        public static T Get<T>(string key)
        {
            var temp = session[key];
            if (temp == null)
                return default(T);
            return temp.ToString().FromJsonString<T>();
        }
    }
}