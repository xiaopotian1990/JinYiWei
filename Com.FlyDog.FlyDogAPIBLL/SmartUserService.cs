using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Dapper;
using Com.IFlyDog.Common;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Secutiry;
using System.Data;
using System.Threading.Tasks;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 用户服务
    /// </summary>
    public class SmartUserService : BaseService, ISmartUserService
    {
        private IHospitalService _hospitalService;
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        public SmartUserService(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }

        /// <summary>
        /// 缓存所有用户通用方法
        /// </summary>
        /// <returns></returns>
        public IEnumerable<SmartUserInfo> GetAll()
        {
            var temp = _redis.StringGet<IEnumerable<SmartUserInfo>>(RedisPreKey.UserInfo);

            if (temp == null)
            {
                string sql = @"select a.ID,a.Account,a.Name,a.Gender,a.DeptID,b.Name as DeptName,a.Status,a.Phone,a.Mobile,a.HospitalID,c.Name as HospitalName,d.RoleID,e.Name as RoleName,
                    e.FZ,e.CYPB,e.SSYY,e.YHRY
                    from SmartUser a 
                    left join SmartDept b on a.DeptID=b.ID
                    left join SmartHospital c on a.HospitalID=c.ID
                    left join SmartUserRole d on a.ID=d.UserID
                    left join SmartRole e on d.RoleID=e.ID";

                TryExecute(() =>
                {
                    var userList = new Dictionary<string, SmartUserInfo>();
                    _connection.Query<SmartUserInfo, RoleInfo, SmartUserInfo>(sql,
                        (user, role) =>
                        {
                            SmartUserInfo userTemp = new SmartUserInfo();
                            if (!userList.TryGetValue(user.ID, out userTemp))
                            {
                                userList.Add(user.ID, userTemp = user);
                            }
                            if (role != null)
                                userTemp.Roles.Add(role);
                            return user;
                        }, null, null, true, splitOn: "RoleID");

                    temp = userList.Values;

                    _redis.StringSet(RedisPreKey.UserInfo, temp);
                });
            }

            return temp;
        }

        /// <summary>
        /// 根据ID查询详细
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartUserInfo> GetDetail(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartUserInfo>();
            result.Message = "用户查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = GetAll();

            #region 条件

            result.Data = temp.Where(u => u.ID == id.ToString()).FirstOrDefault();

            #endregion

            return result;
        }

        /// <summary>
        /// 按条件查询用户
        /// </summary>
        /// <param name="dto">条件</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartUserInfo>>> GetPages(SmartUserSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartUserInfo>>>();
            result.Message = "用户查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = GetAll();

            #region 条件
            if (dto.HospitalID == 0)
            {
                var hospitals = _hospitalService.Get(dto.HospitalID);
                temp = from u in temp
                       from n in hospitals.Data
                       where u.HospitalID == n.ID
                       select u;
            }
            else
            {
                temp = temp.Where(u => u.HospitalID == dto.HospitalID.ToString());
            }

            if (dto.DeptId != 0)
            {
                temp = temp.Where(u => u.DeptID == dto.DeptId.ToString());
            }

            if (!dto.Name.IsNullOrEmpty())
            {
                temp = temp.Where(u => u.Name.Contains(dto.Name));
            }

            if (dto.RoleID != 0)
            {
                temp = temp.Where(u => u.Roles.Any(m => m.RoleID == dto.RoleID.ToString()));
            }

            if (dto.Status != CommonStatus.All)
            {
                temp = temp.Where(u => u.Status == dto.Status);
            }

            result.Data = new Pages<IEnumerable<SmartUserInfo>>();
            result.Data.PageTotals = temp.Count();
            result.Data.PageSize = dto.PageSize;
            result.Data.PageNum = dto.PageNum;

            result.Data.PageDatas = temp.OrderBy(u => u.Name).Skip(dto.PageSize * (dto.PageNum - 1)).Take(dto.PageSize);

            #endregion

            return result;
        }

        /// <summary>
        /// 按条件查询用户
        /// </summary>
        /// <param name="dto">条件</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartUserInfo>> Get(SmartUserSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartUserInfo>>();
            result.Message = "用户查询成功";
            result.ResultType = IFlyDogResultType.Success;
            dto.Status = CommonStatus.Use;//查询使用中的
            var temp = GetAll();

            #region 条件
            if (dto.HospitalID == 0)
            {
                var hospitals = _hospitalService.Get(dto.HospitalID);
                temp = from u in temp
                       from n in hospitals.Data
                       where u.HospitalID == n.ID
                       select u;
            }
            else
            {
                temp = temp.Where(u => u.HospitalID == dto.HospitalID.ToString());
            }

            if (dto.DeptId != 0 && dto.DeptId != -1)
            {
                temp = temp.Where(u => u.DeptID == dto.DeptId.ToString());
            }

            if (!dto.Name.IsNullOrEmpty())
            {
                temp = temp.Where(u => u.Name.Contains(dto.Name));
            }

            if (dto.RoleID != 0)
            {
                temp = temp.Where(u => u.Roles.Any(m => m.RoleID == dto.RoleID.ToString()));
            }

            if (dto.Status != CommonStatus.All)
            {
                temp = temp.Where(u => u.Status == dto.Status);
            }

            result.Data = temp;

            #endregion

            return result;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="dto">用户信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(UserAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (!_hospitalService.HasHospital(dto.UserHospitalID, dto.HospitalID))
            {
                result.Message = "对不起，您无权操作其他家的医院！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 10)
            {
                result.Message = "名称最多10个字！";
                return result;
            }

            if (dto.Account.IsNullOrEmpty())
            {
                result.Message = "账号不能为空！";
                return result;
            }
            else if (dto.Account.Length > 20)
            {
                result.Message = "账号最多20个字！";
                return result;
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            if (!dto.Phone.IsMobileNumber())
            {
                result.Message = "手机号格式不正确！";
                return result;
            }

            if (!_hospitalService.HasHospital(dto.UserHospitalID, dto.HospitalID))
            {
                result.Message = "对不起，您无权操作其他家的医院！";
                return result;
            }

            if (dto.Roles == null || dto.Roles.Count() == 0)
            {
                result.Message = "对不起，请选择用户角色！";
                return result;
            }

            TryTransaction(() =>
            {
                int num = _connection.Query<int>("select count(ID) from SmartUser where Account=@Account", new { Account = dto.Account }, _transaction).FirstOrDefault();

                if (num > 0)
                {
                    result.Message = "对不起，该账号已经被使用！";
                    return false;
                }

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                var temp = new
                {
                    ID = id,
                    Account = dto.Account,
                    Password = HashHelper.GetMd5("123456"),
                    Name = dto.Name,
                    Gender = dto.Gender,
                    DeptID = dto.DeptID,
                    Status = CommonStatus.Use,
                    Remark = dto.Remark,
                    Phone = dto.Phone,
                    Mobile = dto.Mobile,
                    HospitalID = dto.HospitalID,
                    Discount = 1,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID
                };

                result.Data = _connection.Execute(
                    "insert into SmartUser([ID],[Account],[Password],[Name],[Gender],[DeptID],[Status],[Remark],[Phone],[Mobile],[HospitalID],[Discount],[CreateTime],[CreateUserID]) values(@ID,@Account,@Password,@Name,@Gender,@DeptID,@Status,@Remark,@Phone,@Mobile,@HospitalID,@Discount,@CreateTime,@CreateUserID)",
                     temp, _transaction);

                var actionTemp = dto.Roles.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), UserID = id, RoleID = u });
                _connection.Execute("insert into [SmartUserRole]([ID],[UserID],[RoleID]) values(@ID,@UserID,@RoleID)", actionTemp, _transaction, 30, CommandType.Text);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.UseAdd,
                    Remark = LogType.UseAdd.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.UserAdd(id, dto.HospitalID);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 用户修改
        /// </summary>
        /// <param name="dto">用户信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(UserAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (!_hospitalService.HasHospital(dto.UserHospitalID, dto.HospitalID))
            {
                result.Message = "对不起，您无权操作其他家的医院！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 10)
            {
                result.Message = "名称最多10个字！";
                return result;
            }

            if (dto.Account.IsNullOrEmpty())
            {
                result.Message = "账号不能为空！";
                return result;
            }
            else if (dto.Account.Length > 20)
            {
                result.Message = "账号最多20个字！";
                return result;
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            if (!dto.Phone.IsMobileNumber())
            {
                result.Message = "手机号格式不正确！";
                return result;
            }

            if (!_hospitalService.HasHospital(dto.UserHospitalID, dto.HospitalID))
            {
                result.Message = "对不起，您无权操作其他家的医院！";
                return result;
            }

            if (dto.Roles == null || dto.Roles.Count() == 0)
            {
                result.Message = "对不起，请选择用户角色！";
                return result;
            }

            if (dto.ID == 1)
            {
                result.Message = "对不起，特殊账号不允许修改！";
                return result;
            }

            TryTransaction(() =>
            {
                int num = _connection.Query<int>("select count(ID) from SmartUser where Account=@Account", new { Account = dto.Account }, _transaction).FirstOrDefault();

                if (num > 0)
                {
                    result.Message = "对不起，该账号已经被使用！";
                    return false;
                }

                result.Data = _connection.Execute(
                    "update [SmartUser] set Account=@Account,Name=@Name,Gender=@Gender,DeptID=@DeptID,Remark=@Remark,Phone=@Phone,Mobile=@Mobile,HospitalID=@HospitalID where ID=@ID",
                     dto, _transaction);

                _connection.Execute("delete from SmartUserRole where UserID=@UserID", new { UserID = dto.ID }, _transaction);

                var actionTemp = dto.Roles.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), UserID = dto.ID, RoleID = u });
                _connection.Execute("insert into [SmartUserRole]([ID],[UserID],[RoleID]) values(@ID,@UserID,@RoleID)", actionTemp, _transaction, 30, CommandType.Text);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.UserUpdate,
                    Remark = LogType.UserUpdate.ToDescription() + dto.ToJsonString()
                });

                CacheDelete.UserUpdate(dto.ID, dto.HospitalID);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 用户使用停用
        /// </summary>
        /// <param name="dto">用户信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(UserStopOrUse dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (!_hospitalService.HasHospital(dto.UserHospitalID, dto.HospitalID))
            {
                result.Message = "对不起，您无权操作其他家的医院！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }
            if (dto.UserID == 1)
            {
                result.Message = "对不起，超级管理员账号不允许修改！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            TryTransaction(() =>
            {
                result.Data = _connection.Execute(
                    "update [SmartUser] set Status=@Status where ID=@ID",
                    new { ID = dto.UserID, Status = dto.Status }, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.UserStopOrUse,
                    Remark = LogType.UserStopOrUse.ToDescription() + "UserID:" + dto.UserID
                });

                result.Message = dto.Status.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            CacheDelete.UserStopOrUse(dto.UserID, dto.HospitalID);

            return result;
        }

        /// <summary>
        /// 密码重置
        /// </summary>
        /// <param name="dto">重置信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> PasswordReset(UserPasswordReset dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (!_hospitalService.HasHospital(dto.UserHospitalID, dto.HospitalID))
            {
                result.Message = "对不起，您无权操作其他家的医院！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            TryTransaction(() =>
            {
                result.Data = _connection.Execute(
                    "update [SmartUser] set [Password]=@Password where ID=@ID",
                     new { ID = dto.UserID, Password = HashHelper.GetMd5("123456") }, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.UserPasswordReset,
                    Remark = LogType.UserPasswordReset.ToDescription() + "UserID:" + dto.UserID
                });

                result.Message = "密码重置成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 获取用户客户权限详细信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, UserCustomerPermissionDetail>> GetCustomerPermissionDetail(long userID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, UserCustomerPermissionDetail>();
            result.ResultType = IFlyDogResultType.Success;
            result.Message = "查询成功";

            UserCustomerPermissionDetail dto = new UserCustomerPermissionDetail();
            dto.UserID = userID.ToString();

            await TryExecuteAsync(async () =>
            {
                var sql = @"select a.HospitalID,b.Name as HospitalName from SmartUserHospital a,SmartHospital b where UserID=@UserID and a.HospitalID=b.ID 
                            select a.DeptID,b.Name as DeptName from SmartUserDept a,SmartDept b where UserID=@UserID and a.DeptID=b.ID 
                            select a.OwinUserID as UserID,b.Name as UserName from SmartUserUser a,SmartUser b where UserID=@UserID and a.OwinUserID=b.ID";
                var multi = await _connection.QueryMultipleAsync(sql, new { UserID = userID });

                var hospitalTask = multi.ReadAsync<HospitalSelect>();
                var DeptTask = multi.ReadAsync<DeptSelect>();
                var UserTask = multi.ReadAsync<UserSelect>();

                dto.Hospitals = await hospitalTask;
                dto.Depts = await DeptTask;
                dto.Users = await UserTask;

                result.Data = dto;
            });

            return result;
        }

        /// <summary>
        /// 获取用户回访权限详细信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, UserCustomerPermissionDetail>> GetCallBackPermissionDetail(long userID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, UserCustomerPermissionDetail>();
            result.ResultType = IFlyDogResultType.Success;
            result.Message = "查询成功";

            UserCustomerPermissionDetail dto = new UserCustomerPermissionDetail();
            dto.UserID = userID.ToString();

            await TryExecuteAsync(async () =>
            {
                var sql = @"select a.HospitalID,b.Name as HospitalName from SmartCallBackHospital a,SmartHospital b where UserID=@UserID and a.HospitalID=b.ID 
                            select a.DeptID,b.Name as DeptName from SmartCallBackDept a,SmartDept b where UserID=@UserID and a.DeptID=b.ID 
                            select a.OwinUserID as UserID,b.Name as UserName from SmartCallBackUser a,SmartUser b where UserID=@UserID and a.OwinUserID=b.ID";
                var multi = await _connection.QueryMultipleAsync(sql, new { UserID = userID });

                var hospitalTask = multi.ReadAsync<HospitalSelect>();
                var DeptTask = multi.ReadAsync<DeptSelect>();
                var UserTask = multi.ReadAsync<UserSelect>();

                dto.Hospitals = await hospitalTask;
                dto.Depts = await DeptTask;
                dto.Users = await UserTask;

                result.Data = dto;
            });

            return result;
        }

        /// <summary>
        /// 设置顾客权限
        /// </summary>
        /// <param name="dto">访问权限</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> SetCustomerPermission(UserCustomerPermission dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.UserID == 1)
            {
                result.Message = "对不起，超级管理员账号不允许修改！";
                return result;
            }

            var result_hospitals = _hospitalService.Get(dto.UserHospitalID);

            foreach (var u in dto.Hospitals)
            {
                if (!result_hospitals.Data.Any(m => m.ID == u.ToString()))
                {
                    result.Message = "对不起，您无权操作其他家的医院！";
                    return result;
                }
            }

            TryTransaction(() =>
            {
                _connection.Execute("delete from SmartUserHospital where UserID=@UserID", dto, _transaction);
                _connection.Execute("delete from SmartUserDept where UserID=@UserID", dto, _transaction);
                _connection.Execute("delete from SmartUserUser where UserID=@UserID", dto, _transaction);

                // var Temp = dto.Roles.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), UserID = id, RoleID = u });
                _connection.Execute("insert into [SmartUserHospital]([ID],[UserID],[HospitalID]) values(@ID,@UserID,@HospitalID)",
                    dto.Hospitals.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), UserID = dto.UserID, HospitalID = u }), _transaction, 30, CommandType.Text);
                _connection.Execute("insert into [SmartUserDept]([ID],[UserID],[DeptID]) values(@ID,@UserID,@DeptID)",
                    dto.Depts.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), UserID = dto.UserID, DeptID = u }), _transaction, 30, CommandType.Text);
                _connection.Execute("insert into [SmartUserUser]([ID],[UserID],[OwinUserID]) values(@ID,@UserID,@OwinUserID)",
                    dto.Users.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), UserID = dto.UserID, OwinUserID = u }), _transaction, 30, CommandType.Text);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.UserCustomerPermissionUpdate,
                    Remark = LogType.UserCustomerPermissionUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "设置客户权限成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 设置访问权限
        /// </summary>
        /// <param name="dto">访问权限</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> SetCustomerCallBackPermission(UserCustomerPermission dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var result_hospitals = _hospitalService.Get(dto.UserHospitalID);

            if (dto.UserID == 1)
            {
                result.Message = "对不起，超级管理员账号不允许修改！";
                return result;
            }

            foreach (var u in dto.Hospitals)
            {
                if (!result_hospitals.Data.Any(m => m.ID == u.ToString()))
                {
                    result.Message = "对不起，您无权操作其他家的医院！";
                    return result;
                }
            }

            TryTransaction(() =>
            {
                _connection.Execute("delete from SmartCallBackHospital where UserID=@UserID", dto, _transaction);
                _connection.Execute("delete from SmartCallBackDept where UserID=@UserID", dto, _transaction);
                _connection.Execute("delete from SmartCallBackUser where UserID=@UserID", dto, _transaction);

                // var Temp = dto.Roles.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), UserID = id, RoleID = u });
                _connection.Execute("insert into [SmartCallBackHospital]([ID],[UserID],[HospitalID]) values(@ID,@UserID,@HospitalID)",
                    dto.Hospitals.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), UserID = dto.UserID, HospitalID = u }), _transaction, 30, CommandType.Text);
                _connection.Execute("insert into [SmartCallBackDept]([ID],[UserID],[DeptID]) values(@ID,@UserID,@DeptID)",
                    dto.Depts.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), UserID = dto.UserID, DeptID = u }), _transaction, 30, CommandType.Text);
                _connection.Execute("insert into [SmartCallBackUser]([ID],[UserID],[OwinUserID]) values(@ID,@UserID,@OwinUserID)",
                    dto.Users.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), UserID = dto.UserID, OwinUserID = u }), _transaction, 30, CommandType.Text);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.UserCustomerCallBackPermissionUpdate,
                    Remark = LogType.UserCustomerCallBackPermissionUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "设置访问权限权限成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 获取分疹人员列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<FZUser>> GetFZUsers(string hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<FZUser>>();
            result.Message = "用户查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var tempFZ = _redis.StringGet<IEnumerable<FZUser>>(RedisPreKey.FZUser + hospitalID + ":" + DateTime.Today.ToShortDateString());
            if (tempFZ != null)
            {
                result.Data = tempFZ;
                return result;
            }

            var temp = GetAll();

            TryExecute(() =>
            {
                var shiftTemp = _connection.Query<FZUser>(
                    @"select a.UserID as ID,b.Name as Shift from SmartShift a
                    left join SmartShiftCategory b on a.[CategoryID]=b.ID
                    where DateDiff(dd,a.[ShiftDate],getdate())=0");


                var a = temp.Where(u => u.Roles.Any(n => n.FZ == 1) && u.HospitalID == hospitalID && u.Status == CommonStatus.Use).ToList();
                var userShift = (from u in temp
                                 join m in shiftTemp
                                 on u.ID equals m.ID into t
                                 where u.Roles.Any(n => n.FZ == 1) && u.HospitalID == hospitalID && u.Status == CommonStatus.Use
                                 from s in t.DefaultIfEmpty
                                 (
                                     new FZUser()
                                     {
                                         ID = u.ID,
                                         Name = u.Name,
                                         Shift = "无排班信息"
                                     }
                                 )
                                 select new FZUser()
                                 {
                                     ID = u.ID,
                                     Name = u.Name,
                                     Shift = s.Shift
                                 }).ToList();

                result.Data = userShift;
                _redis.StringSet(RedisPreKey.FZUser + hospitalID + ":" + DateTime.Today.ToShortDateString(), userShift, _redis.ToTimeSpan(24 * 60 * 60));
            });

            return result;
        }

        /// <summary>
        /// 获取参与排班用户
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CYPBUser>> GetCYPBUsers(SmartUserSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CYPBUser>>();

            TryExecute(() =>
            {
                string sql = @"SELECT DISTINCT a.ID,a.Account,a.Name,f.Name AS DeptName
                    FROM dbo.SmartUser a 
                    left JOIN dbo.SmartUserRole d ON a.ID=d.UserID
                    left JOIN dbo.SmartRole e ON d.RoleID=e.ID AND e.CYPB=1
                    left JOIN dbo.SmartDept f ON a.DeptID=f.ID
                    WHERE 1=1 ";

                if (!string.IsNullOrWhiteSpace(dto.DeptId.ToString()) && dto.DeptId.ToString() != "-1")
                {
                    sql += " And a.DeptID='" + dto.DeptId + "'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql += @" And a.Name LIKE '%" + dto.Name + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Account))
                {
                    sql += @" And a.Account LIKE '%" + dto.Account + "%'";
                }

                sql += " And a.HospitalID='" + dto.HospitalID + "'";


                var shiftTemp = _connection.Query<CYPBUser>(sql);
                result.Data = shiftTemp;
                result.Message = "用户查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;

        }

        /// <summary>
        /// 获取参与预约用户
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>> GetSSYYUsers(long hospitalID, DateTime date)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();

            await TryExecuteAsync(async () =>
            {
                result.Data =await _connection.QueryAsync<Select>(
                    @"SELECT DISTINCT a.ID,a.Name+'  |   '+ case when c.Name is null then '无排班信息' else c.Name end as Name
                    FROM dbo.SmartUser a 
                    left JOIN dbo.SmartUserRole d ON a.ID=d.UserID
                    inner JOIN dbo.SmartRole e ON d.RoleID=e.ID AND e.SSYY=1
                    left JOIN SmartShift b ON a.ID=b.UserID and b.ShiftDate=@ShiftDate
					left join SmartShiftCategory c on b.CategoryID=c.ID
					where a.HospitalID=@HospitalID and a.Status=@Status", new { HospitalID = hospitalID, ShiftDate = date.Date, Status = CommonStatus.Use });

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;

        }
    }
}
