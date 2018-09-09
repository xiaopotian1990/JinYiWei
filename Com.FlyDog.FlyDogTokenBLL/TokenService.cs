using Com.JinYiWei.Common.WebAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.JinYiWei.Common.Data;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.MongoDB;
using Com.JinYiWei.Common.Extensions;
using MongoDB.Bson;

namespace Com.FlyDog.FlyDogTokenBLL
{
    public class TokenService : ITokenService
    {
        //private MongoDBHelper<Client> helper = new MongoDBHelper<Client>(Key.MongoDBTokenConnection,Key.MongoDBToken);

        public int GetExpriedMinutes()
        {
            return 60 * 24;
        }

        public async Task<CommonResult<CommonResultType, string[]>> VerifyUser(string appid, string appsecret)
        {
            CommonResult<CommonResultType, string[]> result = new CommonResult<CommonResultType, string[]>();
            result.ResultType = CommonResultType.Failed;

            if (appid.IsNullOrEmpty())
            {
                result.Message = "appid不能为空";
                return result;
            }
            if (appsecret.IsNullOrEmpty())
            {
                result.Message = "appsecret不能为空";
                return result;
            }

            //var client = (await helper.LoadEntities(u => u.AppID == new ObjectId(appid) && u.AppSecret == appsecret)).FirstOrDefault();
            var client = new Client()
            {
                AppID = new ObjectId("58e3058d721e2a2b0c38d5af"),
                AppSecret = "2f7d7cf6b551489ca1357a87af51a030",
                Name = "c36ca189e67f44fa9d1078ca95b0da1f",
                SignKey = "admin",
                Roles = new List<string>() { "CRM" }
            };

            if (client == null)
            {
                result.Message = "appi与appsecret不正确";
                return result;
            }

            result.Data = client.Roles.ToArray<string>();

            result.Message = "获取权限成功";
            result.ResultType = CommonResultType.Success;
            return result;
        }
    }
}
