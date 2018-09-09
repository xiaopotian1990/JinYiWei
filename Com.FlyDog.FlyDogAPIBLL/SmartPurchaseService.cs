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
    public class SmartPurchaseService : BaseService, ISmartPurchaseService
    {
        private IHospitalPrintService _hospitalPrintService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="hospitalPrintService"></param>
        public SmartPurchaseService(IHospitalPrintService hospitalPrintService)
        {
            _hospitalPrintService = hospitalPrintService;
        }
        #endregion

        /// <summary>
        /// 添加进货信息 (先插入进货信息表，然后是进货信息详情表，最后是库存表)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartPurchaseAdd dto)
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
                result.Message = "进货日期不能为空!";
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


            if (dto.SmartPurchaseDetail.Count <= 0)
            {
                result.Message = "进货详细不可为空!";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                //通过创建人ID查询一条数据
                #region 验证用户 先注释，等测试完成在放开 20170204
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();
                //判断是否有数据
                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }
                #endregion

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                _connection.Execute("insert into SmartPurchase(ID,WarehouseID,SupplierID,CreateUserID,CreateTime,Status,Remark,No,CreateDate) values (@ID,@WarehouseID,@SupplierID, @CreateUserID, @CreateTime, @Status, @Remark, @No, @CreateDate)",
                 new { ID = id, WarehouseID = dto.WarehouseID, SupplierID = dto.SupplierID, CreateUserID = dto.CreateUserID, CreateTime = dto.CreateTime, Status = dto.Status, Remark = dto.Remark, No = KCAutoNumber.Instance().CKNumber("JH"), CreateDate = dto.CreateDate }, _transaction);  //进货信息

                foreach (var u in dto.SmartPurchaseDetail)
                {
                    _connection.Execute("insert into SmartPurchaseDetail(ID,PurchaseID,ProductID,Num,Price,Amount,Batch,Expiration) values (@ID,@PurchaseID,@ProductID, @Num, @Price, @Amount, @Batch, @Expiration)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            PurchaseID = id,
                            ProductID = u.ProductID,
                            Num = u.Num,
                            Price = u.Price,
                            Amount = Convert.ToDouble(u.Num) * Convert.ToDouble(u.Price),
                            Batch = u.Batch,
                            Expiration = u.Expiration
                        }, _transaction);

                    if (dto.Status != "0")
                    {//暂存状态，不存库存表，只有进货状态才会存库存表
                        var KcId = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                        _connection.Execute(@"insert into SmartStock(ID,WarehouseID,ProductID,Num,Price,Amount,DetailID,Type,Batch,Expiration)
 values(@ID, @WarehouseID, @ProductID, @Num, @Price, @Amount, @DetailID, @Type,@Batch,@Expiration)",
                         new { ID = KcId, WarehouseID = dto.WarehouseID, ProductID = u.ProductID, Num = u.Num, Price = u.Price, Amount = Convert.ToDouble(u.Num) * Convert.ToDouble(u.Price), DetailID = id, Type = 1, Batch = u.Batch, Expiration = u.Expiration }, _transaction); //库存表  DetailID 入库主键id（根据类型走，哪个类型就是哪个类型的主键）
                    }

                };//进货信息详情



                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = Convert.ToInt64(dto.CreateUserID),
                    Type = LogType.SmartPurchaseAdd,
                    Remark = LogType.SmartPurchaseAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询所有进货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartPurchaseInfo>>> Get(SmartPurchaseSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartPurchaseInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<SmartPurchaseInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"select DISTINCT sp.ID,sp.Status,sp.No,Convert(varchar(30),sp.CreateTime,23) AS CreateTime, sp.WarehouseID,sw.Name as WarehouseName,ss.Name as SupplierName,sp.SupplierID, su.Name as CreateName,sp.Remark from SmartPurchase as sp
                    LEFT join SmartPurchaseDetail as spd on sp.ID=spd.PurchaseID 
                    LEFT join SmartWarehouse as sw on sp.WarehouseID=sw.ID 
                    LEFT join SmartSupplier as ss on sp.SupplierID=ss.ID 
                    LEFT join  SmartUser as su on sp.CreateUserID =su.ID where 1=1";

                sql2 = @"select COUNT(DISTINCT sp.ID) as Count from SmartPurchase as sp 
                        LEFT join SmartPurchaseDetail as spd on sp.ID=spd.PurchaseID 
                        LEFT join SmartWarehouse as sw on sp.WarehouseID=sw.ID 
                        LEFT join SmartSupplier as ss on sp.SupplierID=ss.ID 
                        LEFT join  SmartUser as su on sp.CreateUserID
                  =su.ID WHERE 1 = 1";

                if (!string.IsNullOrWhiteSpace(dto.WarehouseID) && dto.WarehouseID != "-1")
                {
                    sql += @" And sp.WarehouseID=" + dto.WarehouseID + "";
                    sql2 += @" And sp.WarehouseID=" + dto.WarehouseID + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.SupplierID) && dto.SupplierID != "-1")
                {
                    sql += @" And sp.SupplierID=" + dto.SupplierID + "";
                    sql2 += @" And sp.SupplierID=" + dto.SupplierID + "";
                }
                //" ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only
                if (!string.IsNullOrWhiteSpace(dto.BeginTime) && !string.IsNullOrWhiteSpace(dto.EndTime))
                {
                    string entTime = dto.EndTime + " 23:59:59";
                    sql += @" And sp.CreateTime between '" + dto.BeginTime + "' and '" + entTime + "'";
                    sql2 += @" And sp.CreateTime between '" + dto.BeginTime + "' and '" + entTime + "'";
                }

                if (!string.IsNullOrWhiteSpace(dto.No))
                {
                    sql += " And sp.No=" + dto.No + "";
                    sql2 += @" And sp.No=" + dto.No + "";
                }
                sql2 += " AND sp.CreateUserID=" + dto.CreateUserID + "";
                sql += " AND sp.CreateUserID=" + dto.CreateUserID + ""; //查询当前用户创建的
                sql += " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<SmartPurchaseInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据医院id查询当前医院所有的进货记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartPurchaseInfo>> GetByHospitalID(SmartPurchaseSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartPurchaseInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                string sql = @"SELECT sp.ID,sw.Name AS WarehouseName,su.Name AS SupplierName,sp.No,Convert(varchar(30),sp.CreateTime,23) AS CreateTime FROM dbo.SmartPurchase AS sp
                     LEFT JOIN SmartWarehouse AS sw ON sp.WarehouseID = sw.ID
                     LEFT JOIN dbo.SmartSupplier AS su ON sp.SupplierID = su.ID WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(dto.WarehouseID) && dto.WarehouseID != "-1")
                {
                    sql += @" And sp.WarehouseID=" + dto.WarehouseID + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.BeginTime) && !string.IsNullOrWhiteSpace(dto.EndTime))
                {
                    sql += @" And sp.CreateTime between '" + dto.BeginTime + "' and '" + dto.EndTime + "'";
                }

                sql += " And sw.HospitalID = " + dto.HospitalID + "";
                sql += @" AND sp.ID NOT IN (SELECT  PurchaseID FROM dbo.SmartInvoiceDetail INNER JOIN SmartInvoice ON SmartInvoiceDetail.InvoiceID=SmartInvoice.ID
  WHERE SmartInvoice.HospitalID = " + dto.HospitalID + ")";//不包含当前医院已经开过发票的进货记录
                result.Data = _connection.Query<SmartPurchaseInfo>(sql);
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        ///根据医院id查询医院内的进货记录(查询出来后按照到期时间排序)
        /// </summary>
        /// <returns></returns>
        //public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartPurchaseInfo>>> GetByHospitalIDData(SmartPurchaseSelect dto)
        //{

        //}



        /// <summary>
        /// 通过id查询一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartPurchaseInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartPurchaseInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartPurchaseInfo>("select ID,WarehouseID,SupplierID,CreateUserID,Convert(varchar(30),CreateTime,23) AS CreateTime,Status,Remark,No,CreateDate from SmartPurchase where ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.SmartPurchaseDetail = new List<SmartPurchaseDetailAdd>();
                result.Data.SmartPurchaseDetail = _connection.Query<SmartPurchaseDetailAdd>(@" SELECT spd.ID,spd.PurchaseID,spd.ProductID,sp.Name AS ProductName,sp.Size,su.Name AS UnitName,spd.Num,spd.Price,spd.Amount,
spd.Batch, Convert(varchar(30),spd.Expiration,23) AS Expiration FROM dbo.SmartPurchaseDetail AS spd left JOIN SmartProduct AS sp ON spd.ProductID = sp.ID
left JOIN SmartUnit AS su ON sp.UnitID = su.ID  WHERE spd.PurchaseID = @PurchaseID", new { PurchaseID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 删除进货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> SmartPurchaseDelete(SmartPurchaseDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var smartPurchase = GetByID(Convert.ToInt64(dto.PurchaseID));

            if (smartPurchase.Data.Status == "1")
            {//说明此条进货信息状态是已经进货了，不能删除，只能删除那些进货状态为暂存的
                result.Message = "该单已进货!";
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
                int delDetail = _connection.Execute("delete SmartPurchase where ID=@ID", new { ID = dto.PurchaseID }, _transaction);
                if (delDetail > 0)
                {
                    _connection.Execute("delete SmartPurchaseDetail where PurchaseID=@PurchaseID", new { PurchaseID = dto.PurchaseID }, _transaction);
                }
                //先把新增仓库管理员功能的代码注释掉，等前边页面调通了之后再放开

                var temp = new { 编号 = dto.PurchaseID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartPurchaseDelte,
                    Remark = LogType.SmartPurchaseDelte.ToDescription() + temp.ToJsonString()
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
        /// 根据仓库进货id查询仓库进货详情拼接成字符串打印出来
        /// </summary>
        /// <param name="purchaspID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, string> SmartPurchasePrint(string purchaspID, long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, string>();
            result.ResultType = IFlyDogResultType.Failed;

            var purchaseData = this.GetByID(Convert.ToInt64(purchaspID));  //得到进货入库详情数据
            var hospitalPrint = _hospitalPrintService.GetByHospitalAndType(hospitalID,"9");  //得到进货入库模板 

            if (purchaseData.Data != null && hospitalPrint.Data != null)
            {
                string table = string.Empty;
                string purchaseDetailTr = string.Empty;
                for (var i = 0; i < purchaseData.Data.SmartPurchaseDetail.Count; i++)
                {
                    purchaseDetailTr += "   <tr value=" +
                    purchaseData.Data.SmartPurchaseDetail[i].ProductID +
                    " class='ele-text-c'>" +
                    "<td>" +
                    purchaseData.Data.SmartPurchaseDetail[i].ProductName +
                    "</td>" +
                    "<td>" +
                     purchaseData.Data.SmartPurchaseDetail[i].Size +
                    "</td>" +
                    "<td>" +
                     purchaseData.Data.SmartPurchaseDetail[i].UnitName +
                    "</td>" +
                     "<td> " + purchaseData.Data.SmartPurchaseDetail[i].Num + "</td>" +
                     "</td>" +
                     "<td>" + purchaseData.Data.SmartPurchaseDetail[i].Price + "</td>" +
                     "<td><label id='lableZPrice'>" + purchaseData.Data.SmartPurchaseDetail[i].Amount + "</label></td>" +
                      "<td>"+ purchaseData.Data.SmartPurchaseDetail[i].Batch + "</td>" +//批号
                       "<td>" + purchaseData.Data.SmartPurchaseDetail[i].Expiration + "</td>" +

                    "<td hidden='hidden' id='smartWarehouseRemarkTdhidden'>" +
                    purchaseData.Data.SmartPurchaseDetail[i].ID +
                    "</td>" +
                    "</tr>";
                }

                table += "<table class='site-table table-hover' style='width:" + hospitalPrint.Data.Width + "px;font-size:" + hospitalPrint.Data.FontSize + "px;font-family:" + hospitalPrint.Data.FontFamily + ";'>";
                table += "<thead><tr><th>药品/物品</th><th>规格</th><th>单位</th><th>数量</th><th>进价</th><th>金额</th><th>批号</th><th>有效日期</th></tr></thead>";
                table += " <tbody id='smartPurchaseDetailTD'>";
                table += purchaseDetailTr;
                table += "</tbody>";
                table += "</table>";

                string printStr = hospitalPrint.Data.Content.ToString();
                printStr = printStr.Replace("$WarehouseName", purchaseData.Data.WarehouseName);
                printStr = printStr.Replace("$No", purchaseData.Data.No);
                printStr = printStr.Replace("$SupplierName", purchaseData.Data.SupplierName);
                printStr = printStr.Replace("$CreateDate", purchaseData.Data.CreateTime);
                printStr = printStr.Replace("$Table", table);
                printStr = printStr.Replace("$CreateUserName", purchaseData.Data.CreateName);

                result.Data = printStr;
            }
            else {
                result.Message = "打印数据查询出现异常!";
            }
            return result;
        }

        /// <summary>
        /// 更新进货信息(如果是已进货状态则不让修改操作，如果是暂存状态则可以进货操作)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartPurchaseUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 数据验证
            var smartPurchase = GetByID(Convert.ToInt64(dto.ID));

            if (smartPurchase.Data.Status == "1")
            {//说明此条进货信息状态是已经进货了，不能再次进货，如果是暂存则可以进货
                result.Message = "该单已进货,不能再次进货!";
                return result;
            }
            #endregion
            TryTransaction(() =>
            {

                ////判断是否有数据
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();
                //判断是否有数据
                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }
                SmartPurchaseDelete spd = new SmartPurchaseDelete();
                spd.PurchaseID = dto.ID;
                spd.CreateUserID = Convert.ToInt64(dto.CreateUserID);

                #region 开始更新操作             
                int delDetail = _connection.Execute("delete SmartPurchase where ID=@ID", new { ID = spd.PurchaseID }, _transaction);
                if (delDetail > 0)
                {
                    _connection.Execute("delete SmartPurchaseDetail where PurchaseID=@PurchaseID", new { PurchaseID = spd.PurchaseID }, _transaction);
                }
                #endregion
                SmartPurchaseAdd spAdd = new SmartPurchaseAdd();
                spAdd.CreateUserID = dto.CreateUserID.ToString();
                spAdd.WarehouseID = dto.WarehouseID;
                spAdd.SupplierID = dto.SupplierID;
                spAdd.CreateTime = dto.CreateTime;
                spAdd.Remark = dto.Remark;
                spAdd.Status = dto.Status;
                spAdd.No = KCAutoNumber.Instance().CKNumber("JH");
                spAdd.SmartPurchaseDetail = dto.SmartPurchaseDetail;

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                _connection.Execute("insert into SmartPurchase(ID,WarehouseID,SupplierID,CreateUserID,CreateTime,Status,Remark,No,CreateDate) values (@ID,@WarehouseID,@SupplierID, @CreateUserID, @CreateTime, @Status, @Remark, @No, @CreateDate)",
                 new { ID = id, WarehouseID = spAdd.WarehouseID, SupplierID = spAdd.SupplierID, CreateUserID = spAdd.CreateUserID, CreateTime = spAdd.CreateTime, Status = spAdd.Status, Remark = spAdd.Remark, No = spAdd.No, CreateDate = spAdd.CreateDate }, _transaction);  //进货信息

                foreach (var u in spAdd.SmartPurchaseDetail)
                {
                    _connection.Execute("insert into SmartPurchaseDetail(ID,PurchaseID,ProductID,Num,Price,Amount,Batch,Expiration) values (@ID,@PurchaseID,@ProductID, @Num, @Price, @Amount, @Batch, @Expiration)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            PurchaseID = id,
                            ProductID = u.ProductID,
                            Num = u.Num,
                            Price = u.Price,
                            Amount = Convert.ToDouble(u.Num) * Convert.ToDouble(u.Price),
                            Batch = u.Batch,
                            Expiration = u.Expiration
                        }, _transaction);

                    if (dto.Status != "0")
                    {//暂存状态，不存库存表，只有进货状态才会存库存表
                        var KcId = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                        _connection.Execute(@"insert into SmartStock(ID,WarehouseID,ProductID,Num,Price,Amount,DetailID,Type,Batch,Expiration)
 values(@ID, @WarehouseID, @ProductID, @Num, @Price, @Amount, @DetailID, @Type,@Batch,@Expiration)",
                         new { ID = KcId, WarehouseID = spAdd.WarehouseID, ProductID = u.ProductID, Num = u.Num, Price = u.Price, Amount = Convert.ToDouble(u.Num) * Convert.ToDouble(u.Price), DetailID = id, Type = 1, Batch = u.Batch, Expiration = u.Expiration }, _transaction); //库存表  DetailID 入库主键id（根据类型走，哪个类型就是哪个类型的主键）
                    }

                };//进货信息详情

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = Convert.ToInt64(dto.CreateUserID),
                    Type = LogType.SmartPurchaseUpdate,
                    Remark = LogType.SmartPurchaseUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }
    }
}
