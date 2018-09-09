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
    public class CouponService : BaseService, ICouponService
    {
        /// <summary>
        /// 查询剩余代金券
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneCoupons>>> GetNoDoneOrders(long hospitalID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneCoupons>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<NoDoneCoupons>(
                    @"select a.ID as CouponID ,b.Name as CouponCategoryName,a.Amount,a.Rest
                    from SmartCoupon a 
                    inner join SmartCouponCategory b on a.CategoryID=b.ID
                    where a.CustomerID=@CustomerID and a.HospitalID=@HospitalID and a.Status=@Status and a.Rest>0", new { CustomerID = customerID, HospitalID = hospitalID, Status = CouponStatus.Effective });
            });

            return result;
        }

        /// <summary>
        /// 手工赠券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> SendCoupon(SendCoupon dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.CouponAmount <= 0)
            {
                result.Message = "兑换券数量应大于0！";
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
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                Task task2 = _connection.ExecuteAsync(
                    @"update SmartCustomer set [Coupon]=[Coupon]+@Coupon where ID=@ID", new { ID = dto.CustomerID, Coupon = dto.CouponAmount }, _transaction);

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
                      Access = CouponType.ManualSend,
                      CategoryID = dto.CouponID,
                      Amount = dto.CouponAmount,
                      Rest = dto.CouponAmount,
                      Remark = dto.Remark,
                      Status = CouponStatus.Effective
                  }, _transaction);

                await Task.WhenAll(task2, task3);

                result.Message = "赠送成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 手动扣减券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeductCoupon(SendCoupon dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.CouponAmount <= 0)
            {
                result.Message = "数量应大于0！";
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
                var coupon = (await _connection.QueryAsync<CanCashierCoupons>(
                       @"SELECT [ID] as CouponID,[HospitalID],[Rest],[Status] FROM [SmartCoupon] where Status=@Status and ID=@ID and CustomerID=@CustomerID", new
                       {
                           ID = dto.CouponID,
                           CustomerID = dto.CustomerID,
                           Status = CouponStatus.Effective
                       }, _transaction)).FirstOrDefault();

                if (coupon == null)
                {
                    result.Message = "代金券不存在或者已经过期！";
                    return false;
                }
                if (coupon.Rest < dto.CouponAmount)
                {
                    result.Message = "代金券余额不足！";
                    return false;
                }
                if (coupon.HospitalID != dto.HospitalID.ToString())
                {
                    result.Message = "对不起，您无权操作其他家医院的代金券！";
                    return false;
                }

                //CouponStatus status = CouponStatus.Effective;
                //if (coupon.Rest == dto.CouponAmount)
                //{
                //    status = CouponStatus.Use;
                //}

                Task task1 = _connection.ExecuteAsync(
                    @"update [SmartCoupon] set Rest=Rest-@Rest where ID=@ID",
                    new
                    {
                        ID = dto.CouponID,
                        //Status = status,
                        Rest = dto.CouponAmount
                    }, _transaction);

                Task task2 = _connection.ExecuteAsync(
                    @"update SmartCustomer set Coupon=Coupon-@Coupon where ID=@ID", new { ID = dto.CustomerID, Coupon = dto.CouponAmount }, _transaction);

                Task task3 = _connection.ExecuteAsync(
                            @"insert into [SmartCouponUsage]([ID],[CouponID],[Amount],[Type],[Remark],CreateUserID)
                        values(@ID,@CouponID,@Amount,@Type,@Remark,@CreateUserID)",
                            new
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                CouponID = dto.CouponID,
                                Amount = dto.CouponAmount,
                                Remark = CouponUsageType.ManualDelete.ToDescription(),
                                Type = CouponUsageType.ManualDelete,
                                CreateUserID=dto.CreateUserID
                            }, _transaction);

                await Task.WhenAll(task1, task2,task3);

                result.Message = "扣减成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
