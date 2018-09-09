using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.Common;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Cache;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 医院信息业务逻辑
    /// </summary>
    public class RoleService : BaseService, IRoleService
    {
        private IHospitalService _hospitalService;
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        public RoleService(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }
        /// <summary>
        /// 查询所有菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<MenuRole>> GetRoleMenu(long roleID = 0)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<MenuRole>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var menuRole = _redis.StringGet<IEnumerable<MenuRole>>(RedisPreKey.RolesMenu + roleID);

            if (menuRole != null)
            {
                result.Data = menuRole;
                return result;
            }

            TryExecute(() =>
            {
                var menuRole_temp = _connection.Query<MenuRole>("SELECT [ID],[Name],PID,Checked='false' FROM  [SmartMenu]");
                var actions = _connection.Query<Child>("SELECT a.[ID],a.[Name],a.[MenuID] as PID,case  when b.ID is null then 'false' else 'true' end as Checked " +
                    "FROM [SmartAction] a left join SmartActionRole b  on a.ID=b.ActionID and b.RoleID=@RoleID", new { RoleID = roleID });

                var temp = menuRole_temp.Where(u => u.PID == null).Select(u => new MenuRole()
                {
                    ID = 'P' + u.ID,
                    PID = "undefined",
                    Checked = u.Checked,
                    Name = u.Name,
                    Lev = 1,
                    Children = menuRole_temp.Where(m => m.PID == u.ID).Select(n => new Child()
                    {
                        ID = 'P' + n.ID,
                        PID = 'P' + n.PID,
                        Checked = n.Checked,
                        Name = n.Name,
                        Lev = 2,
                        Children = actions.Where(h => h.PID == n.ID).Select(m => new Child()
                        {
                            ID = m.ID,
                            Checked = m.Checked,
                            Name = m.Name,
                            PID = 'P' + m.PID,
                            Lev = 3
                        })
                    }).ToList()
                }).ToList();

                //for (int i = 0; i < temp.Count(); i++)
                //{
                //    for (int m = 0; m < temp[i].Children.Count(); m++)
                //    {
                //        for(int n)
                //    }
                //}

                foreach (var u in temp)
                {
                    foreach (var m in u.Children)
                    {
                        foreach (var n in m.Children)
                        {
                            if (n.Checked)
                            {
                                m.Checked = true;
                                u.Checked = true;
                                break;
                            }
                        }
                    }
                }

                _redis.StringSet(RedisPreKey.RolesMenu + roleID, temp);
                result.Data = temp;
            });
            return result;
        }

        /// <summary>
        /// 查询所有角色
        /// </summary>
        /// <param name="userHositalID"></param>
        /// <param name="hositalID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Role>> GetAllRole(long userHositalID, long hositalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Role>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;
            //var temp = _redis.StringGet<IEnumerable<Role>>(RedisPreKey.RolesOfHospital + hositalID);

            //if (temp != null)
            //{
            //    result.Data = temp;
            //    return result;
            //}

            if (!_hospitalService.HasHospital(userHositalID, hositalID))
            {
                result.Message = "对不起，您无权操作其他家的医院！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Role>("select a.[ID],a.[Name],a.[Remark],a.[CreateTime],'['+d.Name+']['+b.Name+']' as CreateUserName,c.Name as HospitalName,a.HospitalID " +
                    "from SmartRole a,SmartUser b,SmartHospital c,SmartHospital d where a.[CreateUserID]=b.ID and a.HospitalID=c.ID and b.HospitalID=d.ID and a.HospitalID=@HospitalID",
                    new { HospitalID = hositalID });

                //_redis.StringSet(RedisPreKey.RolesOfHospital + hositalID, result.Data);
            });

            return result;
        }

        /// <summary>
        /// 查询角色详细
        /// </summary>
        /// <param name="userHositalID">操作人所属ID</param>
        /// <param name="hositalID">角色所属ID</param>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, RoleDetail> GetRoleDetail(long userHositalID, long hositalID, long roleID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, RoleDetail>();

            if (!_hospitalService.HasHospital(userHositalID, hositalID))
            {
                result.Message = "对不起，您无权操作其他家的医院！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<RoleDetail>("select [ID],[Name],[Remark],[FZ],[YHRY],[CYPB],[SSYY],[CKLXFS],[CKYPCBJ],[HospitalID] from [SmartRole] where ID=@ID",
                    new { ID = roleID }).FirstOrDefault();

                if (result.Data != null)
                    result.Data.MenuRole = GetRoleMenu(roleID).Data;

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 角色添加
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(RoleAdd dto)
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
            else if (dto.Name.Length > 20)
            {
                result.Message = "名称最多20个字！";
                return result;
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            TryTransaction(() =>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                var temp = new
                {
                    ID = id,
                    Name = dto.Name,
                    Remark = dto.Remark,
                    CreateTime = DateTime.Now,
                    FZ = dto.FZ,
                    YHRY = dto.YHRY,
                    CYPB = dto.CYPB,
                    SSYY = dto.SSYY,
                    CKLXFS = dto.CKLXFS,
                    CKYPCBJ = dto.CKYPCBJ,
                    CreateUserID = dto.CreateUserID,
                    HospitalID = dto.HospitalID
                };

                result.Data = _connection.Execute(
                    "insert into [SmartRole]([ID],[Name],[Remark],[CreateTime],[FZ],[YHRY],[CYPB],[SSYY],[CKLXFS],[CKYPCBJ],[CreateUserID],[HospitalID])" +
                    " values (@ID,@Name,@Remark,@CreateTime,@FZ,@YHRY,@CYPB,@SSYY,@CKLXFS,@CKYPCBJ,@CreateUserID,@HospitalID)",
                     temp, _transaction);

                var actionTemp = dto.ActionIDS.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), ActionID = u, RoleID = id });
                _connection.Execute("insert into [SmartActionRole]([ID],[ActionID],[RoleID]) values(@ID,@ActionID,@RoleID)", actionTemp, _transaction, 30, CommandType.Text);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.RoleAdd,
                    Remark = LogType.RoleAdd.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.RoleAdd(dto.HospitalID);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 角色更新
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(RoleUpdate dto)
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
            else if (dto.Name.Length > 20)
            {
                result.Message = "名称最多20个字！";
                return result;
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            TryTransaction(() =>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                var temp = new
                {
                    ID = dto.ID,
                    Name = dto.Name,
                    Remark = dto.Remark,
                    CreateTime = DateTime.Now,
                    FZ = dto.FZ,
                    YHRY = dto.YHRY,
                    CYPB = dto.CYPB,
                    SSYY = dto.SSYY,
                    CKLXFS = dto.CKLXFS,
                    CKYPCBJ = dto.CKYPCBJ,
                    UpdateUserID = dto.CreateUserID,
                    HospitalID = dto.HospitalID
                };

                result.Data = _connection.Execute(
                    "update [SmartRole] set" +
                    " Name=@Name,Remark=@Remark,FZ=@FZ,YHRY=@YHRY,CYPB=@CYPB,SSYY=@SSYY,CKLXFS=@CKLXFS,CKYPCBJ=@CKYPCBJ,HospitalID=@HospitalID where ID=@ID",
                     temp, _transaction);

                var actionTemp = dto.ActionIDS.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), ActionID = u, RoleID = dto.ID });

                _connection.Execute("delete from SmartActionRole where RoleID=@RoleID", new { RoleID = dto.ID }, _transaction);

                _connection.Execute("insert into [SmartActionRole]([ID],[ActionID],[RoleID]) values(@ID,@ActionID,@RoleID)", actionTemp, _transaction, 30, CommandType.Text);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.RoleUpdate,
                    Remark = LogType.RoleUpdate.ToDescription() + temp.ToJsonString()
                });

                var userIDS = _connection.Query<long>("select UserID from SmartUserRole where RoleID=@RoleID", new { RoleID = dto.ID }, _transaction);

                CacheDelete.RoleUpdate(dto.ID, dto.HospitalID, userIDS);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 角色删除
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(RoleDelete dto)
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
                var temp = new { RoleID = dto.RoleID };
                int number = _connection.Execute("select count(ID) from SmartUserRole where RoleID=@RoleID", temp, _transaction);

                if (number > 0)
                {
                    result.Message = "角色已经被分配，无法删除，请先给已分配用户选择其他角色！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return false;
                }

                result.Data = _connection.Execute("delete from [SmartRole] where ID=@RoleID", temp, _transaction);

                _connection.Execute("delete from [SmartActionRole] where [RoleID]=@RoleID", temp, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.RoleDelete,
                    Remark = LogType.RoleDelete.ToDescription() + "角色ID：" + dto.RoleID
                });

                CacheDelete.RoleDelete(dto.RoleID, dto.HospitalID);

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }


    }
}
