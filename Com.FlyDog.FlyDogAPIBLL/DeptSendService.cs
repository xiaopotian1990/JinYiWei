using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class DeptSendService : BaseService, IDeptSendService
    {
        /// <summary>
        /// 科室发料请求
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptSendInfo>>> GetDeptSendInfo(long hospitalID, long userID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<DeptSendInfo>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            //var order=await _orderService.getd
            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<DeptSendInfo>(
                   @"select a.ID,b.CustomerID,g.Name as CustomerName,c.Name as ChargeName,b.CreateTime,e.Name as Warehouse,
                   d.Name as Product,d.Size,h.Name as UnitName,a.Num
                   from SmartOperationProduct a
                   inner join SmartOperation b on a.OperationID=b.ID and b.HospitalID=@HospitalID
                   inner join SmartCharge c on b.ChargeID=c.ID
                   inner join SmartProduct d on a.ProductID=d.ID
                   inner join SmartWarehouse e on a.WarehouseID=e.ID 
                   inner join SmartWarehouseManager f on e.ID=f.WarehouseID and f.UserID=@UserID 
                   inner join SmartCustomer g on b.CustomerID=g.ID
                   left join SmartUnit h on d.UnitID=h.ID
                   where a.Status=@Status", new { Status = OperationProductStatus.No, HospitalID = hospitalID, UserID = userID });
            });

            return result;
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Send(DeptSendAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            List<Task> tasks = new List<Task>();

            await TryTransactionAsync(async () =>
            {
                OperationProductAdd temp = (await _connection.QueryAsync<OperationProductAdd>(
                    @"select [ProductID],[WarehouseID],[Num] from [SmartOperationProduct] where [ID]=@ID and [Status]=@Status", new { ID = dto.ID, Status = OperationProductStatus.No }, _transaction)).FirstOrDefault();

                if (temp == null)
                {
                    result.Message = "发货请求不存在或者已经发货！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return false;
                }

                var productTemp = await _connection.QueryAsync<WarehouseTemp>(
                    @"select ID,Num from [SmartStock] where [WarehouseID]=@WarehouseID and [ProductID]=@ProductID order by CASE WHEN Expiration IS NULL THEN '2099-01-01' ELSE Expiration END",
                    new { WarehouseID = temp.WarehouseID, ProductID = temp.ProductID }, _transaction);

                if (productTemp.Sum(u => u.Num) < temp.Num)
                {
                    result.Message = "库存不足，无法发货！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return false;
                }

                foreach (var u in productTemp)
                {
                    if (temp.Num == 0)
                    {
                        break;
                    }

                    if (u.Num > temp.Num)
                    {
                        tasks.Add(_connection.ExecuteAsync(
                            @"update [SmartStock] set [Num]=[Num]-@Num,Amount=Num*Price where ID=@ID", new { ID = u.ID, Num = temp.Num }, _transaction));
                        break;
                    }
                    else if (u.Num == temp.Num)
                    {
                        tasks.Add(_connection.ExecuteAsync(
                            @"delete from [SmartStock] where ID=@ID", new { ID = u.ID }, _transaction));
                        break;
                    }
                    else
                    {
                        tasks.Add(_connection.ExecuteAsync(
                            @"delete from [SmartStock] where ID=@ID", new { ID = u.ID }, _transaction));
                        temp.Num = temp.Num - u.Num;
                    }
                }

                tasks.Add(_connection.ExecuteAsync(
                    @"update [SmartOperationProduct] set [SendTime]=@SendTime,[SendUser]=@SendUser,[Status]=@Status where ID=@ID",
                    new { SendTime = DateTime.Now, SendUser = dto.CreateUserID, Status = OperationProductStatus.Yes, ID = dto.ID }, _transaction));

                await Task.WhenAll(tasks);
                result.ResultType = IFlyDogResultType.Success;
                result.Message = "发货成功！";
                return true;
            });

            return result;
        }

        /// <summary>
        /// 今日发货记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptSend>>> GetDeptSendToday(long hospitalID, long userID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<DeptSend>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            //var order=await _orderService.getd
            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<DeptSend>(
                   @"select a.ID,b.CustomerID,g.Name as CustomerName,a.[SendTime],e.Name as Warehouse,c.Name as CreateUserName,
                   d.Name as Product,d.Size,h.Name as UnitName,a.Num
                   from SmartOperationProduct a
                   inner join SmartOperation b on a.OperationID=b.ID
                   inner join SmartProduct d on a.ProductID=d.ID
                   inner join SmartWarehouse e on a.WarehouseID=e.ID 
                   inner join SmartCustomer g on b.CustomerID=g.ID
                   left join SmartUnit h on d.UnitID=h.ID
				   inner join SmartUser c on a.[SendUser]=c.ID
                   where a.Status=@Status and a.SendUser=@SendUser and DateDiff(dd,a.SendTime,getdate())=0 order by a.SendTime", new { Status = OperationProductStatus.Yes, HospitalID = hospitalID, SendUser = userID });
            });

            return result;
        }
    }
}
