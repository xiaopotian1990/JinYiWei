using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class PhotoService : BaseService, IPhotoService
    {
        /// <summary>
        /// 图片批量上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> BatchAdd(BatchPhotoAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Type == PhotoType.Consult)
            {
                if (dto.SymptomID == null || dto.SymptomID == 0)
                {
                    result.Message = "请选择咨询项目！";
                    return result;
                }
            }

            if (dto.Type == PhotoType.Before || dto.Type == PhotoType.Under || dto.Type == PhotoType.After)
            {
                if (dto.ChargeID == null || dto.ChargeID == 0)
                {
                    result.Message = "请选择治疗项目！";
                    return result;
                }
            }

            if (dto.Images == null || dto.Images.Count() == 0)
            {
                result.Message = "请先上传图片！";
                return result;
            }

            if (!string.IsNullOrWhiteSpace(dto.Remark)&&dto.Remark.Length>200) {
                result.Message = "备注描述不能超过200个字符！";
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

                result.Data = await _connection.ExecuteAsync(
                    @"insert into [SmartPhoto]([ID],[CustomerID],[CreateUserID],[CreateTime],[ChargeID],[ImageUrl],[Remark],[SymptomID],[Type],[ReducedImage]) 
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@ChargeID,@ImageUrl,@Remark,@SymptomID,@Type,@ReducedImage)",
                    dto.Images.Select(u => new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = dto.CreateTime.Date,
                        ChargeID = dto.ChargeID,
                        ImageUrl = u.BigImage,
                        Remark = dto.Remark,
                        SymptomID = dto.SymptomID,
                        Type = dto.Type,
                        ReducedImage = u.ReducedImage
                    }), _transaction, 30, System.Data.CommandType.Text);

                result.ResultType = IFlyDogResultType.Success;
                result.Message = "上传成功！";
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询顾客照片详细
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, CustomerPhoto>> GetByCustomerID(long userID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CustomerPhoto>();

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(userID, customerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }
                var temp = await _connection.QueryAsync<Photo>(
                    @"SELECT a.[ID],[CustomerID],d.Name as CreateUserName,a.[CreateTime],b.Name as ChargeName,[ImageUrl],a.[Remark],c.Name as SymptomName,[Type],[ReducedImage] 
                    FROM [SmartPhoto] a
                    left join SmartCharge b on a.ChargeID=b.ID
                    left join SmartSymptom c on a.SymptomID=c.ID
					left join SmartUser d on a.CreateUserID=d.ID
                    where CustomerID=@CustomerID", new { CustomerID = customerID });

                CustomerPhoto photo = new CustomerPhoto();
                foreach (var u in temp)
                {
                    if (u.Type == PhotoType.Consult)
                    {
                        photo.Consult.Add(u);
                    }
                    else if (u.Type == PhotoType.Before)
                    {
                        photo.Before.Add(u);
                    }
                    else if (u.Type == PhotoType.Under)
                    {
                        photo.Under.Add(u);
                    }
                    else if (u.Type == PhotoType.After)
                    {
                        photo.After.Add(u);
                    }
                    else if (u.Type == PhotoType.Other)
                    {
                        photo.Other.Add(u);
                    }
                }

                result.Data = photo;
                result.ResultType = IFlyDogResultType.Success;
                result.Message = "查询成功";
            });

            return result;
        }

        /// <summary>
        /// 图片批量上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(PhotoDelete dto)
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
                    @"delete from [SmartPhoto] where ID=@ID and [CustomerID]=@CustomerID", dto);

                result.ResultType = IFlyDogResultType.Success;
                result.Message = "删除成功！";
            });

            return result;
        }
    }
}
