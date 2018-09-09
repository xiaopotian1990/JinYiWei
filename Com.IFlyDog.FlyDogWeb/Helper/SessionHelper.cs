using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;
using Com.JinYiWei.Common.Extensions;
using System;

namespace Com.IFlyDog.FlyDogWeb.Helper
{
    public class Session
    {

        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        public int sessionTime = 10 * 60 * 60;//单位 s

        public object this[string key]
        {
            get
            {
                key = "Session:" + key + ":" + CookieHelper.CreatSessionCookie();

                var obj = _redis.StringGet<object>(key);
                if (obj != null)
                {
                    _redis.KeyExpire(key, _redis.ToTimeSpan(sessionTime));
                }

                return obj;
            }
            set
            {
                if (value == null)
                {
                    key = "Session:" + key + ":" + CookieHelper.CreatSessionCookie(); 
                    _redis.KeyDelete(key);
                }
                else
                {
                    SetSession(key, value);
                }
            }
        }
        public void SetSession(string key, object value)
        {
            if (key.IsNullOrEmpty())
            {
                throw new Exception("Key is Null or Epmty");
            }
            key = "Session:" + key + ":" + CookieHelper.CreatSessionCookie();
            _redis.StringSet<object>(key, value, _redis.ToTimeSpan(sessionTime));
        }

        public string SessionId
        {
            get { return CookieHelper.CreatSessionCookie(); }
        }

        public void RemoveSession(string key)
        {
            if (key.IsNullOrEmpty())
            {
                throw new Exception("Key is Null or Epmty");
            }
            key = "Session:" + key + ":" + CookieHelper.CreatSessionCookie();

            _redis.KeyDelete(key);
        }

        /// <summary>
        /// 续期
        /// </summary>
        public void Postpone(string key)
        {
            if (key.IsNullOrEmpty())
            {
                throw new Exception("Key is Null or Epmty");
            }
            key = "Session:" + key + ":" + CookieHelper.CreatSessionCookie();
            _redis.KeyExpire(key, _redis.ToTimeSpan(sessionTime));
        }
    }
}