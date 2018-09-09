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
    /// 采购发票业务处理类
    /// </summary>
    public class InvoiceService : BaseService, IInvoiceService
    {
        /// <summary>
        /// 添加采购发票
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(InvoiceAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            ///判断DTO是否为空

            if (string.IsNullOrWhiteSpace(dto.SupplierID)&& dto.SupplierID!="-1")
            {
                result.Message = "供应商不能为空!";
                return result;
            }
            if (string.IsNullOrWhiteSpace(dto.Code))
            {
                result.Message = "发票号不能为空!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                result.Message = "名称不能为空!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Amount))
            {
                result.Message = "金额不能为空!";
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

            if (dto.InvoiceDetailAdd.Count <= 0)
            {
                result.Message = "发票详细不可为空!";
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
                _connection.Execute(@"insert into SmartInvoice(ID,SupplierID,Code,Name,Amount,CreateTime,CreateUserID,HospitalID,Remark,BillDate) 
VALUES(@ID, @SupplierID, @Code, @Name, @Amount, @CreateTime, @CreateUserID, @HospitalID, @Remark, @BillDate)",
                 new { ID = id, SupplierID = dto.SupplierID, Code = dto.Code, Name = dto.Name, Amount = dto.Amount, CreateTime = DateTime.Now, CreateUserID = dto.CreateUserID, HospitalID = dto.HospitalID, Remark = dto.Remark, BillDate=dto.BillDate }, _transaction);  //采购发票记录表

                foreach (var u in dto.InvoiceDetailAdd)
                {
                    _connection.Execute("insert into SmartInvoiceDetail(ID,InvoiceID,PurchaseID) values(@ID, @InvoiceID, @PurchaseID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            InvoiceID = id,
                            PurchaseID = u.PurchaseID
                        }, _transaction); //采购发票记录详情表
                };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = Convert.ToInt64(dto.CreateUserID),
                    Type = LogType.InvoiceAdd,
                    Remark = LogType.InvoiceAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询所有采购单信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<InvoiceInfo>>> Get(InvoiceSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<InvoiceInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<InvoiceInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"
SELECT DISTINCT si.ID,Convert(varchar(30),si.CreateTime,23) AS CreateTime,si.SupplierID,ss.Name AS SupplierName,si.Code,si.Name,si.Amount,si.CreateUserID,su.Name AS CreateUserName,Convert(varchar(30),si.BillDate,23) AS BillDate,si.Remark FROM SmartInvoice AS si 
INNER JOIN dbo.SmartSupplier AS ss ON si.SupplierID=ss.ID
INNER JOIN dbo.SmartUser AS su ON si.CreateUserID=su.ID WHERE 1=1";

                sql2 = @"SELECT COUNT(si.ID) AS Count FROM SmartInvoice AS si 
INNER JOIN dbo.SmartSupplier AS ss ON si.SupplierID=ss.ID
INNER JOIN dbo.SmartUser AS su ON si.CreateUserID=su.ID WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(dto.SupplierID)&& dto.SupplierID!="-1")
                {
                    sql += @" And si.SupplierID=" + dto.SupplierID + "";
                    sql2 += @" And si.SupplierID=" + dto.SupplierID + "";
                }


                //" ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only
                if (!string.IsNullOrWhiteSpace(dto.BeginDate) && !string.IsNullOrWhiteSpace(dto.EndDate))
                {
                    sql += @" And si.BillDate between '" + dto.BeginDate + "' and '" + dto.EndDate + "'";
                    sql2 += @" And si.BillDate between '" + dto.BeginDate + "' and '" + dto.EndDate + "'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Code))
                {
                    sql += " And si.Code=" + dto.Code + "";
                    sql2 += @" And si.Code=" + dto.Code + "";
                }
                sql2 += " AND ss.HospitalID=" + dto.HospitalID + "";
                sql += " AND ss.HospitalID=" + dto.HospitalID + "";
                sql += " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<InvoiceInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询采购详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, InvoiceInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, InvoiceInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<InvoiceInfo>(@"select ID,SupplierID,Code,Name,Amount,Convert(varchar(30),CreateTime,23) AS CreateTime,CreateUserID,HospitalID,Remark,Convert(varchar(30),BillDate,23) AS BillDate from SmartInvoice where ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.InvoiceDetailAdd = new List<InvoiceDetailAdd>();
                result.Data.InvoiceDetailAdd = _connection.Query<InvoiceDetailAdd>(@"SELECT sidt.ID,sidt.PurchaseID,sp.No,sw.Name AS WarehouseName,ss.Name AS SupplierName,sp.CreateTime FROM SmartInvoiceDetail AS sidt 
INNER JOIN dbo.SmartPurchase AS sp ON sidt.PurchaseID=sp.ID
INNER JOIN dbo.SmartWarehouse AS sw ON sp.WarehouseID=sw.ID
INNER JOIN dbo.SmartSupplier AS ss ON sp.SupplierID=ss.ID WHERE sidt.InvoiceID=@InvoiceID", new { InvoiceID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 删除采购发票
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> InvoiceDel(InvoiceDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

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

                int delDetail = _connection.Execute("DELETE SmartInvoice WHERE ID=@ID", new { ID = dto.InvoiceID }, _transaction);
                if (delDetail > 0)
                {
                    _connection.Execute("DELETE SmartInvoiceDetail WHERE InvoiceID=@InvoiceID", new { InvoiceID = dto.InvoiceID }, _transaction);
                }

                var temp = new { 编号 = dto.InvoiceID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.InvoiceDelete,
                    Remark = LogType.InvoiceDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        public IFlyDogResult<IFlyDogResultType, int> Update(InvoiceUpdate dto)
        {
            throw new NotImplementedException();
        }
    }
}
