using System.Web.Http;
using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 登陆相关API
    /// </summary>
    public class LoginController : ApiController
    {
        private ILoginService _loginService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="loginService"></param>
        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        /// <summary>
        /// 登录    
        /// <param name="login">登录信息</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, LoginUserInfo> Login(Login login)
        {
            return _loginService.Login(login);
        }
    }
}