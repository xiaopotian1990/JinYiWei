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
    public class PointService : BaseService, IPointService
    {
        /// <summary>
        /// 获取顾客积分信息
        /// </summary>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, CustomerPointInfo>> GetPointInfo(long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CustomerPointInfo>();

            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = (await _connection.QueryAsync<CustomerPointInfo>(
                    @"select ID as CustomerID,Name as CustomerName,Point from SmartCustomer where ID=@ID", new { ID = customerID })).FirstOrDefault();
            });

            return result;
        }

        /// <summary>
        /// 手动增加扣减积分
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeductPoint(DeductPoint dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Point <= 0)
            {
                result.Message = "积分数量应大于0！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                result.Message = "请填写备注";
                return result;
            }

            if (dto.Remark.Length >= 200)
            {
                result.Message = "备注不能大于200字";
                return result;
            }

            await TryTransactionAsync(async () =>
            {
                if (dto.Type == PointType.ManualRebate)
                {
                    var point = (await _connection.QueryAsync<decimal>(
                       @"select Point from SmartCustomer where ID=@ID", new { ID = dto.CustomerID }, _transaction)).FirstOrDefault();

                    if (point < dto.Point)
                    {
                        result.Message = string.Format("您的积分只有{0}，余额不足！", point);
                        return false;
                    }
                    dto.Point = dto.Point * -1;
                }

                Task task1 = _connection.ExecuteAsync(
                    @"insert into [SmartPoint]([ID],[CustomerID],[CreateUserID],[CreateTime],[Type],[Amount],[Remark],[ConsumeAmount],[HospitalID]) 
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@Type,@Amount,@Remark,@ConsumeAmount,@HospitalID)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        Type = dto.Type,
                        Amount = dto.Point,
                        Remark = dto.Remark,
                        ConsumeAmount = 0,
                        HospitalID = dto.HospitalID
                    }, _transaction);

                Task task2 = _connection.ExecuteAsync(
                    @"update SmartCustomer set Point=Point+@Point where ID=@ID", new { ID = dto.CustomerID, Point = dto.Point }, _transaction);

                await Task.WhenAll(task1, task2);

                result.Message = "兑换成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 积分兑换券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> PointToCoupon(PointToCoupon dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.PointAmount <= 0)
            {
                result.Message = "积分数量应大于0！";
                return result;
            }

            if (dto.CouponAmount <= 0)
            {
                result.Message = "兑换券数量应大于0！";
                return result;
            }

            await TryTransactionAsync(async () =>
            {

                var point = (await _connection.QueryAsync<decimal>(
                   @"select Point from SmartCustomer where ID=@ID", new { ID = dto.CustomerID }, _transaction)).FirstOrDefault();

                if (point < dto.PointAmount)
                {
                    result.Message = string.Format("您的积分只有{0}，余额不足！", point);
                    return false;
                }
                dto.PointAmount = dto.PointAmount * -1;

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                Task task1 = _connection.ExecuteAsync(
                    @"insert into [SmartPoint]([ID],[CustomerID],[CreateUserID],[CreateTime],[Type],[Amount],[Remark],[ConsumeAmount],[HospitalID]) 
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@Type,@Amount,@Remark,@ConsumeAmount,@HospitalID)",
                    new
                    {
                        ID = id,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        Type = PointType.CouponExchange,
                        Amount = dto.PointAmount,
                        Remark = string.Format("{0}积分兑换{1}券", dto.PointAmount * -1, dto.CouponAmount),
                        ConsumeAmount = 0,
                        HospitalID = dto.HospitalID
                    }, _transaction);

                Task task2 = _connection.ExecuteAsync(
                    @"update SmartCustomer set Point=Point+@Point,[Coupon]=[Coupon]+@Coupon where ID=@ID", new { ID = dto.CustomerID, Point = dto.PointAmount, Coupon = dto.CouponAmount }, _transaction);

                Task task3 = _connection.ExecuteAsync(
                  @"insert into [SmartCoupon]([ID],[HospitalID],[CustomerID],[CreateUserID],[CreateTime],[Access],[CategoryID],[Amount],[Rest],[Remark],Status) 
                    values(@ID,@HospitalID,@CustomerID,@CreateUserID,@CreateTime,@Access,@CategoryID,@Amount,@Rest,@Remark,@Status)",
                  new
                  {
                      ID = id,
                      HospitalID = dto.HospitalID,
                      CustomerID = dto.CustomerID,
                      CreateUserID = dto.CreateUserID,
                      CreateTime = DateTime.Now,
                      Access = CouponType.PointChange,
                      CategoryID = dto.CouponCategory,
                      Amount = dto.CouponAmount,
                      Rest = dto.CouponAmount,
                      Remark = string.Format("{0}积分兑换{1}券", dto.PointAmount * -1, dto.CouponAmount),
                      Status = CouponStatus.Effective
                  }, _transaction);

                await Task.WhenAll(task1, task2, task3);

                result.Message = "兑换成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
