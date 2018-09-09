using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using Com.JinYiWei.Common.DataAccess;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 库存盘点业务逻辑
    /// </summary>
    public class CheckService : BaseService, ICheckService
    {
        /// <summary>
        /// 添加盘点信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(CheckAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            ///判断DTO是否为空
            if (string.IsNullOrWhiteSpace(dto.WarehouseID)|| dto.WarehouseID=="-1")
            {
                result.Message = "仓库不能为空!";
                return result;
            }
            if (string.IsNullOrWhiteSpace(dto.CreateDate))
            {
                result.Message = "日期不能为空!";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字符！";
                return result;
            }

            if (dto.CheckDetailAdd==null||dto.CheckDetailAdd.Count <= 0)
            {
                result.Message = "盘点详细不可为空!";
                return result;
            }

            #endregion

            TryTransaction(() =>
            {
                //通过创建人ID查询一条数据
                #region 验证用户
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();
                //判断是否有数据
                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }
                #endregion
                SmartStockService smartStockService = new SmartStockService();

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                _connection.Execute(@"insert into SmartCheck(ID,WarehouseID,CreateUserID,CreateTime,Status,Remark,No,CreateDate) 
VALUES(@ID, @WarehouseID, @CreateUserID, @CreateTime, @Status, @Remark, @No, @CreateDate)",
                 new { ID = id, WarehouseID = dto.WarehouseID, CreateUserID = dto.CreateUserID, CreateTime = DateTime.Now, Status = dto.Status, Remark = dto.Remark, No = KCAutoNumber.Instance().CKNumber("PD"), CreateDate = dto.CreateDate }, _transaction);  //盘点记录表

                foreach (var u in dto.CheckDetailAdd)
                {
                    _connection.Execute("insert into SmartCheckDetail(ID,CheckID,ProductID,Status,Num,Price,Amount)values(@ID, @CheckID, @ProductID,@Status, @Num, @Price, @Amount)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CheckID = id,
                            ProductID = u.CheckProductId,
                            Status = u.Status,
                            Num = u.Num,
                            Price = u.Price,
                            Amount = u.Num * Convert.ToDouble(u.Price)
                        }, _transaction); //盘点记录详情表
                  
                        if (u.Status == "0")
                        {//0盘盈，新增一条库存记录
                            _connection.Execute(@"insert into SmartStock(ID,WarehouseID,ProductID,Num,Price,Amount,DetailID,Type,Batch,Expiration)
VALUES(@ID, @WarehouseID, @ProductID,@Num, @Price, @Amount, @DetailID,@Type,@Batch,@Expiration)",
                             new
                             {
                                 ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                 WarehouseID = dto.WarehouseID,
                                 ProductID = u.CheckProductId,
                                 Num = u.Num,
                                 Price = u.Price,
                                 Amount = u.Num * Convert.ToDouble(u.Price),
                                 DetailID = id,
                                 Type = 3,
                                 Batch=u.Batch,
                                 Expiration=u.Expiration

                             }, _transaction); //更新此批进货记录库存
                        }
                        else
                        { //等于1 是盘亏，减少此条药物品的库存数量

                            var stockInfo = smartStockService.GetByID(Convert.ToInt64(u.StockId));//得到库存表中的此批进货信息
                            int kcNum = stockInfo.Data.Num;//此批进货药物品库存数量
                            int NewNum = kcNum - u.Num;//库存数量 减去此批药品要盘亏的数量等于最新的数量
                            if (NewNum == 0)
                            {
                                _connection.Execute("DELETE SmartStock WHERE ID=@ID",
                               new
                               {
                                   ID = u.StockId
                               }, _transaction); //如果库存数量等于0了，删除此批进货记录库存
                            }
                            else
                            {
                                _connection.Execute("UPDATE SmartStock SET Num=@Num WHERE ID=@ID",
                        new
                        {
                            Num = NewNum,
                            ID = stockInfo.Data.ID
                        }, _transaction); //更新此批进货记录库存
                            }
                        }
                };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CheckAdd,
                    Remark = LogType.CheckAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;

        }

        /// <summary>
        /// 查询全部盘盈盘亏记录 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CheckInfo>>> Get(CheckSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CheckInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<CheckInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"
SELECT sc.ID,sc.Status,sc.No,Convert(varchar(30),sc.CreateDate,23) as CreateDate,sc.WarehouseID,sw.Name AS WarehouseName,sc.CreateUserID,su.Name AS CreateUserName,sc.Remark FROM SmartCheck AS sc 
LEFT JOIN dbo.SmartWarehouse AS sw ON sc.WarehouseID=sw.ID LEFT JOIN dbo.SmartUser AS su ON sc.CreateUserID=su.ID WHERE 1=1";

                sql2 = @"SELECT COUNT(sc.ID) AS Count FROM SmartCheck AS sc 
LEFT JOIN dbo.SmartWarehouse AS sw ON sc.WarehouseID=sw.ID LEFT JOIN dbo.SmartUser AS su ON sc.CreateUserID=su.ID WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(dto.WarehouseID)&& dto.WarehouseID!="-1")
                {
                    sql += @" And sc.WarehouseID=" + dto.WarehouseID + "";
                    sql2 += @" And sc.WarehouseID=" + dto.WarehouseID + "";
                }

                //" ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only
                if (!string.IsNullOrWhiteSpace(dto.BeginDate) && !string.IsNullOrWhiteSpace(dto.EndDate))
                {
                    string entTime = dto.EndDate + " 23:59:59";
                    sql += @" And sc.CreateDate between '" + dto.BeginDate + "' and '" + entTime + "'";
                    sql2 += @" And sc.CreateDate between '" + dto.BeginDate + "' and '" + entTime + "'";
                }

                if (!string.IsNullOrWhiteSpace(dto.No))
                {
                    sql += " And sc.No='" + dto.No + "'";
                    sql2 += @" And sc.No='" + dto.No + "'";
                }
                sql2 += " AND sc.CreateUserID=" + dto.CreateUserId + "";
                sql += " AND sc.CreateUserID="+dto.CreateUserId+"";
                sql += " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<CheckInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询库存盘点详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CheckInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CheckInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<CheckInfo>(@"select ID,WarehouseID,CreateUserID,CreateTime,Status,Remark,No,Convert(varchar(30),CreateDate,23) AS CreateDate from SmartCheck where ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.CheckDetailAdd = new List<CheckDetailAdd>();
                result.Data.CheckDetailAdd = _connection.Query<CheckDetailAdd>(@"SELECT scd.ID,scd.CheckID,scd.ProductID,sp.Name AS ProductName,sp.Size,su.Name AS KcName,scd.Num,scd.Price,scd.Amount,scd.Status FROM SmartCheckDetail AS scd 
LEFT JOIN dbo.SmartProduct AS sp ON scd.ProductID = sp.ID
LEFT JOIN dbo.SmartUnit AS su ON sp.UnitID = su.ID WHERE scd.CheckID=@CheckID", new { CheckID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            return result;
        }

        public IFlyDogResult<IFlyDogResultType, int> Update(CheckUpdate dto)
        {
            throw new NotImplementedException();
        }
    }
}
