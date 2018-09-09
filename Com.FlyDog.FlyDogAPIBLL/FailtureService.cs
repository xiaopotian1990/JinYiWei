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
    public class FailtureService : BaseService, IFailtureService
    {
        /// <summary>
        /// 获取顾客未成交列表
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Failture>>> GetByCustomerID(long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Failture>>();
            result.ResultType = IFlyDogResultType.Success;
            result.Message = "查询成功";

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<Failture>(
                    @"SELECT a.[ID],a.CustomerID,'【'+e.Name+'】【'+d.Name +'】' as CreateUserName,a.[CreateTime],b.Name as CategoryName,a.Content
                    FROM SmartFailture a
                    left join SmartFailtureCategory b on b.ID=a.CategoryID
                    left join SmartUser d on d.ID=a.CreateUserID
                    left join SmartHospital e on d.HospitalID=e.ID where CustomerID=@CustomerID order by a.CreateTime desc", new { CustomerID = customerID });
            });

            return result;
        }

        /// <summary>
        /// 获取未成交详细信息
        /// </summary>
        /// <param name="ID">咨记录ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, FailtureDetail>> GetDetail(long ID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, FailtureDetail>();
            result.ResultType = IFlyDogResultType.Success;
            result.Message = "查询成功";

            await TryExecuteAsync(async () =>
            {
                await TryExecuteAsync(async () =>
                {
                    result.Data = (await _connection.QueryAsync<FailtureDetail>(
                        @"select [ID],[CustomerID],[Content],[CategoryID] from [SmartFailture] where ID=@ID ", new { ID = ID })).FirstOrDefault();
                });
            });

            return result;
        }

        /// <summary>
        /// 未成交修改
        /// </summary>
        /// <param name="dto">未成交信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Update(FailtureAddUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.ID == 0)
            {
                result.Message = "请选择未成交记录ID！";
                return result;
            }

            if (dto.CategoryID == 0)
            {
                result.Message = "请选择未成交类型！";
                return result;
            }

            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "未成交原因不能为空！";
                return result;
            }

            if (dto.Content.Length >= 500)
            {
                result.Message = "未成交原因不能超过500！";
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
                result.Data = await _connection.ExecuteAsync(@"update [SmartFailture] set [CategoryID]=@CategoryID,[Content]=@Content where [ID]=@ID and CustomerID=@CustomerID", dto);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 添加未成交
        /// </summary>
        /// <param name="dto">未成交信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(FailtureAddUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.CategoryID == 0)
            {
                result.Message = "请选择未成交类型！";
                return result;
            }

            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "未成交原因不能为空！";
                return result;
            }

            if (dto.Content.Length > 500)
            {
                result.Message = "未成交原因不能超过500！";
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
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                result.Data = await _connection.ExecuteAsync(@"insert into [SmartFailture]([ID],[CustomerID],[CreateUserID],[CreateTime],[Content],[CategoryID]) 
                                         values(@ID,@CustomerID,@CreateUserID,@CreateTime,@Content,@CategoryID)",
                                         new
                                         {
                                             ID = id,
                                             CustomerID = dto.CustomerID,
                                             CreateUserID = dto.CreateUserID,
                                             CreateTime = DateTime.Now,
                                             CategoryID = dto.CategoryID,
                                             Content = dto.Content
                                         });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 未成交删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(FailtureDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                result.Data = await _connection.ExecuteAsync(@"delete from [SmartFailture] where [ID]=@ID and CustomerID=@CustomerID", dto);

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
    }
}
