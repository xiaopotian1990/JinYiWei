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
    /// 科室领用业务逻辑
    /// </summary>
    public class UseService : BaseService, IUseService
    {

        private IHospitalPrintService _hospitalPrintService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="hospitalPrintService"></param>
        public UseService(IHospitalPrintService hospitalPrintService)
        {
            _hospitalPrintService = hospitalPrintService;
        }
        #endregion

        /// <summary>
        /// 添加科室领用信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(UseAdd dto)
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

            if (string.IsNullOrWhiteSpace(dto.DeptID) || dto.DeptID == "-1")
            {
                result.Message = "部门不能为空!";
                return result;
            }
            if (string.IsNullOrWhiteSpace(dto.CreateDate))
            {
                result.Message = "日期不能为空!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.UseUserID))
            {
                result.Message = "领用人不能为空!";
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

            if (dto.UseDetailAdd.Count <= 0)
            {
                result.Message = "领用详细不可为空!";
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
                _connection.Execute(@"insert into SmartUse(ID,WarehouseID,CreateUserID,CreateTime,CreateDate,No,DeptID,Status,UseUserID,Remark)
 values(@ID, @WarehouseID, @CreateUserID, @CreateTime, @CreateDate, @No, @DeptID, @Status, @UseUserID,@Remark)",
                 new { ID = id, WarehouseID = dto.WarehouseID, CreateUserID = dto.CreateUserID, CreateTime = DateTime.Now, CreateDate = dto.CreateDate, No = KCAutoNumber.Instance().CKNumber("SY"), DeptID = dto.DeptID, Status = dto.Status, UseUserID = dto.UseUserID, Remark = dto.Remark }, _transaction);  //领用记录表

                foreach (var u in dto.UseDetailAdd)
                {
                    _connection.Execute(" insert into SmartUseDetail(ID,UseID,ProductID,Num,Price,Amount) VALUES(@ID,@UseID, @ProductID, @Num, @Price, @Amount)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            UseID = id,
                            ProductID = u.ProductID,
                            Num = u.Num,
                            Price = u.Price,
                            Amount = u.Num * Convert.ToDouble(u.Price)
                        }, _transaction); //领用记录详情表

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
                    Type = LogType.UseAdd,
                    Remark = LogType.UseAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 获取所有科室领用信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<UseInfo>>> Get(UseSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<UseInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<UseInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT su.ID,su.No,Convert(varchar(30),su.CreateDate,23) AS CreateDate,su.DeptID,sd.Name AS DeptName,su.UseUserID,sUser.Name AS UseName,su.WarehouseID,sw.Name AS WarehouseName,su.CreateUserID,smartUser.Name AS CreateName,su.Remark,su.Status FROM SmartUse AS su INNER JOIN dbo.SmartDept AS sd ON su.DeptID=sd.ID
                    left JOIN dbo.SmartUser AS sUser ON su.UseUserID=sUser.ID
                    left JOIN dbo.SmartWarehouse AS sw ON su.WarehouseID=sw.ID
                    left JOIN dbo.SmartUser AS smartUser ON su.CreateUserID=smartUser.ID WHERE 1=1";

                sql2 = @"SELECT COUNT(su.ID) AS Count FROM SmartUse AS su 
                        left JOIN dbo.SmartDept AS sd ON su.DeptID=sd.ID
                        left JOIN dbo.SmartUser AS sUser ON su.UseUserID=sUser.ID
                        left JOIN dbo.SmartWarehouse AS sw ON su.WarehouseID=sw.ID
                        left JOIN dbo.SmartUser AS smartUser ON su.CreateUserID=smartUser.ID WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(dto.WarehouseID) && dto.WarehouseID != "-1")
                {
                    sql += @" And su.WarehouseID=" + dto.WarehouseID + "";
                    sql2 += @" And su.WarehouseID=" + dto.WarehouseID + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.DeptID) && dto.DeptID != "-1")
                {
                    sql += @" And su.DeptID=" + dto.DeptID + "";
                    sql2 += @" And su.DeptID=" + dto.DeptID + "";
                }
                //" ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only
                if (!string.IsNullOrWhiteSpace(dto.BeginTime) && !string.IsNullOrWhiteSpace(dto.EndTime))
                {
                    string entTime = dto.EndTime + " 23:59:59";
                    sql += @" And su.CreateDate between '" + dto.BeginTime + "' and '" + entTime + "'";
                    sql2 += @" And su.CreateDate between '" + dto.BeginTime + "' and '" + entTime + "'";
                }

                if (!string.IsNullOrWhiteSpace(dto.No))
                {
                    sql += " And su.No=" + dto.No + "";
                    sql2 += @" And su.No=" + dto.No + "";
                }

                sql2 += " AND su.CreateUserID=" + dto.CreateUserId + "";
                sql += " AND su.CreateUserID=" + dto.CreateUserId + "";
                sql += " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<UseInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;

        }

        /// <summary>
        /// 根据id获取科室领用详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, UseInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, UseInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<UseInfo>(@"select ID,WarehouseID,CreateUserID,Convert(varchar(30),CreateTime,23) AS CreateTime,Convert(varchar(30),CreateDate,23) AS CreateDate,No,DeptID,Status,UseUserID,Remark from SmartUse where ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.UseDetail = new List<UseDetailAdd>();
                result.Data.UseDetail = _connection.Query<UseDetailAdd>(@" SELECT sud.ID,sud.UseID,sp.ID AS ProductID,sp.Name AS ProductName,sp.Size,su.Name AS KcName,sud.Num,sud.Price,sud.Amount FROM SmartUseDetail AS sud 
                    left JOIN  SmartProduct AS sp ON sud.ProductID = sp.ID
                    left JOIN  dbo.SmartUnit AS su ON sp.UnitID = su.ID WHERE sud.UseID=@UseID", new { UseID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 根据科室领用id查询科室领用详情拼接成字符串打印出来
        /// </summary>
        /// <param name="UseID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, string> SmartUsePrint(string UseID, long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, string>();
            result.ResultType = IFlyDogResultType.Failed;

            var useData = this.GetByID(Convert.ToInt64(UseID));  //得到科室领用数据详情
            var hospitalPrint = _hospitalPrintService.GetByHospitalAndType(hospitalID, "13");//得到科室领用数据详情 
            if (useData.Data != null && hospitalPrint.Data != null)
            {

                string table = string.Empty;
                string useDetailTr = string.Empty;
                for (var i = 0; i < useData.Data.UseDetail.Count; i++)
                {
                    useDetailTr += "<tr value=" +
                                       useData.Data.UseDetail[i].ProductID +
                                       ">" +
                                       "<td>" +
                                       useData.Data.UseDetail[i].ProductName +
                                       "</td>" +
                                       "<td>" +
                                        useData.Data.UseDetail[i].Size +
                                       "</td>" +
                                       "<td>" +
                                        useData.Data.UseDetail[i].KcName +
                                       "</td>" +
                                        "<td>" + useData.Data.UseDetail[i].Num + "</td>" +
                                        "</td>" +
                                        "<td>" + useData.Data.UseDetail[i].Price + "</td>" +
                                        "<td>" + useData.Data.UseDetail[i].Amount + "</td>" +
                                       "<td hidden='hidden' id='smartWarehouseRemarkTdhidden'>" +
                                       useData.Data.UseDetail[i].ID +
                                       "</td>" +
                                       "</tr>";
                }

                table += "<table class='site-table table-hover' style='width:" + hospitalPrint.Data.Width + "px;font-size:" + hospitalPrint.Data.FontSize + "px;font-family:" + hospitalPrint.Data.FontFamily + ";'>";
                table += "<thead><tr><th>名称</th><th>规格</th><th>单位</th><th>数量</th><th>进价</th><th>金额</th></tr></thead>";
                table += "<tbody id='smartUseDetailTD'>";
                table += useDetailTr;
                table += "</tbody>";
                table += "</table>";

                string printStr = hospitalPrint.Data.Content.ToString();
                printStr = printStr.Replace("$WarehouseName ", useData.Data.WarehouseName);
                printStr = printStr.Replace("$No", useData.Data.No);
                printStr = printStr.Replace("$DeptName", useData.Data.DeptName);
                printStr = printStr.Replace("$CreateDate", useData.Data.CreateDate);
                printStr = printStr.Replace("$Table", table);
                printStr = printStr.Replace("$CreateUserName", useData.Data.CreateName);

                result.Data = printStr;
            }
            else {
                result.Message = "打印数据查询出现异常!";
            }

            return result;
        }

        public IFlyDogResult<IFlyDogResultType, int> Update(UseUpdate dto)
        {
            throw new NotImplementedException();
        }
    }
}
