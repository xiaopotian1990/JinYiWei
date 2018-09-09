using Com.IFlyDog.Common;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Cache;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.Secutiry;
using Com.JinYiWei.Common.WebAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.FlyDogWeb
{


    public static class WebAPIHelper
    {
        //WebAPIUri Config
        static string _url = ConfigurationManager.AppSettings["ApiUri"];
        static string tokenUrl = ConfigurationManager.AppSettings["ApiUriToken"];
        //appid
        static string appid = ConfigurationManager.AppSettings["appid"];
        static string appsecret = ConfigurationManager.AppSettings["appsecred"];
        static string signKey = ConfigurationManager.AppSettings["sign_key"];

        private static RedisStackExchangeHelper _cache = new RedisStackExchangeHelper();
        /// <summary>
        /// POST提交，返回具体的类，异步
        /// </summary>
        /// <typeparam name="T">泛型，返回值类型</typeparam>
        /// <typeparam name="S">泛型，传入的值类型</typeparam>
        /// <param name="url">API地址</param>
        /// <param name="tokenUrl">Token地址</param>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="signKey">signKey</param>
        /// <param name="data">传入的具体的值</param>
        /// <returns>返回值</returns>
        public static async Task<T> Post<T, S>(string url, S data)
        {
            var token = await GetToken(tokenUrl).ConfigureAwait(false);


            string timestamp = DateTime.Now.ToTimeStamp();

            Dictionary<string, string> getCollection = new Dictionary<string, string>();
            getCollection.Add("appid",  appid);
            getCollection.Add("timestamp", timestamp);
            getCollection.Add("data", JsonConvert.SerializeObject(data));
            getCollection.Add("token", token.Data.Access_token);
            string sign = getCollection.GetClientSign("sign_key", "sign", signKey);

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("appid",  appid);
            client.DefaultRequestHeaders.Add("timestamp", timestamp);
            client.DefaultRequestHeaders.Add("token", token.Data.Access_token);
            client.DefaultRequestHeaders.Add("sign", sign);

            HttpResponseMessage response = await client.PostAsJsonAsync(_url+ url, data).ConfigureAwait(false);
            return await response.Content.ReadAsAsync<T>().ConfigureAwait(false);
        }

        /// <summary>
        /// POST提交，返回具体的类，异步
        /// </summary>
        /// <typeparam name="T">泛型，返回值类型</typeparam>
        /// <typeparam name="S">泛型，传入的值类型</typeparam>
        /// <param name="url">API地址</param>
        /// <param name="tokenUrl">Token地址</param>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="signKey">signKey</param>
        /// <param name="data">传入的具体的值</param>
        /// <returns>返回值</returns>
        public static T PostResult<T, S>(string url, S data)
        {
            var token = GetTokenResult(tokenUrl);


            string timestamp = DateTime.Now.ToTimeStamp();

            Dictionary<string, string> getCollection = new Dictionary<string, string>();
            getCollection.Add("appid", appid);
            getCollection.Add("timestamp", timestamp);
            getCollection.Add("data", JsonConvert.SerializeObject(data));
            getCollection.Add("token", token.Data.Access_token);
            string sign = getCollection.GetClientSign("sign_key", "sign", signKey);

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("appid", appid);
            client.DefaultRequestHeaders.Add("timestamp", timestamp);
            client.DefaultRequestHeaders.Add("token", token.Data.Access_token);
            client.DefaultRequestHeaders.Add("sign", sign);

            HttpResponseMessage response = client.PostAsJsonAsync(_url + url, data).Result;
            return response.Content.ReadAsAsync<T>().Result;
        }

        /// <summary>
        /// POST，返回JSON字符串，异步
        /// </summary>
        /// <typeparam name="T">泛型，传入参数类型</typeparam>
        /// <param name="url">API地址</param>
        /// <param name="tokenUrl">Token地址</param>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="signKey">signKey</param>
        /// <param name="data">传入的具体的值</param>
        /// <returns></returns>
        public static async Task<string> Post<T>(string url, T data)
        {
            var token = await GetToken( tokenUrl).ConfigureAwait(false);


            string timestamp = DateTime.Now.ToTimeStamp();

            Dictionary<string, string> getCollection = new Dictionary<string, string>();
            getCollection.Add("appid",  appid);
            getCollection.Add("timestamp", timestamp);
            getCollection.Add("data", JsonConvert.SerializeObject(data));
            getCollection.Add("token", token.Data.Access_token);
            string sign = getCollection.GetClientSign("sign_key", "sign",  signKey);

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("appid",  appid);
            client.DefaultRequestHeaders.Add("timestamp", timestamp);
            client.DefaultRequestHeaders.Add("token", token.Data.Access_token);
            client.DefaultRequestHeaders.Add("sign", sign);
            

            HttpResponseMessage response = await client.PostAsJsonAsync(_url+ url, data).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// POST，返回JSON字符串，同步
        /// </summary>
        /// <typeparam name="T">泛型，传入参数类型</typeparam>
        /// <param name="url">API地址</param>
        /// <param name="tokenUrl">Token地址</param>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="signKey">signKey</param>
        /// <param name="data">传入的具体的值</param>
        /// <returns></returns>
        public static string PostResult<T>(string url, T data)
        {
            var token = GetTokenResult(tokenUrl);


            string timestamp = DateTime.Now.ToTimeStamp();

            Dictionary<string, string> getCollection = new Dictionary<string, string>();
            getCollection.Add("appid", appid);
            getCollection.Add("timestamp", timestamp);
            getCollection.Add("data", JsonConvert.SerializeObject(data));
            getCollection.Add("token", token.Data.Access_token);
            string sign = getCollection.GetClientSign("sign_key", "sign", signKey);

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("appid", appid);
            client.DefaultRequestHeaders.Add("timestamp", timestamp);
            client.DefaultRequestHeaders.Add("token", token.Data.Access_token);
            client.DefaultRequestHeaders.Add("sign", sign);


            HttpResponseMessage response = client.PostAsJsonAsync(_url + url, data).Result;
            return response.Content.ReadAsStringAsync().Result;
        }

        /// <summary>
        /// GET请求，返回具体的类，异步
        /// </summary>
        /// <typeparam name="T">泛型，传入参数类型</typeparam>
        /// <param name="url">API地址</param>
        /// <param name="tokenUrl">Token地址</param>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="signKey">signKey</param>
        /// <param name="parames">URL字符串组</param>
        /// <returns></returns>
        public static async Task<T> Get<T>(string url, Dictionary<string, string> parames)
        {
            var token = await GetToken( tokenUrl).ConfigureAwait(false);


            string timestamp = DateTime.Now.ToTimeStamp();

            Dictionary<string, string> getCollection = new Dictionary<string, string>();
            getCollection.Add("appid", appid);
            getCollection.Add("timestamp", timestamp);
            getCollection.Add("token", token.Data.Access_token);

            foreach(var u in parames)
            {
                getCollection.Add(u.Key, u.Value);
            }

            string sign = getCollection.GetClientSign("sign_key", "sign", signKey);

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("appid", appid);
            client.DefaultRequestHeaders.Add("timestamp", timestamp);
            client.DefaultRequestHeaders.Add("token", token.Data.Access_token);
            client.DefaultRequestHeaders.Add("sign", sign);
            string fullUrl = parames.GetFullURL(_url+ url);
            HttpResponseMessage response = await client.GetAsync(fullUrl).ConfigureAwait(false);
            //string a = await response.Content.ReadAsStringAsync();
            return await response.Content.ReadAsAsync<T>().ConfigureAwait(false);
        }

        /// <summary>
        /// GET请求，返回具体的类 ,同步
        /// </summary>
        /// <typeparam name="T">泛型，传入参数类型</typeparam>
        /// <param name="url">API地址</param>
        /// <param name="tokenUrl">Token地址</param>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="signKey">signKey</param>
        /// <param name="parames">URL字符串组</param>
        /// <returns></returns>
        public static T GetResult<T>(string url, Dictionary<string, string> parames)
        {
            var token = GetTokenResult(tokenUrl);


            string timestamp = DateTime.Now.ToTimeStamp();

            Dictionary<string, string> getCollection = new Dictionary<string, string>();
            getCollection.Add("appid", appid);
            getCollection.Add("timestamp", timestamp);
            getCollection.Add("token", token.Data.Access_token);

            foreach (var u in parames)
            {
                getCollection.Add(u.Key, u.Value);
            }

            string sign = getCollection.GetClientSign("sign_key", "sign", signKey);

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("appid", appid);
            client.DefaultRequestHeaders.Add("timestamp", timestamp);
            client.DefaultRequestHeaders.Add("token", token.Data.Access_token);
            client.DefaultRequestHeaders.Add("sign", sign);
            string fullUrl = parames.GetFullURL(_url + url);
            HttpResponseMessage response = client.GetAsync(fullUrl).Result;
            //string a = await response.Content.ReadAsStringAsync();
            return response.Content.ReadAsAsync<T>().Result;
        }



        /// <summary>
        /// GET请求，返回JSON字符串,异步
        /// </summary>
        /// <param name="url">API地址</param>
        /// <param name="tokenUrl">Token地址</param>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="signKey">signKey</param>
        /// <param name="parames">URL字符串组</param>
        /// <returns></returns>
        public static async Task<string> Get(string url, Dictionary<string, string> parames)
        {
            var token = await GetToken( tokenUrl).ConfigureAwait(false);


            string timestamp = DateTime.Now.ToTimeStamp();

            Dictionary<string, string> getCollection = new Dictionary<string, string>();
            getCollection.Add("appid",  appid);
            getCollection.Add("timestamp", timestamp);
            getCollection.Add("token", token.Data.Access_token);

            foreach (var u in parames)
            {
                getCollection.Add(u.Key, u.Value);
            }

            string sign = getCollection.GetClientSign("sign_key", "sign", signKey);

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("appid", appid);
            client.DefaultRequestHeaders.Add("timestamp", timestamp);
            client.DefaultRequestHeaders.Add("token", token.Data.Access_token);
            client.DefaultRequestHeaders.Add("sign", sign);
            string fullUrl = parames.GetFullURL(_url+ url);
            HttpResponseMessage response = await client.GetAsync(fullUrl).ConfigureAwait(false);
            //string a = await response.Content.ReadAsStringAsync();
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// GET请求，返回JSON字符串,同步
        /// </summary>
        /// <param name="url">API地址</param>
        /// <param name="tokenUrl">Token地址</param>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="signKey">signKey</param>
        /// <param name="parames">URL字符串组</param>
        /// <returns></returns>
        public static string GetResult(string url, Dictionary<string, string> parames)
        {
            var token =  GetTokenResult(tokenUrl);

            string timestamp = DateTime.Now.ToTimeStamp();

            Dictionary<string, string> getCollection = new Dictionary<string, string>();
            getCollection.Add("appid", appid);
            getCollection.Add("timestamp", timestamp);
            getCollection.Add("token", token.Data.Access_token);

            foreach (var u in parames)
            {
                getCollection.Add(u.Key, u.Value);
            }

            string sign = getCollection.GetClientSign("sign_key", "sign", signKey);

            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("appid", appid);
            client.DefaultRequestHeaders.Add("timestamp", timestamp);
            client.DefaultRequestHeaders.Add("token", token.Data.Access_token);
            client.DefaultRequestHeaders.Add("sign", sign);
            string fullUrl = parames.GetFullURL(_url + url);
            HttpResponseMessage response = client.GetAsync(fullUrl).Result;
            //string a = await response.Content.ReadAsStringAsync();
            return response.Content.ReadAsStringAsync().Result;
        }

        public static async Task<IFlyDogResult<IFlyDogResultType, TokenResult>> GetToken(string url)
        {
            var t = _cache.StringGet<IFlyDogResult<IFlyDogResultType, TokenResult>>(RedisPreKey.ToKen + appid);

            if (t != null)
                return t;

            string timestamp = DateTime.Now.ToTimeStamp();

            var token = new { appid =  appid, appsecret = appsecret, grant_type = "client_credentials" };

            Dictionary<string, string> getCollection = new Dictionary<string, string>();
            getCollection.Add("appid",  appid);
            getCollection.Add("timestamp", timestamp);
            getCollection.Add("data", JsonConvert.SerializeObject(token));
            string sign = getCollection.GetClientSign("sign_key", "sign", signKey);
            //getCollection.Add("sign", md51);
            // string valuesUri = "api/Test/TTest?phoneNum=111111&identifier=222222";
            //string valuesUri = string.Format("api/NewToken/Token");
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("appid",  appid);
            client.DefaultRequestHeaders.Add("timestamp", timestamp);
            client.DefaultRequestHeaders.Add("sign", sign);
            HttpResponseMessage response = await client.PostAsJsonAsync(url, token).ConfigureAwait(false);
            //response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<IFlyDogResult<IFlyDogResultType, TokenResult>>().ConfigureAwait(false);


            if (result.ResultType != IFlyDogResultType.Success)
            {
                throw new Exception(result.Message);
            }
            _cache.StringSet(RedisPreKey.ToKen +  appid, result, _cache.ToTimeSpan(result.Data.Expires_in));
            //_cache.SetExpire(Key.FlyDogToken +  appid, result.Data.Expires_in);
            return result;
        }

        /// <summary>
        /// 同步获取token方法
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static IFlyDogResult<IFlyDogResultType, TokenResult> GetTokenResult(string url)
        {
            var t = _cache.StringGet<IFlyDogResult<IFlyDogResultType, TokenResult>>(RedisPreKey.ToKen + appid);

            if (t != null)
                return t;

            string timestamp = DateTime.Now.ToTimeStamp();

            var token = new { appid = appid, appsecret = appsecret, grant_type = "client_credentials" };

            Dictionary<string, string> getCollection = new Dictionary<string, string>();
            getCollection.Add("appid", appid);
            getCollection.Add("timestamp", timestamp);
            getCollection.Add("data", JsonConvert.SerializeObject(token));
            string sign = getCollection.GetClientSign("sign_key", "sign", signKey);
            //getCollection.Add("sign", md51);
            // string valuesUri = "api/Test/TTest?phoneNum=111111&identifier=222222";
            //string valuesUri = string.Format("api/NewToken/Token");
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("appid", appid);
            client.DefaultRequestHeaders.Add("timestamp", timestamp);
            client.DefaultRequestHeaders.Add("sign", sign);
            HttpResponseMessage response = client.PostAsJsonAsync(url, token).Result;
            //response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsAsync<IFlyDogResult<IFlyDogResultType, TokenResult>>().Result;


            if (result.ResultType != IFlyDogResultType.Success)
            {
                throw new Exception(result.Message);
            }
            _cache.StringSet(RedisPreKey.ToKen + appid, result, _cache.ToTimeSpan(result.Data.Expires_in));
            //_cache.SetExpire(Key.FlyDogToken +  appid, result.Data.Expires_in);
            return result;
        }
    }
}
