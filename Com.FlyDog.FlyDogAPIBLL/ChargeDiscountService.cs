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

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 项目折扣业务处理
    /// </summary>
    public class ChargeDiscountService : BaseService, IChargeDiscountService
    {
        /// <summary>
        /// 添加项目折扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(ChargeDiscountAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            if (dto.Discount.IsNullOrEmpty())
            {
                result.Message = "项目折扣不能为空！";
                return result;
            }

            if (dto.StartTime.IsNullOrEmpty())
            {
                result.Message = "开始时间不能为空！";
                return result;
            }

            if (dto.EndTime.IsNullOrEmpty())
            {
                result.Message = "结束时间不能为空！";
                return result;
            }

            #region 开启事物操作
            TryTransaction(() =>
            {

                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute(@"insert into SmartChargeDiscount(ID,ScopeLimit,ChargeID,ChargeCategoryID,Discount,StartTime,EndTime,Status,HospitalID) 
VALUES(@ID, @ScopeLimit, @ChargeID, @ChargeCategoryID, @Discount, @StartTime, @EndTime, @Status,@HospitalID)",
                    new { ID = id, ScopeLimit = dto.ScopeLimit, ChargeID = dto.ScopeLimit == "2" ? dto.ChargeID : "0", ChargeCategoryID = dto.ScopeLimit == "1" ? dto.ChargeCategoryID : "0", Discount = dto.Discount, StartTime = dto.StartTime, EndTime = dto.EndTime, Status = dto.Status, HospitalID=dto.HospitalID }, _transaction);

                var temp = new { 编号 = result.Data };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.ChargeDiscountAdd,
                    Remark = LogType.ChargeDiscountAdd.ToDescription() + temp.ToJsonString()
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
        ///查询所有项目折扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeDiscountInfo>>> Get(ChargeDiscountSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeDiscountInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;
                result.Data = new Pages<IEnumerable<ChargeDiscountInfo>>();

                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT ID, ScopeLimit, 
                            (CASE ScopeLimit WHEN 0 THEN '所有项目'
                            WHEN 1 THEN(SELECT Name FROM dbo.SmartChargeCategory WHERE ID = ChargeCategoryID)
                            WHEN 2 THEN(SELECT Name FROM SmartCharge WHERE ID = ChargeID) END) AS ProjectName, ChargeID, ChargeCategoryID, Discount, Convert(varchar(30),StartTime,23) AS StartTime,Convert(varchar(30),EndTime,23) AS EndTime, Status FROM dbo.SmartChargeDiscount WHERE HospitalID=@HospitalID
                         ORDER BY Status desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";
                sql2 = "SELECT COUNT(ID) FROM dbo.SmartChargeDiscount WHERE HospitalID=@HospitalID";

                result.Data.PageDatas = _connection.Query<ChargeDiscountInfo>(sql,new { HospitalID =dto.HospitalID});
                result.Data.PageTotals = _connection.Query<int>(sql2, new { HospitalID = dto.HospitalID }).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询项目折扣详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ChargeDiscountInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ChargeDiscountInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<ChargeDiscountInfo>(@"SELECT ID, ScopeLimit, ChargeID, ChargeCategoryID,(CASE ScopeLimit WHEN 0 THEN '所有项目'
                            WHEN 1 THEN(SELECT Name FROM dbo.SmartChargeCategory WHERE ID = ChargeCategoryID)
                            WHEN 2 THEN(SELECT Name FROM SmartCharge WHERE ID = ChargeID) END) AS ProjectName, 
							 Discount, 	Convert(varchar(30),StartTime,23) StartTime, 
							Convert(varchar(30),EndTime,23) EndTime, Status FROM dbo.SmartChargeDiscount where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 修改项目折扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(ChargeDiscountUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Discount.IsNullOrEmpty())
            {
                result.Message = "项目折扣不能为空！";
                return result;
            }

            if (dto.StartTime.IsNullOrEmpty())
            {
                result.Message = "开始时间不能为空！";
                return result;
            }

            if (dto.EndTime.IsNullOrEmpty())
            {
                result.Message = "结束时间不能为空！";
                return result;
            }

            TryTransaction(() =>
            {
                result.Data = _connection.Execute("UPDATE SmartChargeDiscount SET ScopeLimit=@ScopeLimit,ChargeID=@ChargeID,ChargeCategoryID=@ChargeCategoryID,Discount=@Discount,StartTime = @StartTime, EndTime = @EndTime, Status = @Status WHERE ID = @ID", new { ID = dto.ID, ScopeLimit = dto.ScopeLimit, ChargeID = dto.ScopeLimit == "2" ? dto.ChargeID : "0", ChargeCategoryID = dto.ScopeLimit == "1" ? dto.ChargeCategoryID : "0", Discount = dto.Discount, StartTime = dto.StartTime, EndTime = dto.EndTime, Status = dto.Status }, _transaction);

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.ChargeDiscountUpdate,
                    Remark = LogType.ChargeDiscountUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;

        }


        /// <summary>
        /// 获取项目折扣
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeDiscount>>> GetChargesDiscount(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeDiscount>>();
            result.ResultType = IFlyDogResultType.Success;
            result.Message = "查询成功";

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<ChargeDiscount>(
                    @"  with tree as 
                    ( 
                    select a.ID,b.Discount from [SmartChargeCategory] a,[SmartChargeDiscount] b where a.ID=b.ChargeCategoryID and ScopeLimit=1 and Status=1 and getdate() between [StartTime] and dateadd(day,1,[EndTime]) and HospitalID=@HospitalID
                    union all
                    select a.ID,b.Discount from SmartChargeCategory a,tree b where a.ParentID=b.ID
                    )
                    select [ChargeID]=0,Discount from [SmartChargeDiscount] where getdate() between [StartTime] and dateadd(day,1,[EndTime]) and ScopeLimit=1 and Status=1 and HospitalID=@HospitalID
                    union 
                    select a.ID,b.Discount from SmartCharge a,tree b where a.CategoryID=b.ID and a.Status=1 
                    union
                    select [ChargeID],Discount from [SmartChargeDiscount] where getdate() between [StartTime] and dateadd(day,1,[EndTime]) and ScopeLimit=2 and Status=1 and HospitalID=@HospitalID",
                    new { HospitalID = hospitalID });
            });

            return result;
        }
    }
}
