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
    /// 仓库调拨业务逻辑
    /// </summary>
    public class AllocateService : BaseService, IAllocateService
    {

        private IHospitalPrintService _hospitalPrintService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="hospitalPrintService"></param>
        public AllocateService(IHospitalPrintService hospitalPrintService)
        {
            _hospitalPrintService = hospitalPrintService;
        }
        #endregion
        /// <summary>
        /// 添加仓库调拨
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(AllocateAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            ///判断DTO是否为空
            if (string.IsNullOrWhiteSpace(dto.FromWarehouseID) && dto.FromWarehouseID != "-1")
            {
                result.Message = "请选择调出仓库!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.ToWarehouseID) && dto.ToWarehouseID != "-1")
            {
                result.Message = "请选择调入仓库!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.DoUserID))
            {
                result.Message = "请选择申请人!";
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

            if (dto.AllocateDetailAdd == null || dto.AllocateDetailAdd.Count <= 0)
            {
                result.Message = "调拨详情不能为空!";
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
                _connection.Execute(@"insert into SmartAllocate(ID,CreateUserID,CreateTime,FromWarehouseID,ToWarehouseID,Status,Remark,No,CreateDate,DoUserID) 
VALUES(@ID, @CreateUserID,@CreateTime, @FromWarehouseID, @ToWarehouseID, @Status, @Remark, @No, @CreateDate,@DoUserID)",
                 new { ID = id, CreateUserID = dto.CreateUserID, CreateTime = DateTime.Now, FromWarehouseID = dto.FromWarehouseID, ToWarehouseID = dto.ToWarehouseID, Status = dto.Status, Remark = dto.Remark, No = KCAutoNumber.Instance().CKNumber("DB"), CreateDate = dto.CreateDate, DoUserID = dto.DoUserID }, _transaction);  //调拨记录表

                foreach (var u in dto.AllocateDetailAdd)
                {
                    _connection.Execute("insert into SmartAllocateDetail(ID,AllocateID,ProductID,Num,Price,Amount) VALUES(@ID, @AllocateID, @ProductID, @Num, @Price, @Amount)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            AllocateID = id,
                            ProductID = u.ProductID,
                            Num = u.Num,
                            Price = u.Price,
                            Amount = u.Num * Convert.ToDouble(u.Price)
                        }, _transaction); //调拨记录详情表

                    _connection.Execute(@"insert into SmartStock(ID,WarehouseID,ProductID,Num,Price,Amount,DetailID,Type,Batch,Expiration)
 values(@ID, @WarehouseID, @ProductID, @Num, @Price, @Amount, @DetailID, @Type, @Batch, @Expiration)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            WarehouseID = dto.ToWarehouseID,//调入仓库id
                            ProductID = u.ProductID,
                            Num = u.Num,
                            Price = u.Price,
                            Amount = u.Num * Convert.ToDouble(u.Price),
                            DetailID = id, //具体的调拨记录id
                            Type = 2, //类型1：入库2：调拨3：盘点4：科室领用
                            Batch = u.Batch,
                            Expiration = u.Expiration
                        }, _transaction); //库存表
                    if (dto.Status != "0")
                    {//暂存状态，不存库存表，只有调拨状态才会存库存表
                        var stockInfo = smartStockService.GetByID(Convert.ToInt64(u.StockId));//得到库存表中的此批进货信息

                        if (stockInfo != null)
                        {
                            int kcNum = stockInfo.Data.Num;//此批进货药物品库存数量
                            int NewNum = kcNum - u.Num;//库存数量 减去此批药品要调出的数量等于最新的数量
                            if (NewNum == 0)
                            {//如果减完之后库存数量等于0了，就删除此条库存了
                                _connection.Execute("DELETE SmartStock WHERE ID=@ID",
                         new
                         {
                             ID = u.StockId
                         }, _transaction); //更新此批进货记录库存
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
                    }
                };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartReturnAdd,
                    Remark = LogType.SmartReturnAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;

                return true;
            });

            return result;
        }

        /// <summary>
        /// 获取所有调拨信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AllocateInfo>>> Get(AllocateSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AllocateInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<AllocateInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"
                    SELECT sa.ID,sa.Status,sa.No,Convert(varchar(30),sa.CreateDate,23) AS CreateDate,sa.FromWarehouseID,sw.Name AS FromWarehouseName,sa.ToWarehouseID,swh.Name AS ToWarehouseName,sus.Name AS DoUserName,su.Name AS CreateUserName,sa.Remark,sa.DoUserID FROM SmartAllocate AS sa
                     left JOIN dbo.SmartUser AS su ON sa.CreateUserID=su.ID 
                     left JOIN SmartUser AS sus ON sa.DoUserID=sus.ID
                    left JOIN dbo.SmartWarehouse AS sw ON sa.FromWarehouseID=sw.ID 
                    left JOIN SmartWarehouse AS swh ON sa.ToWarehouseID=swh.ID WHERE 1=1";

                sql2 = @"SELECT COUNT(sa.ID) AS Count FROM SmartAllocate AS sa
                         left JOIN dbo.SmartUser AS su ON sa.CreateUserID=su.ID left JOIN SmartUser AS sus ON sa.DoUserID=sus.ID
                        left JOIN dbo.SmartWarehouse AS sw ON sa.FromWarehouseID=sw.ID left JOIN SmartWarehouse AS swh ON sa.ToWarehouseID=swh.ID WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(dto.FromWarehouseID) && dto.FromWarehouseID != "-1")
                {
                    sql += @" And sa.FromWarehouseID=" + dto.FromWarehouseID + "";
                    sql2 += @" And sa.FromWarehouseID=" + dto.FromWarehouseID + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.ToWarehouseID) && dto.ToWarehouseID != "-1")
                {
                    sql += @" And sa.ToWarehouseID=" + dto.ToWarehouseID + "";
                    sql2 += @" And sa.ToWarehouseID=" + dto.ToWarehouseID + "";
                }
                //" ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only
                if (!string.IsNullOrWhiteSpace(dto.BeginDate) && !string.IsNullOrWhiteSpace(dto.EndDate))
                {
                    string entTime = dto.EndDate + " 23:59:59";
                    sql += @" And sa.CreateDate between '" + dto.BeginDate + "' and '" + entTime + "'";
                    sql2 += @" And sa.CreateDate between '" + dto.BeginDate + "' and '" + entTime + "'";
                }

                if (!string.IsNullOrWhiteSpace(dto.No))
                {
                    sql += " And sa.No='" + dto.No + "'";
                    sql2 += @" And sa.No='" + dto.No + "'";
                }

                sql2 += "AND sa.CreateUserID=" + dto.CreateUserId + "";
                sql += " AND sa.CreateUserID=" + dto.CreateUserId + "";
                sql += " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<AllocateInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询调拨详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, AllocateInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, AllocateInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<AllocateInfo>(@"select ID,CreateUserID,Convert(varchar(30),CreateTime,23) AS CreateTime,FromWarehouseID,ToWarehouseID,Status,Remark,No,Convert(varchar(30),CreateDate,23) AS CreateDate,DoUserID from SmartAllocate where ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.AllocateDetail = new List<AllocateDetailAdd>();
                result.Data.AllocateDetail = _connection.Query<AllocateDetailAdd>(@" SELECT sad.ID,sad.AllocateID,sp.ID AS ProductID,sp.Name AS ProductName,sp.Size,su.Name AS KcName,sad.Num,sad.Price,sad.Amount
 FROM SmartAllocateDetail AS sad INNER JOIN SmartProduct AS sp ON sad.ProductID = sp.ID
 INNER JOIN dbo.SmartUnit AS su ON sp.UnitID = su.ID WHERE sad.AllocateID=@AllocateID", new { AllocateID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        public IFlyDogResult<IFlyDogResultType, int> SmartReturnDelete(AllocateDelete dto)
        {
            throw new NotImplementedException();
        }

        public IFlyDogResult<IFlyDogResultType, int> Update(AllocateUpdate dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 根据仓库进货id查询详情打印
        /// </summary>
        /// <param name="returnID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, string> SmartAllocatePrint(string allocateID, long hospitalID) {
            var result = new IFlyDogResult<IFlyDogResultType, string>();
            result.ResultType = IFlyDogResultType.Failed;

            var returnData = this.GetByID(Convert.ToInt64(allocateID));  //得到仓库调拨详情数据
            var hospitalPrint = _hospitalPrintService.GetByHospitalAndType(hospitalID, "11");//得到仓库调拨模板 
            if (returnData.Data!=null&& hospitalPrint.Data!=null) {

                string table = string.Empty;
                string allocateDetailTr = string.Empty;
                for (var i = 0; i < returnData.Data.AllocateDetail.Count; i++)
                {
                    allocateDetailTr += "<tr value=" +
                    returnData.Data.AllocateDetail[i].ProductID +
                    ">" +
                    "<td>" +
                    returnData.Data.AllocateDetail[i].ProductName +
                    "</td>" +
                    "<td>" +
                    returnData.Data.AllocateDetail[i].Size +
                    "</td>" +
                    "<td>" +
                    returnData.Data.AllocateDetail[i].KcName +
                    "</td>" +
                    "<td>" +
                    returnData.Data.AllocateDetail[i].Num +
                    "</td>" +
                    "</td>" +
                    "<td>" +
                    returnData.Data.AllocateDetail[i].Price +
                    "</td>" +
                    "<td>" +
                    returnData.Data.AllocateDetail[i].Amount +
                    "</td>" +
                    "<td hidden='hidden' id='smartWarehouseRemarkTdhidden'>" +
                    returnData.Data.AllocateDetail[i].ID +
                    "</td>" +
                    "</tr>";
                }

                table += "<table class='site-table table-hover' style='width:" + hospitalPrint.Data.Width + "px;font-size:" + hospitalPrint.Data.FontSize + "px;font-family:" + hospitalPrint.Data.FontFamily + ";'>";
                table += "<thead><tr><th>名称</th><th>规格</th><th>单位</th><th>数量</th><th>进价</th><th>金额</th></tr></thead>";
                table += "<tbody id='smartAllocateDetailTD'>";
                table += allocateDetailTr;
                table += "</tbody>";
                table += "</table>";

                string printStr = hospitalPrint.Data.Content.ToString();
                printStr= printStr.Replace("$FromWarehouseName", returnData.Data.FromWarehouseName);
                printStr= printStr.Replace("$ToWarehouseName", returnData.Data.ToWarehouseName);
                printStr= printStr.Replace("$No", returnData.Data.No);
                printStr= printStr.Replace("$CreateDate", returnData.Data.CreateDate);
                printStr=printStr.Replace("$Table", table);
                printStr= printStr.Replace("$CreateUserName", returnData.Data.CreateUserName);

                result.Data = printStr;
            }
            else
            {
                result.Message = "打印数据查询出现异常!";
            }

            return result;
        }
    }
}
