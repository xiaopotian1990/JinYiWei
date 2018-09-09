using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 各种KEY
    /// </summary>
    public static class Key
    {
        /// <summary>
        /// redis保存的爱飞狗Token开头信息
        /// </summary>
        public static string FlyDogToken = "FlyDogToken:";


        /// <summary>
        /// Web API是否使用测试
        /// </summary>
        public static int CS = Convert.ToInt32(ConfigurationManager.AppSettings["CS"]);

        /// <summary>
        /// WorkID
        /// </summary>
        public static int WorkID = Convert.ToInt32(ConfigurationManager.AppSettings["WorkID"]);
        /// <summary>
        /// DataCenterID
        /// </summary>
        public static int DataCenterID = Convert.ToInt32(ConfigurationManager.AppSettings["DataCenterID"]);

        /// <summary>
        /// token
        /// </summary>
        public static string MongoDBToken = ConfigurationManager.AppSettings["MongoDBToken"];
        /// <summary>
        /// token
        /// </summary>
        public static string MongoDBTokenConnection = ConfigurationManager.AppSettings["MongoDBTokenConnection"];

        /// <summary>
        /// 医院数量
        /// </summary>
        public static int HospitalCount = Convert.ToInt32(ConfigurationManager.AppSettings["HospitalCount"]);

        /// <summary>
        /// SSM
        /// </summary>
        public static string SSMApiUri = ConfigurationManager.AppSettings["SSMApiUri"];
        /// <summary>
        /// SSM
        /// </summary>
        public static string SSMApiUriToken = ConfigurationManager.AppSettings["SSMApiUriToken"];
        /// <summary>
        /// SSM
        /// </summary>
        public static string SSMAppid = ConfigurationManager.AppSettings["SSMAppid"];
        /// <summary>
        /// SSM
        /// </summary>
        public static string SSMAppsecred = ConfigurationManager.AppSettings["SSMAppsecred"];
        /// <summary>
        /// SSM
        /// </summary>
        public static string SSMSignKey = ConfigurationManager.AppSettings["SSMSignKey"];
        /// <summary>
        /// SSM
        /// </summary>
        public static string SSMRedis = "SSMToken:";
    }
}
