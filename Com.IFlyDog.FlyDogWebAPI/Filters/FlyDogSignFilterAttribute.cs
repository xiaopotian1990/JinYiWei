using System.Linq;
using System.Threading.Tasks;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.MongoDB;
using Com.JinYiWei.WebAPI.Filters;
using MongoDB.Bson;

namespace Com.IFlyDog.FlyDogWebAPI.Filters
{
    /// <summary>
    /// </summary>
    public class FlyDogSignFilterAttribute : SignFilterAttribute
    {
        /// <summary>
        /// </summary>
        //private readonly MongoDBHelper<Client> helper = new MongoDBHelper<Client>(Key.MongoDBTokenConnection,Key.MongoDBToken);

        /// <summary>
        /// </summary>
        /// <param name="appid"></param>
        /// <returns></returns>
        protected override async Task<string> GeSignKey(string appid)
        {
            //var client = (await helper.LoadEntities(u => u.AppID == new ObjectId(appid))).FirstOrDefault();
            //if (client == null)
            //    return null;
            //return client.SignKey;
            return "c36ca189e67f44fa9d1078ca95b0da1f";
        }
    }
}