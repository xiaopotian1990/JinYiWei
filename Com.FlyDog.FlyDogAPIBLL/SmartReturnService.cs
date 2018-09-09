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
    /// <summary>
    /// 退货信息bll
    /// </summary>
    public class SmartReturnService : BaseService, ISmartReturnService
    {

        private IHospitalPrintService _hospitalPrintService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="hospitalPrintService"></param>
        public SmartReturnService(IHospitalPrintService hospitalPrintService)
        {
            _hospitalPrintService = hospitalPrintService;
        }
        #endregion

        /// <summary>
        /// 添加退货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartReturnAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            ///判断DTO是否为空
            if (string.IsNullOrWhiteSpace(dto.WarehouseID) || dto.WarehouseID == "-1")
            {
                result.Message = "仓库不能为空!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.SupplierID) || dto.SupplierID == "-1")
            {
                result.Message = "供应商不能为空!";
                return result;
            }
            if (string.IsNullOrWhiteSpace(dto.CreateTime))
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

            if (dto.SmartReturnDetail == null || dto.SmartReturnDetail.Count <= 0)
            {
                result.Message = "退货详细不可为空!";
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
                _connection.Execute("insert into SmartReturn(ID,SupplierID,WarehouseID,CreateUserID,CreateTime,Status,Remark,No,CreateDate) values(@ID, @SupplierID, @WarehouseID, @CreateUserID, @CreateTime, @Status, @Remark, @No, @CreateDate)",
                 new { ID = id, SupplierID = dto.SupplierID, WarehouseID = dto.WarehouseID, CreateUserID = dto.CreateUserID, CreateTime = dto.CreateTime, Status = dto.Status, Remark = dto.Remark, No = KCAutoNumber.Instance().CKNumber("TH"), CreateDate = dto.CreateDate }, _transaction);  //退货记录表

                foreach (var u in dto.SmartReturnDetail)
                {
                    _connection.Execute("insert into SmartReturnDetail(ID,ReturnID,ProductID,Num,Price,Amount) values(@ID, @ReturnID, @ProductID, @Num, @Price, @Amount)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            ReturnID = id,
                            ProductID = u.ProductID,
                            Num = u.Num,
                            Price = u.Price,
                            Amount = u.Num * Convert.ToDouble(u.Price)
                        }, _transaction); //退货记录详情表

                    var stockInfo = smartStockService.GetByID(Convert.ToInt64(u.StockId));//得到库存表中的此批进货信息
                    if (stockInfo != null)
                    {
                        int kcNum = stockInfo.Data.Num;//此批进货药物品库存数量
                        int NewNum = kcNum - u.Num;//库存数量 减去此批药品要退货的数量等于最新的数量
                        if (NewNum == 0)
                        {
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
                };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = Convert.ToInt64(dto.CreateUserID),
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
        /// 查询所有退货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartReturnInfo>>> Get(SmartReturnSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartReturnInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<SmartReturnInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"select sr.ID,sr.Status,sr.No,Convert(varchar(30),sr.CreateTime,23) AS CreateTime,sr.WarehouseID,sw.Name as WarehouseName,sr.SupplierID,ss.Name as SupplierName,sr.CreateUserID,su.Name as CreateUserName,sr.Remark from SmartReturn as sr left join SmartWarehouse as sw on sr.WarehouseID=sw.ID left join SmartSupplier as ss on sr.SupplierID=ss.ID 
                    left join SmartUser as su on sr.CreateUserID =su.ID where 1=1";

                sql2 = @"select COUNT(sr.ID) as Count from SmartReturn as sr left join SmartWarehouse as sw
                        on sr.WarehouseID=sw.ID left join SmartSupplier as ss on sr.SupplierID=ss.ID left join SmartUser as su 
                        on sr.CreateUserID =su.ID WHERE 1 = 1";

                if (!string.IsNullOrWhiteSpace(dto.WarehouseID) && dto.WarehouseID != "-1")
                {
                    sql += @" And sr.WarehouseID=" + dto.WarehouseID + "";
                    sql2 += @" And sr.WarehouseID=" + dto.WarehouseID + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.SupplierID) && dto.SupplierID != "-1")
                {
                    sql += @" And sr.SupplierID=" + dto.SupplierID + "";
                    sql2 += @" And sr.SupplierID=" + dto.SupplierID + "";
                }
                //" ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only
                if (!string.IsNullOrWhiteSpace(dto.BeginTime) && !string.IsNullOrWhiteSpace(dto.EndTime))
                {
                    string entTime = dto.EndTime + " 23:59:59";
                    sql += @" And sr.CreateTime between '" + dto.BeginTime + "' and '" + entTime + "'";
                    sql2 += @" And sr.CreateTime between '" + dto.BeginTime + "' and '" + entTime + "'";
                }

                if (!string.IsNullOrWhiteSpace(dto.No))
                {
                    sql += " And sr.No=" + dto.No + "";
                    sql2 += @" And sr.No=" + dto.No + "";
                }
                sql2 += " AND sr.CreateUserID=" + dto.CreateUserId + "";
                sql += " AND sr.CreateUserID=" + dto.CreateUserId + "";
                sql += " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<SmartReturnInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询退货信息详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartReturnInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartReturnInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartReturnInfo>("select ID,SupplierID,WarehouseID,CreateUserID,Convert(varchar(30),CreateTime,23) AS CreateTime,Status,Remark,No,CreateDate from SmartReturn where ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.SmartReturnDetail = new List<SmartReturnDetailAdd>();
                result.Data.SmartReturnDetail = _connection.Query<SmartReturnDetailAdd>(@"SELECT srd.ID,srd.ReturnID,srd.ProductID,sp.Name AS ProductName,sp.Size,su.Name AS KcName,srd.Num,srd.Price,srd.Amount
 FROM dbo.SmartReturnDetail AS srd left JOIN dbo.SmartProduct AS sp
ON srd.ProductID = sp.ID left JOIN dbo.SmartUnit AS su ON sp.UnitID = su.ID WHERE srd.ReturnID = @ReturnID", new { ReturnID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 删除退货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> SmartReturnDelete(SmartReturnDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var smartReturn = GetByID(Convert.ToInt64(dto.ReturnID));

            if (smartReturn.Data.Status == 1)
            {//说明此条退货信息状态是已经退货了，不能删除，只能删除那些退货状态为暂存的
                result.Message = "该单已退货!";
                return result;
            }

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 验证用户合法性
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();
                //判断是否有数据
                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }
                #endregion

                #region 开始更新操作

                int delDetail = _connection.Execute("delete SmartReturn where ID=@ID", new { ID = dto.ReturnID }, _transaction);
                if (delDetail > 0)
                {
                    _connection.Execute("delete SmartReturnDetail where ReturnID=@ReturnID", new { ReturnID = dto.ReturnID }, _transaction);
                }

                var temp = new { 编号 = dto.ReturnID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartReturnDelte,
                    Remark = LogType.SmartReturnDelte.ToDescription() + temp.ToJsonString()
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
        /// 根据仓库退货id查询仓库退货详情拼接成字符串打印出来
        /// </summary>
        /// <param name="returnID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, string> SmartReturnPrint(string returnID,long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, string>();
            result.ResultType = IFlyDogResultType.Failed;

            var returnData = this.GetByID(Convert.ToInt64(returnID));  //得到仓库调拨详情数据
            var hospitalPrint = _hospitalPrintService.GetByHospitalAndType(hospitalID,"10");//得到退货出库模板 

            if (returnData.Data != null && hospitalPrint.Data != null)
            {

                string table = string.Empty;
                string returnDetailTr = string.Empty;
                for (var i = 0; i < returnData.Data.SmartReturnDetail.Count; i++)
                {
                    returnDetailTr += "<tr value=" +
                         returnData.Data.SmartReturnDetail[i].ProductID +
                         ">" +
                         "<td>" +
                         returnData.Data.SmartReturnDetail[i].ProductName +
                         "</td>" +
                         "<td>" +
                          returnData.Data.SmartReturnDetail[i].Size +
                         "</td>" +
                         "<td>" +
                          returnData.Data.SmartReturnDetail[i].KcName +
                         "</td>" +
                          "<td>" + returnData.Data.SmartReturnDetail[i].Num + "</td>" +
                          "</td>" +
                          "<td>" + returnData.Data.SmartReturnDetail[i].Price + "</td>" +
                          "<td>" + returnData.Data.SmartReturnDetail[i].Amount + "</td>" +
                         "<td hidden='hidden' id='smartWarehouseRemarkTdhidden'>" +
                         returnData.Data.SmartReturnDetail[i].ID +
                         "</td>" +
                         "</tr>";
                }

                table += "<table class='site-table table-hover' style='width:"+ hospitalPrint.Data.Width+ "px;font-size:"+ hospitalPrint.Data.FontSize + "px;font-family:"+ hospitalPrint.Data.FontFamily + ";'>";
                table += "<thead><tr><th>药品/物品</th><th>规格</th><th>单位</th><th>数量</th><th>进价</th><th>金额</th></tr></thead>";
                table += " <tbody id='smartReturnDetailTD'></tbody>";
                table += returnDetailTr;
                table += "</tbody>";
                table += "</table>";

                string printStr = hospitalPrint.Data.Content.ToString();
                printStr = printStr.Replace("$WarehouseName", returnData.Data.WarehouseName);
                printStr = printStr.Replace("$No", returnData.Data.No);
                printStr = printStr.Replace("$SupplierName", returnData.Data.SupplierName);
                printStr = printStr.Replace("$CreateDate", returnData.Data.CreateTime);
                printStr = printStr.Replace("$Table", table);
                printStr = printStr.Replace("$CreateUserName", returnData.Data.CreateUserName);

                result.Data = printStr;
            }
            else {
                result.Message = "打印数据查询出现异常!";
            }
            return result;
        }

        /// <summary>
        /// 更新退货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartReturnUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            ///判断DTO是否为空
            if (string.IsNullOrWhiteSpace(dto.WarehouseID) || dto.WarehouseID == "-1")
            {
                result.Message = "仓库不能为空!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.SupplierID) || dto.SupplierID == "-1")
            {
                result.Message = "供应商不能为空!";
                return result;
            }
            if (string.IsNullOrWhiteSpace(dto.CreateTime))
            {
                result.Message = "退货日期不能为空!";
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

            if (dto.SmartReturnDetail == null || dto.SmartReturnDetail.Count <= 0)
            {
                result.Message = "退货详细不可为空!";
                return result;
            }

            #endregion

            TryTransaction(() =>
            {
                //int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();
                ////判断是否有数据
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();
                //判断是否有数据
                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }

                result.Data = _connection.Execute("UPDATE SmartReturn SET WarehouseID=@WarehouseID,SupplierID=@SupplierID,Remark=@Remark,CreateTime=@CreateTime WHERE ID = @ID", dto, _transaction);

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = Convert.ToInt64(dto.CreateUserID),
                    Type = LogType.SmartReturnUpdate,
                    Remark = LogType.SmartReturnUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
