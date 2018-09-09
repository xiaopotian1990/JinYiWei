using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class Client
    {
        /// <summary>
        /// App ID
        /// </summary>
        [BsonId]
        public ObjectId AppID { get; set; }
        /// <summary>
        /// AppSecret
        /// </summary>
        public string AppSecret { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 密钥
        /// </summary>
        public string SignKey { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public IList<string> Roles { get; set; }
    }
}
