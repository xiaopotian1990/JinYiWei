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
    public class PromoterCommissionService : BaseService, IPromoterCommissionService
    {
        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CommissionOut(CommissionOut dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            if (dto.Amount <= 0)
            {
                result.Message = "提现金额应大于0！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

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
                var commission = (await _connection.QueryAsync<decimal>(
                    @"select Commission from SmartCustomer where ID=@ID", new { ID = dto.CustomerID }, _transaction)).FirstOrDefault();

                if (commission < dto.Amount)
                {
                    result.Message = "提现金额不足！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return false;
                }

                await _connection.ExecuteAsync(
                          @"insert into [SmartCommissionUsage]([ID],[CustomerID],[Type],[Remark],CreateTime,CreateUserID,Amount,HospitalID) 
                        values(@ID,@CustomerID,@Type,@Remark,@CreateTime,@CreateUserID,@Amount,@HospitalID)", new
                          {
                              ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                              CustomerID = dto.CustomerID,
                              CreateTime = DateTime.Now,
                              Type = CommissionType.Out,
                              Remark = dto.Remark,
                              CreateUserID = dto.CreateUserID,
                              Amount = dto.Amount * -1,
                              HospitalID = dto.HospitalID
                          }, _transaction);

                await _connection.ExecuteAsync(
                    @"update SmartCustomer set Commission=Commission-@Amount where ID=@ID", new { Amount = dto.Amount, ID = dto.CustomerID }, _transaction);

                result.Message = "提现成功！";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

           
            return result;
        }
    }
}
