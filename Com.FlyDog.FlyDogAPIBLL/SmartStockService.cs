using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Dapper;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class SmartStockService : BaseService, ISmartStockService
    {
        /// <summary>
        /// 新增库存
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartStockAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 开启事物操作
            TryTransaction(() =>
            {
                #region 判断用户权限
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }
                #endregion

                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute("insert into SmartStock(ID,WarehouseID,ProductID,Num,Price,Amount,DetailID,Type)values(@ID, @WarehouseID, @ProductID, @Num, @Price, @Amount, @DetailID, @Type))",
                    new { ID = id, WarehouseID = dto.WarehouseID, ProductID = dto.ProductID, Num = dto.Num, Price = dto.Price, Amount = dto.Amount, DetailID = dto.DetailID, Type = dto.Type }, _transaction);

                var temp = new { 编号 = result.Data, WarehouseID = dto.WarehouseID, ProductID = dto.ProductID, Num = dto.Num, Price = dto.Price, Amount = dto.Amount, DetailID = dto.DetailID, Type = dto.Type };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartStockAdd,
                    Remark = LogType.SmartStockAdd.ToDescription() + temp.ToJsonString()
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
        /// 查询当前医院的库存信息[  sql语句还可以优化写法，现在先用这样，有时间了在优化]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartStockInfo>> Get(SmartStockSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartStockInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                string warehouseID = string.Empty;
                if (!string.IsNullOrWhiteSpace(dto.WarehouseID) && dto.WarehouseID != "-1")
                {

                }

                string sql = @"SELECT ss.ID, ss.ID AS StockId, ss.WarehouseID,sw.Name AS WarehouseName,ss.ProductID,sp.Name AS WPName ,sp.Size,sp.PinYin,sp.CategoryID,spc.Name AS ProductCategoryName,su.Name AS UnitName,ss.Num,ss.Amount,ss.Price,ss.Batch,ss.Expiration FROM SmartStock AS ss
                                LEFT JOIN dbo.SmartWarehouse AS sw ON ss.WarehouseID=sw.ID
                                LEFT JOIN dbo.SmartProduct AS sp ON ss.ProductID=sp.ID
                                LEFT JOIN dbo.SmartProductCategory AS spc ON sp.CategoryID=spc.ID
                                LEFT JOIN dbo.SmartUnit AS su ON sp.UnitID=su.ID
                                WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(dto.PinYin))
                {

                    sql += " AND sp.PinYin LIKE '%" + dto.PinYin + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.WPName))
                {
                    sql += @" AND sp.Name LIKE '%" + dto.WPName + "%'";
                }


                if (!string.IsNullOrWhiteSpace(dto.WarehouseID) && dto.WarehouseID != "-1")
                {
                    sql += @" AND ss.WarehouseID=" + dto.WarehouseID + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.CategoryID) && dto.CategoryID != "-1")
                {
                    sql += @" AND sp.CategoryID=" + dto.CategoryID + "";
                }

                sql += " AND ss.WarehouseID IN(SELECT WarehouseID FROM dbo.SmartWarehouseManager WHERE UserID=" + dto.CreateUserId + ")";
                sql += @" GROUP BY ss.WarehouseID,ss.ID,sw.Name,ss.ProductID,sp.Name
,sp.Size,sp.PinYin,sp.CategoryID,spc.Name,su.Name,ss.Num,ss.Amount,ss.Price,ss.Batch,ss.Expiration";
                sql += " ORDER BY ss.Expiration,sp.Name";
                result.Data = _connection.Query<SmartStockInfo>(sql);
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据条件查询库存药物品信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartStockProductInfo>> GetByCondition(SmartStockSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartStockProductInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                string sql = @"SELECT ss.ID AS StockId,sp.ID as ProductID,sp.Name AS ProductName,sp.PinYin,sp.Size,spc.ID AS CategoryID,spc.Name AS ProductCategoryName,
su.Name AS UnitName,ss.Num,ss.Price,ss.Amount,ss.Batch,Convert(varchar(30),ss.Expiration,23) AS Expiration FROM dbo.SmartStock AS ss 
LEFT JOIN dbo.SmartProduct AS sp ON ss.ProductID=sp.ID
 LEFT JOIN dbo.SmartProductCategory AS spc ON sp.CategoryID=spc.ID 
 LEFT JOIN dbo.SmartUnit AS su ON sp.UnitID=su.ID 
  WHERE 1 = 1";

                if (!string.IsNullOrWhiteSpace(dto.PinYin))
                {
                    sql += @" AND sp.PinYin LIKE '%" + dto.PinYin + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.WPName))
                {
                    sql += @" AND sp.Name LIKE '%" + dto.WPName + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.CategoryID) && dto.CategoryID != "-1")
                {
                    sql += @" AND sp.CategoryID =" + dto.CategoryID + "";
                }

                sql += @" AND ss.WarehouseID=" + dto.WarehouseID + "";
                sql += " ORDER BY ss.Expiration,sp.Name";
                //, new { WarehouseID= dto.WarehouseID, Type= dto.Type }
                result.Data = _connection.Query<SmartStockProductInfo>(sql);
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据医院id查询库存商品，按照有效期排序
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartStockInfo>>> GetByHospitalIDData(SmartStockSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartStockInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<SmartStockInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @" SELECT ss.ID,ss.WarehouseID,sw.Name AS WarehouseName,sp.Name AS ProductName,ss.num,ss.Price,ss.Amount,ss.Batch,Convert(varchar(30),ss.Expiration,23) AS Expiration FROM dbo.SmartStock AS ss
                    left JOIN dbo.SmartWarehouse AS sw ON ss.WarehouseID=sw.ID
                    left JOIN dbo.SmartProduct AS sp ON ss.ProductID=sp.ID WHERE 1=1 ";

                sql2 = @"SELECT COUNT(ss.ID) AS Count FROM dbo.SmartStock AS ss
                        left JOIN dbo.SmartWarehouse AS sw ON ss.WarehouseID = sw.ID
                        left JOIN dbo.SmartProduct AS sp ON ss.ProductID = sp.ID WHERE 1 = 1  ";

                if (!string.IsNullOrWhiteSpace(dto.BeginTime) && !string.IsNullOrWhiteSpace(dto.EndTime))
                {
                    sql += @" And ss.Expiration between '" + dto.BeginTime + "' and '" + dto.EndTime + "'";
                    sql2 += @" And ss.Expiration between '" + dto.BeginTime + "' and '" + dto.EndTime + "'";
                }
                else
                {//如果等于空，说明用户没有选择时间，则默认查询距离当前日期30天以内的数据
                    sql += @" And ss.Expiration between '" + DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                    sql2 += @" And ss.Expiration between '" + DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd") + "' and '" + DateTime.Now.ToString("yyyy-MM-dd") + "'";
                }

                sql += " AND sw.HospitalID=" + dto.HospitalID + "";
                sql2 += " AND sw.HospitalID=" + dto.HospitalID + "";
                sql += " ORDER by ss.Expiration  OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<SmartStockInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        ///  根据库存id来查询库存详情
        /// </summary>
        /// <param name="no"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartStockInfo> GetByID(long Id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartStockInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartStockInfo>("SELECT ID,WarehouseID,ProductID,Num,Price,Amount,DetailID,Type,Batch,Convert(varchar(30),Expiration,23) AS Expiration FROM dbo.SmartStock WHERE ID=@ID", new { ID = Id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        public IFlyDogResult<IFlyDogResultType, int> Update(SmartStockUpdate dto)
        {
            throw new NotImplementedException();
        }
    }
}
