using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Com.JinYiWei.Common.DataAccess;
using Com.FlyDog.IFlyDogAPIBLL;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class FrontDeskService : BaseService, IFrontDeskService
    {
        /// <summary>
        /// 添加候诊
        /// </summary>
        /// <param name="dto">候诊信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddWaitAsync(WaitAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.CustomerID == 0)
            {
                result.Message = "请选择候诊顾客！";
                return result;
            }
            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注不能超过50个字！";
                return result;
            }

            await TryExecuteAsync(async () =>
            {

                var num = (await _connection.QueryAsync<int>(
                    "select count(1) from SmartWait where DateDiff(dd,[CreateTime],getdate())=0 and Status=@Status and HospitalID=@HospitalID and CustomerID=@CustomerID"
                    , new { HospitalID = dto.HospitalID, Status = CommonStatus.Stop, CustomerID = dto.CustomerID })).FirstOrDefault();
                if (num > 0)
                {
                    result.Message = "该顾客已经在今日候诊列表中！";
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                    @"insert into [SmartWait]([ID],[CustomerID],[HospitalID],[CreateTime],[CreateUserID],[Status],[Remark]) 
                    values(@ID,@CustomerID,@HospitalID,@CreateTime,@CreateUserID,@Status,@Remark)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        HospitalID = dto.HospitalID,
                        CreateTime = DateTime.Now,
                        CreateUserID = dto.CreateUserID,
                        Remark = dto.Remark,
                        Status = CommonStatus.Stop
                    });

                result.Message = "添加成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 分诊时查询出顾客粗略信息
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, CustomerInfoBefaultTriage>> GetCustomerInfoBefaultTriageAsync(long customerID, long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CustomerInfoBefaultTriage>();

            await TryExecuteAsync(async () =>
            {
                result.Data = (await _connection.QueryAsync<CustomerInfoBefaultTriage>(
                    @"select a.ID as CustomerID,a.Name as CustomerName,c.Name as Symptom,b.Name as ManagerUserName,b.ID as ManagerUserID
                    from SmartCustomer a
					left join SmartOwnerShip d on a.ID=d.CustomerID and d.HospitalID=@HospitalID and d.EndTime>@Time and d.Type=@Type
                    left join SmartUser b on d.UserID=b.ID and b.HospitalID=@HospitalID
                    left join SmartSymptom c on a.CurrentConsultSymptomID=c.ID
                    where a.ID=@ID"
                    , new { ID = customerID, HospitalID = hospitalID, Time = DateTime.Now, Type = OwnerShipType.Manager })).FirstOrDefault();

                result.Message = "查询成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }


        /// <summary>
        /// 分诊
        /// </summary>
        /// <param name="dto">分诊信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddTriageAsync(TriageAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.CustomerID == 0)
            {
                result.Message = "请选择分诊顾客！";
                return result;
            }
            if (dto.Type == TriageType.Consult)
            {
                if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
                {
                    result.Message = "备注不能超过50个字！";
                    return result;
                }
            }
            if (dto.SelectID == 0)
            {
                result.Message = "请选择咨询师或者治疗部门！";
                return result;
            }

            await TryTransactionAsync(async () =>
            {
                int time = 0;
                List<Task> tasks = new List<Task>();
                DateTime now = DateTime.Now;
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                var customer = (await _connection.QueryAsync(
                    @"select b.ID as CurrentManagerUserID,[VisitTimes],[FirstDealTime] from [SmartCustomer] a
					left join SmartOwnerShip c on a.ID=c.CustomerID and c.HospitalID=@HospitalID and c.EndTime>@Time and c.Type=@Type
                    left join SmartUser b on c.UserID=b.ID and b.HospitalID=@HospitalID  where a.ID=@ID",
                    new { ID = dto.CustomerID, HospitalID = dto.HospitalID, Time = DateTime.Now, Type = OwnerShipType.Manager }, _transaction)).FirstOrDefault();
                if (dto.Type == TriageType.Treat)
                {
                    if (customer.CurrentManagerUserID == null)
                    {
                        result.Message = "该顾客还没有现场咨询师，请先分配现场咨询师！";
                        return false;
                    }
                }

                var visitCount = (await _connection.QueryAsync<int>(
                    "select count(ID) from [SmartVisit] where CustomerID=@CustomerID and DateDiff(dd,[CreateTime],getdate())=0 and [HospitalID]=@HospitalID",
                    new { CustomerID = dto.CustomerID, HospitalID = dto.HospitalID }, _transaction)).FirstOrDefault();

                if (visitCount > 0)
                {
                    var visitDeptCount = (await _connection.QueryAsync<int>(
                                            @"select count(ID) from [SmartDeptVisit] where CustomerID=@CustomerID and DateDiff(dd,[CreateTime],getdate())=0 and DeptID=@DeptID",
                                            new { CustomerID = dto.CustomerID, DeptID = dto.SelectID }, _transaction)).FirstOrDefault();

                    if (visitDeptCount > 0)
                    {
                        result.Message = "该顾客今日已经分诊过该科室，无需重复分配！";
                        return false;
                    }
                }


                if (visitCount == 0)
                {
                    var visitType = VisitType.First;
                    if (customer.VisitTimes > 0)
                    {
                        if (customer.FirstDealTime == null)
                        {
                            visitType = VisitType.Twice;
                        }
                        else
                        {
                            if (dto.Type == TriageType.Treat)
                                visitType = VisitType.Check;
                            else if (dto.Type == TriageType.Consult)
                                visitType = VisitType.Again;
                        }
                    }
                    long? UserID = null;
                    long? DeptID = null;
                    if (dto.Type == TriageType.Treat)
                    {
                        DeptID = dto.SelectID;
                    }
                    else if (dto.Type == TriageType.Consult)
                    {
                        UserID = dto.SelectID;
                    }

                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into SmartVisit([ID],[CustomerID],CreateUserID,[UserID],[CreateTime],[HospitalID],[VisitType],DeptID) 
                        values(@ID,@CustomerID,@CreateUserID,@UserID,@CreateTime,@HospitalID,@VisitType,@DeptID)",
                        new
                        {
                            ID = id,
                            CustomerID = dto.CustomerID,
                            CreateUserID = dto.CreateUserID,
                            UserID = UserID,
                            CreateTime = now,
                            HospitalID = dto.HospitalID,
                            VisitType = visitType,
                            DeptID = DeptID
                        }, _transaction));

                    time = 1;
                }
                else
                {
                    if (dto.Type == TriageType.Consult)
                    {
                        tasks.Add(_connection.ExecuteAsync(
                            @"update SmartVisit set CreateTime=@CreateTime,UserID=@UserID where CustomerID=@CustomerID and DateDiff(dd,[CreateTime],getdate())=0 and [HospitalID]=@HospitalID",
                            new { CustomerID = dto.CustomerID, DeptID = dto.SelectID, CreateTime = now, UserID = dto.SelectID, HospitalID = dto.HospitalID }, _transaction));
                    }
                }



                if (dto.Type == TriageType.Treat)
                {
                    tasks.Add(_connection.ExecuteAsync(
                                            @"insert into [SmartDeptVisit]([ID],[HospitalID],[CustomerID],[DeptID],[CreateUserID],[CreateTime]) 
                                                    values(@ID,@HospitalID,@CustomerID,@DeptID,@CreateUserID,@CreateTime)",
                                            new
                                            {
                                                ID = id,
                                                CustomerID = dto.CustomerID,
                                                CreateTime = DateTime.Now,
                                                DeptID = dto.SelectID,
                                                CreateUserID = dto.CreateUserID,
                                                HospitalID = dto.HospitalID
                                            }, _transaction));
                }
                else if (dto.Type == TriageType.Consult)
                {
                    if (customer.CurrentManagerUserID == null)
                    {
                        tasks.Add(_connection.ExecuteAsync(@"insert into [SmartOwnerShip]([ID],[CustomerID],[UserID],[StartTime],[EndTime],[Type],[HospitalID],[Remark]) 
                                                           values(@ID,@CustomerID,@UserID,@StartTime,@EndTime,@Type,@HospitalID,@Remark)",
                                                       new
                                                       {
                                                           ID = id,
                                                           CustomerID = dto.CustomerID,
                                                           UserID = dto.SelectID,
                                                           StartTime = now,
                                                           EndTime = DateTime.MaxValue,
                                                           Type = OwnerShipType.Manager,
                                                           HospitalID = dto.HospitalID,
                                                           Remark = "首次分诊分配"
                                                       }, _transaction));

                        tasks.Add(_connection.ExecuteAsync(
                                                @"insert into [SmartTriage]([ID],[CustomerID],[CreateUserID],[CreateTime],[AssignUserID],[Remark],[HospitalID]) 
                                                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@AssignUserID,@Remark,@HospitalID)",
                                                new
                                                {
                                                    ID = id,
                                                    CustomerID = dto.CustomerID,
                                                    HospitalID = dto.HospitalID,
                                                    CreateTime = DateTime.Now,
                                                    AssignUserID = dto.SelectID,
                                                    CreateUserID = dto.CreateUserID,
                                                    Remark = dto.Remark,
                                                }, _transaction));
                    }
                    else
                    {
                        var countTriage = (await _connection.QueryAsync<int>(
                            @"select count(ID) from SmartTriage where CustomerID=@CustomerID and DateDiff(dd,[CreateTime],getdate())=0 and [HospitalID]=@HospitalID and AssignUserID=@SelectID",
                            dto, _transaction)).FirstOrDefault();

                        if (countTriage == 0)
                        {
                            tasks.Add(_connection.ExecuteAsync(
                                                @"insert into [SmartTriage]([ID],[CustomerID],[CreateUserID],[CreateTime],[AssignUserID],[Remark],[HospitalID]) 
                                                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@AssignUserID,@Remark,@HospitalID)",
                                                new
                                                {
                                                    ID = id,
                                                    CustomerID = dto.CustomerID,
                                                    HospitalID = dto.HospitalID,
                                                    CreateTime = DateTime.Now,
                                                    AssignUserID = dto.SelectID,
                                                    CreateUserID = dto.CreateUserID,
                                                    Remark = dto.Remark,
                                                }, _transaction));
                        }
                    }
                }



                var waitID = (await _connection.QueryAsync<long>(
                    @"select [ID] from [SmartWait] where [CustomerID]=@CustomerID and DateDiff(dd,[CreateTime],getdate())=0 and [HospitalID]=@HospitalID",
                    new
                    {
                        CustomerID = dto.CustomerID,
                        HospitalID = dto.HospitalID
                    }, _transaction)).FirstOrDefault();

                if (waitID != 0)
                {
                    tasks.Add(_connection.ExecuteAsync("update [SmartWait] set [Status]=@Status,FinishUserID=@FinishUserID,FinishTime=@FinishTime where ID=@ID",
                       new
                       {
                           Status = CommonStatus.Use,
                           FinishUserID = dto.CreateUserID,
                           FinishTime = now,
                           ID = waitID
                       }, _transaction));
                }

                string updateCustomer = "";
                //if (TriageType.Consult == dto.Type)
                //{
                //    updateCustomer += ",CurrentManagerUserID =case when CurrentManagerUserID is null then @CurrentManagerUserID else CurrentManagerUserID end";
                //}
                if (customer.VisitTimes == 0)
                {
                    updateCustomer += ",MemberCategoryID=1 ";
                    tasks.Add(_connection.ExecuteAsync(
                    @"insert into [SmartMember]([ID],[CustomerID],[CreateTime],[CategoryID],[Remark],[CreateUserID]) values(@ID,@CustomerID,@CreateTime,@CategoryID,@Remark,@CreateUserID)",
                    new
                    {
                        ID = id,
                        CustomerID = dto.CustomerID,
                        CreateTime = now,
                        CategoryID = 1,
                        Remark = "默认上门成为",
                        CreateUserID = 1
                    }, _transaction));
                }

                tasks.Add(_connection.ExecuteAsync(
                string.Format(@"update [SmartCustomer] set FirstVisitTime=case when FirstVisitTime is null then @VisitTime else FirstVisitTime end,
                    FirstVisitHospitalID=case  when FirstVisitHospitalID is null then @HospitalID else FirstVisitHospitalID end,
                    VisitTimes=VisitTimes+@Time,LastVisitTime=@VisitTime,LastVisitHospitalID=@HospitalID{0} where ID=@CustomerID", updateCustomer),
                new
                {
                    CustomerID = dto.CustomerID,
                    HospitalID = dto.HospitalID,
                    Time = time,
                    VisitTime = now,
                }, _transaction));

                tasks.Add(_connection.ExecuteAsync(
                    @"update [SmartTreat] set Status=@Status where CustomerID=@CustomerID and DateDiff(dd,AppointmentDate,getdate())=0 and HospitalID=@HospitalID",
                    new
                    {
                        CustomerID = dto.CustomerID,
                        HospitalID = dto.HospitalID,
                        Status = AppointmentStatus.Yes
                    }, _transaction));

                tasks.Add(_connection.ExecuteAsync(
                    @"update [SmartAppointment] set Status=@Status where CustomerID=@CustomerID and DateDiff(dd,AppointmentDate,getdate())=0 and HospitalID=@HospitalID",
                    new
                    {
                        CustomerID = dto.CustomerID,
                        HospitalID = dto.HospitalID,
                        Status = AppointmentStatus.Yes
                    }, _transaction));


                await Task.WhenAll(tasks);
                result.Message = "分诊成功！";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 获取今日候诊列表
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Wait>>> GetWaitTodayAsync(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Wait>>();

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<Wait>(
                    @"select a.ID,a.CustomerID,b.Name as CustomerName,b.ImageUrl,c.Name as ManagerUserName,a.Remark,a.CreateTime from SmartWait a
                    inner join SmartCustomer b on a.CustomerID=b.ID
					left join SmartOwnerShip d on a.CustomerID=d.CustomerID and d.HospitalID=@HospitalID and d.EndTime>@Time and d.Type=@Type
                    left join SmartUser c on d.UserID=c.ID and c.HospitalID=@HospitalID
                    where DateDiff(dd,a.CreateTime,getdate())=0 and a.Status=0 and a.HospitalID=@HospitalID
                    order by a.CreateTime",
                    new
                    {
                        HospitalID = hospitalID,
                        Status = CommonStatus.Stop,
                        Type = OwnerShipType.Manager,
                        Time = DateTime.Now
                    });

                result.Message = "查询成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 获取今日候诊列表
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<TriageToday>>> GetTriageTodayAsync(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<TriageToday>>();

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<TriageToday>(
                    @"SELECT a.ID,a.CustomerID,b.Name as CustomerName,c.Name as CreateUserName,a.CreateTime,d.Name as AssignUserName,a.Remark
                    FROM [SmartTriage] a
                    inner join SmartCustomer b on a.CustomerID=b.ID
                    inner join SmartUser c on a.CreateUserID=c.ID
                    inner join SmartUser d on a.AssignUserID=d.ID
                    where a.HospitalID=@HospitalID and DateDiff(dd,a.CreateTime,getdate())=0 order by a.CreateTime",
                    new
                    {
                        HospitalID = hospitalID,
                    });

                result.Message = "查询成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 获取今日上门记录
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<VisitToday>>> GetVisitTodayAsync(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<VisitToday>>();

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<VisitToday>(
                      @"select a.ID,a.CustomerID,e.Name as CustomerName,f.Name as ManagerUserName,a.CreateTime,
                      a.VisitType,case when a.VisitType in(1,2) then 1 else 2 end as CustomerType,
                      c.Name as CreateUserName,case when d.Name is null then b.Name else  d.Name end as AssignUserName
                      from SmartVisit a
					  left join SmartDept b on a.DeptID=b.ID
                      left join SmartUser c on a.CreateUserID=c.ID
                      left join SmartUser d on a.UserID=d.ID
                      left join SmartCustomer e on a.CustomerID=e.ID
					  left join SmartOwnerShip g on a.CustomerID=g.CustomerID and g.HospitalID=@HospitalID and g.EndTime>@Time and g.Type=@Type
                      left join SmartUser f on g.UserID=f.ID and f.HospitalID=@HospitalID
                      where a.HospitalID=@HospitalID and DateDiff(dd,a.CreateTime,getdate())=0 order by AssignUserName",
                    new
                    {
                        HospitalID = hospitalID,
                        Type = OwnerShipType.Manager,
                        Time = DateTime.Now
                    });

                //var dic = new Dictionary<string, VisitToday>();
                //foreach (var u in temp)
                //{
                //    VisitToday m = new VisitToday();
                //    if (!dic.TryGetValue(u.CustomerID, out m))
                //    {
                //        dic.Add(u.CustomerID, m = u);
                //    }
                //}

                //result.Data = dic.Values;

                result.Message = "查询成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
    }
}
