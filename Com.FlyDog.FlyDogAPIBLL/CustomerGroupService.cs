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
using Com.IFlyDog.Common;
using Com.JinYiWei.Common.Helper;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 客户组业务逻辑
    /// </summary>
    public class CustomerGroupService : BaseService, ICustomerGroupService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(CustomerGroupAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "名称最多20个字符！";
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
                result.Data = _connection.Execute("insert into SmartCustomerGroup(ID,Name,CreateUserID,Remark) values (@ID,@Name,@CreateUserID,@Remark)",
                    new { ID = id, Name = dto.Name, CreateUserID = dto.CreateUserID, Remark = dto.Remark }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CustomerGroupAdd,
                    Remark = LogType.CustomerGroupAdd.ToDescription() + temp.ToJsonString()
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
        /// 按照条件筛选出客户结果后保存客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> CustomerFilterFiltrateAdd(CustomerFilterFiltrateAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.SaveResult == 1)
            {//覆盖现有客户组
                if (Convert.ToInt64(dto.GroupID) == 0|| dto.GroupID == -1)
                {
                    result.Message = "请选择要覆盖的客户组！";
                    return result;
                }

                if (dto.FiltrateCustormInfoAdd==null||dto.FiltrateCustormInfoAdd.Count==0) {
                    result.Message = "要保存的客户不能为空！";
                    return result;
                }

                TryTransaction(() =>
                {
                    #region 开始数据操作动作              
                    _connection.Execute("DELETE SmartCustomerGroupDetail WHERE GroupID=@GroupID", new { GroupID = dto.GroupID }, _transaction);//覆盖到现有客户组之前先把现有客户组相关联的数据清空

                    foreach (var item in dto.FiltrateCustormInfoAdd)
                    {
                        var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                        result.Data = _connection.Execute("insert into SmartCustomerGroupDetail(ID,CustomerID,GroupID) values (@ID,@CustomerID,@GroupID)", new { ID = id, CustomerID = item.CustormID, GroupID = dto.GroupID }, _transaction);
                    }//将新客户组新增到现有客户组中

                    var temp = new { 编号 = result.Data, FilterID = dto.GroupID, 类型 = "客户筛选结果添加客户组" };
                    #endregion

                    #region 记录日志
                    AddOperationLog(new SmartOperationLog()
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CreateTime = DateTime.Now,
                        CreateUserID = dto.CreateUserID,
                        Type = LogType.CustomerFilterFiltrateAdd,
                        Remark = LogType.CustomerFilterFiltrateAdd.ToDescription() + temp.ToJsonString()
                    });
                    #endregion

                    result.Message = "保存成功";
                    result.ResultType = IFlyDogResultType.Success;
                    return true;
                });
            }
            else if (dto.SaveResult == 2)
            {//保存到新客户组
                if (string.IsNullOrWhiteSpace(dto.GroupName))
                {
                    result.Message = "请填写要保存的新客户组名称！";
                    return result;
                }

                if (dto.FiltrateCustormInfoAdd == null || dto.FiltrateCustormInfoAdd.Count == 0)
                {
                    result.Message = "要保存的客户不能为空！";
                    return result;
                }

                TryTransaction(() =>
                {
                    #region 开始数据操作动作
                    var pid = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                    _connection.Execute("insert into SmartCustomerGroup(ID,Name,CreateUserID,Remark) values (@ID,@Name,@CreateUserID,@Remark)", new { ID = pid, Name = dto.GroupName, CreateUserID = dto.CreateUserID, Remark = " " }, _transaction);//保存到新的客户组前先新增一个客户组信息

                    foreach (var item in dto.FiltrateCustormInfoAdd)
                    {
                        var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                        result.Data = _connection.Execute("insert into SmartCustomerGroupDetail(ID,CustomerID,GroupID) values (@ID,@CustomerID,@GroupID)", new { ID = id, CustomerID = item.CustormID, GroupID = pid }, _transaction);
                    }//将新筛选的客户结果和新增的客户组信息进行关联

                    var temp = new { 编号 = result.Data, FilterID = dto.GroupID, 类型 = "客户筛选结果添加新的客户组" };
                    #endregion

                    #region 记录日志
                    AddOperationLog(new SmartOperationLog()
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CreateTime = DateTime.Now,
                        CreateUserID = dto.CreateUserID,
                        Type = LogType.CustomerFilterFiltrateAdd,
                        Remark = LogType.CustomerFilterFiltrateAdd.ToDescription() + temp.ToJsonString()
                    });
                    #endregion

                    result.Message = "保存成功";
                    result.ResultType = IFlyDogResultType.Success;
                    return true;
                });
            }
            return result;
        }

        /// <summary>
        /// 查询用户创建的所有结果集
        /// </summary>
        /// <returns></returns>
        //public IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerFilterInfo>> GetCustomerFilter(long createUserID)
        //{
        //    var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerFilterInfo>>();

        //    #region 开始查询数据动作
        //    TryExecute(() =>
        //    {
        //        result.Data = _connection.Query<CustomerFilterInfo>("SELECT ID,Name,CreateUserID,HospitalID,Remark FROM dbo.SmartCustomerFilter WHERE CreateUserID=@CreateUserID", new { CreateUserID = createUserID });
        //        result.Message = "查询成功";
        //        result.ResultType = IFlyDogResultType.Success;
        //    });
        //    #endregion
        //    return result;
        //}

        /// <summary>
        /// 根据客户组id查询客户组详情中的客户id
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<long>> GetByFilterID(long groupID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<long>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<long>("SELECT CustomerID FROM dbo.SmartCustomerGroupDetail WHERE GroupID = @GroupID", new { GroupID = groupID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 批量添加回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CustomerGroupBatchCallbackAdd(CustomerGroupBatchCallbackAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            if (dto.CallbackCategoryID <= 0)
            {
                result.Message = "请选择回访类型";
                return result;
            }
            if (dto.CallbackUserID <= 0)
            {
                result.Message = "回访人员不能为空";
                return result;
            }
            if (dto.Info.IsNullOrEmpty())
            {
                result.Message = "回访计划不能为空";
                return result;
            }
            if (dto.Info.Length >= 50)
            {
                result.Message = "回访计划不能超过50字";
                return result;
            }
            if (dto.CallbackTime <= DateTime.Today.AddDays(-1))
            {
                result.Message = "回访时间不能早于今天";
                return result;
            }

            await TryExecuteAsync(async () =>
            {
                var temp = await _connection.QueryAsync<long>(
                    @"select CustomerID from [SmartCustomerGroupDetail] where [GroupID]=@GroupID", new { GroupID = dto.CustomerGroupID });
                var now = DateTime.Now;
                var data = temp.Select(u => new
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CustomerID = u,
                    Name = dto.Info,
                    CreateUserID = dto.CreateUserID,
                    CreateTime = now,
                    CategoryID = dto.CallbackCategoryID,
                    UserID = dto.CallbackUserID,
                    TaskTime = dto.CallbackTime.Date,
                    Status = CallbackStatus.Remind
                });


                result.Data = await _connection.ExecuteAsync(
                    @"insert into [SmartCallback]([ID],[CustomerID],[CreateUserID],[CreateTime],[CategoryID],[Name],[UserID],[TaskTime],[Status]) 
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@CategoryID,@Name,@UserID,@TaskTime,@Status)", data);

                result.Message = "添加回访提醒成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 批量发送短信
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CustomerGroupBatchSSMAdd(BatchSSM dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "短信内容不能为空！";
                return result;
            }

            await TryExecuteAsync(async () =>
            {
                var temp = (await _connection.QueryAsync<SSMTemp>(
                    @"select distinct a.CustomerID,b.[Mobile] from [SmartCustomerGroupDetail] a,SmartCustomer b where [GroupID]=@GroupID and a.CustomerID=b.ID", new { GroupID = dto.GroupID })).ToList();
                var now = DateTime.Now;

                string phones = "";
                BatchSubmit batch = new BatchSubmit();
                List<SSMLogAdd> ssmLogs = new List<SSMLogAdd>();
                for (int i = 0; i < temp.Count(); i++)
                {
                    phones += temp[i].Mobile + ",";
                    ssmLogs.Add(new SSMLogAdd()
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        Content = dto.Content,
                        CreateUserID = dto.CreateUserID,
                        CustomerID = temp[i].CustomerID,
                        Phone = temp[i].Mobile
                    });
                    if ((i + 1) % 490 == 0)
                    {
                        batch.Data.Add(new BatchTemp() { content = dto.Content, phones = phones });
                        phones = "";
                    }
                }

                APIHelper _apiHelper = new APIHelper(Key.SSMApiUri, Key.SSMApiUriToken, Key.SSMAppid, Key.SSMAppsecred, Key.SSMSignKey, Key.SSMRedis);
                var r = await _apiHelper.Post<IFlyDogResult<IFlyDogResultType, BatchSubmitResult>, BatchSubmit>("/api/SSM/BatchSubmit", batch);

                if (r.ResultType != IFlyDogResultType.Success)
                {
                    result.Message = r.Message;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                    @"insert into [SmartSSMLog]([ID],[CustomerID],[Phone],[Content],[CreateUserID]) 
                    values(@ID,@CustomerID,@Phone,@Content,@CreateUserID)", ssmLogs);

                result.Message = "批量发送短信成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 添加客户组详情客户
        /// </summary>CustomerGroupBatchCallbackAdd
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> CustomerGroupDetailAdd(CustomerGroupDetailAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.UserListAdd == null || dto.UserListAdd.Count == 0)
            {
                result.Message = "请输入用户编号！";
                return result;
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {

                foreach (var item in dto.UserListAdd)
                {
                    if (item.UserID != 0)
                    {
                        var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                        result.Data = _connection.Execute("insert into SmartCustomerGroupDetail(ID,CustomerID,GroupID) values (@ID,@CustomerID,@GroupID)",
                        new { ID = id, CustomerID = item.UserID, GroupID = dto.CustomerGroupID }, _transaction);
                    }
                }

                var temp = new { 编号 = result.Data, 名称 = "添加用户组详情用户" };

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CustomerGroupDetailAdd,
                    Remark = LogType.CustomerGroupDetailAdd.ToDescription() + temp.ToJsonString()
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
        ///删除全部客户组客户详情
        /// </summary>
        /// <param name="dto"></param>CustomerGroupDetailAdd
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> CustomerGroupDetailDeleteAll(CustomerGroupDetailDeleteAll dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("delete SmartCustomerGroupDetail where GroupID=@GroupID", new { GroupID = dto.CustomerGroupID }, _transaction);

                var temp = new { 编号 = dto.CustomerGroupID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CustomerGroupDetailDelete,
                    Remark = LogType.CustomerGroupDetailDelete.ToDescription() + temp.ToJsonString()
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
        /// 删除客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(CustomerGroupDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 开始事物操作
            TryTransaction(() =>
            {
                string sql = @"DELETE SmartCustomerGroup WHERE ID=@ID
                                DELETE SmartCustomerGroupDetail WHERE GroupID = @GroupID";
                #region 开始更新操作
                result.Data = _connection.Execute(sql, new { ID = dto.ID, GroupID = dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CustomerGroupDelete,
                    Remark = LogType.CustomerGroupDelete.ToDescription() + temp.ToJsonString()
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
        /// 列表展示客户组
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerGroupInfo>> Get(CustomerGroupSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerGroupInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                string sql = @"SELECT ID,Name,CreateUserID,Remark FROM dbo.SmartCustomerGroup WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql += @" AND Name LIKE '%" + dto.Name + "%'";
                }
                sql += " AND CreateUserID=@CreateUserID";
                result.Data = _connection.Query<CustomerGroupInfo>(sql, new { CreateUserID = dto.createUserID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据客户组id查询客户组用户
        /// </summary>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CustomerGroupDetailInfo>>>> GetByCustomerGroupID(CustomerGroupDetailSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CustomerGroupDetailInfo>>>();

            result.Data = new Pages<IEnumerable<CustomerGroupDetailInfo>>();
            result.Data.PageNum = dto.PageNum;
            result.Data.PageSize = dto.PageSize;

            #region 开始查询数据动作
            await TryExecuteAsync(async () =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;
                result.Data.PageTotals = (await _connection.QueryAsync<int>(
                    @"select count(CustomerID) from SmartCustomerGroupDetail where GroupID=@CustomerGroupID", dto)).FirstOrDefault();

                result.Data.PageDatas = await _connection.QueryAsync<CustomerGroupDetailInfo>(
                        @"select a.CustomerID,b.Name,b.Gender,c.Name as Channel,d.Name as ConsultCharge,e.Name as MemberCategoryName,
						f.Name as ShareMemberCategoryName,'【'+l.Name+'】【'+i.Name+'】' as ExploitName,'【'+m.Name+'】【'+k.Name+'】' as ManagerName
                        from [SmartCustomerGroupDetail] a
                        left join SmartCustomer b on a.CustomerID=b.ID
						left join SmartChannel c on b.ChannelID=c.ID
						left join SmartSymptom d on b.CurrentConsultSymptomID=d.ID
						left join SmartMemberCategory e on b.MemberCategoryID=e.ID
						left join SmartShareCategory f on b.ShareMemberCategoryID=f.Name 
                        left join SmartOwnerShip h on a.ID=h.CustomerID and h.Type=1 and h.EndTime>getdate()
						left join SmartUser i on h.UserID=i.ID
						left join SmartHospital l on i.HospitalID=l.ID
						left join SmartOwnerShip j on a.ID=j.CustomerID and j.Type=2 and j.EndTime>getdate()
						left join SmartUser k on j.UserID=k.ID
						left join SmartHospital m on k.HospitalID=m.ID
						where a.GroupID=@CustomerGroupID 
                        order by a.CustomerID desc  OFFSET @StartRows ROWS FETCH NEXT @EndRows ROWS only", new { StartRows = startRow, EndRows = endRow, CustomerGroupID = dto.CustomerGroupID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询客户组详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CustomerGroupInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CustomerGroupInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<CustomerGroupInfo>("SELECT ID,Name,CreateUserID,Remark FROM dbo.SmartCustomerGroup WHERE ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(CustomerGroupUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "名称最多20个字符！";
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
                result.Data = _connection.Execute("update SmartCustomerGroup set Name = @Name,Remark=@Remark where ID = @ID", new { Name = dto.Name, Remark = dto.Remark, ID = dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CustomerGroupUpdate,
                    Remark = LogType.CustomerGroupUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 合并客户组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> MergeCustomerFilterAdd(MergeCustomerFilter dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var customerFilter1 = GetByFilterID(dto.CustomerFilterIDOne);//得到选择的客户组1包含的客户信息
            var customerFilter2 = GetByFilterID(dto.CustomerFilterIDTwo);//得到选择的客户组2包含的客户信息

            List<long> customerFilterDetailList = new List<long>();//临时存储筛选出来的客户id数据

            if (dto.MergeType == 1)
            { //取客户组1和客户组2都存在的用户id(交集)
                customerFilterDetailList = customerFilter1.Data.Intersect(customerFilter2.Data).ToList();
            }
            else if (dto.MergeType == 2)
            {//在客户组1或者客户组2(并集)
                customerFilterDetailList = customerFilter1.Data.Union(customerFilter2.Data).ToList();
            }
            else if (dto.MergeType == 3)
            {//在客户组1但是不在客户组2(差集)
                customerFilterDetailList = customerFilter1.Data.Except(customerFilter2.Data).ToList();
            }

            if (dto.SaveResult == 1)
            {//覆盖现有客户组
                if (Convert.ToInt64(dto.GroupID) == 0)
                {
                    result.Message = "请选择要覆盖的客户组！";
                    return result;
                }

                TryTransaction(() =>
                {
                    #region 开始数据操作动作              
                    _connection.Execute("DELETE SmartCustomerGroupDetail WHERE GroupID=@GroupID", new { GroupID = dto.GroupID }, _transaction);//覆盖到现有客户组之前先把现有客户组相关联的数据清空

                    foreach (var item in customerFilterDetailList)
                    {
                        var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                        result.Data = _connection.Execute("insert into SmartCustomerGroupDetail(ID,CustomerID,GroupID) values (@ID,@CustomerID,@GroupID)", new { ID = id, CustomerID = item, GroupID = dto.GroupID }, _transaction);
                    }//将新客户组新增到现有客户组中

                    var temp = new { 编号 = result.Data, FilterID = dto.GroupID, 类型 = "合并客户组" };
                    #endregion

                    #region 记录日志
                    AddOperationLog(new SmartOperationLog()
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CreateTime = DateTime.Now,
                        CreateUserID = dto.CreateUserID,
                        Type = LogType.MergeCustomer,
                        Remark = LogType.MergeCustomer.ToDescription() + temp.ToJsonString()
                    });
                    #endregion

                    result.Message = "合并成功";
                    result.ResultType = IFlyDogResultType.Success;
                    return true;
                });
            }
            else if (dto.SaveResult == 2)
            {//保存到新客户组
                if (string.IsNullOrWhiteSpace(dto.GroupName))
                {
                    result.Message = "请填写要保存的新客户组名称！";
                    return result;
                }

                TryTransaction(() =>
                {
                    #region 开始数据操作动作
                    var pid = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                    _connection.Execute("insert into SmartCustomerGroup(ID,Name,CreateUserID,Remark) values (@ID,@Name,@CreateUserID,@Remark)", new { ID = pid, Name = dto.GroupName, CreateUserID = dto.CreateUserID, Remark = " " }, _transaction);//保存到新的客户组先新增一个客户组信息

                    foreach (var item in customerFilterDetailList)
                    {
                        var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                        result.Data = _connection.Execute("insert into SmartCustomerGroupDetail(ID,CustomerID,GroupID) values (@ID,@CustomerID,@GroupID)", new { ID = id, CustomerID = item, GroupID = pid }, _transaction);
                    }//将新筛选的客户结果和新增的客户组信息进行关联

                    var temp = new { 编号 = result.Data, GroupID = dto.GroupID, 类型 = "合并结果集" };
                    #endregion

                    #region 记录日志
                    AddOperationLog(new SmartOperationLog()
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CreateTime = DateTime.Now,
                        CreateUserID = dto.CreateUserID,
                        Type = LogType.MergeCustomer,
                        Remark = LogType.MergeCustomer.ToDescription() + temp.ToJsonString()
                    });
                    #endregion

                    result.Message = "合并成功";
                    result.ResultType = IFlyDogResultType.Success;
                    return true;
                });
            }
            return result;
        }

        /// <summary>
        /// 新增结果集
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        //public IFlyDogResult<IFlyDogResultType, int> CustomerFilterAdd(CustomerFilterAdd dto)
        //{
        //    var result = new IFlyDogResult<IFlyDogResultType, int>();
        //    result.ResultType = IFlyDogResultType.Failed;

        //    #region 数据验证
        //    if (dto.Name.IsNullOrEmpty())
        //    {
        //        result.Message = "名称不能为空！";
        //        return result;
        //    }
        //    else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
        //    {
        //        result.Message = "名称最多20个字符！";
        //        return result;
        //    }
        //    #endregion

        //    #region 开启事物操作
        //    TryTransaction(() =>
        //    {

        //        #region 开始数据操作动作
        //        var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
        //        result.Data = _connection.Execute("insert into SmartCustomerFilter(ID,Name,CreateUserID,HospitalID,Remark) values (@ID,@Name,@CreateUserID,@HospitalID,@Remark)",
        //            new { ID = id, Name = dto.Name, CreateUserID = dto.CreateUserID, HospitalID = dto.HospitalID, Remark = dto.Remark }, _transaction);

        //        var temp = new { 编号 = result.Data, 名称 = dto.Name };
        //        #endregion

        //        #region 记录日志
        //        AddOperationLog(new SmartOperationLog()
        //        {
        //            ID = id,
        //            CreateTime = DateTime.Now,
        //            CreateUserID = dto.CreateUserID,
        //            Type = LogType.CustomerFilterAdd,
        //            Remark = LogType.CustomerFilterAdd.ToDescription() + temp.ToJsonString()
        //        });
        //        #endregion

        //        result.Message = "添加成功";
        //        result.ResultType = IFlyDogResultType.Success;
        //        return true;
        //    });
        //    #endregion
        //    return result;
        //}

        /// <summary>
        /// 新增结果集详情信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        //public IFlyDogResult<IFlyDogResultType, int> CustomerFilterDetailAdd(CustomerFilterDetailAdd dto)
        //{
        //    var result = new IFlyDogResult<IFlyDogResultType, int>();
        //    result.ResultType = IFlyDogResultType.Failed;

        //    if (dto.CustormInfoAdd == null || dto.CustormInfoAdd.Count == 0)
        //    {
        //        result.Message = "客户不能为空！";
        //        return result;
        //    }

        //    #region 开启事物操作            
        //    TryTransaction(() =>
        //    {
        //        #region 开始数据操作动作
        //        foreach (var item in dto.CustormInfoAdd)
        //        {
        //            var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
        //            result.Data = _connection.Execute("insert into SmartCustomerFilterDetail(ID,CustomerID,FilterID) values (@ID,@CustomerID,@FilterID)",
        //                new { ID = id, CustomerID = item.CustomerID, FilterID = dto.FilterID }, _transaction);
        //        }

        //        var temp = new { 编号 = result.Data, 名称 = dto.FilterID };
        //        #endregion

        //        #region 记录日志
        //        AddOperationLog(new SmartOperationLog()
        //        {
        //            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
        //            CreateTime = DateTime.Now,
        //            CreateUserID = dto.CreateUserID,
        //            Type = LogType.CustomerFilterDetailAdd,
        //            Remark = LogType.CustomerFilterDetailAdd.ToDescription() + temp.ToJsonString()
        //        });
        //        #endregion
        //        result.Message = "添加成功";
        //        result.ResultType = IFlyDogResultType.Success;
        //        return true;
        //    });
        //    #endregion
        //    return result;
        //}

        #region 按照条件查询接口
        /// <summary>
        /// 基本条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetBasicConditionSelect(BasicConditionSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>();

            string sql_where = "";

            if (dto.HospitalID > 0)
            {
                sql_where += " and b.HospitalID=@HospitalID ";
            }
            if (dto.DeveloperUserID > 0)
            {
                sql_where += " and (b.UserID=@DeveloperUserID and b.Type=1) ";
            }
            if (dto.ConsultantUserID > 0)
            {
                sql_where += " and (b.UserID=@ConsultantUserID and b.Type=2) ";
            }

            if (!dto.Phone.IsNullOrEmpty())
            {
                sql_where += " and (a.Mobile=@Phone or a.[MobileBackup]=@Phone) ";
            }
            if (dto.Gender > 0)
            {
                sql_where += " and a.[Gender]=Gender ";
            }
            if (dto.ChannelID > 0)
            {
                sql_where += " and a.[ChannelID]=@ChannelID ";
            }
            if (dto.MemberCategoryID > 0)
            {
                sql_where += " and a.[MemberCategoryID]=@MemberCategoryID ";
            }
            if (dto.ShareMemberCategoryID > 0)
            {
                sql_where += " and a.[ShareMemberCategoryID]=@ShareMemberCategoryID ";
            }

            if (dto.DoorStatus == 0)
            {
                sql_where += " and a.VisitTimes=0 ";
            }
            else if (dto.DoorStatus == 1)
            {
                sql_where += " and a.VisitTimes>0 ";
            }

            if (dto.ClinchStatus == 0)
            {
                sql_where += " and FirstDealTime is null ";
            }
            else if (dto.ClinchStatus == 1)
            {
                sql_where += " and FirstDealTime is not null ";
            }

            if (dto.WeChatStatus == 0)
            {
                sql_where += " and WeChatBind is null ";
            }
            if (dto.WeChatStatus == 1)
            {
                sql_where += " and WeChatBind is not null ";
            }

            if (dto.RegisterBeginTime != null && dto.RegisterEndTime != null)
            {
                dto.RegisterEndTime = dto.RegisterEndTime.Value.Date.AddDays(1);
                sql_where += " and a.CreateTime between @RegisterBeginTime and @RegisterEndTime ";
            }
            if (dto.FirstVisitBeginTime != null && dto.FirstVisitEndTime != null)
            {
                dto.FirstVisitEndTime = dto.FirstVisitEndTime.Value.Date.AddDays(1);
                sql_where += " and a.FirstVisitTime between @FirstVisitBeginTime and @FirstVisitEndTime ";
            }
            if (dto.LastVisitBeginTime != null && dto.LastVisitEndTime != null)
            {
                dto.LastVisitEndTime = dto.LastVisitEndTime.Value.Date.AddDays(1);
                sql_where += " and a.LastVisitTime between @LastVisitBeginTime and @LastVisitEndTime ";
            }

            await TryExecuteAsync(async () =>
            {
                var temp = await _connection.QueryAsync<ConsultConditionSelectTemp>(
                    string.Format(@"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@CreateUserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@CreateUserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@CreateUserID
                        )
                        select a.ID,a.Name as Content from SmartCustomer a,SmartOwnerShip b,treeUser c where a.ID=b.CustomerID and b.UserID=c.ID  and b.EndTime> getdate() {0}", sql_where), dto);


                if (!dto.Name.IsNullOrEmpty())
                {
                    if (!dto.Name.IsNullOrEmpty())
                    {
                        temp = temp.Where(u => u.Content.Contains(dto.Name));
                    }
                }
                result.Data = temp.Select(u => u.ID).Distinct().Select(n => new AddConditionSelectResult() { ID = n });

                result.ResultType = IFlyDogResultType.Success;
                result.Message = "查询成功！";
            });

            return result;
        }

        /// <summary>
        /// 账户条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetAccountConditionSelect(AccountConditionSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>();

            string sql_where = "";
            if (dto.HospitalID > 0)
            {
                sql_where += " and b.HospitalID=@HospitalID ";
            }
            if (dto.BalanceMoney != null)
            {
                sql_where += string.Format(" and a.[Deposit]{0}@BalanceMoney ", dto.BalanceConditionType);
            }
            if (dto.CouponMoney != null)
            {
                sql_where += string.Format(" and a.[Coupon]{0}@CouponMoney ", dto.CouponConditionType);
            }
            if (dto.PointCount != null)
            {
                sql_where += string.Format(" and a.[Point]{0}@PointCount ", dto.PointConditionType);
            }
            if (dto.CleanMoney != null)
            {
                sql_where += string.Format(" and a.[CashCardTotalAmount]{0}@CleanMoney ", dto.CleanConditionType);
            }
            if (dto.Commission != null)
            {
                sql_where += string.Format(" and a.[Commission]{0}@Commission ", dto.CommissionType);
            }

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<AddConditionSelectResult>(
                    string.Format(@"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@CreateUserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@CreateUserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@CreateUserID
                        )
                        select distinct a.ID from SmartCustomer a,SmartOwnerShip b,treeUser c where a.ID=b.CustomerID and b.UserID=c.ID  and b.EndTime> getdate() {0}", sql_where), dto);

                result.ResultType = IFlyDogResultType.Success;
                result.Message = "查询成功！";
            });

            return result;
        }

        /// <summary>
        /// 上门条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetDoorConditionSelect(DoorConditionSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>();

            string sql_where = "";
            if (dto.HospitalID > 0)
            {
                sql_where += " and b.HospitalID=@HospitalID ";
            }
            if (dto.DoorNumber != null)
            {
                sql_where += string.Format(" and a.[VisitTimes]{0}@DoorNumber ", dto.DoorConditionType);
            }
            if (dto.FirstDoorBeginTime != null && dto.FirstDoorEndTime != null)
            {
                dto.FirstDoorEndTime = dto.FirstDoorEndTime.Value.Date.AddDays(1);
                sql_where += " and a.FirstVisitTime between @FirstDoorBeginTime and @FirstDoorEndTime ";
            }
            if (dto.LastDoorBeginTime != null && dto.LastDoorEndTime != null)
            {
                dto.LastDoorEndTime = dto.LastDoorEndTime.Value.Date.AddDays(1);
                sql_where += " and a.LastVisitTime between @LastDoorBeginTime and @LastDoorEndTime ";
            }

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<AddConditionSelectResult>(
                    string.Format(@"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@CreateUserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@CreateUserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@CreateUserID
                        )
                        select distinct a.ID from SmartCustomer a,SmartOwnerShip b,treeUser c where a.ID=b.CustomerID and b.UserID=c.ID  and b.EndTime> getdate() {0}", sql_where), dto);

                result.ResultType = IFlyDogResultType.Success;
                result.Message = "查询成功！";
            });

            return result;
        }

        /// <summary>
        /// 订单条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetOrderConditionSelect(OrderConditionSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>();

            string sql_where = "";
            string sql_where_hospital = "";
            if (dto.HospitalID > 0)
            {
                sql_where_hospital += " and b.HospitalID=@HospitalID ";
            }
            if (dto.PaymentBeginTime != null && dto.PaymentEndTime != null)
            {
                dto.PaymentEndTime = dto.PaymentEndTime.Value.Date.AddDays(1);
                sql_where += " and a.PaidTime between @PaymentBeginTime and @PaymentEndTime ";
            }

            await TryExecuteAsync(async () =>
            {
                string sql_where_other = "";
                string sql_where_category = "";
                if (dto.ConsumptionMoney != null)
                {
                    if (dto.PaymentBeginTime != null && dto.PaymentEndTime != null)
                    {
                        sql_where_other += string.Format(@" inner join (select distinct CustomerID from SmartOrder where PaidStatus!=1 and PaidTime 
                        between @PaymentBeginTime and @PaymentEndTime  group by CustomerID having sum(FinalPrice){0}@ConsumptionMoney) h on a.CustomerID=h.CustomerID ", dto.ConsumptionConditionType);
                    }
                    else
                    {
                        sql_where_other += string.Format(@" inner join (select distinct CustomerID from SmartOrder where PaidStatus!=1
                        group by CustomerID having sum(FinalPrice){0}@ConsumptionMoney) h on a.CustomerID=h.CustomerID ", dto.ConsumptionConditionType);
                    }

                }
                if (dto.ScopeLimit != null && dto.ScopeLimit == 1 && dto.ChargeID > 0)
                {
                    sql_where_other += " inner join SmartOrderDetail d on a.ID=d.OrderID and d.ChargeID=@ChargeID ";
                }
                else if (dto.ScopeLimit != null && dto.ScopeLimit == 2 && dto.ChargeCategoryID > 0)
                {
                    sql_where_category += @",
						categorytree as
                        (
                            select ID from SmartChargeCategory where ID=@ChargeCategoryID 
                            union all
                            select a.ID from SmartChargeCategory a,categorytree b where a.ParentID=b.ID
                        ) ";
                    sql_where_other += @" inner join SmartOrderDetail d on a.ID=d.OrderID
						inner join SmartCharge f on d.ChargeID=f.ID 
						inner join categorytree g on f.CategoryID=g.ID ";
                }
                else if (dto.ScopeLimit != null && dto.ScopeLimit == 3 && dto.ItemCategoryID > 0)
                {
                    sql_where_other += @" inner join SmartOrderDetail d on a.ID=d.OrderID
						inner join SmartItemChargeDetail e on d.ChargeID=e.ChargeID and e.ItemID=@ItemCategoryID ";
                }

                result.Data = await _connection.QueryAsync<AddConditionSelectResult>(
                     string.Format(@"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@CreateUserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@CreateUserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@CreateUserID
                        )
                        {1}
						select distinct a.CustomerID as ID
						from SmartOrder a
						inner join SmartOwnerShip b on a.CustomerID=b.CustomerID and b.EndTime> getdate() {3}
						inner join treeUser c on b.UserID=c.ID
                        {2}
						where a.PaidStatus!=1 {0}", sql_where, sql_where_category, sql_where_other, sql_where_hospital), dto);


                result.ResultType = IFlyDogResultType.Success;
                result.Message = "查询成功！";
            });

            return result;
        }

        /// <summary>
        /// 咨询条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetConsultConditionSelect(ConsultConditionSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>();

            string sql_where = "";
            if (dto.HospitalID > 0)
            {
                sql_where += " and b.HospitalID=@HospitalID ";
            }

            if (dto.ConsultChargeID > 0)
            {
                sql_where += " and e.SymptomID=@ConsultChargeID ";
            }
            if (dto.ConsultBeginTime != null && dto.ConsultEndTime != null)
            {
                dto.ConsultEndTime = dto.ConsultEndTime.Value.Date.AddDays(1);
                sql_where += " and d.CreateTime between @ConsultBeginTime and @ConsultEndTime ";
            }
            if (dto.ConsultNumberConditionNum != null)
            {
                sql_where += string.Format(" and a.[ConsultTimes]{0}@ConsultNumberConditionNum ", dto.ConsultNumberConditionType);
            }
            if (dto.ConsultType > 0)
            {
                sql_where += " and d.Tool=@ConsultType ";
            }
            if (dto.FirstConsultBeginTime != null && dto.FirstConsultEndTime != null)
            {
                dto.FirstConsultEndTime = dto.FirstConsultEndTime.Value.Date.AddDays(1);
                sql_where += " and a.[FirstConsultTime] between @FirstConsultBeginTime and @FirstConsultEndTime ";
            }
            if (dto.LastConsultBeginTime != null && dto.LastConsultEndTime != null)
            {
                dto.LastConsultEndTime = dto.LastConsultEndTime.Value.Date.AddDays(1);
                sql_where += " and a.[LastConsultTime] between @LastConsultBeginTime and @LastConsultEndTime ";
            }



            await TryExecuteAsync(async () =>
            {
                var temp = await _connection.QueryAsync<ConsultConditionSelectTemp>(
                    string.Format(@"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@CreateUserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@CreateUserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@CreateUserID
                        )
                        select distinct a.ID,d.Content 
                        from SmartCustomer a,SmartOwnerShip b,treeUser c,SmartConsult d,SmartConsultSymptomDetail e
						where a.ID=b.CustomerID and b.EndTime> getdate() and b.UserID=c.ID and a.ID=d.CustomerID and d.ID=e.ConsultID {0}", sql_where), dto);

                if (!dto.ConsultContent.IsNullOrEmpty())
                {
                    temp = temp.Where(u => u.Content.Contains(dto.ConsultContent));
                }

                result.Data = temp.Select(u => u.ID).Distinct().Select(n => new AddConditionSelectResult() { ID = n });

                result.ResultType = IFlyDogResultType.Success;
                result.Message = "查询成功！";
            });

            return result;
        }

        /// <summary>
        /// 执行条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetExecuteConditionSelect(ExecuteConditionSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>();

            string sql_where = "";
            string sql_where_hospital = "";
            if (dto.HospitalID > 0)
            {
                sql_where_hospital += " and b.HospitalID=@HospitalID ";
            }
            if (dto.LastExecuteBeginTime != null && dto.LastExecuteEndTime != null)
            {
                dto.LastExecuteEndTime = dto.LastExecuteEndTime.Value.Date.AddDays(1);
                sql_where += " and a.CreateTime between @LastExecuteBeginTime and @LastExecuteEndTime ";
            }
            if (dto.ExecuteUserID != null)
            {
                sql_where += " and a.CreateUserID=@ExecuteUserID ";
            }

            await TryExecuteAsync(async () =>
            {
                string sql_where_other = "";
                string sql_where_category = "";

                if (dto.ScopeLimit != null && dto.ScopeLimit == 1 && dto.ChargeID > 0)
                {
                    sql_where += " a.ChargeID=@ChargeID ";
                }
                else if (dto.ScopeLimit != null && dto.ScopeLimit == 2 && dto.ChargeCategoryID > 0)
                {
                    sql_where_category += @",
						categorytree as
                        (
                            select ID from SmartChargeCategory where ID=@ChargeCategoryID 
                            union all
                            select a.ID from SmartChargeCategory a,categorytree b where a.ParentID=b.ID
                        ) ";
                    sql_where_other += @" inner join SmartCharge f on a.ChargeID=f.ID 
						inner join categorytree g on f.CategoryID=g.ID ";
                }
                else if (dto.ScopeLimit != null && dto.ScopeLimit == 3 && dto.ItemCategoryID > 0)
                {
                    sql_where_other += @" inner join SmartItemChargeDetail e on a.ChargeID=e.ChargeID and e.ItemID=@ItemCategoryID ";
                }

                result.Data = await _connection.QueryAsync<AddConditionSelectResult>(
                     string.Format(@"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@CreateUserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@CreateUserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@CreateUserID
                        )
                        {1}
						select distinct a.CustomerID as ID
						from SmartOperation a
						inner join SmartOwnerShip b on a.CustomerID=b.CustomerID and b.EndTime> getdate() 
						inner join treeUser c on b.UserID=c.ID
                        {2}
						{0}", sql_where, sql_where_category, sql_where_other, sql_where_hospital), dto);


                result.ResultType = IFlyDogResultType.Success;
                result.Message = "查询成功！";
            });

            return result;
        }

        /// <summary>
        /// 会员条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetMemberConditionSelect(MemberConditionSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>();

            string sql_where = "";
            if (dto.HospitalID > 0)
            {
                sql_where += " and b.HospitalID=@HospitalID ";
            }

            if (dto.MemberCategoryID > 0)
            {
                sql_where += " and a.[MemberCategoryID]=@MemberCategoryID ";
            }
            if (dto.JoinBeginTime != null && dto.JoinEndTime != null)
            {
                sql_where += " and d.CreateTime between @JoinBeginTime and @JoinEndTime ";
            }

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<AddConditionSelectResult>(
                    string.Format(@"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@CreateUserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@CreateUserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@CreateUserID
                        )
                        select distinct a.ID 
                        from SmartCustomer a,SmartOwnerShip b,treeUser c,SmartMember d
						where a.ID=b.CustomerID and b.EndTime> getdate() and b.UserID=c.ID and a.ID=d.CustomerID {0}", sql_where), dto);

                result.ResultType = IFlyDogResultType.Success;
                result.Message = "查询成功！";
            });

            return result;
        }

        /// <summary>
        /// 未成交条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetFailtureConditionSelect(FailtureConditionSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>();

            string sql_where = "";
            if (dto.HospitalID > 0)
            {
                sql_where += " and b.HospitalID=@HospitalID ";
            }

            if (dto.FailtureCategoryID > 0)
            {
                sql_where += " and d.CategoryID=@FailtureCategoryID ";
            }
            if (dto.SubmitBeginTime != null && dto.SubmitEndTime != null)
            {
                sql_where += " and d.CreateTime between @SubmitBeginTime and @SubmitEndTime ";
            }

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<AddConditionSelectResult>(
                    string.Format(@"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@CreateUserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@CreateUserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@CreateUserID
                        )
                        select distinct a.ID 
                        from SmartCustomer a,SmartOwnerShip b,treeUser c,SmartFailture d
						where a.ID=b.CustomerID and b.EndTime> getdate() and b.UserID=c.ID and a.ID=d.CustomerID {0}", sql_where), dto);

                result.ResultType = IFlyDogResultType.Success;
                result.Message = "查询成功！";
            });

            return result;
        }

        /// <summary>
        /// 标签条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>> GetTagConditionSelect(TagConditionSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>();

            string sql_where = "";
            if (dto.HospitalID > 0)
            {
                sql_where += " and b.HospitalID=@HospitalID ";
            }

            for (int i = 0; i < dto.TagInfoAdd.Count(); i++)
            {
                if (i == 0)
                {
                    sql_where += " and (a.TagID=" + dto.TagInfoAdd[i].TagID;
                }
                else
                {
                    sql_where += " or a.TagID=" + dto.TagInfoAdd[i].TagID;
                }

                if (i == dto.TagInfoAdd.Count() - 1)
                {
                    sql_where += " )";
                }
            }

            foreach (var u in dto.TagInfoAdd)
            {

            }

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<AddConditionSelectResult>(
                    string.Format(@"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@CreateUserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@CreateUserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@CreateUserID
                        )
                        select distinct a.CustomerID as ID 
                        from SmartCustomerTag a,SmartOwnerShip b,treeUser c
						where a.CustomerID=b.CustomerID and b.EndTime> getdate() and b.UserID=c.ID {0}", sql_where), dto);

                result.ResultType = IFlyDogResultType.Success;
                result.Message = "查询成功！";
            });

            return result;
        }

        /// <summary>
        /// 回访条件查询
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>> GetCallbackConditionSelect(CallbackConditionSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AddConditionSelectResult>>();

            return result;
        }
        #endregion
    }
}
