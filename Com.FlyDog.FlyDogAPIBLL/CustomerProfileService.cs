using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
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
    /// <summary>
    /// 客户档案
    /// </summary>
    public class CustomerProfileService : BaseService, ICustomerProfileService
    {
        /// <summary>
        /// 系统设置服务
        /// </summary>
        private IOptionService _optionService;
        public CustomerProfileService(IOptionService optionService)
        {
            _optionService = optionService;
        }

        #region 预约情况
        /// <summary>
        /// 客户档案里面的预约情况
        /// </summary>
        /// <param name="customeID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileAppoint>> GetAppointment(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProfileAppoint>();

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(userID, customerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                ProfileAppoint data = new ProfileAppoint();
                var task1 = _connection.QueryMultipleAsync(
                    @"select a.ID,a.CustomerID,a.CreateTime,a.AppointmentDate,a.AppointmentStartTime,a.AppointmentEndTime,a.Content,'【'+d.Name+'】【'+b.Name+'】' as CreateUserName,
                    c.Name as HospitalName,a.Code,a.Status from SmartAppointment a
                    left join SmartUser b on a.CreateUserID=b.ID
                    left join SmartHospital c on a.HospitalID=c.ID
                    left join SmartHospital d on b.HospitalID=d.ID
                    where a.CustomerID=@CustomerID
					select a.ID,a.CustomerID,a.CreateTime,a.AppointmentDate,a.AppointmentStartTime,a.AppointmentEndTime,a.Remark,'【'+d.Name+'】【'+b.Name+'】' as CreateUserName,
                    e.Name as ChargeName,f.Name as UserName,c.Name as HospitalName,a.Status
                    from SmartTreat a
                    left join SmartUser b on a.CreateUserID=b.ID
					left join SmartCharge e on a.ChargeID=e.ID
					left join SmartUser f on a.UserID=f.ID
                    left join SmartHospital c on a.HospitalID=c.ID
                    left join SmartHospital d on b.HospitalID=d.ID
                    where a.CustomerID=@CustomerID", new { CustomerID = customerID });


                var task2 = _connection.QueryAsync<ProfileSurgery>(
                    @"select a.ID,a.CustomerID,a.CreateTime,a.AppointmentDate,a.AppointmentStartTime,a.AppointmentEndTime,a.Remark,'【'+d.Name+'】【'+b.Name+'】' as CreateUserName,
                    a.AnesthesiaType,g.ChargeID,e.Name as ChargeName,a.UserID,f.Name as UserName,c.Name as HospitalName 
                    from SmartSurgery a
					inner join SmartSurgeryDetail g on a.ID=g.SurgeryID
                    inner join SmartUser b on a.CreateUserID=b.ID
					inner join SmartCharge e on g.ChargeID=e.ID
					inner join SmartUser f on a.UserID=f.ID
                    inner join SmartHospital c on a.HospitalID=c.ID
                    inner join SmartHospital d on b.HospitalID=d.ID
					where a.CustomerID=@CustomerID", new { CustomerID = customerID });

                var surgerys = await task2;
                var muti = await task1;

                var appointmentTask = muti.ReadAsync<ProfileAppointment>();
                var treatTask = muti.ReadAsync<ProfileTreat>();

                var list = new Dictionary<string, ProfileSurgery>();
                foreach (var u in surgerys)
                {
                    if (list.Keys.Contains(u.ID))
                    {
                        list[u.ID].ChargeName += "," + u.ChargeName;
                    }
                    else
                    {
                        list.Add(u.ID, u);
                    }
                }

                data.SurgeryList = list.Values;

                data.AppointmentList = await appointmentTask;
                data.TreatList = await treatTask;

                result.Data = data;
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
        #endregion

        #region 上门情况
        /// <summary>
        /// 客户档案上门情况
        /// </summary>
        /// <param name="userID">操作用户</param>
        /// <param name="customeID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileVisitCase>> GetVisit(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProfileVisitCase>();

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(userID, customerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                ProfileVisitCase data = new ProfileVisitCase();
                var multi = await _connection.QueryMultipleAsync(
                    @"select a.ID,a.CustomerID,a.CreateTime,
                      a.VisitType,case when a.VisitType in(1,2) then 1 else 2 end as CustomerType,
                      '【'+d.Name+'】【'+c.Name+'】' as CreateUserName,b.Name as HospitalName
                      from SmartVisit a
					  inner join SmartHospital b on a.HospitalID=b.ID
                      inner join SmartUser c on a.CreateUserID=c.ID
					  inner join SmartHospital d on c.HospitalID=d.ID
                      where a.CustomerID=@CustomerID order by a.CreateTime desc

                      SELECT a.ID,a.CustomerID,b.Name as HospitalName, '【'+e.Name+'】【'+c.Name+'】' as CreateUserName,	
				   	  a.CreateTime,'【'+f.Name+'】【'+d.Name+'】' as AssignUserName,a.Remark
                      FROM [SmartTriage] a
					  inner join SmartHospital b on a.HospitalID=b.ID
                      inner join SmartUser c on a.CreateUserID=c.ID
					  inner join SmartHospital e on c.HospitalID=e.ID
                      inner join SmartUser d on a.AssignUserID=d.ID
					  inner join SmartHospital f on d.HospitalID=f.ID
                      where a.CustomerID=@CustomerID order by a.CreateTime desc

                      select a.ID,a.CustomerID,c.Name as HospitalName,b.Name as DeptName,a.CreateTime 
					  from SmartDeptVisit a
					  inner join SmartDept b on a.DeptID=b.ID
					  inner join SmartHospital c on a.HospitalID=c.ID
					  where a.CustomerID=@CustomerID order by a.CreateTime desc

                      select a.ID,a.CustomerID,b.Name as HospitalName,c.Name as CreateUserName,d.Name as BedName,a.InTime,a.OutTime,a.Status,a.Remark
                      from SmartInpatient a
                      inner join SmartHospital b on a.HospitalID=b.ID
                      inner join SmartUser c on a.CreateUserID=c.ID
                      inner join SmartBed d on a.BedID=d.ID
                      where a.CustomerID=@CustomerID order by a.InTime desc", new { CustomerID = customerID });

                var task1 = multi.ReadAsync<ProfileVisit>();
                var task2 = multi.ReadAsync<ProfileTriage>();
                var task3 = multi.ReadAsync<ProfileDeptVisit>();
                var task4 = multi.ReadAsync<ProfileInpatient>();

                data.VisitList = await task1;
                data.TriageList = await task2;
                data.DeptVisitList = await task3;
                data.InpatientList = await task4;

                result.Data = data;
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
        #endregion

        #region 标签
        /// <summary>
        /// 标签组选择
        /// </summary>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileTagGroup>>> SelectTageGroup()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileTagGroup>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                var list = new Dictionary<string, ProfileTagGroup>();
                await _connection.QueryAsync<ProfileTagGroup, ProfileTag, ProfileTagGroup>(
                    @"select a.ID as GroupID,a.Name as GroupName,b.TagID,c.Content as TagName from SmartTagGroup a
                   inner join SmartTagGroupDetail b on a.ID=b.GroupID
                   inner join SmartTag c on b.TagID=c.ID and c.Status=@Status",
                    (group, tag) =>
                    {
                        ProfileTagGroup temp = new ProfileTagGroup();
                        if (!list.TryGetValue(group.GroupID, out temp))
                        {
                            list.Add(group.GroupID, temp = group);
                        }
                        if (tag != null)
                            temp.Tags.Add(tag);
                        return group;
                    }, new { Status = CommonStatus.Use }, null, true, splitOn: "TagID");

                result.Data = list.Values;
            });

            return result;
        }

        /// <summary>
        /// 添加顾客标签的时候查询出来的详细信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerTag>> GetCustomerTageGroup(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProfileCustomerTag>();
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

                var list = new Dictionary<string, ProfileCustomerTag>();
                await _connection.QueryAsync<ProfileCustomerTag, ProfileTag, ProfileCustomerTag>(
                    @"select a.ID as CustomerID,b.TagID,c.Content as TagName from SmartCustomer a
                    left join SmartCustomerTag b on a.ID=b.CustomerID
                    left join SmartTag c on b.TagID=c.ID
                    where a.ID=@CustomerID",
                    (group, tag) =>
                    {
                        ProfileCustomerTag temp = new ProfileCustomerTag();
                        if (!list.TryGetValue(group.CustomerID, out temp))
                        {
                            list.Add(group.CustomerID, temp = group);
                        }
                        if (tag != null)
                            temp.Tags.Add(tag);
                        return group;
                    }, new { CustomerID = customerID }, null, true, splitOn: "TagID");

                result.Data = list.Values.FirstOrDefault();
            });

            return result;
        }

        /// <summary>
        /// 客户档案添加标签
        /// </summary>
        /// <param name="dto">标签信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddTag(ProfileCustomerTagAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryTransactionAsync(async () =>
            {
                if (!await HasCustomerOAuthTransactionAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return false;
                }
                DateTime now = DateTime.Now;
                await _connection.ExecuteAsync(@"delete from [SmartCustomerTag] where CustomerID=@CustomerID", new { CustomerID = dto.CustomerID }, _transaction);
                result.Data = await _connection.ExecuteAsync(
                    @"insert into [SmartCustomerTag]([ID],[CustomerID],[TagID],[CreateUserID],[CreateTime]) values(@ID,@CustomerID,@TagID,@CreateUserID,@CreateTime)",
                    dto.Tags.Select(u => new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        TagID = u,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = now
                    }), _transaction);
                return true;
            });

            return result;
        }
        #endregion

        #region 关系
        /// <summary>
        /// 客户档案添加关系
        /// </summary>
        /// <param name="dto">关系信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddRelation(ProfileRelationAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.Message = "添加成功";
            result.ResultType = IFlyDogResultType.Success;

            if (dto.CustomerID == dto.RelationCustomerID)
            {
                result.Message = "自己不能添加自己";
                result.ResultType = IFlyDogResultType.Failed;
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

                var count = (await _connection.QueryAsync<int>(
                    @"select count(ID) from [SmartCustomerRelation] where [RelationID]=@RelationID and ((CustomerID=@CustomerID1 and RelationCustomerID=@RelationCustomerID1) or (CustomerID=@CustomerID2 and RelationCustomerID=@RelationCustomerID2))",
                    new
                    {
                        RelationID = dto.RelationID,
                        CustomerID1 = dto.CustomerID,
                        RelationCustomerID1 = dto.RelationCustomerID,
                        CustomerID2 = dto.RelationCustomerID,
                        RelationCustomerID2 = dto.CustomerID
                    })).FirstOrDefault();
                if (count > 0)
                {
                    result.Message = "改关系已经存在，无需重复添加！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                    @"insert into [SmartCustomerRelation]([ID],[CustomerID],[RelationCustomerID],[RelationID],[CreateTime],[CreateUserID]) 
                    values(@ID,@CustomerID,@RelationCustomerID,@RelationID,@CreateTime,@CreateUserID)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        RelationCustomerID = dto.RelationCustomerID,
                        RelationID = dto.RelationID,
                        CreateTime = DateTime.Now,
                        CreateUserID = dto.CreateUserID
                    });
            });

            return result;
        }

        /// <summary>
        /// 获取客户间关系
        /// </summary>
        /// <param name="userID">操作人ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileRelation>>> GetRelation(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileRelation>>();
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

                result.Data = await _connection.QueryAsync<ProfileRelation>(
                    @"select a.ID,f.Name as RelationName,a.CustomerID,c.Name as RelationCustomerName,'【'+e.Name+'】【'+d.Name+'】' as CreateUserName,a.CreateTime 
                    from SmartCustomerRelation a
                    inner join SmartCustomer c on a.RelationCustomerID=c.ID
                    inner join SmartUser d on a.CreateUserID=d.ID
                    inner join SmartHospital e on d.HospitalID=e.ID
                    inner join SmartRelation f on a.RelationID=f.ID
                    where a.CustomerID=@CustomerID 
                    union all
                    select a.ID,f.Name as RelationName,a.RelationCustomerID as CustomerID,b.Name as RelationCustomerName,'【'+e.Name+'】【'+d.Name+'】' as CreateUserName,a.CreateTime 
                    from SmartCustomerRelation a
                    inner join SmartCustomer b on a.CustomerID=b.ID
                    inner join SmartUser d on a.CreateUserID=d.ID
                    inner join SmartHospital e on d.HospitalID=e.ID
                    inner join SmartRelation f on a.RelationID=f.ID
                    where a.RelationCustomerID=@CustomerID",
                    new { CustomerID = customerID });
            });

            return result;
        }

        /// <summary>
        /// 客户档案删除关系
        /// </summary>
        /// <param name="dto">关系信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeleteRelation(CommonDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.Message = "删除成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                    @"delete from [SmartCustomerRelation] where ID=@ID",
                    new
                    {
                        ID = dto.ID
                    });
            });

            return result;
        }

        #endregion

        #region 客户档案
        /// <summary>
        /// 客户档案详细查询
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <param name="hospitalID">操作人所在医院ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerDetail>> GetCustomerDetail(long userID, long customerID, long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProfileCustomerDetail>();

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(userID, customerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                var customer_temp = _connection.QueryAsync<ProfileCustomerDetail>(
                      @"select distinct a.ID as CustomerID,a.Name as CustomerName,a.CreateTime,'【'+c.Name+'】【'+b.Name+'】' as CreateUserName,a.Gender,a.ChannelID,d.Name as ChannelName,
                      a.Mobile,a.MobileBackup,a.CurrentConsultSymptomID as ConsultSymptomID,e.Name as ConsultSymptomName,g.Name as ExploitUserName,i.Name as ManagerUserName,
                      a.StoreID,j.Name as StoreName,a.PromoterID,k.Name as PromoterName,m.Name+' '+l.Name as City,a.Address,a.ConsultTimes,a.FirstConsultTime,a.LastConsultTime,
                      n.Name as FirstVisitHospital,a.FirstVisitTime,a.VisitTimes,a.LastVisitTime,o.Name as LastVisitHospital,a.Birthday,datediff(year,a.Birthday,getdate()) as Age,
                      a.WechatBindTime,a.Remark,a.QQ,a.WeChat,a.[Custom1],a.[Custom2],a.[Custom3],a.[Custom4],a.[Custom5],a.[Custom6],a.[Custom7],a.[Custom8],a.[Custom9],a.[Custom10],
                      a.ImageUrl,p.Name as MemberCategoryName,p.Icon as MemberCategoryImage,q.Name as ShareCategoryName,q.Icon as ShareCategoryImage,
                      case when a.VisitTimes=0 then '未上门' else '已上门' end as ComeType,case when a.CashCardTotalAmount=0 then '未成交' else '已成交' end as DealType
                      from [SmartCustomer] a
                      left join SmartUser b on a.CreateUserID=b.ID
                      left join SmartHospital c on b.HospitalID=c.ID
                      left join SmartChannel d on a.ChannelID=d.ID
                      left join SmartSymptom e on a.CurrentConsultSymptomID=e.ID
                      left join SmartOwnerShip f on a.ID=f.CustomerID and f.Type=@ExploitType and f.EndTime>=getdate() 
                      left join SmartUser g on f.UserID=g.ID and g.HospitalID=@HospitalID
                      left join SmartOwnerShip h on a.ID=h.CustomerID and h.Type=@ManagerType and h.HospitalID=1 and h.EndTime>=getdate() 
                      left join SmartUser i on h.UserID=i.ID and i.HospitalID=@HospitalID
                      left join SmartStore j on a.StoreID=j.ID
                      left join SmartCustomer k on a.PromoterID=k.ID
                      left join SmartCity l on a.CityID=l.ID
                      left join SmartProvince m on l.ProvinceID=m.ID
                      left join SmartHospital n on a.FirstVisitHospitalID=n.ID
                      left join SmartHospital o on a.LastVisitHospitalID=o.ID
                      left join SmartMemberCategory p on a.MemberCategoryID=p.ID
                      left join SmartShareCategory q on q.ID=a.ShareMemberCategoryID where a.ID=@ID",
                      new { ID = customerID, HospitalID = hospitalID, ExploitType = OwnerShipType.Exploit, ManagerType = OwnerShipType.Manager });

                var symptoms = _connection.QueryAsync<string>(
                      @"select distinct c.Name from SmartConsult a,SmartConsultSymptomDetail b,SmartSymptom c where a.CustomerID=@CustomerID and a.ID=b.ConsultID and b.SymptomID=c.ID", new { CustomerID = customerID });

                var tags = _connection.QueryAsync<string>(
                      @"select distinct b.Content from SmartCustomerTag a,SmartTag b where a.CustomerID=@CustomerID and a.TagID=b.ID", new { CustomerID = customerID });

                var equitys = _connection.QueryAsync<string>(
                    @"select c.Name 
                    from SmartMemberCategoryEquity a
                    inner join SmartCustomer b on a.CategoryID=b.MemberCategoryID and b.ID=@CustomerID
                    inner join SmartEquity c on a.EquityID=c.ID and c.Status=@Status", new { Status = CommonStatus.Use, CustomerID = customerID });

                result.Data = (await customer_temp).FirstOrDefault();
                result.Data.Symptoms = await symptoms;
                result.Data.Tags = await tags;
                result.Data.Equitys = await equitys;

                var custom = _optionService.Get();
                result.Data.Custom1Name = custom.Data.Customer1Value;
                result.Data.Custom2Name = custom.Data.Customer2Value;
                result.Data.Custom3Name = custom.Data.Customer3Value;
                result.Data.Custom4Name = custom.Data.Customer4Value;
                result.Data.Custom5Name = custom.Data.Customer5Value;
                result.Data.Custom6Name = custom.Data.Customer6Value;
                result.Data.Custom7Name = custom.Data.Customer7Value;
                result.Data.Custom8Name = custom.Data.Customer8Value;
                result.Data.Custom9Name = custom.Data.Customer9Value;
                result.Data.Custom10Name = custom.Data.Customer10Value;
            });

            return result;
        }

        /// <summary>
        /// 修改渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateChannel(ProfileCustomerUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.Message = "更新成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                    @"update SmartCustomer set ChannelID=@ChannelID where ID=@ID", new { ID = dto.CustomerID, ChannelID = dto.ChannelID });
            });

            return result;
        }

        /// <summary>
        /// 修改联系方式
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateMobile(ProfileCustomerUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.Message = "更新成功";
            result.ResultType = IFlyDogResultType.Success;

            if (!dto.Mobile.IsMobileNumber())
            {
                result.Message = "手机格式不正确";
                result.ResultType = IFlyDogResultType.Failed;
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
                    @"update SmartCustomer set Mobile=@Mobile where ID=@ID", new { ID = dto.CustomerID, Mobile = dto.Mobile });
            });

            return result;
        }

        /// <summary>
        /// 修改备用联系方式
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateMobileBackup(ProfileCustomerUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.Message = "更新成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                    @"update SmartCustomer set MobileBackup=@MobileBackup where ID=@ID", new { ID = dto.CustomerID, MobileBackup = dto.MobileBackup });
            });

            return result;
        }

        /// <summary>
        /// 更新主咨询项目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateCurrentConsultSymptom(ProfileCustomerUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.Message = "更新成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                    @"update SmartCustomer set CurrentConsultSymptomID=@CurrentConsultSymptomID where ID=@ID", new { ID = dto.CustomerID, CurrentConsultSymptomID = dto.CurrentConsultSymptomID });
            });

            return result;
        }

        /// <summary>
        /// 更新推荐店家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateStore(ProfileCustomerUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.Message = "更新成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                    @"update SmartCustomer set StoreID=@StoreID where ID=@ID", new { ID = dto.CustomerID, StoreID = dto.StoreID });
            });

            return result;
        }

        /// <summary>
        /// 清除微信绑定
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> ClearWechatBinding(ProfileCustomerUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.Message = "更新成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                    @"update SmartCustomer set WechatBindTime=null,WeChatBind=null where ID=@ID", new { ID = dto.CustomerID });
            });

            return result;
        }

        /// <summary>
        /// 顾客资料编辑之前获取信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerInfo>> GetCustomerInfoUpdate(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProfileCustomerInfo>();
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

                result.Data = (await _connection.QueryAsync<ProfileCustomerInfo>(
                      @"select a.ID as CustomerID,a.Name as CustomerName,a.Gender,m.ID as ProvinceID,a.CityID,a.Address,a.Birthday,a.Remark,a.QQ,a.WeChat,
                      a.[Custom1],a.[Custom2],a.[Custom3],a.[Custom4],a.[Custom5],a.[Custom6],a.[Custom7],a.[Custom8],a.[Custom9],a.[Custom10]
                      from [SmartCustomer] a
                      left join SmartCity l on a.CityID=l.ID
                      left join SmartProvince m on l.ProvinceID=m.ID
                      where a.ID=@ID",
                      new { ID = customerID })).FirstOrDefault();

                var custom = _optionService.Get();
                result.Data.Custom1Name = custom.Data.Customer1Value;
                result.Data.Custom2Name = custom.Data.Customer2Value;
                result.Data.Custom3Name = custom.Data.Customer3Value;
                result.Data.Custom4Name = custom.Data.Customer4Value;
                result.Data.Custom5Name = custom.Data.Customer5Value;
                result.Data.Custom6Name = custom.Data.Customer6Value;
                result.Data.Custom7Name = custom.Data.Customer7Value;
                result.Data.Custom8Name = custom.Data.Customer8Value;
                result.Data.Custom9Name = custom.Data.Customer9Value;
                result.Data.Custom10Name = custom.Data.Customer10Value;
            });

            return result;
        }

        /// <summary>
        /// 顾客详细资料编辑
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CustomerInfoUpdate(ProfileCustomerInfoUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.Message = "更新成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                      @"update [SmartCustomer] set [Name]=@CustomerName,[Gender]=@Gender,[QQ]=@QQ,[WeChat]=@WeChat,[Remark]=@Remark,
                      [Birthday]=@Birthday,[Address]=@Address,[CityID]=@CityID,[Custom1]=@Custom1,[Custom2]=@Custom2,[Custom3]=@Custom3,
                      [Custom4]=@Custom4,[Custom5]=@Custom5,[Custom6]=@Custom6,[Custom7]=@Custom7,[Custom8]=@Custom8,[Custom9]=@Custom9,[Custom10]=@Custom10
                      where ID=@CustomerID", dto);
            });

            return result;
        }
        #endregion

        #region 店家
        /// <summary>
        /// 添加佣金
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddStoreCommission(ProfileStoreCommissionAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Amount <= 0)
            {
                result.Message = "佣金不能小于0元！";
                return result;
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length > 500)
            {
                result.Message = "备注不能超过500字";
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

                long? storeID = (await _connection.QueryAsync<long?>(
                    @"select StoreID from [SmartCustomer] where ID=@ID", new { ID = dto.CustomerID })).FirstOrDefault();

                if (storeID == null || storeID == 0)
                {
                    result.Message = "请先为顾客分配店家！";
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                      @"insert into [SmartCommission]([ID],[CustomerID],[StoreID],[CreateTime],[CreateUserID],[Amount],[Remark],[HospitalID]) 
                      values(@ID,@CustomerID,@StoreID,@CreateTime,@CreateUserID,@Amount,@Remark,@HospitalID)",
                      new
                      {
                          ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                          CustomerID = dto.CustomerID,
                          StoreID = storeID,
                          CreateTime = DateTime.Now,
                          CreateUserID = dto.CreateUserID,
                          Amount = dto.Amount,
                          Remark = dto.Remark,
                          HospitalID = dto.HospitalID
                      });

                result.Message = "更新成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 删除佣金
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeleteStoreCommission(ProfileStoreCommissionDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                      @"delete from [SmartCommission] where [ID]=@ID and [CustomerID]=@CustomerID",
                      new
                      {
                          CustomerID = dto.CustomerID,
                          ID = dto.ID,
                      });

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 店铺佣金记录
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerStoreCommission>> GetCustomerStoreCommission(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProfileCustomerStoreCommission>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            result.Data = new ProfileCustomerStoreCommission();
            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(userID, customerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                var commission = await _connection.QueryAsync<ProfileCustomerStoreCommissionDetail>(
                    @"select a.ID,a.StoreID,b.Name as StoreName,d.Name as HospitalName,c.Name as CreateUserName,a.CreateTime,a.Amount,a.Remark 
                        from [SmartCommission] a
                        left join SmartStore b on a.StoreID=b.ID
                        left join SmartUser c on a.CreateUserID=c.ID
                        left join SmartHospital d on a.HospitalID=d.ID
                        where a.CustomerID=@CustomerID", new { CustomerID = customerID });

                result.Data.TotalAmount = commission.Sum(u => u.Amount);
                result.Data.List = commission;
            });

            return result;
        }
        #endregion

        #region 激活券
        /// <summary>
        /// 查询激活码信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileActiveCoupon>> GetActiveCoupon(string code)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProfileActiveCoupon>();
            result.ResultType = IFlyDogResultType.Failed;

            if (code.IsNullOrEmpty())
            {
                result.Message = "激活码不能为空！";
                return result;
            }

            await TryExecuteAsync(async () =>
            {

                result.Data = (await _connection.QueryAsync<ProfileActiveCoupon>(
                      @"select a.Code,b.Name as ActiveName,c.Name as CouponCategoryName,b.Amount,b.CreateDate,b.Expiration,b.IsRepetition,a.CouponID from [SmartCouponActivityDetail] a
                      inner join SmartCouponActivity b on a.ActivityID=b.ID
                      inner join SmartCouponCategory c on b.CategoryID=c.ID
                      where a.Code=@Code",
                      new
                      {
                          Code = code
                      })).FirstOrDefault();

                if (result.Data == null)
                {
                    result.Message = "激活码不存在！";
                    return;
                }

                if (result.Data.CouponID != null && result.Data.CouponID != 0)
                {
                    result.Message = "该激活码已经被激活！";
                    result.Data = null;
                    return;
                }

                if (result.Data.Expiration < DateTime.Today)
                {
                    result.Data.IsEfficacy = CouponActiveStatus.NoEffective;
                }
                else
                {
                    result.Data.IsEfficacy = CouponActiveStatus.Effective;
                }

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 券激活
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddActiveCoupon(ProfileActiveCouponAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Code.IsNullOrEmpty())
            {
                result.Message = "激活码不能为空！";
                return result;
            }

            await TryTransactionAsync(async () =>
            {
                if (!await HasCustomerOAuthTransactionAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return false;
                }

                var temp = (await _connection.QueryAsync<ProfileActiveCoupon>(
                      @"select a.Code,b.Name as ActiveName,c.Name as CouponCategoryName,b.Amount,b.CreateDate,b.Expiration,b.IsRepetition,a.CouponID,b.ID as ActiveID,b.CategoryID 
                      from [SmartCouponActivityDetail] a
                      inner join SmartCouponActivity b on a.ActivityID=b.ID
                      inner join SmartCouponCategory c on b.CategoryID=c.ID
                      where a.Code=@Code",
                      new
                      {
                          Code = dto.Code
                      }, _transaction)).FirstOrDefault();

                if (temp == null)
                {
                    result.Message = "激活码不存在！";
                    return false;
                }

                if (temp.CouponID != null && temp.CouponID != 0)
                {
                    result.Message = "该激活码已经被激活使用了！";
                    return false;
                }

                if (temp.Expiration < DateTime.Today)
                {
                    result.Message = "该激活码已经失效！";
                    return false;
                }

                if (temp.IsRepetition == CouponActiveStatus.NoReception)
                {
                    var count = (await _connection.QueryAsync<int>(
                        @"select count(a.ID) from SmartCoupon a
                        inner join SmartCouponActivityDetail b on a.ID=b.CouponID
                        inner join SmartCouponActivity c on b.ActivityID=c.ID and c.ID=@ActiveID
                        where a.CustomerID=@CustomerID", new { ActiveID = temp.ActiveID, CustomerID = dto.CustomerID }, _transaction)).FirstOrDefault();

                    if (count > 0)
                    {
                        result.Message = "您已经使用过该次活动的激活券，不可重复使用！！";
                        return false;
                    }
                }

                long couponID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                Task task1 = _connection.ExecuteAsync(
                    @"update [SmartCouponActivityDetail] set [CouponID]=@CouponID where Code=@Code", new { Code = dto.Code, CouponID = couponID }, _transaction);

                Task task2 = _connection.ExecuteAsync(
                    @"insert into [SmartCoupon]([ID],[HospitalID],[CustomerID],[CreateUserID],[CreateTime],[Access],[CategoryID],[Amount],[Rest],[Remark],[Status]) 
                    values(@ID,@HospitalID,@CustomerID,@CreateUserID,@CreateTime,@Access,@CategoryID,@Amount,@Rest,@Remark,@Status)",
                    new
                    {
                        ID = couponID,
                        HospitalID = dto.HospitalID,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        Access = CouponType.CodeActive,
                        CategoryID = temp.CategoryID,
                        Amount = temp.Amount,
                        Rest = temp.Amount,
                        Remark = CouponType.CodeActive.ToDescription() + "：" + dto.Code,
                        Status = CouponStatus.Effective
                    }, _transaction);
                Task task3 = _connection.ExecuteAsync(
                    @"update SmartCustomer set [Coupon]=[Coupon]+@Coupon where ID=@ID", new { ID = dto.CustomerID, Coupon = temp.Amount }, _transaction);

                await Task.WhenAll(task1, task2, task3);

                result.Message = "激活成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
        #endregion

        #region 负责用户
        /// <summary>
        /// 客户档案获取负责用户
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileOwinnerShip>> GetOwinerShip(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProfileOwinnerShip>();

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(userID, customerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                ProfileOwinnerShip data = new ProfileOwinnerShip();
                var multi = await _connection.QueryAsync<ProfileOwinerShipTemp>(
                    @"select a.CustomerID,c.Name as HospitalName,b.Name as UserName,a.StartTime,a.EndTime,a.Remark,a.Type 
                    from [SmartOwnerShip] a
                    inner join SmartUser b on a.UserID=b.ID
                    inner join SmartHospital c on a.HospitalID=c.ID
                    where a.CustomerID=@CustomerID and getdate() between a.StartTime and a.EndTime", new { CustomerID = customerID });

                foreach (var u in multi)
                {
                    if (u.Type == OwnerShipType.Exploit)
                        data.Exploits.Add(u);
                    else if (u.Type == OwnerShipType.Manager)
                        data.Managers.Add(u);
                }

                result.Data = data;
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询历史信息
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileOwinerShipTemp>>> GetOwinerShipHistory(long userID, long customerID, OwnerShipType type)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ProfileOwinerShipTemp>>();

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(userID, customerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }
                result.Data = await _connection.QueryAsync<ProfileOwinerShipTemp>(
                    @"select a.CustomerID,c.Name as HospitalName,b.Name as UserName,a.StartTime,a.EndTime,a.Remark,a.Type 
                    from [SmartOwnerShip] a
                    inner join SmartUser b on a.UserID=b.ID
                    inner join SmartHospital c on a.HospitalID=c.ID
                    where a.CustomerID=@CustomerID and a.Type=@Type", new { CustomerID = customerID, Type = type });

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }


        #endregion

        #region 订单情况
        /// <summary>
        /// 客户档案订单情况
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileOrders>> GetOrders(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProfileOrders>();
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

                result.Data = new ProfileOrders();

                var muti = await _connection.QueryMultipleAsync(
                    @"select a.ID as OrderID,a.CustomerID,a.CreateUserID,b.Name as CreateUserName,a.HospitalID,c.Name as HospitalName,a.Remark,
                    a.CreateTime,a.TotalPrice,a.FinalPrice,a.PaidStatus,a.PaidTime,a.AuditStatus,a.DebtAmount,
					case when TotalPrice>0 then FinalPrice*100/TotalPrice else 0 end as Discount
                    from [SmartOrder] a 
                    inner join SmartUser b on a.CreateUserID=b.ID
                    inner join SmartHospital c on a.HospitalID=c.ID
                    where a.CustomerID=@CustomerID and a.InpatientID is null

					select a.ID as OrderID,a.CustomerID,a.CreateUserID,b.Name as CreateUserName,a.HospitalID,c.Name as HospitalName,a.Remark,
                    a.CreateTime,a.TotalPrice,a.FinalPrice,a.PaidStatus,a.PaidTime,a.AuditStatus,a.DebtAmount,
					case when TotalPrice>0 then FinalPrice*100/TotalPrice else 0 end as Discount
                    from [SmartOrder] a 
                    inner join SmartUser b on a.CreateUserID=b.ID
                    inner join SmartHospital c on a.HospitalID=c.ID
                    where a.CustomerID=@CustomerID and a.InpatientID is not null 

					select a.ID as OrderID,a.CustomerID,a.HospitalID,d.Name as HospitalName,a.CreateTime,e.Name as CreateUserName,
                    a.Amount,a.PaidStatus,a.PaidTime,a.Remark
                    from [SmartDepositOrder] a
                    inner join SmartHospital d on a.HospitalID=d.ID
                    inner join SmartUser e on a.CreateUserID=e.ID where a.CustomerID=@CustomerID

					select a.ID as OrderID,a.HospitalID,d.Name as HospitalName,a.CreateTime,e.Name as CreateUserName,
                    a.Amount,a.Deposit,a.PaidStatus,a.PaidTime,a.Remark,a.AuditStatus
                    from [SmartDepositRebateOrder] a
                    inner join SmartHospital d on a.HospitalID=d.ID
                    inner join SmartUser e on a.CreateUserID=e.ID 
					where a.CustomerID=@CustomerID

					select a.ID as OrderID,a.HospitalID,d.Name as HospitalName,a.CreateTime,e.Name as CreateUserName,
                    a.Amount,a.Point,a.PaidStatus,a.PaidTime,a.Remark,a.AuditStatus
                    from SmartBackOrder a
                    inner join SmartHospital d on a.HospitalID=d.ID
                    inner join SmartUser e on a.CreateUserID=e.ID where a.CustomerID=@CustomerID", new { CustomerID = customerID });


                result.Data.Orders = await muti.ReadAsync<Order>();
                result.Data.InpatientOrders = await muti.ReadAsync<Order>();
                result.Data.DepositOrders = await muti.ReadAsync<DepositOrder>();
                result.Data.DepositRebateOrders = await muti.ReadAsync<DepositRebateOrder>();
                result.Data.BackOrders = await muti.ReadAsync<BackOrder>();
            });

            return result;
        }
        #endregion

        #region 账户情况
        /// <summary>
        /// 客户档案账户情况
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileMoney>> GetMoney(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProfileMoney>();
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

                result.Data = (await _connection.QueryAsync<ProfileMoney>(
                    @"select Point,Deposit,Coupon,Commission from SmartCustomer where ID=@ID", new { ID = customerID })).FirstOrDefault();

                var muti = await _connection.QueryMultipleAsync(
                      @"select a.ID,a.CreateTime,b.Name as CouponCategoryName,c.Name as HospitalName,a.Access,
                      a.Amount,a.Rest,d.Name as CreateUserName,a.Remark,a.Status,
                      case b.TimeLimit when  1 then '2099-12-31' when 2 then b.EndDate when 3 then DateAdd(d, b.Days, a.CreateTime)  end as OverDueTime
                      from SmartCoupon a
                      left join SmartCouponCategory b on a.CategoryID=b.ID
                      left join SmartHospital c on a.HospitalID=c.ID
                      left join SmartUser d on a.CreateUserID=d.ID
                      where a.CustomerID=@CustomerID and a.Rest>0 and a.Status=@Status1

                      select a.ID,a.CreateTime,b.Name as CouponCategoryName,c.Name as HospitalName,a.Access,
                      a.Amount,a.Rest,d.Name as CreateUserName,a.Remark,a.Status,
                      case b.TimeLimit when  1 then '2099-12-31' when 2 then b.EndDate when 3 then DateAdd(d, b.Days, a.CreateTime)  end as OverDueTime
                      from SmartCoupon a
                      left join SmartCouponCategory b on a.CategoryID=b.ID
                      left join SmartHospital c on a.HospitalID=c.ID
                      left join SmartUser d on a.CreateUserID=d.ID
                      where a.CustomerID=@CustomerID and a.Status=@Status2

                      select a.ID,a.CreateTime,b.Name as DepositChargeName,c.Name as HospitalName,a.Access,
                      a.Amount,a.Rest,d.Name as CreateUserName,a.Remark
                      from SmartDeposit a
                      left join SmartDepositCharge b on a.ChargeID=b.ID
                      left join SmartHospital c on a.HospitalID=c.ID
                      left join SmartUser d on a.CreateUserID=d.ID
                      where a.CustomerID=@CustomerID and a.Rest>0

                      select a.CreateTime,a.Type,b.Name as HospitalName,a.Amount,a.Remark,c.Name as CreateUserName 
					  from SmartPoint a
					  inner join SmartHospital b on a.HospitalID=b.ID
					  inner join SmartUser c on a.CreateUserID=c.ID
					  where a.CustomerID=@CustomerID

					  select a.CreateTime,b.Name as DepositChargeName,a.Access,a.Amount,c.Name as CreateUserName,a.Remark
					  from SmartDeposit a
					  inner join SmartDepositCharge b on a.ChargeID=b.ID
					  inner join SmartUser c on a.CreateUserID=c.ID
					  where a.CustomerID=@CustomerID
					  union all
					  select d.CreateTime,e.Name,5,a.Amount*-1,c.Name,'收银单号：'+d.No
					  from SmartDepositUsage a
					  inner join SmartDeposit b on a.DepositID=b.ID and b.CustomerID=@CustomerID
					  inner join SmartUser c on b.CreateUserID=c.ID
					  inner join SmartCashier d on a.CashierID=d.ID
					  inner join SmartDepositCharge e on b.ChargeID=e.ChargeID

					  select a.CreateTime,b.Name as CouponCategoryName,a.Access,a.Amount,c.Name as CreateUserName,a.Remark
					  from SmartCoupon a
					  inner join SmartCouponCategory b on a.CategoryID=b.ID
					  inner join SmartUser c on a.CreateUserID=c.ID
					  where a.CustomerID=@CustomerID
					  union all
					  select d.CreateTime,e.Name,a.Type+4,a.Amount*-1,c.Name,a.Remark
					  from SmartCouponUsage a
					  inner join SmartCoupon b on a.CouponID=b.ID and b.CustomerID=@CustomerID
					  inner join SmartUser c on b.CreateUserID=c.ID
					  inner join SmartCashier d on a.CashierID=d.ID
					  inner join SmartCouponCategory e on b.CategoryID=e.ChargeID

					  select a.CreateTime,a.Type,a.Amount as Amount,b.Name as CreateUserName,a.Remark
					  from SmartCommissionUsage a
					  inner join SmartUser b on a.CreateUserID=b.ID
					  where a.CustomerID=@CustomerID 
					  union all
					  select a.CreateTime,a.Type,a.Amount*-1 as Amount,b.Name as CreateUserName,a.Remark
					  from SmartCommissionUsage a
					  inner join SmartUser b on a.CreateUserID=b.ID
					  where a.ToCustomerID=@CustomerID ", new { CustomerID = customerID, Status1 = CouponStatus.Effective, Status2 = CouponStatus.OverDue });



                result.Data.Coupons = await muti.ReadAsync<ProfileCoupon>();
                result.Data.OverDueCoupons = await muti.ReadAsync<ProfileCoupon>();
                result.Data.Deposits = await muti.ReadAsync<ProfileDeposit>();
                result.Data.PointChanges = await muti.ReadAsync<ProfilePointChange>();
                result.Data.DepositChanges = await muti.ReadAsync<ProfileDepositChange>();
                result.Data.CouponChanges = await muti.ReadAsync<ProfileCouponChange>();
                result.Data.CommissionChanges = await muti.ReadAsync<ProfileCommissionChange>();
            });

            return result;
        }
        #endregion

        #region 划扣记录
        /// <summary>
        /// 客户档案划扣记录
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType,IEnumerable< OperationToday>>> GetOperation(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<OperationToday>>();
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

                var lsit = new Dictionary<string, OperationToday>();

                await _connection.QueryAsync<OperationToday, Operator, OperationToday>(
                        @"select a.ID as OperationID,a.CustomerID,g.Name as CustomerName,d.Name as ChargeName,a.ChargeID,h.Name as HospitalName,
                        a.Num,a.CreateTime,f.Name as CreateUserName,b.UserID,e.Name as UserName,b.PositionID,c.Name as PositionName 
                        from SmartOperation a
                        left join SmartOperator b on a.ID=b.OperationID
                        left join SmartPosition c on b.PositionID=c.ID
                        left join SmartCharge d on a.ChargeID=d.ID
                        left join SmartUser e on b.UserID=e.ID
                        left join SmartUser f on f.ID=a.CreateUserID
                        left join SmartCustomer g on a.CustomerID=g.ID
						left join SmartHospital h on h.ID=a.HospitalID
                        where a.CustomerID=@CustomerID order by a.CreateTime desc",
                        (o, p) =>
                        {
                            OperationToday temp = new OperationToday();
                            if (!lsit.TryGetValue(o.OperationID, out temp))
                            {
                                lsit.Add(o.OperationID, temp = o);
                            }
                            if (p != null)
                                temp.OperatorList.Add(p);
                            return o;
                        }, new { CustomerID = customerID }, null, true, splitOn: "UserID");

                result.Data = lsit.Values;
            });

            return result;
        }
        #endregion

        #region 消费项目
        /// <summary>
        /// 客户档案消费项目
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ProfileCustomerCharges>> GetCharges(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProfileCustomerCharges>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            ProfileCustomerCharges data = new ProfileCustomerCharges();

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(userID, customerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                data.Charges = await _connection.QueryAsync<ProfileCharges>(
                    @"select g.Name as HospitalName,a.ID,f.PaidTime,e.Name as ChargeName,h.Name as UnitName,e.Size,a.Num-a.RestNum-isnull(sum(b.Num) ,0) as UseNum,
                    i.Name as CreateUserName, a.Num,isnull(sum(b.Num),0) as RebateNum,a.FinalPrice,sum(c.CashCardAmount) as CashCardAmount,isnull(sum(d.DepositAmount),0) as Deposit,
				    sum(c.CouponAmount) as Coupon,a.FinalPrice-sum(c.CashCardAmount)-sum(c.DepositAmount)-sum(c.CouponAmount)-sum(c.CommissionAmount) as Debt,
                    sum(c.CommissionAmount) as Commission,isnull(sum(d.CashCardAmount+d.DepositAmount),0) as RebateAmount
                    from SmartOrderDetail a
                    left join SmartCharge e on a.ChargeID=e.ID
                    inner join SmartOrder f on a.OrderID=f.ID and f.PaidStatus!=@PaidStatus and f.CustomerID=@CustomerID
                    left join SmartBackOrderDetail b on a.ID=b.DetailID and b.OrderID in (select ID from SmartBackOrder where PaidStatus!=@PaidStatus)
                    left join SmartCashierCharge c on a.ID=c.ReferID and c.OrderType in (1,2)
                    left join SmartCashierCharge d on a.ID=d.ReferID and d.OrderType=4 
					left join SmartHospital g on f.HospitalID=g.ID
					left join SmartUnit h on e.UnitID=h.ID
					left join SmartUser i on f.CreateUserID=i.ID
                    group by a.ID,e.Name,g.Name,h.Name,e.Size,a.Num,a.RestNum,b.Num,i.Name, a.FinalPrice,f.PaidTime",
                    new
                    {

                        PaidStatus = PaidStatus.NotPaid,
                        CustomerID = customerID
                    });

                foreach(var u in data.Charges)
                {
                    data.Amount += u.FinalPrice;
                    data.CashCardAmount += u.CashCardAmount;
                    data.Commission += u.Commission;
                    data.Coupon += u.Coupon;
                    data.Debt += u.Debt;
                    data.Deposit += u.Deposit;
                    data.RebateAmount += u.RebateAmount;
                }

                result.Data = data;
            });

            return result;
        }
        #endregion

        #region 推荐情况
        /// <summary>
        /// 客户档案账户情况
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
       // public async Task<IFlyDogResult<IFlyDogResultType, ProfilePromoteInfo>> GetPromoteInfo(long userID, long customerID)
       // {
       //     var result = new IFlyDogResult<IFlyDogResultType, ProfileMoney>();
       //     result.Message = "查询成功";
       //     result.ResultType = IFlyDogResultType.Success;

       //     await TryExecuteAsync(async () =>
       //     {
       //         if (!await HasCustomerOAuthAsync(userID, customerID))
       //         {
       //             result.Message = "对不起，您无权操作该用户！";
       //             result.ResultType = IFlyDogResultType.NoAuth;
       //             return;
       //         }

       //         result.Data = (await _connection.QueryAsync<ProfileMoney>(
       //             @"select Point,Deposit,Coupon,Commission from SmartCustomer where ID=@ID", new { ID = customerID })).FirstOrDefault();

       //         var muti = await _connection.QueryMultipleAsync(
       //               @"select a.ID,a.CreateTime,b.Name as CouponCategoryName,c.Name as HospitalName,a.Access,
       //               a.Amount,a.Rest,d.Name as CreateUserName,a.Remark,a.Status,
       //               case b.TimeLimit when  1 then '2099-12-31' when 2 then b.EndDate when 3 then DateAdd(d, b.Days, a.CreateTime)  end as OverDueTime
       //               from SmartCoupon a
       //               left join SmartCouponCategory b on a.CategoryID=b.ID
       //               left join SmartHospital c on a.HospitalID=c.ID
       //               left join SmartUser d on a.CreateUserID=d.ID
       //               where a.CustomerID=@CustomerID and a.Rest>0 and a.Status=@Status1

       //               select a.ID,a.CreateTime,b.Name as CouponCategoryName,c.Name as HospitalName,a.Access,
       //               a.Amount,a.Rest,d.Name as CreateUserName,a.Remark,a.Status,
       //               case b.TimeLimit when  1 then '2099-12-31' when 2 then b.EndDate when 3 then DateAdd(d, b.Days, a.CreateTime)  end as OverDueTime
       //               from SmartCoupon a
       //               left join SmartCouponCategory b on a.CategoryID=b.ID
       //               left join SmartHospital c on a.HospitalID=c.ID
       //               left join SmartUser d on a.CreateUserID=d.ID
       //               where a.CustomerID=@CustomerID and a.Status=@Status2

       //               select a.ID,a.CreateTime,b.Name as DepositChargeName,c.Name as HospitalName,a.Access,
       //               a.Amount,a.Rest,d.Name as CreateUserName,a.Remark
       //               from SmartDeposit a
       //               left join SmartDepositCharge b on a.ChargeID=b.ID
       //               left join SmartHospital c on a.HospitalID=c.ID
       //               left join SmartUser d on a.CreateUserID=d.ID
       //               where a.CustomerID=@CustomerID and a.Rest>0

       //               select a.CreateTime,a.Type,b.Name as HospitalName,a.Amount,a.Remark,c.Name as CreateUserName 
					  //from SmartPoint a
					  //inner join SmartHospital b on a.HospitalID=b.ID
					  //inner join SmartUser c on a.CreateUserID=c.ID
					  //where a.CustomerID=@CustomerID

					  //select a.CreateTime,b.Name as DepositChargeName,a.Access,a.Amount,c.Name as CreateUserName,a.Remark
					  //from SmartDeposit a
					  //inner join SmartDepositCharge b on a.ChargeID=b.ID
					  //inner join SmartUser c on a.CreateUserID=c.ID
					  //where a.CustomerID=@CustomerID
					  //union all
					  //select d.CreateTime,e.Name,5,a.Amount*-1,c.Name,'收银单号：'+d.No
					  //from SmartDepositUsage a
					  //inner join SmartDeposit b on a.DepositID=b.ID and b.CustomerID=@CustomerID
					  //inner join SmartUser c on b.CreateUserID=c.ID
					  //inner join SmartCashier d on a.CashierID=d.ID
					  //inner join SmartDepositCharge e on b.ChargeID=e.ChargeID

					  //select a.CreateTime,b.Name as CouponCategoryName,a.Access,a.Amount,c.Name as CreateUserName,a.Remark
					  //from SmartCoupon a
					  //inner join SmartCouponCategory b on a.CategoryID=b.ID
					  //inner join SmartUser c on a.CreateUserID=c.ID
					  //where a.CustomerID=@CustomerID
					  //union all
					  //select d.CreateTime,e.Name,a.Type+4,a.Amount*-1,c.Name,a.Remark
					  //from SmartCouponUsage a
					  //inner join SmartCoupon b on a.CouponID=b.ID and b.CustomerID=@CustomerID
					  //inner join SmartUser c on b.CreateUserID=c.ID
					  //inner join SmartCashier d on a.CashierID=d.ID
					  //inner join SmartCouponCategory e on b.CategoryID=e.ChargeID

					  //select a.CreateTime,a.Type,a.Amount as Amount,b.Name as CreateUserName,a.Remark
					  //from SmartCommissionUsage a
					  //inner join SmartUser b on a.CreateUserID=b.ID
					  //where a.CustomerID=@CustomerID 
					  //union all
					  //select a.CreateTime,a.Type,a.Amount*-1 as Amount,b.Name as CreateUserName,a.Remark
					  //from SmartCommissionUsage a
					  //inner join SmartUser b on a.CreateUserID=b.ID
					  //where a.ToCustomerID=@CustomerID ", new { CustomerID = customerID, Status1 = CouponStatus.Effective, Status2 = CouponStatus.OverDue });



       //         result.Data.Coupons = await muti.ReadAsync<ProfileCoupon>();
       //         result.Data.OverDueCoupons = await muti.ReadAsync<ProfileCoupon>();
       //         result.Data.Deposits = await muti.ReadAsync<ProfileDeposit>();
       //         result.Data.PointChanges = await muti.ReadAsync<ProfilePointChange>();
       //         result.Data.DepositChanges = await muti.ReadAsync<ProfileDepositChange>();
       //         result.Data.CouponChanges = await muti.ReadAsync<ProfileCouponChange>();
       //         result.Data.CommissionChanges = await muti.ReadAsync<ProfileCommissionChange>();
       //     });

       //     return result;
       // }
        #endregion
    }
}
