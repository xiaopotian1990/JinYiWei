using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Cache;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.Secutiry;
using Com.JinYiWei.Common.WebAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APITest
{
    public static class WebAPIHelper
    {
        //private static ICache _cache = CacheManager.CreateCache();
        /// <summary>
        /// POST提交，返回具体的类
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
        public static async Task<T> Post<T, S>(string url, string tokenUrl, string appid, string appsecret, string signKey, S data)
        {
            var token = await GetToken(tokenUrl, appid, appsecret, signKey);


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

            HttpResponseMessage response = await client.PostAsJsonAsync(url, data);
            return await response.Content.ReadAsAsync<T>();
        }

        /// <summary>
        /// POST，返回JSON字符串
        /// </summary>
        /// <typeparam name="T">泛型，传入参数类型</typeparam>
        /// <param name="url">API地址</param>
        /// <param name="tokenUrl">Token地址</param>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="signKey">signKey</param>
        /// <param name="data">传入的具体的值</param>
        /// <returns></returns>
        public static async Task<String> Post<T>(string url, string tokenUrl, string appid, string appsecret, string signKey, T data)
        {
            var token = await GetToken(tokenUrl, appid, appsecret, signKey);


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

            HttpResponseMessage response = await client.PostAsJsonAsync(url, data);
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// GET请求，返回具体的类
        /// </summary>
        /// <typeparam name="T">泛型，传入参数类型</typeparam>
        /// <param name="url">API地址</param>
        /// <param name="tokenUrl">Token地址</param>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="signKey">signKey</param>
        /// <param name="parames">URL字符串组</param>
        /// <returns></returns>
        public static async Task<T> Get<T>(string url, string tokenUrl, string appid, string appsecret, string signKey, Dictionary<string, string> parames)
        {
            var token = await GetToken(tokenUrl, appid, appsecret, signKey);


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
            string fullUrl = parames.GetFullURL(url);
            HttpResponseMessage response = await client.GetAsync(fullUrl);
            //string a = await response.Content.ReadAsStringAsync();
            return await response.Content.ReadAsAsync<T>();
        }

        /// <summary>
        /// GET请求，返回JSON字符串
        /// </summary>
        /// <param name="url">API地址</param>
        /// <param name="tokenUrl">Token地址</param>
        /// <param name="appid">appid</param>
        /// <param name="appsecret">appsecret</param>
        /// <param name="signKey">signKey</param>
        /// <param name="parames">URL字符串组</param>
        /// <returns></returns>
        public static async Task<string> Get(string url, string tokenUrl, string appid, string appsecret, string signKey, Dictionary<string, string> parames)
        {
            var token = await GetToken(tokenUrl, appid, appsecret, signKey);


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
            string fullUrl = parames.GetFullURL(url);
            HttpResponseMessage response = await client.GetAsync(fullUrl);
            //string a = await response.Content.ReadAsStringAsync();
            return await response.Content.ReadAsStringAsync();
        }

        public static async Task<IFlyDogResult<IFlyDogResultType, TokenResult>> GetToken(string url, string appid, string appsecret, string signKey)
        {
            //var t = _cache.Item_Get<IFlyDogResult<IFlyDogResultType, TokenResult>>(Key.FlyDogToken + appid);

            //if (t != null)
            //    return t;

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
            HttpResponseMessage response = await client.PostAsJsonAsync(url, token);
            //response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsAsync<IFlyDogResult<IFlyDogResultType, TokenResult>>();


            if (result.ResultType != IFlyDogResultType.Success)
            {
                throw new Exception(result.Message);
            }
            //_cache.Item_Set(Key.FlyDogToken + appid, result);
            //_cache.SetExpire(Key.FlyDogToken + appid, result.Data.Expires_in);
            return result;
        }
    }
}
