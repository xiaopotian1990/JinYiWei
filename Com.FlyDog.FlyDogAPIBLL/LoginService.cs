using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.APIDTO;
using System.Data;
using Com.JinYiWei.Common.DataAccess;
using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.Common;
using Com.JinYiWei.Common.Secutiry;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class LoginService : BaseService, ILoginService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();

        /// <summary>
        /// 登录    
        /// <param name="login">登录信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, LoginUserInfo> Login(Login login)
        {
            var result = new IFlyDogResult<IFlyDogResultType, LoginUserInfo>();
            result.ResultType = IFlyDogResultType.Failed;


            TryExecute(() =>
            {
                var user = _connection.Query<LoginUserInfo>(@"select a.ID,a.Account,a.Name,a.Gender,a.Status,a.Phone,a.Mobile,a.HospitalID,h.Name as HospitalName 
                                                               from SmartUser a 
                                                               left join SmartHospital h on a.HospitalID=h.ID
                                                               where a.Account=@Account and a.Password=@Password",
                    new { Account = login.Account, Password = HashHelper.GetMd5(login.Password) }).FirstOrDefault();

                if (user == null)
                {
                    result.Message = "用户名密码不正确";
                    return;
                }

                if (user.Status == CommonStatus.Stop)
                {
                    result.Message = "对不起，您的账号被停用";
                    return;
                }


                result.Message = "登陆成功";
                result.ResultType = IFlyDogResultType.Success;
                var userRedis = _redis.StringGet<LoginUserInfo>(RedisPreKey.UserLogin + user.ID);

                if (userRedis != null)
                {
                    user.Menus = userRedis.Menus;
                    user.Actions = userRedis.Actions;
                    result.Data = user;
                    return;
                }

                var tmepMenu = _connection.Query<MenuTemp>(@"select distinct a.ID,e.ID as MenuID,e.Name as MenuName,e.Icon,e.URL,e.Sort,e.PID,f.Name as ParentMenuName,f.Sort as PSort,f.Icon as PIcon,d.Name as ActionName,d.ControllerAction
                                  from SmartUser a left join SmartUserRole b on a.ID=b.UserID
                                  left join SmartActionRole c on c.RoleID=b.RoleID
                                  left join SmartAction d on c.ActionID=d.ID
                                  left join SmartMenu e on d.MenuID=e.ID
                                  left join SmartMenu f on e.PID=f.ID 
                                  where a.ID=@ID", new { ID = user.ID });

                user.Actions = tmepMenu.Select(u => u.ControllerAction).Distinct();

                var temp = new Dictionary<long, LeftMenu>();
                foreach (var u in tmepMenu)
                {
                    LeftMenu left = new LeftMenu();
                    if (!temp.TryGetValue(u.PID, out left))
                    {
                        temp.Add(u.PID, left = new LeftMenu() { title = u.ParentMenuName, href = "", icon = u.PIcon, children = new List<LeftMenuChild>() });
                    }

                    if (!left.children.Any(m => m.title == u.MenuName))
                        left.children.Add(new LeftMenuChild() { title = u.MenuName, href = u.URL, icon = u.Icon });
                }

                user.Menus = temp.Values.ToList();
                result.Data = user;

                _redis.StringSet(RedisPreKey.UserLogin + user.ID, user);

            });

            return result;
        }
    }
}
