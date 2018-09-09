using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class InpatientService : BaseService, IInpatientService
    {
        /// <summary>
        /// 住院
        /// </summary>
        /// <param name="dto">住院信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> In(InpatientAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length > 50)
            {
                result.Message = "备注最多50个字符！";
                return result;
            }

            await TryTransactionAsync(async () =>
            {
                var bedStatus = (await _connection.QueryAsync<BedStatus?>(
                    @"select Usage from [SmartBed] where [ID]=@ID and HospitalID=@HospitalID and Status=@Status", new { ID = dto.BedID, HospitalID = dto.HospitalID, Status = CommonStatus.Use }, _transaction)).FirstOrDefault();

                if (bedStatus == null)
                {
                    result.Message = "该床位不存在或已经被停用！";
                    return false;
                }

                if (bedStatus == BedStatus.Use)
                {
                    result.Message = "该床位已经被使用！";
                    return false;
                }

                var count = (await _connection.QueryAsync<int>(
                    @"select count([ID]) from [SmartInpatient] where [CustomerID]=@CustomerID and [Status]=@Status", new { CustomerID = dto.CustomerID, Status = InpatientStatus.In }, _transaction)).FirstOrDefault();
                if (count > 0)
                {
                    result.Message = "该用户正在住院中,无需重复办理住院！";
                    return false;
                }

                Task task1 = _connection.ExecuteAsync(
                    @"insert into [SmartInpatient](ID,HospitalID,CustomerID,CreateUserID,BedID,InTime,Status,Remark) values(@ID,@HospitalID,@CustomerID,@CreateUserID,@BedID,@InTime,@Status,@Remark)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        HospitalID = dto.HospitalID,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        BedID = dto.BedID,
                        InTime = DateTime.Now,
                        Status = InpatientStatus.In,
                        Remark = dto.Remark
                    }, _transaction);

                Task task2 = _connection.ExecuteAsync(
                    @"update [SmartBed] set [Usage]=@Usage where [ID]=@ID", new { ID = dto.BedID, Usage = BedStatus.Use }, _transaction);

                await Task.WhenAll(task1, task2);

                result.Message = "住院成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 出院
        /// </summary>
        /// <param name="dto">出院信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Out(Inpatientout dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            await TryTransactionAsync(async () =>
            {
                var inpatientStatus = (await _connection.QueryAsync<InpatientStatus?>(
                    @"select [Status] from [SmartInpatient] where [ID]=@ID and [HospitalID]=@HospitalID and [BedID]=@BedID", dto, _transaction)).FirstOrDefault();

                if (inpatientStatus == null)
                {
                    result.Message = "该记录不存在！";
                    return false;
                }

                if (inpatientStatus == InpatientStatus.Out)
                {
                    result.Message = "该顾客已经出院！";
                    return false;
                }

                Task task1 = _connection.ExecuteAsync(
                    @"update [SmartInpatient] set [Status]=@Status,[OutTime]=@OutTime where ID=@ID and [HospitalID]=@HospitalID and [BedID]=@BedID",
                    new
                    {
                        Status = InpatientStatus.Out,
                        OutTime = DateTime.Now,
                        ID = dto.ID,
                        HospitalID = dto.HospitalID,
                        BedID = dto.BedID
                    }, _transaction);

                Task task2 = _connection.ExecuteAsync(
                    @"update [SmartBed] set [Usage]=@Usage where [ID]=@ID", new { ID = dto.BedID, Usage = BedStatus.Free }, _transaction);

                await Task.WhenAll(task1, task2);
                result.Message = "出院成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 住院工作台住院列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<InpatientIn>>> GetIn(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<InpatientIn>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<InpatientIn>(
                    @"select a.ID,a.CustomerID,c.Name as CustomerName,c.ImageUrl,a.BedID,b.Name as BedName,a.InTime,a.Remark,d.ID as OrderID
                    from SmartInpatient a
                    inner join SmartBed b on a.BedID=b.ID
                    inner join SmartCustomer c on a.CustomerID=c.ID
					left join SmartOrder d on a.ID=d.InpatientID and d.PaidStatus=@PaidStatus
                    where a.HospitalID=@HospitalID and a.[Status]=@Status", new { HospitalID = hospitalID, Status=InpatientStatus.In,PaidStatus= PaidStatus.NotPaid });
            });

            return result;
        }
    }
}
