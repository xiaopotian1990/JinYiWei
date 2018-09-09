using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.APIDTO.CallbackGroup;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class CallbackService : BaseService, ICallbackService
    {
        /// <summary>
        /// 个人回访情况
        /// </summary>
        /// <param name="dto">查询条件</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Callback>>> GetCallbackByCustomerID(long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Callback>>();
            result.ResultType = IFlyDogResultType.Failed;

            await TryExecuteAsync(async () =>
            {


                result.Data = await _connection.QueryAsync<Callback>(
                                  string.Format(
                                  @"SELECT  a.[ID],a.[CustomerID],'【'+i.Name+'】【'+c.Name+'】' as CreateUserName,a.[CreateTime],f.Name as Tool,[Content],d.Name as CategoryName,a.[Name],
                                  '【'+j.Name+'】【'+e.Name+'】' as UserName,[TaskTime],[TaskCreateTime],'【'+k.Name+'】【'+g.Name+'】' as TaskCreateUser,a.[Status]
                                  FROM [SmartCallback] a
								  left join SmartUser c on a.CreateUserID=c.ID
								  left join SmartCallbackCategory d on a.CategoryID=d.ID 
								  left join SmartUser e on a.UserID=e.ID
								  left join SmartTool f on a.Tool=f.ID
								  left join SmartUser g on a.TaskCreateUserID=g.ID
								  left join SmartHospital i on c.HospitalID=i.ID
								  left join SmartHospital j on e.HospitalID=j.ID
								  left join SmartHospital k on g.HospitalID=k.ID
                                  where a.CustomerID=@CustomerID order by a.Status,a.TaskTime desc"), new { CustomerID = customerID });

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
        /// <summary>
        /// 客户档案里面添加回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CallbackAdd(CallbackAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            result.ResultType = IFlyDogResultType.Failed;

            if (dto.CategoryID == 0)
            {
                result.Message = "请选择回访类型";
                return result;
            }
            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "回访内容不能为空";
                return result;
            }
            if (dto.Content.Length >= 500)
            {
                result.Message = "回访内容不能超过500字";
                return result;
            }
            if (dto.Tool == 0)
            {
                result.Message = "沟通方式不能为空";
                return result;
            }

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                await _connection.ExecuteAsync(
                    @"insert into [SmartCallback]([ID],[CustomerID],[CreateUserID],[CreateTime],[Tool],[Content],[CategoryID],[UserID],[TaskTime],[TaskCreateTime],[TaskCreateUserID],[Status]) 
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@Tool,@Content,@CategoryID,@UserID,@TaskTime,@TaskCreateTime,@TaskCreateUserID,@Status)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        Tool = dto.Tool,
                        Content = dto.Content,
                        CategoryID = dto.CategoryID,
                        UserID = dto.CreateUserID,
                        TaskTime = DateTime.Today,
                        TaskCreateTime = DateTime.Now,
                        TaskCreateUserID = dto.CreateUserID,
                        Status = CallbackStatus.Done
                    });

                result.Message = "添加回访成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 回访工作台回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CallbackAddByDesk(CallbackAddByDesk dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "回访内容不能为空";
                return result;
            }
            if (dto.Content.Length >= 500)
            {
                result.Message = "回访内容不能超过500字";
                return result;
            }
            if (dto.Tool == 0)
            {
                result.Message = "沟通方式不能为空";
                return result;
            }

            if (dto.IsNext == 1)
            {
                if (dto.NextCategoryID == 0)
                {
                    result.Message = "请选择回访类型";
                    return result;
                }
                if (dto.NextName.IsNullOrEmpty())
                {
                    result.Message = "回访计划不能为空";
                    return result;
                }
                if (dto.NextName.Length >= 50)
                {
                    result.Message = "回访计划不能超过50字";
                    return result;
                }
                if (dto.NextTaskTime <= DateTime.Today.AddDays(-1))
                {
                    result.Message = "回访时间不能早于今天";
                    return result;
                }
            }

            await TryTransactionAsync(async () =>
            {

                Task task1 = _connection.ExecuteAsync(
                    @"update SmartCallback set Tool=@Tool,Content=@Content,TaskCreateUserID=@TaskCreateUserID,TaskCreateTime=@TaskCreateTime,Status=@Status where ID=@ID",
                    new
                    {
                        Tool = dto.Tool,
                        Content = dto.Content,
                        TaskCreateUserID = dto.CreateUserID,
                        TaskCreateTime = DateTime.Now,
                        Status = CallbackStatus.Done,
                        ID = dto.ID
                    }
                    , _transaction);

                if (dto.IsNext == 1)
                {
                    Task task2 = _connection.ExecuteAsync(
                    @"insert into [SmartCallback]([ID],[CustomerID],[CreateUserID],[CreateTime],[CategoryID],[Name],[UserID],[TaskTime],[Status]) 
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@CategoryID,@Name,@UserID,@TaskTime,@Status)",
                   new
                   {
                       ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                       CustomerID = dto.CustomerID,
                       Name = dto.NextName,
                       CreateUserID = dto.CreateUserID,
                       CreateTime = DateTime.Now,
                       CategoryID = dto.NextCategoryID,
                       UserID = dto.NextUserID,
                       TaskTime = dto.NextTaskTime.Date,
                       Status = CallbackStatus.Remind
                   }, _transaction);
                    await task2;
                }

                await task1;

                result.Message = "回访成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 客户档案里面添加回访提醒
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CallbackRemindAdd(CallbackRemindAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            if (dto.CategoryID == 0)
            {
                result.Message = "请选择回访类型";
                return result;
            }
            if (dto.UserID == 0)
            {
                result.Message = "回访人员不能为空";
                return result;
            }
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "回访计划不能为空";
                return result;
            }
            if (dto.Name.Length >= 50)
            {
                result.Message = "回访计划不能超过50字";
                return result;
            }
            if (dto.TaskTime <= DateTime.Today.AddDays(-1))
            {
                result.Message = "回访时间不能早于今天";
                return result;
            }

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                await _connection.ExecuteAsync(
                    @"insert into [SmartCallback]([ID],[CustomerID],[CreateUserID],[CreateTime],[CategoryID],[Name],[UserID],[TaskTime],[Status]) 
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@CategoryID,@Name,@UserID,@TaskTime,@Status)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        Name = dto.Name,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        CategoryID = dto.CategoryID,
                        UserID = dto.UserID,
                        TaskTime = dto.TaskTime.Date,
                        Status = CallbackStatus.Remind
                    });

                result.Message = "添加回访提醒成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 客户档案里面添加回访计划
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CallbackPlanAdd(CallbackPlanAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            if (dto.SetID == 0)
            {
                result.Message = "请选择回访计划";
                return result;
            }
            if (dto.UserID == 0)
            {
                result.Message = "回访人员不能为空";
                return result;
            }
            //if (dto.TaskTime <= DateTime.Today.AddDays(-1))
            //{
            //    result.Message = "回访时间不能早于今天";
            //    return result;
            //}

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                var details = await _connection.QueryAsync<SmartCallbackSetDetailAdd>("SELECT [CategoryID],[Name],[Days] FROM [SmartCallbackSetDetail] where SetID=@SetID",
                    new { SetID = dto.SetID }, _transaction);

                await _connection.ExecuteAsync(
                    @"insert into [SmartCallback]([ID],[CustomerID],[CreateUserID],[CreateTime],[CategoryID],[Name],[UserID],[TaskTime],[Status]) 
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@CategoryID,@Name,@UserID,@TaskTime,@Status)",
                    details.Select(u => new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        CategoryID = u.CategoryID,
                        Name = u.Name,
                        UserID = dto.UserID,
                        TaskTime = dto.TaskTime.Date.AddDays(u.Days),
                        Status = CallbackStatus.Remind
                    }), _transaction);

                result.Message = "添加回访计划成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 回访工作台查询
        /// </summary>
        /// <param name="dto">查询条件</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Callback>>>> CallbackSelect(CallbackSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Callback>>>();
            result.ResultType = IFlyDogResultType.Failed;
            dto.EndTime = dto.EndTime.AddDays(1);

            result.Data = new Pages<IEnumerable<Callback>>();
            int startRow = dto.PageSize * (dto.PageNum - 1);
            int endRow = dto.PageSize;

            if (dto.StartTime >= dto.EndTime)
            {
                result.Message = "开始时间不能大于结束时间";
                return result;
            }

            string sql = " where a.TaskTime between @StartTime and @EndTime ";

            if (dto.CategoryID != -1)
            {
                sql += " and a.[CategoryID]=@CategoryID ";
            }

            if (dto.Status != CallbackStatus.All)
            {
                sql += " and a.Status=@Status ";
            }

            if (dto.UserID != 0)
            {
                sql += " and a.UserID=@UserID ";
            }

            if (!dto.Name.IsNullOrEmpty())
            {
                sql += " and a.Name=@Name ";
            }

            if (dto.CustomerID != 0)
            {
                sql += " and a.[CustomerID]=@CustomerID ";
            }

            await TryExecuteAsync(async () =>
            {
                var count = (await _connection.QueryAsync<int>(
                                  string.Format(@"with treeall as
                                  (
                                  select distinct b.ID from SmartCallbackHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@LoginUserID
                                  union all
                                  select distinct b.ID from SmartCallbackDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@LoginUserID
                                  union all
                                  select distinct OwinUserID from SmartCallbackUser where UserID=@LoginUserID
                                  ),
								  tree as
								  (
								  select distinct ID from treeall
								  )
                                  SELECT count(distinct a.[ID]) 
                                  FROM [SmartCallback] a
								  left join SmartCustomer b on a.CustomerID=b.ID 
								  left join SmartUser c on a.CreateUserID=c.ID
								  left join SmartCallbackCategory d on a.CategoryID=d.ID 
								  left join SmartUser e on a.UserID=e.ID
								  left join SmartTool f on a.Tool=f.ID
								  left join SmartUser g on a.TaskCreateUserID=g.ID
								  inner join tree h on a.UserID=h.ID
                                  {0} ", sql), dto)).FirstOrDefault();

                result.Data.PageDatas = await _connection.QueryAsync<Callback>(
                                  string.Format(@"with treeall as
                                  (
                                  select distinct b.ID from SmartCallbackHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@LoginUserID
                                  union all
                                  select distinct b.ID from SmartCallbackDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@LoginUserID
                                  union all
                                  select distinct OwinUserID from SmartCallbackUser where UserID=@LoginUserID
                                  ),
								  tree as
								  (
								  select distinct ID from treeall
								  )
                                  SELECT  a.[ID],a.[CustomerID],b.Name as CustomerName,'【'+i.Name+'】【'+c.Name+'】' as CreateUserName,a.[CreateTime],f.Name as Tool,[Content],d.Name as CategoryName,a.[Name],
                                  '【'+j.Name+'】【'+e.Name+'】' as UserName,[TaskTime],[TaskCreateTime],'【'+k.Name+'】【'+g.Name+'】' as TaskCreateUser,a.[Status]
                                  FROM [SmartCallback] a
								  left join SmartCustomer b on a.CustomerID=b.ID 
								  left join SmartUser c on a.CreateUserID=c.ID
								  left join SmartCallbackCategory d on a.CategoryID=d.ID 
								  left join SmartUser e on a.UserID=e.ID
								  left join SmartTool f on a.Tool=f.ID
								  left join SmartUser g on a.TaskCreateUserID=g.ID
								  left join SmartHospital i on c.HospitalID=i.ID
								  left join SmartHospital j on e.HospitalID=j.ID
								  left join SmartHospital k on g.HospitalID=k.ID
								  inner join tree h on a.UserID=h.ID
                                  {0} order by a.Status,a.TaskTime desc offset {1} ROWS FETCH NEXT {2} ROWS only", sql, startRow, endRow), dto);

                result.Data.PageTotals = count;
                result.Data.PageSize = dto.PageSize;
                result.Data.PageNum = dto.PageNum;

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 客户档案查询回访记录
        /// </summary>
        /// <param name="customerID"><顾客ID/param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Callback>>>> CallbackSelectByCustomerID(long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Callback>>>();

            string sql = " and a.CustomerID=@CustomerID ";

            await TryExecuteAsync(async () =>
            {
                result.Data.PageDatas = await _connection.QueryAsync<Callback>(
                                  string.Format(
                                  @"SELECT a.[ID],a.[CustomerID],b.Name as CustomerName,'【'+i.Name+'】【'+c.Name+'】' as CreateUserName,a.[CreateTime],f.Name as Tool,[Content],d.Name as CategoryName,a.[Name],
                                  '【'+j.Name+'】【'+e.Name+'】' as UserName,[TaskTime],[TaskCreateTime],'【'+k.Name+'】【'+g.Name+'】' as TaskCreateUser,a.[Status]
                                  FROM [SmartCallback] a,SmartCustomer b,SmartUser c,SmartCallbackCategory d,SmartUser e,SmartTool f,SmartUser g,SmartHospital i,SmartHospital j,SmartHospital k
                                  where a.CategoryID=d.ID and a.CreateUserID=c.ID and a.CustomerID=b.ID and a.UserID=e.ID and a.Tool=f.ID and a.TaskCreateUserID=g.ID and c.HospitalID=i.ID and e.HospitalID=j.ID and g.HospitalID=k.ID
                                  {0} order by a.Status,a.TaskTime", sql), new { CustomerID = customerID });

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 更新回访之前获取回访详细
        /// </summary>
        /// <param name="ID">回访记录ID</param>
        /// <param name="userID">回访人</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, CallbackUpdateDetail>> GetCallbackUpdateDetail(long ID, long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CallbackUpdateDetail>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(userID, customerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }
                result.Data = (await _connection.QueryAsync<CallbackUpdateDetail>(
                    @"select a.ID,a.TaskCreateTime,a.TaskCreateUserID,b.Name as TaskCreateUserName,a.CategoryID,d.Name as CategoryName,
                    a.Tool,c.Name as ToolName,a.CustomerID,a.Content 
                    from [SmartCallback] a
                    inner join SmartUser b on a.TaskCreateUserID=b.ID
                    inner join SmartTool c on a.Tool=c.ID
					inner join SmartCallbackCategory d on a.CategoryID=d.ID
                    where a.ID=@ID and a.CustomerID=@CustomerID", new { ID = ID, CustomerID=customerID })).FirstOrDefault();
            });

            return result;
        }

        /// <summary>
        /// 修改回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateCallback(CallbackUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "回访内容不能为空";
                return result;
            }
            if (dto.Content.Length >= 500)
            {
                result.Message = "回访内容不能超过500字";
                return result;
            }
            if (dto.Tool == 0)
            {
                result.Message = "沟通方式不能为空";
                return result;
            }

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }
                //int num = (await _connection.QueryAsync<int>("select count(ID) from SmartCallback where TaskCreateUserID=@TaskCreateUserID and ID=@ID", new { TaskCreateUserID = dto.CreateUserID, ID = dto.ID })).FirstOrDefault();
                //if (num == 0)
                //{
                //    result.Message = "对不起，您没有权限修改该回访！";
                //    return;
                //}

                result.Data = await _connection.ExecuteAsync(
                     @"update SmartCallback set Tool=@Tool,Content=@Content where ID=@ID and CustomerID=@CustomerID",
                     new
                     {
                         Tool = dto.Tool,
                         Content = dto.Content,
                         ID = dto.ID,
                         CustomerID = dto.CustomerID
                     });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 更新回访提醒之前获取回访提醒详细
        /// </summary>
        /// <param name="ID">回访记录ID</param>
        /// <param name="userID">回访人</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, CallbackRemindDetail>> GetCallbackRemindDetail(long ID, long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CallbackRemindDetail>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(userID, customerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }
                result.Data = (await _connection.QueryAsync<CallbackRemindDetail>(
                    @"select a.ID,a.UserID,b.Name as UserName,a.CategoryID,c.Name as CategoryName,a.TaskTime,a.Name,a.CustomerID 
                   from [SmartCallback] a
                   inner join SmartUser b on a.UserID=b.ID
                   inner join SmartCallbackCategory c on a.CategoryID=c.ID
                   where a.ID=@ID and a.CUstomerID=@CustomerID", new { ID = ID, CustomerID=customerID })).FirstOrDefault();
            });

            return result;
        }

        /// <summary>
        /// 修改回访提醒
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateCallbackRemind(CallbackRemindUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            result.ResultType = IFlyDogResultType.Failed;

            if (dto.CategoryID == 0)
            {
                result.Message = "请选择回访类型";
                return result;
            }
            if (dto.UserID == 0)
            {
                result.Message = "回访人员不能为空";
                return result;
            }
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "回访计划不能为空";
                return result;
            }
            if (dto.Name.Length >= 50)
            {
                result.Message = "回访计划不能超过50字";
                return result;
            }
            if (dto.TaskTime <= DateTime.Today.AddDays(-1))
            {
                result.Message = "回访时间不能早于今天";
                return result;
            }

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }
                //int num = (await _connection.QueryAsync<int>("select count(ID) from SmartCallback where CreateUserID=@CreateUserID and ID=@ID", new { CreateUserID = dto.CreateUserID, ID = dto.ID })).FirstOrDefault();
                //if (num == 0)
                //{
                //    result.Message = "对不起，您没有权限修改该回访！";
                //    return;
                //}

                result.Data = await _connection.ExecuteAsync(
                     @"update SmartCallback set [CategoryID]=@CategoryID,[Name]=@Name,[UserID]=@UserID,[TaskTime]=@TaskTime where ID=@ID and CustomerID=@CustomerID", dto);

                result.Message = "修改回访提醒成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 回访删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(CallbackDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            result.ResultType = IFlyDogResultType.Failed;

            if (dto.CallbackID == 0)
            {
                result.Message = "请选择回访记录";
                return result;
            }
            if (dto.CustomerID == 0)
            {
                result.Message = "顾客ID不能为空";
                return result;
            }

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                     @"delete from SmartCallback where ID=@ID and CustomerID=@CustomerID", dto);

                result.Message = "删除回访提醒成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 获取回放详细，在回访工作台点击回访查询出来
        /// </summary>
        /// <param name="ID">回访记录ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, CallbackDetail>> GetCallbackDetail(long ID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CallbackDetail>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = (await _connection.QueryAsync<CallbackDetail>(
                    @"select a.ID,b.ID as CustomerID,a.Tool,b.Name as CustomerName,datediff(year,b.Birthday,getdate()) as Age,b.Mobile,b.MobileBackup,b.Remark,c.Name as ChannelName,
                    case when b.Gender=1 then '男' else '女' end as Gender,d.Name as CategoryName
                    from SmartCallback a
					left join SmartCallbackCategory d on a.CategoryID=d.ID
                    left join SmartCustomer b on a.CustomerID=b.ID
                    left join SmartChannel c on b.ChannelID=c.ID 
                    where a.ID=@ID ", new { ID = ID })).FirstOrDefault();
            });

            return result;
        }

        /// <summary>
        /// 获取可使用的回访计划
        /// </summary>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CallbackSet>>> GetCallbackSet()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CallbackSet>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<CallbackSet>(
                    @"select [ID] as SetID,[Name],[Remark] from [SmartCallbackSet] where Status=@Status", new { Status = CommonStatus.Use });
            });

            return result;
        }

        /// <summary>
        /// 获取回访计划详细内容
        /// </summary>
        /// <param name="setID">回访计划ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CallbackSetDetail>>> GetCallbackSetDetail(long setID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CallbackSetDetail>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<CallbackSetDetail>(
                    @"select b.Name as CallbackCategoryName,a.Days,a.Name as Remark 
                    from [SmartCallbackSetDetail] a
                    inner join SmartCallbackCategory b on a.CategoryID=b.ID
                    where a.SetID=@SetID", new { SetID = setID });
            });

            return result;
        }
    }
}
