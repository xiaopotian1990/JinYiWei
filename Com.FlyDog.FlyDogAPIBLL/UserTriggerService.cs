using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.DataAccess;
using Dapper;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 用户通知业务处理
    /// </summary>
    public class UserTriggerService : BaseService, IUserTriggerService
    {
        /// <summary>
        /// 添加用户通知
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(UserTriggerAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 50)
            {
                result.Message = "名称最多50个字符！";
                return result;
            }

            if (dto.Type == 0 || dto.Type == -1)
            {
                result.Message = "请选择触发条件！";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Info))
            {
                result.Message = "请输入提醒信息！";
                return result;
            }
            else if (!dto.Info.IsNullOrEmpty() && dto.Info.Length >= 50)
            {
                result.Message = "提醒信息最多50个字符！";
                return result;
            }

            if (dto.CustomerType == 1)
            {
                if (string.IsNullOrWhiteSpace(dto.MemberCategoryID))
                {
                    result.Message = "请选择会员类型！";
                    return result;
                }
            }

            if (dto.CustomerType == 2)
            {
                if (string.IsNullOrWhiteSpace(dto.CustomerGroupID))
                {
                    result.Message = "请选择客户组！";
                    return result;
                }
            }

            if (dto.CustomerType == 3)
            {
                if (string.IsNullOrWhiteSpace(dto.ShareCategoryID))
                {
                    result.Message = "请选择分享家类型！";
                    return result;
                }
            }

            if (dto.AllUsers==0&&dto.ExploitUserStatus==0&&dto.ManagerUserStatus==0&&dto.AssignDeptInfoAdd==null&&dto.AssignUserInfoAdd==null) {
                result.Message = "请选择提醒人员类型！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字符！";
                return result;
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {
                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute(@"insert into SmartUserTrigger(ID,Name,Type,CustomerType,CustomerGroupID,MemberCategoryID,ShareCategoryID,Info,AllUsers,ExploitUserStatus,ManagerUserStatus,Remark) VALUES(@ID, @Name, @Type, @CustomerType, @CustomerGroupID, @MemberCategoryID, @ShareCategoryID, @Info, @AllUsers, @ExploitUserStatus,@ManagerUserStatus,@Remark)",
                    new { ID = id, Name = dto.Name, Type = dto.Type, CustomerType = dto.CustomerType, CustomerGroupID = dto.CustomerGroupID, MemberCategoryID = dto.MemberCategoryID, ShareCategoryID = dto.ShareCategoryID, Info = dto.Info, AllUsers = dto.AllUsers, ExploitUserStatus = dto.ExploitUserStatus, ManagerUserStatus = dto.ManagerUserStatus, Remark = dto.Remark }, _transaction);


                if (dto.AssignUserInfoAdd!=null) {
                    foreach (var u in dto.AssignUserInfoAdd)
                    {
                        _connection.Execute("insert into SmartUserTriggerDetail(ID,TriggerID,UserID) VALUES(@ID, @TriggerID, @UserID)",
                            new
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                TriggerID = id,
                                UserID = u.ID
                            }, _transaction); //用户通知方案详情表
                    };
                }

                if (dto.AssignDeptInfoAdd!=null) {
                    foreach (var u in dto.AssignDeptInfoAdd)
                    {
                        _connection.Execute("insert into SmartUserTriggerDeptDetail(ID,TriggerID,DeptID) VALUES(@ID, @TriggerID, @DeptID)",
                            new
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                TriggerID = id,
                                DeptID = u.ID
                            }, _transaction); //用户通知方案部门详情表
                    };
                }
                



                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.UserTriggerAdd,
                    Remark = LogType.UserTriggerAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 删除用户通知
        /// </summary>
        /// <param name="dto"></param>
        ///// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(UserTriggerDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 开始事物操作
            TryTransaction(() =>
            {
                string sql = @"DELETE SmartUserTrigger WHERE ID=@ID
             DELETE SmartUserTriggerDetail WHERE TriggerID = @TriggerID
             DELETE SmartUserTriggerDeptDetail WHERE TriggerID = @TriggerID1";
                #region 开始更新操作
                result.Data = _connection.Execute(sql, new { ID=dto.ID, TriggerID=dto.ID, TriggerID1 =dto.ID}, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartUnitDelete,
                    Remark = LogType.SmartUnitDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion
                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取全部用户通知
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<UserTriggerInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<UserTriggerInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<UserTriggerInfo>(@"SELECT ID,Name,Type,CustomerType,CustomerGroupID,MemberCategoryID,Info,AllUsers,(CASE CustomerType WHEN 0 THEN '全体'
                            WHEN 1 THEN(SELECT Name FROM dbo.SmartMemberCategory WHERE ID = SmartUserTrigger.MemberCategoryID)
                            WHEN 2 THEN(SELECT Name FROM SmartCustomerGroup WHERE ID = SmartUserTrigger.CustomerGroupID)

                            WHEN 3 THEN(SELECT Name FROM dbo.SmartShareCategory WHERE ID = SmartUserTrigger.ShareCategoryID)

                             END) AS CustomerScope, ExploitUserStatus, ManagerUserStatus,
                             Remark FROM dbo.SmartUserTrigger");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, UserTriggerInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, UserTriggerInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<UserTriggerInfo>(@"SELECT ID,Name,Type,CustomerType,CustomerGroupID,MemberCategoryID,Info,AllUsers,(CASE CustomerType WHEN 0 THEN '全体'
                            WHEN 1 THEN(SELECT Name FROM dbo.SmartMemberCategory WHERE ID=SmartUserTrigger.MemberCategoryID)
                            WHEN 2 THEN(SELECT Name FROM SmartCustomerGroup WHERE ID = SmartUserTrigger.CustomerGroupID)
							WHEN 3 THEN(SELECT Name FROM dbo.SmartShareCategory WHERE ID=SmartUserTrigger.ShareCategoryID)
							 END) AS CustomerScope,ExploitUserStatus,ManagerUserStatus,
							 Remark FROM dbo.SmartUserTrigger WHERE ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.AssignUserInfoAdd = new List<AssignUserInfo>();
                result.Data.AssignUserInfoAdd = _connection.Query<AssignUserInfo>(@"SELECT sutd.UserID AS ID,su.Name FROM SmartUserTriggerDetail AS sutd LEFT JOIN dbo.SmartUser AS su ON sutd.UserID=su.ID WHERE TriggerID=@TriggerID", new { TriggerID = id }).ToList();

                result.Data.AssignDeptInfoAdd = new List<AssignDeptInfo>();
                result.Data.AssignDeptInfoAdd = _connection.Query<AssignDeptInfo>(@"SELECT sutd.DeptID AS ID,sd.Name FROM SmartUserTriggerDeptDetail AS sutd LEFT JOIN dbo.SmartDept AS sd ON sutd.DeptID=sd.ID WHERE TriggerID=@TriggerID", new { TriggerID = id }).ToList();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新用户通知
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(UserTriggerUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 50)
            {
                result.Message = "名称最多50个字符！";
                return result;
            }

            if (dto.Type == 0 || dto.Type == -1)
            {
                result.Message = "请选择触发条件！";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Info))
            {
                result.Message = "请输入提醒信息！";
                return result;
            }
            else if (!dto.Info.IsNullOrEmpty() && dto.Info.Length >= 50)
            {
                result.Message = "提醒信息最多50个字符！";
                return result;
            }

            if (dto.CustomerType == 1)
            {
                if (string.IsNullOrWhiteSpace(dto.MemberCategoryID))
                {
                    result.Message = "请选择会员类型！";
                    return result;
                }
            }

            if (dto.CustomerType == 2)
            {
                if (string.IsNullOrWhiteSpace(dto.CustomerGroupID))
                {
                    result.Message = "请选择客户组！";
                    return result;
                }
            }

            if (dto.CustomerType == 3)
            {
                if (string.IsNullOrWhiteSpace(dto.ShareCategoryID))
                {
                    result.Message = "请选择分享家类型！";
                    return result;
                }
            }

            if (dto.AllUsers == 0 && dto.ExploitUserStatus == 0 && dto.ManagerUserStatus == 0 && dto.AssignDeptInfoAdd == null && dto.AssignUserInfoAdd == null)
            {
                result.Message = "请选择提醒人员类型！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字符！";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                #region 开始更新操作

                string sql = @"
                 DELETE SmartUserTriggerDetail WHERE TriggerID=@TriggerID1
                   DELETE SmartUserTriggerDeptDetail WHERE TriggerID=@TriggerID
                ";

                _connection.Execute(sql,
                   new { TriggerID1 = dto.ID, TriggerID = dto.ID }, _transaction); //删除3张映射表中的数据

                if (dto.AssignUserInfoAdd!=null) {
                    foreach (var u in dto.AssignUserInfoAdd)
                    {
                        _connection.Execute("insert into SmartUserTriggerDetail(ID,TriggerID,UserID) VALUES(@ID, @TriggerID, @UserID)",
                            new
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                TriggerID = dto.ID,
                                UserID = u.ID
                            }, _transaction); //用户通知方案详情表
                    };
                }

                if (dto.AssignDeptInfoAdd!=null) {
                    foreach (var u in dto.AssignDeptInfoAdd)
                    {
                        _connection.Execute("insert into SmartUserTriggerDeptDetail(ID,TriggerID,DeptID) VALUES(@ID, @TriggerID, @DeptID)",
                            new
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                TriggerID = dto.ID,
                                DeptID = u.ID
                            }, _transaction); //用户通知方案部门详情表
                    };
                }
               

                result.Data = _connection.Execute(@"UPDATE SmartUserTrigger SET Name=@Name,Type=@Type,CustomerType=@CustomerType,
                CustomerGroupID = @CustomerGroupID, MemberCategoryID = @MemberCategoryID, ShareCategoryID = @ShareCategoryID, Info = @Info,
                AllUsers = @AllUsers, ExploitUserStatus = @ExploitUserStatus, ManagerUserStatus = @ManagerUserStatus, Remark = @Remark
                 WHERE ID = @ID", new { ID = dto.ID, Name = dto.Name, Type = dto.Type, CustomerType = dto.CustomerType, CustomerGroupID = dto.CustomerGroupID, MemberCategoryID = dto.MemberCategoryID, ShareCategoryID = dto.ShareCategoryID, Info = dto.Info, AllUsers = dto.AllUsers, ExploitUserStatus = dto.ExploitUserStatus, ManagerUserStatus = dto.ManagerUserStatus, Remark = dto.Remark }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.UserTriggerUpdate,
                    Remark = LogType.UserTriggerUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
