using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
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
    public class ConsultService : BaseService, IConsultService
    {
        /// <summary>
        /// 获取顾客咨列表
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Consult>>> GetConsult(long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Consult>>();
            result.ResultType = IFlyDogResultType.Success;
            result.Message = "查询成功";

            await TryExecuteAsync(async () =>
            {
                var temp = await _connection.QueryAsync<Consult>(
                    @"SELECT a.[ID],a.CustomerID,'【'+e.Name+'】【'+d.Name +'】' as CreateUserName,a.[CreateTime],c.Name as Tool,[Content],f.Name as Symptoms
                    FROM [SmartConsult] a
                    left join SmartConsultSymptomDetail b on a.ID=b.ConsultID
                    left join SmartSymptom f on f.ID=b.SymptomID
                    left join SmartTool c on c.ID=a.Tool
                    left join SmartUser d on d.ID=a.CreateUserID
                    left join SmartHospital e on d.HospitalID=e.ID where CustomerID=@CustomerID order by b.ID", new { CustomerID = customerID });

                var list = new Dictionary<string, Consult>();
                foreach (var u in temp)
                {
                    if (list.Keys.Contains(u.ID))
                    {
                        list[u.ID].Symptoms += "," + u.Symptoms;
                    }
                    else
                    {
                        list.Add(u.ID, u);
                    }
                }
                result.Data = list.Values;
            });

            return result;
        }

        /// <summary>
        /// 获取咨询详细信息
        /// </summary>
        /// <param name="ID">咨询记录ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ConsultDetail>> GetConsultDetail(long ID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ConsultDetail>();
            result.ResultType = IFlyDogResultType.Success;
            result.Message = "查询成功";

            await TryExecuteAsync(async () =>
            {
                var list = new Dictionary<string, ConsultDetail>();
                var temp = await _connection.QueryAsync<ConsultDetail, SymptomDetail, ConsultDetail>(
                    @"SELECT a.[ID],a.CustomerID,'【'+e.Name+'】【'+d.Name +'】' as CreateUserName,a.CustomerID,a.[CreateTime],a.Tool as ToolID,c.Name as Tool,[Content],b.SymptomID,f.Name as SymptomName
                    FROM [SmartConsult] a
                    left join SmartConsultSymptomDetail b on a.ID=b.ConsultID
                    left join SmartSymptom f on f.ID=b.SymptomID
                    left join SmartTool c on c.ID=a.Tool
                    left join SmartUser d on d.ID=a.CreateUserID
                    left join SmartHospital e on d.HospitalID=e.ID where a.ID=@ID order by b.ID",
                    (consult, detail) =>
                    {
                        ConsultDetail u;
                        if (!list.TryGetValue(consult.ID, out u))
                        {
                            list.Add(consult.ID, u = consult);
                        }
                        if (detail != null)
                            u.Symptoms.Add(detail);
                        return consult;
                    },
                    new { ID = ID }, null, true, splitOn: "SymptomID");

                result.Data = list.Values.FirstOrDefault();
            });

            return result;
        }

        /// <summary>
        /// 咨询修改
        /// </summary>
        /// <param name="dto">咨询内容</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> ConsultUpdate(ConsultAddUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.ID == 0)
            {
                result.Message = "请选择咨询记录ID！";
                return result;
            }

            if (dto.ToolID == 0)
            {
                result.Message = "请选择沟通方式！";
                return result;
            }

            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "咨询内容不能为空！";
                return result;
            }


            if (!dto.Content.IsNullOrEmpty() && dto.Content.Length > 500)
            {
                result.Message = "咨询内容不能超过500个字符！";
                return result;
            }

            if (dto.SymptomIDS == null || dto.SymptomIDS.Count() == 0)
            {
                result.Message = "咨询项目数量要至少为1！";
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
                var task1 = _connection.ExecuteAsync(@"update [SmartConsult] set [Tool]=@ToolID,[Content]=@Content where [ID]=@ID", dto, _transaction);

                await _connection.ExecuteAsync(@"delete from SmartConsultSymptomDetail where ConsultID=@ConsultID", new { ConsultID = dto.ID }, _transaction);

                var task2 = _connection.ExecuteAsync(@"insert into [SmartConsultSymptomDetail]([ID],[ConsultID],[SymptomID]) values(@ID,@ConsultID,@SymptomID)",
                      dto.SymptomIDS.Select(u => new
                      {
                          ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                          ConsultID = dto.ID,
                          SymptomID = u
                      }), _transaction, 30, CommandType.Text);

                await Task.WhenAll(task1, task2);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 添加咨询
        /// </summary>
        /// <param name="dto">咨询信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> ConsultAdd(ConsultAddUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.ToolID == 0)
            {
                result.Message = "请选择沟通方式！";
                return result;
            }

            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "咨询内容不能为空！";
                return result;
            }

            if (dto.SymptomIDS == null || dto.SymptomIDS.Count() == 0)
            {
                result.Message = "咨询项目数量要至少为1！";
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

                DateTime now = DateTime.Now;

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                var task1 = _connection.ExecuteAsync(@"insert into [SmartConsult]([ID],[CustomerID],[CreateUserID],[CreateTime],[Tool],[Content]) 
                                         values(@ID,@CustomerID,@CreateUserID,@CreateTime,@Tool,@Content)",
                                         new
                                         {
                                             ID = id,
                                             CustomerID = dto.CustomerID,
                                             CreateUserID = dto.CreateUserID,
                                             CreateTime = now,
                                             Tool = dto.ToolID,
                                             Content = dto.Content
                                         }, _transaction);

                var task2 = _connection.ExecuteAsync(@"insert into [SmartConsultSymptomDetail]([ID],[ConsultID],[SymptomID]) values(@ID,@ConsultID,@SymptomID)",
                      dto.SymptomIDS.Select(u => new
                      {
                          ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                          ConsultID = id,
                          SymptomID = u
                      }), _transaction, 30, CommandType.Text);

                var task3 = _connection.ExecuteAsync(
                    @"update SmartCustomer set LastConsultTime=@Time,ConsultTimes=ConsultTimes+1,FirstConsultTime=case when FirstConsultTime is null then @Time else FirstConsultTime end 
                     where ID=@ID", new { Time = now, ID = dto.CustomerID }, _transaction);
                await Task.WhenAll(task1, task2);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 咨询删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> ConsultDelete(ConsultDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthTransactionAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                var count = (await _connection.QueryAsync<int>(@"select count(ID) from SmartConsult where CustomerID=@CustomerID", new { CustomerID = dto.CustomerID }, _transaction)).FirstOrDefault();

                if (count <= 1)
                {
                    result.Message = "对不起，最后一条咨询记录不允许删除！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return;
                }

                var task1 = _connection.ExecuteAsync(@"delete from [SmartConsult] where [ID]=@ID", new { ID = dto.ID }, _transaction);

                var task2 = _connection.ExecuteAsync(@"delete from SmartConsultSymptomDetail where ConsultID=@ConsultID", new { ConsultID = dto.ID }, _transaction);

                await Task.WhenAll(task1, task2);

                var time = (await _connection.QueryAsync<DateTime?>(
                    @"select top 1 [CreateTime] from [SmartConsult] where [CustomerID]=@CustomerID order by CreateTime desc",
                    new { CustomerID = dto.CustomerID }, _transaction)).FirstOrDefault();

                if (time != null)
                {
                    await _connection.ExecuteAsync(
                     @"update SmartCustomer set LastConsultTime=@Time,ConsultTimes=ConsultTimes-1 where ID=@ID", new { Time = time, ID = dto.CustomerID }, _transaction);
                }

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
    }
}
