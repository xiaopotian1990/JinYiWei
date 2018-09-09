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
    /// 审核工作台业务逻辑
    /// </summary>
    public class AuditWorkbenchService : BaseService, IAuditWorkbenchService
    {

        private IAuditRuleService _auditRuleService;
        private IOwnerShipService _ownerShipService;
        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="auditRuleService"></param>
        public AuditWorkbenchService(IAuditRuleService auditRuleService, IOwnerShipService ownerShipService)
        {
            _auditRuleService = auditRuleService;
            _ownerShipService = ownerShipService;
        }
        #endregion
        /// <summary>
        /// 开始审核操作
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> AuditOperationAdd(AuditOperationAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            List<BatchConsultantUserTemp> listCustormID = new List<BatchConsultantUserTemp>(); //存储筛选出来的合法用户
            List<BatchConsultantUserUpdateTemp> listCustormUpdateID = new List<BatchConsultantUserUpdateTemp>(); //存储需要更新时间的客户
            #region 开启事物操作
            TryTransaction(() =>
            {
                //1、先插入到审核表中一条记录
                _connection.Execute("insert into SmartAudit(ID,OrderID,UserID,Level,Status,Content,OrderType,CreateTime) VALUES(@ID, @OrderID, @UserID, @Level, @Status, @Content, @OrderType, @CreateTime)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        OrderID = dto.AuditDataID,
                        UserID = dto.AuditUserID,
                        Level = dto.UserLevel,
                        Status = dto.Status,
                        Content = dto.Content,
                        OrderType = dto.AutitType,
                        CreateTime = DateTime.Now
                    }, _transaction);

                //2、 判断是否审核通过，不通过在插入记录表之后直接修改目标表审核状态
                if (dto.Status == 3)//3 审核不通过
                {
                    if (dto.AutitType == 1)
                    {//项目单
                        _connection.Execute("update SmartOrder set AuditStatus = 3 where ID = @ID", new { ID = dto.AuditDataID }, _transaction);// 更新审核项目单(将状态更新成审核不成功)
                    }
                    else if (dto.AutitType == 2)
                    {//退项目单
                        _connection.Execute("update SmartBackOrder set AuditStatus = 3 where ID = @ID", new { ID = dto.AuditDataID }, _transaction);// 更新退项目单(将状态更新成审核不成功)
                    }
                    else if (dto.AutitType == 3)
                    {//退预收款单
                        _connection.Execute("update SmartDepositRebateOrder set AuditStatus = 3 where ID = @ID", new { ID = dto.AuditDataID }, _transaction);// 更新退预收款单(将状态更新成审核不成功)
                    }
                    else if (dto.AutitType == 4)
                    {//开发人变更
                        _connection.Execute("update dbo.SmartOwnerShipOrder set AuditStatus = 3 where ID = @ID", new { ID = dto.AuditDataID }, _transaction);// 更新开发人变更(将状态更新成审核不成功)
                    }
                    else if (dto.AutitType == 5)
                    {//咨询人变更
                        _connection.Execute("update dbo.SmartOwnerShipOrder set AuditStatus = 3 where ID = @ID", new { ID = dto.AuditDataID }, _transaction);// 更新咨询人变更(将状态更新成审核不成功)
                    }
                }
                else
                {
                    //审核通过 
                    if (dto.UserLevel == dto.MaxLevel)
                    {//如果用户所处审核级别等于该类型最大审核级别
                        if (dto.AutitType == 1)
                        {//项目单
                            _connection.Execute(" update SmartOrder set AuditStatus = 4 where ID = @ID", new { ID = dto.AuditDataID }, _transaction);// 更新审核项目单(将状态更新成审核成功)
                        }
                        else if (dto.AutitType == 2)
                        {//退项目单
                            _connection.Execute("  update SmartBackOrder set AuditStatus = 4 where ID = @ID", new { ID = dto.AuditDataID }, _transaction);// 更新退项目单(将状态更新成审核成功)
                        }
                        else if (dto.AutitType == 3)
                        {//退预收款单
                            _connection.Execute("update SmartDepositRebateOrder set AuditStatus = 4 where ID = @ID", new { ID = dto.AuditDataID }, _transaction);// 更新退预收款单(将状态更新成审核成功)
                        }
                        else if (dto.AutitType == 4)
                        {//开发人变更
                            _connection.Execute("update dbo.SmartOwnerShipOrder set AuditStatus = 4 where ID = @ID", new { ID = dto.AuditDataID }, _transaction);// 更新开发人变更(将状态更新成审核不成功)
                            #region 开始操作SmartOwnerShip 表
                            //更新完成之后先更新SmartOwnerShip 表中原开发人员时间，在新增一条记录
                            //var newUserCustormData = _ownerShipService.GetByFiltrate("1", dto.NewID, dto.HospitalID);//得到新的开发人员客户信息
                            //List<SingleCustormInfo> list = newUserCustormData.Data.ToList();
                            //if (list != null && list.Count > 0)
                            //{
                            //    if (!list.Exists(o => o.CustomerID == dto.CustomerID))
                            //    {
                            //        listCustormID.Add(new BatchConsultantUserTemp()
                            //        {
                            //            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            //            CustomerID = dto.CustomerID,
                            //            UserID = dto.NewID,
                            //            StartTime = DateTime.Now,
                            //            EndTime1 = "9999-12-31 23:59:59.997",
                            //            Type = 1,
                            //            HospitalID = dto.HospitalID,
                            //            Remark = " "
                            //        });
                            //    }
                            //}
                            //else
                            //{
                            listCustormID.Add(new BatchConsultantUserTemp()
                                {
                                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                    CustomerID = dto.CustomerID,
                                    UserID = dto.NewID,
                                    StartTime = DateTime.Now,
                                    EndTime1 = "9999-12-31 23:59:59.997",
                                    Type = 1,
                                    HospitalID = dto.HospitalID,
                                    Remark = " "
                                });
                            //}

                            listCustormUpdateID.Add(new BatchConsultantUserUpdateTemp
                            {
                                CustomerID = dto.CustomerID,
                                UserID = dto.OldID,
                                EndTime = DateTime.Now,
                                Type = 1,
                                HospitalID = dto.HospitalID
                            });

                            _connection.Execute("UPDATE SmartOwnerShip SET EndTime=@EndTime WHERE UserID=@UserID AND Type=@Type AND HospitalID=@HospitalID AND  GETDATE() BETWEEN StartTime and EndTime", listCustormUpdateID, _transaction);

                            result.Data = _connection.Execute("insert into SmartOwnerShip(ID,CustomerID,UserID,StartTime,EndTime,Type,HospitalID,Remark) VALUES(@ID, @CustomerID, @UserID, @StartTime, @EndTime1, @Type, @HospitalID, @Remark)", listCustormID, _transaction);
                            #endregion
                        }
                        else if (dto.AutitType == 5)
                        {//咨询人变更
                            _connection.Execute("update dbo.SmartOwnerShipOrder set AuditStatus = 4 where ID = @ID", new { ID = dto.AuditDataID }, _transaction);// 更新咨询人变更(将状态更新成审核不成功)
                            #region 开始操作SmartOwnerShip表
                            //if (!list.Exists(o => o.CustomerID == dto.CustomerID))
                            //{//如果新用户不存在此客户
                                listCustormID.Add(new BatchConsultantUserTemp()
                                {
                                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                    CustomerID = dto.CustomerID,
                                    UserID = dto.NewID,
                                    StartTime = DateTime.Now,
                                    EndTime1 = "9999-12-31 23:59:59.997",
                                    Type = 2,
                                    HospitalID = dto.HospitalID,
                                    Remark = " "
                                });
                            //}

                            listCustormUpdateID.Add(new BatchConsultantUserUpdateTemp
                            {
                                CustomerID = dto.CustomerID,
                                UserID = dto.OldID,
                                EndTime = DateTime.Now,
                                Type = 2,
                                HospitalID = dto.HospitalID
                            });

                            _connection.Execute("UPDATE SmartOwnerShip SET EndTime=@EndTime WHERE UserID=@UserID AND Type=@Type AND HospitalID=@HospitalID AND  GETDATE() BETWEEN StartTime and EndTime", listCustormUpdateID, _transaction);

                            result.Data = _connection.Execute("insert into SmartOwnerShip(ID,CustomerID,UserID,StartTime,EndTime,Type,HospitalID,Remark) VALUES(@ID, @CustomerID, @UserID, @StartTime, @EndTime1, @Type, @HospitalID, @Remark)", listCustormID, _transaction);
                            #endregion
                        }
                    }
                }
                #region 开始数据操作动作
                var temp = new { 编号 = result.Data, 名称 = "审核数据", 审核数据id = dto.AuditDataID, 审核状态 = dto.Status, 审核级别 = dto.UserLevel, 最大审核级别 = dto.MaxLevel };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.AuditUserID,
                    Type = LogType.AuditOperationAdd,
                    Remark = LogType.AuditOperationAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "审核成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 查询所有待审核的信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditWorkbenchInfo>>> GetAllAudit(AuditWorkbenchSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditWorkbenchInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<AuditWorkbenchInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @" WITH tree as
	                    (SELECT so.ID,so.CustomerID,so.AuditStatus,sc.Name AS CustomerName,('项目单') AS TypeName,('1') AS TypeValue,sh.Name AS HospitalName,su.Name AS CreateUserName,so.CreateTime,so.Remark FROM SmartOrder AS so
	                    LEFT JOIN dbo.SmartCustomer AS sc ON so.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON so.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON so.CreateUserID=su.ID  WHERE so.AuditStatus=1 AND so.HospitalID=@HospitalID1
	                    UNION ALL 
	                    --退预收款订单
	                    SELECT sdro.ID,sdro.CustomerID,sdro.AuditStatus,sc.Name AS CustomerName,('退预收款单') AS TypeName,('3') AS TypeValue,sh.Name AS HospitalName,su.Name AS CreateUserName,sdro.CreateTime,sdro.Remark FROM dbo.SmartDepositRebateOrder AS sdro
	                    LEFT JOIN dbo.SmartCustomer AS sc ON sdro.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON sdro.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON sdro.CreateUserID=su.ID  WHERE sdro.AuditStatus=1 AND sdro.HospitalID=@HospitalID2
	                    --退项目单表
	                    UNION ALL
	                    SELECT sbo.ID,sbo.CustomerID,sbo.AuditStatus,sc.Name AS CustomerName,('退项目单') AS TypeName,('2') AS TypeValue,sh.Name AS HospitalName,su.Name AS CreateUserName,sbo.CreateTime,sbo.Remark FROM dbo.SmartBackOrder AS sbo
	                    LEFT JOIN dbo.SmartCustomer AS sc ON sbo.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON sbo.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON sbo.CreateUserID=su.ID   WHERE sbo.AuditStatus=1 AND sbo.HospitalID=@HospitalID3
						--顾客归属关系变更表(开发人员变更)
					    UNION ALL
						 SELECT soso.ID,soso.CustomerID,soso.AuditStatus,sc.Name AS CustomerName,('开发人变更') AS TypeName,('4') AS TypeValue,
						 sh.Name AS HospitalName,su.Name AS CreateUserName,soso.CreateTime,soso.Content FROM dbo.SmartOwnerShipOrder AS soso
						  LEFT JOIN dbo.SmartCustomer AS sc ON soso.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON soso.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON soso.CreateUserID=su.ID WHERE soso.Type=1 AND soso.AuditStatus=1 AND soso.HospitalID=@HospitalID4
						--顾客归属关系变更表(咨询人员变更)
					    UNION ALL
						SELECT soso.ID,soso.CustomerID,soso.AuditStatus,sc.Name AS CustomerName,('咨询人变更') AS TypeName,('5') AS TypeValue,
						 sh.Name AS HospitalName,su.Name AS CreateUserName,soso.CreateTime,soso.Content FROM dbo.SmartOwnerShipOrder AS soso
						  LEFT JOIN dbo.SmartCustomer AS sc ON soso.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON soso.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON soso.CreateUserID=su.ID WHERE soso.Type=2 AND soso.AuditStatus=1 AND soso.HospitalID=@HospitalID5
						)  	
	                    SELECT * FROM tree a  WHERE 1=1";

                sql2 = @"  WITH tree as
	                    (SELECT so.ID,so.CustomerID,so.AuditStatus,sc.Name AS CustomerName,('项目单') AS TypeName,('1') AS TypeValue,sh.Name AS HospitalName,su.Name AS CreateUserName,so.CreateTime,so.Remark FROM SmartOrder AS so
	                    LEFT JOIN dbo.SmartCustomer AS sc ON so.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON so.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON so.CreateUserID=su.ID  WHERE so.AuditStatus=1 AND so.HospitalID=@HospitalID1
	                    UNION ALL 
	                    --退预收款订单
	                    SELECT sdro.ID,sdro.CustomerID,sdro.AuditStatus,sc.Name AS CustomerName,('退预收款单') AS TypeName,('3') AS TypeValue,sh.Name AS HospitalName,su.Name AS CreateUserName,sdro.CreateTime,sdro.Remark FROM dbo.SmartDepositRebateOrder AS sdro
	                    LEFT JOIN dbo.SmartCustomer AS sc ON sdro.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON sdro.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON sdro.CreateUserID=su.ID  WHERE sdro.AuditStatus=1 AND sdro.HospitalID=@HospitalID2
	                    --退项目单表
	                    UNION ALL
	                    SELECT sbo.ID,sbo.CustomerID,sbo.AuditStatus,sc.Name AS CustomerName,('退项目单') AS TypeName,('2') AS TypeValue,sh.Name AS HospitalName,su.Name AS CreateUserName,sbo.CreateTime,sbo.Remark FROM dbo.SmartBackOrder AS sbo
	                    LEFT JOIN dbo.SmartCustomer AS sc ON sbo.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON sbo.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON sbo.CreateUserID=su.ID   WHERE sbo.AuditStatus=1 AND sbo.HospitalID=@HospitalID3
						--顾客归属关系变更表(开发人员变更)
					    UNION ALL
						 SELECT soso.ID,soso.CustomerID,soso.AuditStatus,sc.Name AS CustomerName,('开发人变更') AS TypeName,('4') AS TypeValue,
						 sh.Name AS HospitalName,su.Name AS CreateUserName,soso.CreateTime,soso.Content FROM dbo.SmartOwnerShipOrder AS soso
						  LEFT JOIN dbo.SmartCustomer AS sc ON soso.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON soso.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON soso.CreateUserID=su.ID WHERE soso.Type=1 AND soso.AuditStatus=1 AND soso.HospitalID=@HospitalID4
						--顾客归属关系变更表(咨询人员变更)
					    UNION ALL
						SELECT soso.ID,soso.CustomerID,soso.AuditStatus,sc.Name AS CustomerName,('咨询人变更') AS TypeName,('5') AS TypeValue,
						 sh.Name AS HospitalName,su.Name AS CreateUserName,soso.CreateTime,soso.Content FROM dbo.SmartOwnerShipOrder AS soso
						  LEFT JOIN dbo.SmartCustomer AS sc ON soso.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON soso.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON soso.CreateUserID=su.ID WHERE soso.Type=2 AND soso.AuditStatus=1 AND soso.HospitalID=@HospitalID5
						) 	
	                    SELECT COUNT(a.ID) AS Count FROM tree a WHERE 1=1";
                sql += " ORDER by a.ID OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<AuditWorkbenchInfo>(sql, new
                {
                    HospitalID1 = dto.HospitalID,
                    HospitalID2 = dto.HospitalID,
                    HospitalID3 = dto.HospitalID,
                    HospitalID4 = dto.HospitalID,
                    HospitalID5 = dto.HospitalID
                });
                result.Data.PageTotals = _connection.Query<int>(sql2, new
                {
                    HospitalID1 = dto.HospitalID,
                    HospitalID2 = dto.HospitalID,
                    HospitalID3 = dto.HospitalID,
                    HospitalID4 = dto.HospitalID,
                    HospitalID5 = dto.HospitalID
                }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 点击审核跳转到审核界面如果是开发人员或者咨询人员需要传递类型 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">4 开发人员  5咨询人员</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, AuditOrderInfo> GetAuditOrderInfo(long id, string type)
        {
            var result = new IFlyDogResult<IFlyDogResultType, AuditOrderInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<AuditOrderInfo>(@"SELECT soso.ID,soso.CustomerID,sc.Name AS CustomerName,sh.Name AS HospitalName,soso.CreateTime,su.Name AS CreateName,suold.Name AS OldName,soso.OldUserID AS OldID,soso.NewUserID AS [NewID],suNew.Name AS NewName,soso.Content FROM SmartOwnerShipOrder as soso
                    LEFT JOIN dbo.SmartCustomer AS sc ON soso.CustomerID=sc.ID
                     LEFT JOIN dbo.SmartHospital AS sh ON soso.HospitalID=sh.ID
                     LEFT JOIN dbo.SmartUser AS su ON soso.CreateUserID=su.ID
                     LEFT JOIN dbo.SmartUser AS suold ON soso.OldUserID=suold.ID
                      LEFT JOIN dbo.SmartUser AS suNew ON soso.NewUserID=suNew.ID WHERE soso.ID=@ID AND soso.Type=@Type", new { ID = id, Type = type == "4" ? 1 : 2 }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            return result;
        }

        /// <summary>
        /// 查询所有审核记录信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditRecordInfo>>> GetAuditRecord(AuditRecordSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditRecordInfo>>>();
            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<AuditRecordInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"WITH tree as
                        (
                        --项目订单记录
                        SELECT so.ID,sh.Name AS HospitalName,su.Name AS CreateUserName,so.CustomerID,sc.Name AS CustomerName,('项目单') AS TypeName,('1') AS TypeValue,so.Remark,
                        sa.Level,sa.Status AS AuditStatus,sa.CreateTime,sa.Content FROM SmartAudit AS sa
                        LEFT JOIN  dbo.SmartOrder AS so ON so.ID=sa.OrderID
                        LEFT JOIN dbo.SmartCustomer AS sc ON so.CustomerID=sc.ID
                        LEFT JOIN dbo.SmartHospital AS sh ON so.HospitalID=sh.ID
                        LEFT JOIN dbo.SmartUser AS su ON so.CreateUserID=su.ID  WHERE sa.Status=3 or sa.Status=4 AND so.HospitalID=@HospitalID1
                        UNION ALL
                        --退预收款订单
                        SELECT sdro.ID,sh.Name AS HospitalName,su.Name AS CreateUserName,sdro.CustomerID,sc.Name AS CustomerName,('退预收款单') AS TypeName,('3') AS TypeValue,sdro.Remark,
                        sa.Level,sa.Status AS AuditStatus,sa.CreateTime,sa.Content
                         FROM SmartAudit AS sa 
                         LEFT JOIN dbo.SmartDepositRebateOrder AS sdro ON sdro.ID=sa.OrderID
                        LEFT JOIN dbo.SmartCustomer AS sc ON sdro.CustomerID=sc.ID
                        LEFT JOIN dbo.SmartHospital AS sh ON sdro.HospitalID=sh.ID
                        LEFT JOIN dbo.SmartUser AS su ON sdro.CreateUserID=su.ID  WHERE sa.Status=3 or sa.Status=4 AND sdro.HospitalID=@HospitalID2
                        UNION ALL
                        --退项目单表
                        SELECT sbo.ID,sh.Name AS HospitalName,su.Name AS CreateUserName,sbo.CustomerID,sc.Name AS CustomerName,('退项目单') AS TypeName,('2') AS TypeValue,sbo.Remark,
                        sa.Level,sa.Status AS AuditStatus,sa.CreateTime,sa.Content FROM SmartAudit AS sa 
                        LEFT JOIN dbo.SmartBackOrder AS sbo ON sbo.ID=sa.OrderID
                        LEFT JOIN dbo.SmartCustomer AS sc ON sbo.CustomerID=sc.ID
                        LEFT JOIN dbo.SmartHospital AS sh ON sbo.HospitalID=sh.ID
                        LEFT JOIN dbo.SmartUser AS su ON sbo.CreateUserID=su.ID  WHERE sa.Status=3 or sa.Status=4 AND sbo.HospitalID=@HospitalID3
                        --顾客归属关系变更表(开发人员变更)
					    UNION ALL
						SELECT soso.ID,sh.Name AS HospitalName,su.Name AS CreateUserName,soso.CustomerID,sc.Name AS CustomerName,('开发人变更') AS TypeName,('4') AS TypeValue,
						soso.Content AS Remark,sa.Level,sa.Status AS AuditStatus,sa.CreateTime,sa.Content FROM SmartAudit AS sa 
						LEFT JOIN dbo.SmartOwnerShipOrder AS soso ON soso.ID=sa.OrderID
						LEFT JOIN dbo.SmartCustomer AS sc ON soso.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON soso.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON soso.CreateUserID=su.ID WHERE soso.Type=1	AND soso.HospitalID=@HospitalID4			
						--顾客归属关系变更表(咨询人员变更)
					    UNION ALL
						SELECT soso.ID,sh.Name AS HospitalName,su.Name AS CreateUserName,soso.CustomerID,sc.Name AS CustomerName,('咨询人变更') AS TypeName,('5') AS TypeValue,
						soso.Content AS Remark,sa.Level,sa.Status AS AuditStatus,sa.CreateTime,sa.Content FROM SmartAudit AS sa 
						LEFT JOIN dbo.SmartOwnerShipOrder AS soso ON soso.ID=sa.OrderID
						LEFT JOIN dbo.SmartCustomer AS sc ON soso.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON soso.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON soso.CreateUserID=su.ID WHERE soso.Type=2 AND soso.HospitalID=@HospitalID5) 	
                        SELECT * FROM tree a WHERE 1=1";

                sql2 = @"
                       WITH tree as
                        (
                        --项目订单记录
                        SELECT so.ID,sh.Name AS HospitalName,su.Name AS CreateUserName,so.CustomerID,sc.Name AS CustomerName,('项目单') AS TypeName,('1') AS TypeValue,so.Remark,
                        sa.Level,sa.Status AS AuditStatus,sa.CreateTime,sa.Content FROM SmartAudit AS sa
                        LEFT JOIN  dbo.SmartOrder AS so ON so.ID=sa.OrderID
                        LEFT JOIN dbo.SmartCustomer AS sc ON so.CustomerID=sc.ID
                        LEFT JOIN dbo.SmartHospital AS sh ON so.HospitalID=sh.ID
                        LEFT JOIN dbo.SmartUser AS su ON so.CreateUserID=su.ID  WHERE sa.Status=3 or sa.Status=4 AND so.HospitalID=@HospitalID1
                        UNION ALL
                        --退预收款订单
                        SELECT sdro.ID,sh.Name AS HospitalName,su.Name AS CreateUserName,sdro.CustomerID,sc.Name AS CustomerName,('退预收款单') AS TypeName,('3') AS TypeValue,sdro.Remark,
                        sa.Level,sa.Status AS AuditStatus,sa.CreateTime,sa.Content
                         FROM SmartAudit AS sa 
                         LEFT JOIN dbo.SmartDepositRebateOrder AS sdro ON sdro.ID=sa.OrderID
                        LEFT JOIN dbo.SmartCustomer AS sc ON sdro.CustomerID=sc.ID
                        LEFT JOIN dbo.SmartHospital AS sh ON sdro.HospitalID=sh.ID
                        LEFT JOIN dbo.SmartUser AS su ON sdro.CreateUserID=su.ID  WHERE sa.Status=3 or sa.Status=4 AND sdro.HospitalID=@HospitalID2
                        UNION ALL
                        --退项目单表
                        SELECT sbo.ID,sh.Name AS HospitalName,su.Name AS CreateUserName,sbo.CustomerID,sc.Name AS CustomerName,('退项目单') AS TypeName,('2') AS TypeValue,sbo.Remark,
                        sa.Level,sa.Status AS AuditStatus,sa.CreateTime,sa.Content FROM SmartAudit AS sa 
                        LEFT JOIN dbo.SmartBackOrder AS sbo ON sbo.ID=sa.OrderID
                        LEFT JOIN dbo.SmartCustomer AS sc ON sbo.CustomerID=sc.ID
                        LEFT JOIN dbo.SmartHospital AS sh ON sbo.HospitalID=sh.ID
                        LEFT JOIN dbo.SmartUser AS su ON sbo.CreateUserID=su.ID  WHERE sa.Status=3 or sa.Status=4 AND sbo.HospitalID=@HospitalID3
                        --顾客归属关系变更表(开发人员变更)
					    UNION ALL
						SELECT soso.ID,sh.Name AS HospitalName,su.Name AS CreateUserName,soso.CustomerID,sc.Name AS CustomerName,('开发人变更') AS TypeName,('4') AS TypeValue,
						soso.Content AS Remark,sa.Level,sa.Status AS AuditStatus,sa.CreateTime,sa.Content FROM SmartAudit AS sa 
						LEFT JOIN dbo.SmartOwnerShipOrder AS soso ON soso.ID=sa.OrderID
						LEFT JOIN dbo.SmartCustomer AS sc ON soso.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON soso.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON soso.CreateUserID=su.ID WHERE soso.Type=1	AND soso.HospitalID=@HospitalID4				
						--顾客归属关系变更表(咨询人员变更)
					    UNION ALL
						SELECT soso.ID,sh.Name AS HospitalName,su.Name AS CreateUserName,soso.CustomerID,sc.Name AS CustomerName,('咨询人变更') AS TypeName,('5') AS TypeValue,
						soso.Content AS Remark,sa.Level,sa.Status AS AuditStatus,sa.CreateTime,sa.Content FROM SmartAudit AS sa 
						LEFT JOIN dbo.SmartOwnerShipOrder AS soso ON soso.ID=sa.OrderID
						LEFT JOIN dbo.SmartCustomer AS sc ON soso.CustomerID=sc.ID
	                    LEFT JOIN dbo.SmartHospital AS sh ON soso.HospitalID=sh.ID
	                    LEFT JOIN dbo.SmartUser AS su ON soso.CreateUserID=su.ID WHERE soso.Type=2 AND soso.HospitalID=@HospitalID5) 	
                        SELECT COUNT(a.ID) AS Count FROM tree a WHERE 1=1 ";

                if (!string.IsNullOrWhiteSpace(dto.AuditType) && dto.AuditType != "-1")
                {
                    sql += @" AND a.TypeValue =" + dto.AuditType + "";
                    sql2 += @" AND a.TypeValue =" + dto.AuditType + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.AuditBeginTime.ToString()) && !string.IsNullOrWhiteSpace(dto.AuditEndTime.ToString()))
                {
                    string auditTime = dto.AuditEndTime.ToString().Replace("0:00:00", "11:59:59");
                    sql += @" And a.CreateTime between '" + dto.AuditBeginTime + "' and '" + auditTime + "'";
                    sql2 += @" And a.CreateTime between '" + dto.AuditBeginTime + "' and '" + auditTime + "'";
                }

                //if (!string.IsNullOrWhiteSpace(dto.CustormNo))//客户编号 先注释，等到时候确定了在放开
                //{
                //    sql += " And a.CustomerID=" + dto.CustormNo + "";
                //    sql2 += @" And a.CustomerID=" + dto.CustormNo + "";
                //}

                sql += " ORDER by a.ID OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<AuditRecordInfo>(sql, new
                {
                    HospitalID1 = dto.HospitalID,
                    HospitalID2 = dto.HospitalID,
                    HospitalID3 = dto.HospitalID,
                    HospitalID4 = dto.HospitalID,
                    HospitalID5 = dto.HospitalID
                });
                result.Data.PageTotals = _connection.Query<int>(sql2, new
                {
                    HospitalID1 = dto.HospitalID,
                    HospitalID2 = dto.HospitalID,
                    HospitalID3 = dto.HospitalID,
                    HospitalID4 = dto.HospitalID,
                    HospitalID5 = dto.HospitalID
                }).FirstOrDefault();
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
            // var result = new IFlyDogResult<IFlyDogResultType, AuditInfo>();
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
        /// 点击查询查询此类型的审核用户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, AuditUserInfo> GetByType(AuditUserSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, AuditUserInfo>();

            TryExecute(() =>
            {//
                AuditUserInfo aui = new AuditUserInfo();
                aui = _connection.Query<AuditUserInfo>(@"SELECT sard.UserID,sard.Level AS UserLevel,sar.Level AS SumLevel FROM SmartAuditRuleDetail AS sard
                 LEFT JOIN SmartAuditRule AS sar ON sard.RuleID = sar.ID WHERE sar.Type = @Type AND sard.UserID = @UserID  AND sar.HospitalID = @HospitalID", new { Type = dto.Type, UserID = dto.UserID, HospitalID = dto.HospitalID }).FirstOrDefault();

                if (aui == null)
                { //如果对象等于空，说明没有查出来有当前用户存在的审核信息
                    AuditUserInfo auiInfo = new AuditUserInfo();
                    auiInfo.UserID = dto.UserID.ToString(); //当前用户id
                    result.Data = auiInfo;
                }
                else
                {
                    result.Data = aui;
                }
                result.Data.AuditUserData = new List<AuditUserData>();
                List<AuditUserData> list = new List<AuditUserData>();
                list = _connection.Query<AuditUserData>(@" SELECT sard.UserID,sard.Level,('【'+sd.Name+'】'+'【'+su.Name+'】') AS UserNameInfo FROM dbo.SmartAuditRuleDetail AS sard
                     LEFT JOIN SmartAuditRule AS sar ON sard.RuleID=sar.ID
                     LEFT JOIN dbo.SmartUser AS su ON su.ID=sard.UserID
                     LEFT JOIN dbo.SmartDept AS sd ON su.DeptID=sd.ID
                     WHERE sar.Type=@Type AND sar.HospitalID=@HospitalID", new { Type = dto.Type, HospitalID = dto.HospitalID }).ToList();
                if (list != null && list.Count > 0)
                {
                    var auditInfo = GetAuditInfo(dto.OrderID, dto.UserID, Convert.ToInt32(dto.Type));
                    if (auditInfo.Data != null && auditInfo.Data.Count() > 0)
                    {
                        foreach (var item in list)
                        {
                            foreach (var itemData in auditInfo.Data)
                            {
                                if (item.Level == itemData.Level && item.UserID == itemData.UserID)
                                {
                                    if (itemData.Status == "4")
                                    {
                                        item.Status = itemData.Status;
                                        item.AuditInfoDetail = itemData.CreateTime + " " + itemData.AuditUserName + " " + "审核通过" + " " + itemData.Content;
                                    }
                                    else if (itemData.Status == "3")
                                    {
                                        item.Status = itemData.Status;
                                        item.AuditInfoDetail = itemData.CreateTime + " " + itemData.AuditUserName + " " + "审核不通过" + " " + itemData.Content;
                                    }
                                }
                            }
                        }
                    }
                }

                result.Data.AuditUserData = list;
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            return result;
        }

    }
}
