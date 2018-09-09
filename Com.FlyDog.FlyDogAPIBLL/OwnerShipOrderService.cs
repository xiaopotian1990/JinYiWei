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
    /// 客户添加咨询/开发人员变更业务逻辑
    /// </summary>
    public class OwnerShipOrderService : BaseService, IOwnerShipOrderService
    {

        private IAuditRuleService _auditRuleService;
        private IOwnerShipService _ownerShipService;
        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="auditRuleService"></param>
        public OwnerShipOrderService(IAuditRuleService auditRuleService, IOwnerShipService ownerShipService)
        {
            _auditRuleService = auditRuleService;
            _ownerShipService = ownerShipService;
        }
        #endregion

        /// <summary>
        /// 添加/编辑咨询人员变更申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> CustomerConsultanAdd(CustomerConsultanAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (Convert.ToInt64(dto.NewUserID) == 0)
            {
                result.Message = "请选择新的用户！";
                return result;
            }

            var auditGZ = _auditRuleService.GetByHtData(dto.HospitalID.ToString(), 4);//在插入数据之前先判断是否存在审核规则，如果不存在审核规则直接通过，
            List<BatchConsultantUserTemp> listCustormID = new List<BatchConsultantUserTemp>(); //存储筛选出来的合法用户
            List<BatchConsultantUserUpdateTemp> listCustormUpdateID = new List<BatchConsultantUserUpdateTemp>();
            //存储需要更新时间的客户

            TryTransaction(() =>
            {
                if (auditGZ.Data > 0)
                {//说明存在此类型数据审核规则，可以审核
                    result.Data = _connection.Execute("insert into SmartOwnerShipOrder(ID,CustomerID,HospitalID,CreateUserID,CreateTime,Type,Content,OldUserID,NewUserID,AuditStatus) VALUES(@ID, @CustomerID, @HospitalID, @CreateUserID, @CreateTime, @Type, @Content, @OldUserID, @NewUserID, @AuditStatus)", new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        HospitalID = dto.HospitalID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        Type = dto.Type,
                        Content = dto.Content,
                        OldUserID = dto.OldUserID,
                        NewUserID = dto.NewUserID,
                        AuditStatus = 1 //1 待审核
                    }, _transaction);
                }
                else
                {//没有此类型数据的审核规则，SmartOwnerShip表
                    //var newUserCustormData = _ownerShipService.GetByFiltrate("2", dto.NewUserID, dto.HospitalID);//得到新的咨询人员客户信息
                    //List<SingleCustormInfo> list =newUserCustormData.Data.ToList();

                    //if (!list.Exists(o => o.CustomerID == dto.CustomerID))
                    //{//如果新用户不存在此客户
                        listCustormID.Add(new BatchConsultantUserTemp()
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CustomerID = dto.CustomerID,
                            UserID = dto.NewUserID,
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
                        UserID = dto.OldUserID,
                        EndTime = DateTime.Now,
                        Type = 2,
                        HospitalID = dto.HospitalID
                    });

                    _connection.Execute("UPDATE SmartOwnerShip SET EndTime=@EndTime WHERE UserID=@UserID AND Type=@Type AND HospitalID=@HospitalID AND  GETDATE() BETWEEN StartTime and EndTime", listCustormUpdateID, _transaction);

                    result.Data = _connection.Execute("insert into SmartOwnerShip(ID,CustomerID,UserID,StartTime,EndTime,Type,HospitalID,Remark) VALUES(@ID, @CustomerID, @UserID, @StartTime, @EndTime1, @Type, @HospitalID, @Remark)", listCustormID, _transaction);
                }

                var temp = new { 编号 = result.Data, 新咨询人员 = dto.NewUserID, 原咨询人员 = dto.OldUserID, 医院 = dto.HospitalID };

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CustomerConsultanAdd,
                    Remark = LogType.CustomerConsultanAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 添加/ 编辑开发人员变更申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> CustomerDeveloperAdd(CustomerDeveloperAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var userData = GetByIDData(dto.OldUserID, dto.HospitalID);
            if (userData.Data == 0)
            {//说明原开发人员不是当前医院的，不能操作 
                result.Message = "开发人员不在本医院，不能更改";
                return result;
            }

            if (Convert.ToInt64(dto.NewUserID) == 0)
            {
                result.Message = "请选择新的用户！";
                return result;
            }

            var auditGZ = _auditRuleService.GetByHtData(dto.HospitalID.ToString(), 5);//在插入数据之前先判断是否存在审核规则，如果不存在审核规则直接通过，
            List<BatchConsultantUserTemp> listCustormID = new List<BatchConsultantUserTemp>(); //存储筛选出来的合法用户
            List<BatchConsultantUserUpdateTemp> listCustormUpdateID = new List<BatchConsultantUserUpdateTemp>(); //存储需要更新时间的客户

            #region 开启事物操作
            TryTransaction(() =>
            {
                #region 开始数据操作动作
                if (auditGZ.Data > 0)
                {//说明存在此审核规则，新增审核数据
                    result.Data = _connection.Execute("insert into SmartOwnerShipOrder(ID,CustomerID,HospitalID,CreateUserID,CreateTime,Type,Content,OldUserID,NewUserID,AuditStatus) VALUES(@ID, @CustomerID, @HospitalID, @CreateUserID, @CreateTime, @Type, @Content, @OldUserID, @NewUserID, @AuditStatus)", new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        HospitalID = dto.HospitalID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        Type = dto.Type,
                        Content = dto.Content,
                        OldUserID = dto.OldUserID,
                        NewUserID = dto.NewUserID,
                        AuditStatus = 1 //1 待审核
                    }, _transaction);
                }
                else
                {//不存在审核规则，直接通过
                    //var newUserCustormData = _ownerShipService.GetByFiltrate("1", dto.NewUserID, dto.HospitalID);//得到新的开发人员客户信息
                    //List<SingleCustormInfo> list = newUserCustormData.Data.ToList();
                    //if (list != null && list.Count > 0)
                    //{
                    //    //if (!list.Exists(o => o.CustomerID == dto.CustomerID))
                    //    //{
                    //        listCustormID.Add(new BatchConsultantUserTemp()
                    //        {
                    //            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    //            CustomerID = dto.CustomerID,
                    //            UserID = dto.NewUserID,
                    //            StartTime = DateTime.Now,
                    //            EndTime1 = "9999-12-31 23:59:59.997",
                    //            Type = 1,
                    //            HospitalID = dto.HospitalID,
                    //            Remark = " "
                    //        });
                    //   // }
                    //}
                    //else {
                        listCustormID.Add(new BatchConsultantUserTemp()
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CustomerID = dto.CustomerID,
                            UserID = dto.NewUserID,
                            StartTime = DateTime.Now,
                            EndTime1 = "9999-12-31 23:59:59.997",
                            Type = 1,
                            HospitalID = dto.HospitalID,
                            Remark = " "
                        });
                   // }                   

                    listCustormUpdateID.Add(new BatchConsultantUserUpdateTemp
                    {
                        CustomerID = dto.CustomerID,
                        UserID = dto.OldUserID,
                        EndTime = DateTime.Now,
                        Type = 1,
                        HospitalID = dto.HospitalID
                    });

                    _connection.Execute("UPDATE SmartOwnerShip SET EndTime=@EndTime WHERE UserID=@UserID AND Type=@Type AND HospitalID=@HospitalID AND  GETDATE() BETWEEN StartTime and EndTime", listCustormUpdateID, _transaction);

                    result.Data = _connection.Execute("insert into SmartOwnerShip(ID,CustomerID,UserID,StartTime,EndTime,Type,HospitalID,Remark) VALUES(@ID, @CustomerID, @UserID, @StartTime, @EndTime1, @Type, @HospitalID, @Remark)", listCustormID, _transaction);
                }

                var temp = new { 编号 = result.Data, 新开发人员 = dto.NewUserID, 原开发人员 = dto.OldUserID, 医院 = dto.HospitalID };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CustomerDeveloperAdd,
                    Remark = LogType.CustomerDeveloperAdd.ToDescription() + temp.ToJsonString()
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
        /// 根据用户id查询用户是否是当前医院的
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetByIDData(long userID, long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) AS Count FROM dbo.SmartUser WHERE ID=@ID AND HospitalID=@HospitalID", new { ID = userID, HospitalID = hospitalID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 咨询/开发人员变更申请 加载
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CustomerUserInfo> GetCustomerUserInfo(CustomerUserSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CustomerUserInfo>();
            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<CustomerUserInfo>(@"  SELECT sos.ID,sd.Name AS DeptName,su.Name AS UserName,sos.UserID FROM dbo.SmartOwnerShip AS sos  LEFT JOIN dbo.SmartUser AS su ON sos.UserID = su.ID
                    LEFT JOIN dbo.SmartDept AS sd ON su.DeptID = sd.ID
					WHERE sos.CustomerID = @CustomerID AND sos.HospitalID = @HospitalID AND sos.Type = @Type
					 AND  GETDATE() BETWEEN StartTime and EndTime", new { CustomerID = dto.CustomerID, HospitalID = dto.HospitalID, Type = dto.Type }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }
    }
}
