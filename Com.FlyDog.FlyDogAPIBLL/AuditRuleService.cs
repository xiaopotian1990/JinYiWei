using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Dapper;
using Com.JinYiWei.Common.Extensions;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 审核规则业务处理
    /// </summary>
    public class AuditRuleService : BaseService, IAuditRuleService
    {


        /// <summary>
        /// 新增审核规则时查询当前医院是否已经存在相应的审核规则，如果存在则不能重复添加
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> GetByHtData(string hospitalID, int Type)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) FROM dbo.SmartAuditRule WHERE HospitalID=@HospitalID AND Type=@Type", new { HospitalID = hospitalID, Type = Type }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 新增审核规则
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(AuditRuleAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var htCount = GetByHtData(dto.HospitalID.ToString(), dto.Type);

            if (htCount.Data > 0)
            {
                if (dto.Type == 1)
                {
                    result.Message = "当前已存在订单折扣审核规则，不能重复添加！";
                    return result;
                }
                else if (dto.Type == 2)
                {
                    result.Message = "当前已存在退项目审核规则，不能重复添加！";
                    return result;
                }
                else if (dto.Type == 3)
                {
                    result.Message = "当前已存在退预收款审核规则，不能重复添加！";
                    return result;
                }
                else if (dto.Type == 4)
                {
                    result.Message = "当前已存在咨询人员变更审核规则，不能重复添加！";
                    return result;
                }
                else if (dto.Type == 5)
                {
                    result.Message = "当前已存在开发人员变更审核规则，不能重复添加！";
                    return result;
                }
            }

            #region 数据验证
            if (dto.Type == 0)
            {
                result.Message = "类型不能为空！";
                return result;
            }

            if (dto.AuditRuleDetailAdd == null || dto.AuditRuleDetailAdd.Count == 0)
            {
                result.Message = "审核用户不能为空！";
                return result;
            }

            if (dto.AuditRuleDetailAdd != null && dto.AuditRuleDetailAdd.Count > 0)
            {
                int a = 0;
                foreach (var item in dto.AuditRuleDetailAdd)
                {
                    if (Convert.ToInt32(item.Level) > a)
                    {//得到用户审核级数中的最大级别
                        a = Convert.ToInt32(item.Level);
                    }
                }

                string mess = string.Empty;
                if (dto.Level > a)
                {//如果选择的审核级数大于用户审核级别，则说明肯定用户审核级数有没选择用户的

                    if (dto.Level == 2)
                        mess = "二级审核人不能为空！";
                    else if (dto.Level == 3)
                        mess = "三级审核人不能为空！";
                    else if (dto.Level == 4)
                        mess = "四级审核人不能为空！";
                    else if (dto.Level == 5)
                        mess = "五级审核人不能为空！";
                    result.Message = mess;
                    return result;
                }
            }

            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {

                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute(@"insert into SmartAuditRule(ID,Type,Amount,Discount,Level,Remark,Status,HospitalID) VALUES(@ID, @Type, @Amount, @Discount, @Level, @Remark, @Status, @HospitalID)", new { ID = id, Type = dto.Type, Amount = dto.Amount, Discount = dto.Discount, Level = dto.Level, Remark = dto.Remark, Status = dto.Status, HospitalID = dto.HospitalID }, _transaction);

                foreach (var u in dto.AuditRuleDetailAdd)
                {
                    _connection.Execute("insert into SmartAuditRuleDetail(ID,RuleID,UserID,Level) values(@ID, @RuleID, @UserID, @Level)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            RuleID = id,
                            UserID = u.UserID,
                            Level = u.Level
                        }, _transaction); //审核规则详情表
                };

                var temp = new { 编号 = result.Data, 类型 = dto.Type, 金额 = dto.Amount, 折扣 = dto.Discount, 级别 = dto.Level, 医院 = dto.HospitalID };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.AuditRuleAdd,
                    Remark = LogType.AuditRuleAdd.ToDescription() + temp.ToJsonString()
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
        /// 查询当前医院的审核用户信息
        /// </summary>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, IEnumerable<AuditRuleDetailInfo>> GetAuditRuleDetail(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AuditRuleDetailInfo>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<AuditRuleDetailInfo>("SELECT sard.ID,sard.UserID,sard.RuleID,sard.Level FROM dbo.SmartAuditRuleDetail AS sard LEFT JOIN SmartAuditRule AS sar ON sar.ID = sard.RuleID WHERE sar.HospitalID = @HospitalID", new { HospitalID = hospitalID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            return result;
        }

        /// <summary>
        /// 查询当前医院下的审核规则
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<AuditRuleInfo>> Get(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AuditRuleInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<AuditRuleInfo>("SELECT sar.ID,sar.Level,sar.Discount,sar.Amount,sar.Type,sar.Remark,sar.Status FROM dbo.SmartAuditRule AS sar WHERE sar.HospitalID=@HospitalID", new { HospitalID = hospitalID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询审核规则详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, AuditRuleInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, AuditRuleInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<AuditRuleInfo>("SELECT ID,Type,Amount,Discount,Level,Remark,Status,HospitalID FROM dbo.SmartAuditRule WHERE ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.AuditUserInfoAdd = new List<AuditUser>();
                result.Data.AuditUserInfoAdd = _connection.Query<AuditUser>(@"SELECT su.ID,su.Name,su.Account,sard.Level FROM SmartAuditRuleDetail AS sard
                    LEFT JOIN dbo.SmartUser AS su ON sard.UserID=su.ID WHERE sard.RuleID=@RuleID", new { RuleID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }



        /// <summary>
        /// 查询出所有待审核数据的类型  AuditWorkbenchInfo
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, IEnumerable<AuditWorkbenchInfo>> GetDSData(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AuditWorkbenchInfo>>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<AuditWorkbenchInfo>(@"WITH tree as
                        (SELECT('1') AS TypeValue FROM SmartOrder AS so

                        LEFT JOIN dbo.SmartCustomer AS sc ON so.CustomerID = sc.ID

                        LEFT JOIN dbo.SmartHospital AS sh ON so.HospitalID = sh.ID

                        LEFT JOIN dbo.SmartUser AS su ON so.CreateUserID = su.ID  WHERE so.AuditStatus = 1 AND so.HospitalID = @HospitalID1

                        UNION ALL
                        --退预收款订单

                        SELECT('3') AS TypeValue FROM dbo.SmartDepositRebateOrder AS sdro

                        LEFT JOIN dbo.SmartCustomer AS sc ON sdro.CustomerID = sc.ID

                        LEFT JOIN dbo.SmartHospital AS sh ON sdro.HospitalID = sh.ID

                        LEFT JOIN dbo.SmartUser AS su ON sdro.CreateUserID = su.ID  WHERE sdro.AuditStatus = 1 AND sdro.HospitalID = @HospitalID2
                        --退项目单表

                        UNION ALL

                        SELECT('2') AS TypeValue FROM dbo.SmartBackOrder AS sbo

                        LEFT JOIN dbo.SmartCustomer AS sc ON sbo.CustomerID = sc.ID

                        LEFT JOIN dbo.SmartHospital AS sh ON sbo.HospitalID = sh.ID

                        LEFT JOIN dbo.SmartUser AS su ON sbo.CreateUserID = su.ID   WHERE sbo.AuditStatus = 1 AND sbo.HospitalID = @HospitalID3
                        --顾客归属关系变更表(开发人员变更)

                        UNION ALL

                         SELECT('4') AS TypeValue FROM dbo.SmartOwnerShipOrder AS soso

                          LEFT JOIN dbo.SmartCustomer AS sc ON soso.CustomerID = sc.ID

                        LEFT JOIN dbo.SmartHospital AS sh ON soso.HospitalID = sh.ID

                        LEFT JOIN dbo.SmartUser AS su ON soso.CreateUserID = su.ID WHERE soso.Type = 1 AND soso.HospitalID = @HospitalID4
                        --顾客归属关系变更表(咨询人员变更)

                        UNION ALL

                        SELECT('5') AS TypeValue FROM dbo.SmartOwnerShipOrder AS soso

                          LEFT JOIN dbo.SmartCustomer AS sc ON soso.CustomerID = sc.ID

                        LEFT JOIN dbo.SmartHospital AS sh ON soso.HospitalID = sh.ID

                        LEFT JOIN dbo.SmartUser AS su ON soso.CreateUserID = su.ID WHERE soso.Type = 2 AND soso.HospitalID = @HospitalID5
                        )SELECT * FROM tree a  WHERE 1 = 1", new {
                        HospitalID1 = hospitalID,
                        HospitalID2 = hospitalID,
                        HospitalID3 = hospitalID,
                        HospitalID4 = hospitalID,
                        HospitalID5 = hospitalID
                    }, _transaction);
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }



        /// <summary>
        /// 启用停用审核规则
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(AuditRuleStopOrUse dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Status==0) {//停用的时候才判断
                var RuleData = GetByID(dto.ID); //得到审核规则信息
                var AuditData = GetDSData(dto.HospitalID);

                if (AuditData.Data.ToList().Exists(o => o.TypeValue == RuleData.Data.Type.ToString()))
                {
                    result.Message = "当前审核规则正在被使用，不能停用!";
                    return result;
                }
            }        

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("update SmartAuditRule set Status = @Status where ID = @ID", new { Status = dto.Status, ID = dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID, 状态 = dto.Status };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.AuditRuleState,
                    Remark = LogType.AuditRuleState.ToDescription() + temp.ToJsonString()
                });
                #endregion
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 更新审核规则
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(AuditRuleUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Type == 0)
            {
                result.Message = "类型不能为空！";
                return result;
            }

            if (dto.AuditRuleDetailAdd == null || dto.AuditRuleDetailAdd.Count == 0)
            {
                result.Message = "审核用户不能为空！";
                return result;
            }

            if (dto.AuditRuleDetailAdd != null && dto.AuditRuleDetailAdd.Count > 0)
            {
                int a = 0;
                foreach (var item in dto.AuditRuleDetailAdd)
                {
                    if (Convert.ToInt32(item.Level) > a)
                    {//得到用户审核级数中的最大级别
                        a = Convert.ToInt32(item.Level);
                    }
                }

                string mess = string.Empty;
                if (dto.Level > a)
                {//如果选择的审核级数大于用户审核级别，则说明肯定用户审核级数有没选择用户的

                    if (dto.Level == 2)
                        mess = "二级审核人不能为空！";
                    else if (dto.Level == 3)
                        mess = "三级审核人不能为空！";
                    else if (dto.Level == 4)
                        mess = "四级审核人不能为空！";
                    else if (dto.Level == 5)
                        mess = "五级审核人不能为空！";
                    result.Message = mess;
                    return result;
                }
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {

                _connection.Execute(@"DELETE SmartAuditRuleDetail WHERE RuleID = @RuleID",
                      new
                      {
                          RuleID = dto.ID
                      }, _transaction); //先把关联表中的数据删掉

                #region 开始数据操作动作
                result.Data = _connection.Execute(@"update SmartAuditRule set Amount = @Amount,Discount=@Discount,Level=@Level,Remark=@Remark where ID = @ID",
                    new { ID = dto.ID, Amount = dto.Amount, Discount = dto.Discount, Level = dto.Level, Remark = dto.Remark }, _transaction);

                foreach (var u in dto.AuditRuleDetailAdd)
                {
                    _connection.Execute("insert into SmartAuditRuleDetail(ID,RuleID,UserID,Level) values(@ID, @RuleID, @UserID, @Level)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            RuleID = dto.ID,
                            UserID = u.UserID,
                            Level = u.Level
                        }, _transaction); //审核规则详情表
                };

                var temp = new { 编号 = result.Data, 类型 = dto.Type, 金额 = dto.Amount, 折扣 = dto.Discount, 级别 = dto.Level, 医院 = dto.HospitalID };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.AuditRuleUpdate,
                    Remark = LogType.AuditRuleUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;

                return true;
            });
            #endregion
            return result;
        }
    }
}
