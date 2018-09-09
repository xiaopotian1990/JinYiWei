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
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class CustomerService : BaseService, ICustomerService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加顾客
        /// </summary>
        /// <param name="dto">顾客信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, long>> AddAsync(CustomerAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, long>();
            result.ResultType = IFlyDogResultType.Failed;

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

            if (!dto.Birthday.IsBetween(DateTime.Today.AddYears(-80), DateTime.Today.AddYears(-10)))
            {
                result.Message = string.Format("生日日期应该在{0}-{1}之间！", DateTime.Today.AddYears(-80).ToShortDateString(), DateTime.Today.AddYears(-10).ToShortDateString());
                return result;
            }

            if (!dto.Mobile.IsNullOrEmpty() && !dto.Mobile.IsMobileNumber())
            {
                result.Message = "手机号格式不正确！";
                return result;
            }

            if (dto.Address.IsNullOrEmpty())
            {
                result.Message = "地址不能为空！";
                return result;
            }
            else if (dto.Address.Length > 50)
            {
                result.Message = "地址最多50个字！";
                return result;
            }

            if (dto.ConsultContent.IsNullOrEmpty())
            {
                result.Message = "咨询内容不能为空！";
                return result;
            }
            else if (dto.ConsultContent.Length > 500)
            {
                result.Message = "咨询内容不能超过500个字符！";
                return result;
            }

            if (dto.SymptomIDS == null || dto.SymptomIDS.Count() == 0)
            {
                result.Message = "咨询项目数量要至少为1！";
                return result;
            }

            if (dto.CustomerRegisterType == CustomerRegisterType.Exploit)
            {
                if (dto.CurrentExploitUserID == 0)
                {
                    result.Message = "请选择开发人员！";
                    return result;
                }
            }
            else if (dto.CustomerRegisterType == CustomerRegisterType.Market)
            {
                if (dto.CurrentExploitUserID == 0)
                {
                    result.Message = "请选择开发人员！";
                    return result;
                }
                if (dto.StoreID == 0)
                {
                    result.Message = "请选择店家！";
                    return result;
                }
            }

            var customerID = _redis.StringIncrement(RedisPreKey.CustomerID, 1);

            await TryTransactionAsync(async () =>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                DateTime now = DateTime.Now;

                var num = (await _connection.QueryAsync<int>("select count(ID) from [SmartCustomer] where [Mobile]=@Mobile or [MobileBackup]=@Mobile", new { Mobile = dto.Mobile }, _transaction)).FirstOrDefault();

                if (num > 0)
                {
                    result.Message = "改手机号已经被登记！";
                    return false;
                }

                long memberCategoryID = 0;
                if (dto.CurrentExploitUserID == 0)
                {
                    dto.CurrentExploitUserID = dto.HospitalID + 1;
                }

                long? storeID = null;
                if (dto.StoreID != 0)
                {
                    storeID = dto.StoreID;
                }

                var sql = @"insert into [SmartCustomer]([ID],[Name],[Gender],[Mobile],[CreateTime],[ChannelID],[CreateUserID],[CreateUserHospitalID],[Deposit],
                            [Coupon],[Point],[FirstConsultTime],[VisitTimes],[ConsultTimes],[LastConsultTime],[Birthday],
                            [Address],[CurrentConsultSymptomID],[MemberCategoryID],[CashCardTotalAmount],[CityID],[StoreID],Source) 
                            values(@ID,@Name,@Gender,@Mobile,@CreateTime,@ChannelID,@CreateUserID,@CreateUserHospitalID,@Deposit,
                            @Coupon,@Point,@FirstConsultTime,@VisitTimes,@ConsultTimes,@LastConsultTime,@Birthday,
                            @Address,@CurrentConsultSymptomID,@MemberCategoryID,@CashCardTotalAmount,@CityID,@StoreID,@Source)";
                var task1 = _connection.ExecuteAsync(sql,
                    new
                    {
                        ID = customerID,
                        Name = dto.Name,
                        Gender = dto.Gender,
                        Mobile = dto.Mobile,
                        CreateTime = now,
                        ChannelID = dto.ChannelID,
                        CreateUserID = dto.CreateUserID,
                        CreateUserHospitalID = dto.HospitalID,
                        Deposit = 0,
                        Coupon = 0,
                        Point = 0,
                        FirstConsultTime = now,
                        VisitTimes = 0,
                        ConsultTimes = 1,
                        LastConsultTime = now,
                        Birthday = dto.Birthday,
                        Address = dto.Address,
                        CurrentConsultSymptomID = dto.SymptomIDS.FirstOrDefault(),
                        MemberCategoryID = memberCategoryID,
                        CashCardTotalAmount = 0,
                        CityID = dto.CityID,
                        StoreID = storeID,
                        Source = dto.CustomerRegisterType
                    }, _transaction);

                var task2 = _connection.ExecuteAsync(@"insert into [SmartConsult]([ID],[CustomerID],[CreateUserID],[CreateTime],[Tool],[Content]) 
                                         values(@ID,@CustomerID,@CreateUserID,@CreateTime,@Tool,@Content)",
                                         new
                                         {
                                             ID = id,
                                             CustomerID = customerID,
                                             CreateUserID = dto.CreateUserID,
                                             CreateTime = now,
                                             Tool = dto.ToolID,
                                             Content = dto.ConsultContent
                                         }, _transaction);

                var task3 = _connection.ExecuteAsync(@"insert into [SmartConsultSymptomDetail]([ID],[ConsultID],[SymptomID]) values(@ID,@ConsultID,@SymptomID)",
                      dto.SymptomIDS.Select(u => new
                      {
                          ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                          ConsultID = id,
                          SymptomID = u
                      }), _transaction, 30, CommandType.Text);

                var task5 = _connection.ExecuteAsync(@"insert into [SmartOwnerShip]([ID],[CustomerID],[UserID],[StartTime],[EndTime],[Type],[HospitalID],[Remark]) 
                                                     values(@ID,@CustomerID,@UserID,@StartTime,@EndTime,@Type,@HospitalID,@Remark)",
                                                     new
                                                     {
                                                         ID = id,
                                                         CustomerID = customerID,
                                                         UserID = dto.CurrentExploitUserID,
                                                         StartTime = now,
                                                         EndTime = DateTime.MaxValue,
                                                         Type = OwnerShipType.Exploit,
                                                         HospitalID = dto.HospitalID,
                                                         Remark = "登记自动分配"
                                                     }, _transaction);

                var task6 = _connection.ExecuteAsync(
                    @"insert into [SmartMember]([ID],[CustomerID],[CreateTime],[CategoryID],[Remark],[CreateUserID]) values(@ID,@CustomerID,@CreateTime,@CategoryID,@Remark,@CreateUserID)",
                    new
                    {
                        ID = id,
                        CustomerID = customerID,
                        CreateTime = now,
                        CategoryID = memberCategoryID,
                        Remark = "默认登记成为",
                        CreateUserID = 1
                    }, _transaction);

                var task4 = AddOperationLogAsync(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CustomerAdd,
                    Remark = LogType.CustomerAdd.ToDescription() + dto.ToJsonString(),
                    CustomerID = customerID
                });
                //Task.WaitAll(task1, task2, task3, task4);
                await Task.WhenAll(task1, task2, task3, task4, task5, task6);

                result.ResultType = IFlyDogResultType.Success;
                result.Message = "顾客登记成功";
                result.Data = customerID;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 顾客识别
        /// </summary>
        /// <param name="name">各种识别码</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerIdentifyInfo>>> CustomerIdentifyAsync(string name)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerIdentifyInfo>>();
            if (name.IsNullOrEmpty())
            {
                result.ResultType = IFlyDogResultType.Failed;
                result.Message = "请输入 顾客编号 / 手机号码 / 电话号码 / 预约码 /姓名";
                return result;
            }

            string sql = "";
            if (name.IsNumeric())
            {
                if (name.IsMobileNumber())
                {
                    sql = " where Mobile=@Name or MobileBackup=@Name ";
                }
                else
                {
                    sql = " where a.ID=@Name ";
                }
            }
            else
            {
                sql = " where a.Name like @CustomerName or b.Code=@Name ";
            }

            await TryExecuteAsync(async () =>
           {
               result.Data = await _connection.QueryAsync<CustomerIdentifyInfo>(
                    string.Format(@"select distinct a.ID,a.Name,case when a.Gender=1 then '男' else '女' end as Gender,replace(a.Mobile,substring(a.Mobile,4,4),'****') as Mobile,c.Name as MemberCategoryName,c.Icon as MemberIcon,d.Name as ShareCategoryName,d.Icon as ShareIcon,
                    e.Name as ChannelName,case when a.VisitTimes=0 then '未上门' else '已上门' end as Come,case when a.CashCardTotalAmount=0 then '未成交' else '已成交' end as Cash, 
                    f.Name as CreateHospital,g.Name as FirstVisitHospital from SmartCustomer a 
                    left join SmartAppointment b on a.ID=b.CustomerID 
                    left join SmartMemberCategory c on c.ID=a.MemberCategoryID 
                    left join SmartShareCategory d on d.ID=a.ShareMemberCategoryID 
                    left join SmartChannel e on e.ID=a.ChannelID 
                    left join SmartHospital f on a.CreateUserHospitalID=f.ID
					left join SmartHospital g on a.FirstVisitHospitalID=g.ID
                    {0}", sql), new { Name = name, CustomerName = "%" + name + "%" });

               result.Message = "查询成功";
               result.ResultType = IFlyDogResultType.Success;
           });

            return result;
        }

        /// <summary>
        /// 查询今日登记顾客
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerCreateToday>>> CustomerCreateTodayAsync(long hospitalID, CustomerRegisterType type)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerCreateToday>>();

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<CustomerCreateToday>(
                    @"select a.ID,a.Name,case when a.Gender=1 then '男' else '女' end as Gender,a.CreateTime,b.Name as CreateUserName,e.Name as ChannelName,c.Name as ConsultSymptom 
                    from SmartCustomer a 
                    left join SmartUser b on b.ID=a.CreateUserID 
                    left join SmartChannel e on e.ID=a.ChannelID 
                    left join SmartSymptom c on c.ID=a.CurrentConsultSymptomID 
                    where DateDiff(dd,a.CreateTime,getdate())=0 and a.CreateUserHospitalID=@HospitalID and Source=@Source",
                    new
                    {
                        HospitalID = hospitalID,
                        Source = type
                    });

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 客户管理查询
        /// </summary>
        /// <param name="dto">筛选条件</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CustomerManager>>>> GetCustomerManager(CustomerSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CustomerManager>>>();
            result.Data = new Pages<IEnumerable<CustomerManager>>();
            result.Data.PageNum = dto.PageNum;
            result.Data.PageSize = dto.PageSize;
            result.ResultType = IFlyDogResultType.Failed;
            if (dto.HospitalID == 0)
            {
                result.Message = "操作人所在医院不能为空！";
                return result;
            }
            if (dto.CreateUserID == 0)
            {
                result.Message = "操作人不能为空！";
                return result;
            }


            int startRow = dto.PageSize * (dto.PageNum - 1);
            int endRow = dto.PageSize;

            #region sql
            string sql_where = " where 1=1 ";
            if (dto.CustomerID > 0)
            {
                sql_where += " and a.ID=@CustomerID ";
            }

            if (!dto.CustomerName.IsNullOrEmpty())
            {
                sql_where += " and a.Name like @CustomerName ";
                dto.CustomerName = "%" + dto.CustomerName + "%";
            }

            if (dto.Mobile.IsMobileNumber())
            {
                sql_where += " and (a.Mobile=@Mobile or a.MobileBackup=@Mobile )";
            }

            if (dto.Gender != GenderEnum.All)
            {
                sql_where += " and a.Gender=@Gender";
            }

            if (dto.ExploitUserID > 0)
            {
                sql_where += " and i.ID=@ExploitUserID ";
            }

            if (dto.ManagerUserID > 0)
            {
                sql_where += " and k.ID=@ManagerUserID ";
            }

            if (dto.ChannelID > 0)
            {
                sql_where += " and a.ChannelID=@ChannelID ";
            }

            if (dto.SymptomID > 0)
            {
                sql_where += " and a.CurrentConsultSymptomID=@SymptomID ";
            }

            if (dto.MemberCategoryID > 0)
            {
                sql_where += " and a.MemberCategoryID=@MemberCategoryID ";
            }

            if (dto.ShareCategoryID > 0)
            {
                sql_where += " and a.ShareMemberCategoryID=@ShareCategoryID ";
            }

            if (dto.FirstVisitTimeStart != null && dto.FirstVisitTimeEnd != null)
            {
                sql_where += " and a.FirstVisitTime between @FirstVisitTimeStart and @FirstVisitTimeEnd ";
                dto.FirstVisitTimeEnd = Convert.ToDateTime(dto.FirstVisitTimeEnd).AddDays(1);
            }
            if (dto.LastVisitTimeStart != null && dto.LastVisitTimeEnd != null)
            {
                sql_where += " and a.LastVisitTime between @LastVisitTimeStart and @LastVisitTimeEnd ";
                dto.LastVisitTimeEnd = Convert.ToDateTime(dto.LastVisitTimeEnd).AddDays(1);
            }
            if (dto.CreateTimeStart != null && dto.CreateTimeEnd != null)
            {
                sql_where += " and a.CreateTime between @CreateTimeStart and @CreateTimeEnd ";
                dto.CreateTimeEnd = Convert.ToDateTime(dto.CreateTimeEnd).AddDays(1);
            }
            if (dto.LastConsultTimeStart != null && dto.LastConsultTimeEnd != null)
            {
                sql_where += " and a.LastConsultTime between @LastConsultTimeStart and @LastConsultTimeEnd ";
                dto.LastConsultTimeEnd = Convert.ToDateTime(dto.LastConsultTimeEnd).AddDays(1);
            }

            if (dto.DealType == DealType.Yes)
            {
                sql_where += " and a.FirstDealTime is not null ";
            }
            else if (dto.DealType == DealType.No)
            {
                sql_where += " and a.FirstDealTime is null ";
            }

            if (dto.ComeType == ComeType.Yes)
            {
                sql_where += " and a.FirstVisitTime is not null ";
            }
            else if (dto.ComeType == ComeType.No)
            {
                sql_where += " and a.FirstVisitTime is null ";
            }

            if (dto.WechatStatus == WechatStatus.Binding)
            {
                sql_where += " and a.WeChatBind is not null ";
            }
            else if (dto.WechatStatus == WechatStatus.UnBinding)
            {
                sql_where += " and a.WeChatBind is null ";
            }

            if (dto.CashStart != null && dto.CashEnd != null)
            {
                sql_where += " and a.CashCardTotalAmount between @CashStart and @CashEnd ";
            }

            if (dto.TagID > 0)
            {
                sql_where = " inner join SmartCustomerTag r on r.CustomerID=a.ID and r.TagID=@TagID " + sql_where;
            }

            if (dto.StoreID > 0)
            {
                sql_where += " and a.StoreID=@StoreID ";
            }

            if (dto.AppointmentStart != null && dto.AppointmentEnd != null)
            {
                sql_where += " and a.LastAppointmentTime between @AppointmentStart and @AppointmentEnd ";
                dto.AppointmentEnd = Convert.ToDateTime(dto.AppointmentEnd).AddDays(1);
            }

            if (dto.VisitHospitalID > 0)
            {
                sql_where = " inner join (select distinct CustomerID from SmartVisit where HospitalID=@VisitHospitalID) as s on a.ID=s.CustomerID " + sql_where;
            }

            string sql = string.Format(@"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@CreateUserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@CreateUserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@CreateUserID
                        ),
						treeCustomer as
						(
						select distinct a.CustomerID from SmartOwnerShip a,treeUser b where a.UserID=b.ID  and a.EndTime> getdate() 
						union
						select distinct CustomerID from SmartTriage where AssignUserID=@CreateUserID and DateDiff(dd,CreateTime,getdate())=0
						)

						select a.ID as CustomerID,a.Name as CustomerName,a.Gender,datediff(year,a.Birthday,getdate()) as Age,d.Icon as MemberCategoryImage,e.Icon as ShareCategoryImage,
						f.Name as Channel,g.Name as Symptom,i.Name as ExploitUserName,k.Name as ManagerUserName,case when a.FirstDealTime is null then 0 else 1 end as DealType,
						case when a.FirstVisitTime is null then 0 else 1 end as ComeType,a.FirstVisitTime,l.Name as FirstVisitHospital,a.LastVisitTime,
						a.CreateTime,m.Name as CreateHospital,n.Name as CreateUserName,o.Name as Store,a.PromoterID,p.Name as PromoterName,
						a.LastAppointmentTime,q.Name as AppointmentHospital
						from SmartCustomer a
						inner join treeCustomer b on a.ID=b.CustomerID
						left join SmartSymptom c on a.CurrentConsultSymptomID=c.ID
						left join SmartMemberCategory d on a.MemberCategoryID=d.ID
						left join SmartShareCategory e on a.ShareMemberCategoryID=e.ID
						left join SmartChannel f on a.ChannelID=f.ID
						left join SmartSymptom g on a.CurrentConsultSymptomID=g.ID
						left join SmartOwnerShip h on a.ID=h.CustomerID and h.Type=1 and h.EndTime>getdate()
						left join SmartUser i on h.UserID=i.ID
						left join SmartOwnerShip j on a.ID=j.CustomerID and j.Type=2 and j.EndTime>getdate() and j.HospitalID=@HospitalID
						left join SmartUser k on j.UserID=k.ID
						left join SmartHospital l on a.FirstVisitHospitalID=l.ID
						left join SmartHospital m on a.CreateUserHospitalID=m.ID
						left join SmartUser n on a.CreateUserID=n.ID
						left join SmartStore o on a.StoreID=o.ID
						left join SmartCustomer p on a.PromoterID=p.ID
						left join SmartHospital q on a.LastAppointmentHospitalID=q.ID {0} 
                        ORDER by a.CreateTime desc OFFSET {1} ROWS FETCH NEXT {2} ROWS only", sql_where, startRow, endRow);

            string sql_count = string.Format(@"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@CreateUserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@CreateUserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@CreateUserID
                        ),
						treeCustomer as
						(
						select distinct a.CustomerID from SmartOwnerShip a,treeUser b where a.UserID=b.ID  and a.EndTime> getdate() 
						union
						select distinct CustomerID from SmartTriage where AssignUserID=@CreateUserID and DateDiff(dd,CreateTime,getdate())=0
						)

						select count(a.ID)
						from SmartCustomer a
						inner join treeCustomer b on a.ID=b.CustomerID
						left join SmartSymptom c on a.CurrentConsultSymptomID=c.ID
						left join SmartMemberCategory d on a.MemberCategoryID=d.ID
						left join SmartShareCategory e on a.ShareMemberCategoryID=e.ID
						left join SmartChannel f on a.ChannelID=f.ID
						left join SmartSymptom g on a.CurrentConsultSymptomID=g.ID
						left join SmartOwnerShip h on a.ID=h.CustomerID  and h.Type=1 and h.EndTime>getdate()
						left join SmartUser i on h.UserID=i.ID
						left join SmartOwnerShip j on a.ID=j.CustomerID and j.Type=2 and j.EndTime>getdate() and j.HospitalID=@HospitalID
						left join SmartUser k on j.UserID=k.ID
						left join SmartHospital l on a.FirstVisitHospitalID=l.ID
						left join SmartHospital m on a.CreateUserHospitalID=m.ID
						left join SmartUser n on a.CreateUserID=n.ID
						left join SmartStore o on a.StoreID=o.ID
						left join SmartCustomer p on a.PromoterID=p.ID
						left join SmartHospital q on a.LastAppointmentHospitalID=q.ID {0}", sql_where);
            #endregion


            await TryExecuteAsync(async () =>
            {

                var task1 = _connection.QueryAsync<CustomerManager>(sql, dto);
                var task2 = _connection.QueryAsync<int>(sql_count, dto);

                result.Data.PageDatas = await task1;
                result.Data.PageTotals = (await task2).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
    }
}
