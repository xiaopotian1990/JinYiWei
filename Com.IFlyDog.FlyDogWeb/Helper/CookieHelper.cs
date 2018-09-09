using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Com.IFlyDog.FlyDogWeb.Helper
{
    public class CookieHelper
    {
        //sessionId 的Key
        const string RedisSessionCookiesId = "RedisSessionCookiesId";
        /// <summary>
        /// 获取设置SessionID值
        /// </summary>
        /// <returns></returns>
        public static string CreatSessionCookie()
        {
            if (HttpContext.Current.Request.Cookies[RedisSessionCookiesId] != null)
            {
                return HttpContext.Current.Request.Cookies[RedisSessionCookiesId].Value.ToString();
            }
            else
            {
                Guid guid = Guid.NewGuid();
                HttpCookie cokie = new HttpCookie(RedisSessionCookiesId);
                cokie.Value = guid.ToString();
                cokie.Expires = System.DateTime.Now.AddDays(1);
                cokie.HttpOnly = true;
                HttpContext.Current.Response.Cookies.Add(cokie);
                return guid.ToString();
            }
        }

        
    }
}