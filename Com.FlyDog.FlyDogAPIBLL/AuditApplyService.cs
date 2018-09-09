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
    /// <summary>
    /// 我的审核申请业务逻辑
    /// </summary>
    public class AuditApplyService : BaseService, IAuditApplyService
    {
        /// <summary>
        /// 查询我的审核申请列表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditApplyInfo>>> Get(AuditApplySelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditApplyInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<AuditApplyInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT soso.ID,soso.CustomerID,sc.Name AS CustomerName,soso.CreateTime,soso.Type,soso.Content,soso.AuditStatus FROM dbo.SmartOwnerShipOrder AS soso LEFT JOIN SmartCustomer AS sc ON soso.CustomerID=sc.ID WHERE 1=1 ";

                sql2 = @"SELECT COUNT(soso.ID) AS Count FROM dbo.SmartOwnerShipOrder AS soso LEFT JOIN SmartCustomer AS sc ON soso.CustomerID=sc.ID WHERE 1=1 ";

                if (!string.IsNullOrWhiteSpace(dto.AuditType.ToString()) && dto.AuditType != -1)
                {
                    sql += @" AND soso.AuditStatus=" + dto.AuditType + "";
                    sql2 += @" AND soso.AuditStatus=" + dto.AuditType + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.BeginTime.ToString()) && !string.IsNullOrWhiteSpace(dto.EndTime.ToString()))
                {
                    string entTime = dto.EndTime.ToString().Replace(" 0:00:00", " 23:59:59");
                    sql += @" And soso.CreateTime between '" + dto.BeginTime + "' and '" + entTime + "'";
                    sql2 += @" And soso.CreateTime between '" + dto.BeginTime + "' and '" + entTime + "'";
                }

                //if (!string.IsNullOrWhiteSpace(dto.CustomerID))
                //{
                //    sql += " And soso.CustomerID=" + dto.CustomerID + "";
                //    sql2 += @" And soso.CustomerID=" + dto.CustomerID + "";
                //} //客户编号查询先注释，现在还不知道客户编号是什么
                sql2 += " AND soso.CreateUserID=" + dto.CreateUserId + "";
                sql += " AND soso.CreateUserID=" + dto.CreateUserId + "";
                sql += " ORDER by soso.ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<AuditApplyInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 查询审核详情是否审核通过
        /// </summary>
        /// <param name="orderID"></param>
        /// <param name="userID"></param>
        /// <param name="orderType"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, IEnumerable<AuditInfo>> GetAuditInfo(long orderID, long userID, int orderType)
        {
            //var result = new IFlyDogResult<IFlyDogResultType, AuditInfo>();
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AuditInfo>>();
            TryExecute(() =>
            {
                result.Data = _connection.Query<AuditInfo>(@"SELECT sa.Level,sa.UserID,sa.CreateTime,su.Name AS AuditUserName,sa.Status,sa.Content FROM dbo.SmartAudit AS sa
                LEFT JOIN dbo.SmartUser AS su ON sa.UserID=su.ID WHERE OrderID=@OrderID AND UserID=@UserID AND OrderType=@OrderType", new { OrderID = orderID, UserID = userID, OrderType = orderType });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            return result;
        }

        /// <summary>
        ///  根据操作i的，类型查询审核详情
        /// </summary>
        /// <param name="orderID">操作id，目前主要是咨询或者开发人员变更id</param>
        /// <param name="type"> 4 咨询人员变更 5 开发人员变更</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, OwnerShipOrderAudit> GetAuditDetail(long orderID, string type,long hospitalID,long userID) {
            var result = new IFlyDogResult<IFlyDogResultType, OwnerShipOrderAudit>();
            TryExecute(() =>
            {
                result.Data = _connection.Query<OwnerShipOrderAudit>("SELECT AuditStatus FROM dbo.SmartOwnerShipOrder WHERE ID=@ID", new { ID = orderID }).FirstOrDefault();

                result.Data.AuditDetailsList = new List<AuditDetails>();
                List<AuditDetails> list = new List<AuditDetails>();
                list= _connection.Query<AuditDetails>(@" SELECT sard.UserID,sard.Level,('【'+sd.Name+'】'+'【'+su.Name+'】') AS AuditUser FROM dbo.SmartAuditRuleDetail AS sard
                     LEFT JOIN SmartAuditRule AS sar ON sard.RuleID=sar.ID
                     LEFT JOIN dbo.SmartUser AS su ON su.ID=sard.UserID
                     LEFT JOIN dbo.SmartDept AS sd ON su.DeptID=sd.ID
                     WHERE sar.Type=@Type AND sar.HospitalID=@HospitalID", new { Type = type, HospitalID = hospitalID }).ToList();

                if (list != null && list.Count > 0)
                {

                    var auditInfo = GetAuditInfo(orderID, userID, Convert.ToInt32(type));
                    if (auditInfo.Data!=null&& auditInfo.Data.Count()>0) {
                        foreach (var item in list)
                        {

                            foreach (var itemData in auditInfo.Data)
                            {
                                if (item.Level == itemData.Level && item.UserID == itemData.UserID)
                                {
                                    if (itemData.Status == "4")
                                    {
                                        item.Status = Convert.ToInt32(itemData.Status);
                                        item.Content = itemData.CreateTime + " " + itemData.AuditUserName + " " + "审核通过" + " " + itemData.Content;
                                        item.CreateTime = itemData.CreateTime;
                                    }
                                    else if (itemData.Status == "3")
                                    {
                                        item.Status = Convert.ToInt32(itemData.Status);
                                        item.Content = itemData.CreateTime + " " + itemData.AuditUserName + " " + "审核不通过" + " " + itemData.Content;
                                        item.CreateTime = itemData.CreateTime;
                                    }
                                    else {
                                        item.CreateTime ="";
                                    }
                                }
                            }                              
                        }
                    }
                  
                }

                //result.Data.AuditDetailsList = _connection.Query<AuditDetails>(@"SELECT Level,('【'+sd.Name+'】'+' '+'【'+su.Name+'】') AS AuditUser,sa.Status,sa.CreateTime,sa.Content FROM dbo.SmartAudit AS sa
                //    LEFT JOIN dbo.SmartUser AS su ON sa.UserID=su.ID
                //    LEFT JOIN dbo.SmartDept AS sd ON su.DeptID=sd.ID 
                //    WHERE OrderID=@OrderID AND OrderType=@OrderType", new { OrderID = orderID, OrderType= type }).ToList();
                result.Data.AuditDetailsList = list;
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }


        /// <summary>
        /// 根据数据id，类型查询审核表中此条数据是否已经被审核过，只要被审核过就不能删除了
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetByOrderIDAndTypeData(long orderID,string orderType)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) AS Count FROM dbo.SmartAudit WHERE OrderID=@OrderID AND OrderType=@OrderType", new { OrderID = orderID, OrderType= orderType }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 取消/删除我的审核申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(AuditApplyDelete dto) {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var orderData = GetByOrderIDAndTypeData(dto.OrderID,dto.OrderType);
            if (orderData.Data>0) {
                result.Message = "当前数据已被审核，不能删除!";
                return result;
            }

            #region 开始事物操作
            TryTransaction(() =>
            {

                #region 开始更新操作
                string sql = "";
                if (dto.OrderType == "4") { //4 开发人员
                    sql = "DELETE dbo.SmartOwnerShipOrder WHERE ID=@ID AND Type=1";
                } else if (dto.OrderType=="5")
                {//咨询人员
                    sql = "DELETE dbo.SmartOwnerShipOrder WHERE ID=@ID AND Type=2";
                }
                result.Data = _connection.Execute(sql, new { ID = dto.OrderID }, _transaction);
                var temp = new { 编号 = dto.OrderID,类型=dto.OrderType=="4"?"取消开发人员变更申请":"取消咨询人员变更申请" };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.AuditApplyDelete,
                    Remark = LogType.AuditApplyDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion
                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;

        }
    }
}
