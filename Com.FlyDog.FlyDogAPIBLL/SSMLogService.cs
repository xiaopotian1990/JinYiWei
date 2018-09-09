using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class SSMLogService:BaseService
    {
        private APIHelper _apiHelper = new APIHelper(Key.SSMApiUri, Key.SSMApiUriToken, Key.SSMAppid, Key.SSMAppsecred, Key.SSMSignKey, Key.SSMRedis);

        
    }
}
