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
    /// 回款记录业务处理
    /// </summary>
    public class SaleBackService : BaseService, ISaleBackService
    {
        /// <summary>
        /// 新增回款记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SaleBackAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            StoreService ss = new StoreService();
            var storeCount = ss.GetByHCCount(dto.StoreID, dto.CreateUserID);

            if (storeCount.Data == 0)
            {
                result.Message = "您不是该店铺管理员，不能操作该店铺！";
                return result;
            }

            #region 数据验证
            if (dto.CreateDate.IsNullOrEmpty())
            {
                result.Message = "回款日期不能为空！";
                return result;
            }

            if (dto.Amount.IsNullOrEmpty())
            {
                result.Message = "回款金额不能为空！";
                return result;
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {

                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute(@"insert into SmartSaleBack(ID,HospitalID,StoreID,CreateTime,CreateUserID,CreateDate,Amount,Remark) 
                   VALUES(@ID, @HospitalID, @StoreID, @CreateTime, @CreateUserID, @CreateDate, @Amount, @Remark)",
                    new { ID = id, HospitalID = dto.HospitalID, StoreID =dto.StoreID, CreateTime=DateTime.Now, CreateUserID =dto.CreateUserID, CreateDate=dto.CreateDate, Amount=dto.Amount, Remark=dto.Remark }, _transaction);

                var temp = new { 编号 = result.Data};
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    Type = LogType.SaleBackAdd,
                    Remark = LogType.SaleBackAdd.ToDescription() + temp.ToJsonString()
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
        /// 删除回款记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(SaleBackDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            StoreService ss = new StoreService();
            var storeCount = ss.GetByHCCount(dto.StoreID, dto.CreateUserID);

            if (storeCount.Data == 0)
            {
                result.Message = "您不是该店铺管理员，不能操作该店铺！";
                return result;
            }

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("DELETE SmartSaleBack WHERE ID=@ID", dto, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    Type = LogType.SaleBackDelete,
                    Remark = LogType.SaleBackDelete.ToDescription() + temp.ToJsonString()
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
        /// 查询所有回款记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SaleBackInfo>>> Get(SaleBackSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SaleBackInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<SaleBackInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT ssb.ID,ssb.HospitalID,ssb.StoreID,ss.Name AS StoreName,ssb.CreateDate,ssb.Amount,su.Name AS CreateUserName,ssb.CreateTime,ssb.Remark FROM dbo.SmartSaleBack AS ssb
                    LEFT JOIN dbo.SmartStore AS ss ON ssb.StoreID=ss.ID
                    LEFT JOIN dbo.SmartUser AS su ON ssb.CreateUserID=su.ID
                    LEFT JOIN dbo.SmartStoreManager AS ssm ON ss.ID=ssm.StoreID
                    WHERE 1=1";

                sql2 = @"SELECT COUNT(ssb.ID) AS Count FROM dbo.SmartSaleBack AS ssb
                            LEFT JOIN dbo.SmartStore AS ss ON ssb.StoreID=ss.ID
                            LEFT JOIN dbo.SmartUser AS su ON ssb.CreateUserID=su.ID
                            LEFT JOIN dbo.SmartStoreManager AS ssm ON ss.ID=ssm.StoreID
                            WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(dto.BeginTime) && !string.IsNullOrWhiteSpace(dto.EndTime))
                {
                    string entTime = dto.EndTime.ToString().Replace(" 0:00:00", " 23:59:59");
                    sql += @" And ssb.CreateDate between '" + dto.BeginTime + "' and '" + entTime + "'";
                    sql2 += @" And ssb.CreateDate between '" + dto.BeginTime + "' and '" + entTime + "'";
                }

                if (!string.IsNullOrWhiteSpace(dto.StoreName))
                {
                    sql += @" AND ss.Name LIKE '%" + dto.StoreName + "%'";
                    sql2 += @" AND ss.Name LIKE '%" + dto.StoreName + "%'";
                }

                sql += " And ssb.HospitalID='" + dto.HospitalID + "'";
                sql += " AND ssm.UserID='"+dto.CreateUserID+"'";
                sql += " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<SaleBackInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询回款记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SaleBackInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SaleBackInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SaleBackInfo>("SELECT ID,HospitalID,StoreID,CreateTime,CreateUserID,CreateDate,Amount,Remark FROM SmartSaleBack WHERE ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新回款记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SaleBackUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            StoreService ss = new StoreService();
            var storeCount = ss.GetByHCCount(dto.StoreID, dto.CreateUserID);

            if (storeCount.Data == 0)
            {
                result.Message = "您不是该店铺管理员，不能操作该店铺！";
                return result;
            }

            #region 数据验证
            if (dto.CreateDate.IsNullOrEmpty())
            {
                result.Message = "回款日期不能为空！";
                return result;
            }

            if (dto.Amount.IsNullOrEmpty())
            {
                result.Message = "回款金额不能为空！";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("update SmartSaleBack set CreateDate = @CreateDate,Amount=@Amount,Remark=@Remark where ID = @ID", new { CreateDate=dto.CreateDate, Amount=dto.Amount, Remark=dto.Remark,ID=dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID, 回款日期 = dto.CreateDate,回款金额=dto.Amount };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    Type = LogType.SaleBackUpdate,
                    Remark = LogType.SaleBackUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
