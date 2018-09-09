using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface ILoginService
    {
        /// <summary>
        /// 登录    
        /// <param name="login">登录信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, LoginUserInfo> Login(Login login);
    }
}
