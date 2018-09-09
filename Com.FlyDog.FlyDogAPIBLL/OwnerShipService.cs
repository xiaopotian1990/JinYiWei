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
    /// 客户归属权业务逻辑
    /// </summary>
    public class OwnerShipService : BaseService, IOwnerShipService
    {

        /// <summary>
        /// 查询当前医院的客户
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, IEnumerable<SingleCustormInfo>> GetByHospitalID(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SingleCustormInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {// AND  GETDATE() BETWEEN StartTime and EndTime 查询当前医院的所有客户应该不用加 时间限制
                result.Data = _connection.Query<SingleCustormInfo>("SELECT DISTINCT sos.CustomerID FROM dbo.SmartOwnerShip AS sos LEFT JOIN SmartCustomer AS sc ON sos.CustomerID = sc.ID WHERE  sos.HospitalID = @HospitalID", new { HospitalID = hospitalID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 批量设置咨询人员归属权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> BatchConsultantUserAdd(BatchConsultantUser dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证及组合
            if (dto.BatchCustormAdd == null || dto.BatchCustormAdd.Count == 0)
            {
                result.Message = "客户编号不能为空！";
                return result;
            }

            if (Convert.ToInt64(dto.UserID) == 0)
            {
                result.Message = "请选择咨询人员！";
                return result;
            }

            List<BatchConsultantUserTemp> listCustormID = new List<BatchConsultantUserTemp>(); //存储筛选出来的合法用户
            List<BatchConsultantUserUpdateTemp> listCustormUpdateID = new List<BatchConsultantUserUpdateTemp>();
            //存储需要更新时间的客户
            string cuID = string.Empty;//存储不是当前医院的或者不是合法的客户

            var hospitalCustormData = GetByHospitalID(dto.HospitalID); //得到当前医院的客户
            var newUserCustormData = GetByFiltrate("2", dto.UserID, dto.HospitalID);//得到新的咨询人员客户信息
            if (hospitalCustormData.Data == null || hospitalCustormData.Data.Count() == 0)
            {
                result.Message = "当前医院还未有客户，不能批量添加！";
                return result;
            }
            else if (hospitalCustormData.Data != null && hospitalCustormData.Data.Count() > 0)
            {
                foreach (var item in dto.BatchCustormAdd)//开始验证填写的客户id合法性
                {
                    if (!hospitalCustormData.Data.ToList().Exists(o => o.CustomerID == item.CustormID))
                    {//如果当前客户id不存在集合中说明不是当前医院的
                        cuID += item.CustormID + " ";
                    }
                    else
                    {//如果有存在的那就说明是当前医院的，并且是合法的
                        if (!newUserCustormData.Data.ToList().Exists(o => o.CustomerID == item.CustormID))
                        {//如果现在这个用户没有这个客户则添加，已有这个客户了则放弃
                            listCustormID.Add(new BatchConsultantUserTemp()
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                CustomerID = item.CustormID,
                                UserID = dto.UserID,
                                StartTime = DateTime.Now,
                                EndTime1 = "9999-12-31 23:59:59.997",
                                Type = 2,
                                HospitalID = dto.HospitalID,
                                Remark = " "
                            });
                        }

                        if (hospitalCustormData.Data.ToList().Exists(o => o.CustomerID == item.CustormID))
                        {
                            listCustormUpdateID.Add(new BatchConsultantUserUpdateTemp
                            {
                                CustomerID = item.CustormID,
                                EndTime = DateTime.Now,
                                Type = 2,
                                HospitalID = dto.HospitalID
                            });
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(cuID)) {
                result.Message = "以下用户为不合法用户，不能添加：" + cuID;
                return result;
            }

            if (listCustormID == null || listCustormID.Count == 0)
            {//说明一个合法用户都没有
                result.Message = "当前咨询人员已存在客户列表信息，不能重复添加";
                return result;
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {
                #region 开始数据操作动作

                _connection.Execute("UPDATE SmartOwnerShip SET EndTime=@EndTime WHERE Type=@Type AND HospitalID=@HospitalID AND CustomerID=@CustomerID AND  GETDATE() BETWEEN StartTime and EndTime", listCustormUpdateID, _transaction);
                result.Data = _connection.Execute("insert into SmartOwnerShip(ID,CustomerID,UserID,StartTime,EndTime,Type,HospitalID,Remark) VALUES(@ID, @CustomerID, @UserID, @StartTime, @EndTime1, @Type, @HospitalID, @Remark)", listCustormID, _transaction);
             
                var temp = new { 编号 = result.Data, 新咨询人员 = dto.UserID, 医院 = dto.HospitalID };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.BatchConsultantUserAdd,
                    Remark = LogType.BatchConsultantUserAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "设置成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 批量设置开发人员归属权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> BatchDeveloperUserAdd(BatchDeveloperUser dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证及组合
            if (dto.BatchCustormAdd == null || dto.BatchCustormAdd.Count == 0)
            {
                result.Message = "客户编号不能为空！";
                return result;
            }

            if (Convert.ToInt64(dto.UserID) == 0)
            {
                result.Message = "请选择开发人员！";
                return result;
            }

            List<BatchConsultantUserTemp> listCustormID = new List<BatchConsultantUserTemp>(); //存储筛选出来的合法用户
            List<BatchConsultantUserUpdateTemp> listCustormUpdateID = new List<BatchConsultantUserUpdateTemp>();
            //存储需要更新时间的客户
            string cuID = string.Empty;//存储不是当前医院的或者不是合法的客户

            var hospitalCustormData = GetByHospitalID(dto.HospitalID); //得到当前医院的客户
            var newUserCustormData = GetByFiltrate("1", dto.UserID, dto.HospitalID);//得到新的咨询人员客户信息
            if (hospitalCustormData.Data == null || hospitalCustormData.Data.Count() == 0)
            {
                result.Message = "当前医院还未有客户，不能批量添加！";
                return result;
            }
            else if (hospitalCustormData.Data != null && hospitalCustormData.Data.Count() > 0)
            {
                foreach (var item in dto.BatchCustormAdd)//开始验证填写的客户id合法性
                {
                    if (!hospitalCustormData.Data.ToList().Exists(o => o.CustomerID == item.CustormID))
                    {//如果当前客户id不存在集合中说明不是当前医院的
                        cuID += item.CustormID + " ";
                    }
                    else
                    {//如果有存在的那就说明是当前医院的，并且是合法的
                        if (!newUserCustormData.Data.ToList().Exists(o => o.CustomerID == item.CustormID))
                        {//如果现在这个用户没有这个客户则添加，已有这个客户了则放弃
                            listCustormID.Add(new BatchConsultantUserTemp()
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                CustomerID = item.CustormID,
                                UserID = dto.UserID,
                                StartTime = DateTime.Now,
                                EndTime1 = "9999-12-31 23:59:59.997",
                                Type = 1,
                                HospitalID = dto.HospitalID,
                                Remark = " "
                            });
                        }

                        if (hospitalCustormData.Data.ToList().Exists(o => o.CustomerID == item.CustormID))
                        {
                            listCustormUpdateID.Add(new BatchConsultantUserUpdateTemp
                            {
                                CustomerID = item.CustormID,
                                EndTime = DateTime.Now,
                                Type = 1,
                                HospitalID = dto.HospitalID
                            });
                        }
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(cuID))
            {
                result.Message = "以下用户为不合法用户，不能添加：" + cuID;
                return result;
            }

            if (listCustormID == null || listCustormID.Count == 0)
            {//说明一个合法用户都没有
                result.Message = "当前咨询人员已存在客户列表信息，不能重复添加";
                return result;
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {

                #region 开始数据操作动作

                _connection.Execute("UPDATE SmartOwnerShip SET EndTime=@EndTime WHERE Type=@Type AND HospitalID=@HospitalID AND CustomerID=@CustomerID AND  GETDATE() BETWEEN StartTime and EndTime", listCustormUpdateID, _transaction);// AND  GETDATE() BETWEEN StartTime and EndTime

                result.Data =  _connection.Execute("insert into SmartOwnerShip(ID,CustomerID,UserID,StartTime,EndTime,Type,HospitalID,Remark) VALUES(@ID, @CustomerID, @UserID, @StartTime, @EndTime1, @Type, @HospitalID, @Remark)", listCustormID, _transaction);
             
                var temp = new { 编号 = result.Data, 新咨询人员 = dto.UserID, 医院 = dto.HospitalID };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.BatchDeveloperUserAdd,
                    Remark = LogType.BatchDeveloperUserAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "设置成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 查询当前医院客户归属权管理
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<OwnerShipInfo>> Get(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<OwnerShipInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<OwnerShipInfo>(@"SELECT  sos.UserID,su.Name AS UserName,sd.Name AS DeptName,su.Status,(SELECT COUNT(CustomerID) FROM SmartOwnerShip WHERE UserID=sos.UserID AND Type=1 AND GETDATE() BETWEEN StartTime and EndTime) AS DeveloperCount,(SELECT COUNT(CustomerID) FROM SmartOwnerShip WHERE UserID = sos.UserID AND Type = 2 AND GETDATE() BETWEEN StartTime and EndTime) AS ConsultantCount FROM dbo.SmartOwnerShip AS sos
                    LEFT JOIN dbo.SmartUser AS su ON sos.UserID = su.ID
                    LEFT JOIN dbo.SmartDept AS sd ON su.DeptID = sd.ID
                    WHERE sos.HospitalID = @HospitalID AND GETDATE() BETWEEN StartTime and EndTime GROUP BY sos.UserID,su.Name,sd.Name,su.Status", new { HospitalID = hospitalID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据条件查询客户信息
        /// </summary>
        /// <param name="type">1 查询开发类型 2 查询咨询类型</param>
        /// <param name="userID">查询开发或者咨询人员客户</param>
        /// <param name="hospitalID">当前医院</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SingleCustormInfo>> GetByFiltrate(string type, long userID, long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SingleCustormInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SingleCustormInfo>("SELECT DISTINCT CustomerID FROM SmartOwnerShip WHERE UserID=@UserID AND Type=@Type AND HospitalID=@HospitalID AND  GETDATE() BETWEEN StartTime and EndTime", new { UserID = userID, Type = type, HospitalID = hospitalID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 单个添加咨询人员客户归属权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> SingleConsultantUserUpdateAdd(SingleConsultantUserUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            List<BatchConsultantUserTemp> listCustormID = new List<BatchConsultantUserTemp>(); //存储筛选出来的合法用户
            List<BatchConsultantUserUpdateTemp> listCustormUpdateID = new List<BatchConsultantUserUpdateTemp>();
            //存储需要更新时间的客户

            var oldUserCustormData = GetByFiltrate("2", dto.OldUserID, dto.HospitalID);//得到原咨询人员客户信息
            var newUserCustormData = GetByFiltrate("2", dto.NewUserID, dto.HospitalID);//得到新的咨询人员客户信息
            List<SingleCustormInfo> list = newUserCustormData.Data.ToList();

            if (oldUserCustormData.Data == null || oldUserCustormData.Data.Count() == 0)
            {
                result.Message = "原咨询人员无客户信息！";
                return result;
            }
            else if (oldUserCustormData.Data != null && oldUserCustormData.Data.Count() > 0)
            {
                foreach (var item in oldUserCustormData.Data.ToList())
                {
                    //if (!list.Exists(o => o.CustomerID == item.CustomerID))
                    //{
                        listCustormID.Add(new BatchConsultantUserTemp()
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CustomerID = item.CustomerID,
                            UserID = dto.NewUserID,
                            StartTime = DateTime.Now,
                            EndTime1 = "9999-12-31 23:59:59.997",
                            Type = 2,
                            HospitalID = dto.HospitalID,
                            Remark = " "
                        });
                   // }

                    listCustormUpdateID.Add(new BatchConsultantUserUpdateTemp
                    {
                        CustomerID = item.CustomerID,
                        UserID = dto.OldUserID,
                        EndTime = DateTime.Now,
                        Type = 2,
                        HospitalID = dto.HospitalID
                    });
                }
            }

            if (Convert.ToInt64(dto.NewUserID) == 0)
            {
                result.Message = "请选择新的调拨用户！";
                return result;
            }

            #region 开启事物操作
            TryTransaction(() =>
            {
                #region 开始数据操作动作
                _connection.Execute("UPDATE SmartOwnerShip SET EndTime=@EndTime WHERE UserID=@UserID AND Type=@Type AND HospitalID=@HospitalID AND  GETDATE() BETWEEN StartTime and EndTime", listCustormUpdateID, _transaction);

                result.Data = _connection.Execute("insert into SmartOwnerShip(ID,CustomerID,UserID,StartTime,EndTime,Type,HospitalID,Remark) VALUES(@ID, @CustomerID, @UserID, @StartTime, @EndTime1, @Type, @HospitalID, @Remark)", listCustormID, _transaction);
            
                var temp = new { 编号 = result.Data, 新咨询人员 = dto.NewUserID, 原咨询人员 = dto.OldUserID, 医院 = dto.HospitalID };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SingleConsultantUserUpdateAdd,
                    Remark = LogType.SingleConsultantUserUpdateAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "调拨成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 单个添加开发人员客户归属权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> SingleDeveLoperUserUpdateAdd(SingleDeveLoperUserUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            List<BatchConsultantUserTemp> listCustormID = new List<BatchConsultantUserTemp>(); //存储筛选出来的合法用户
            List<BatchConsultantUserUpdateTemp> listCustormUpdateID = new List<BatchConsultantUserUpdateTemp>();
            //存储需要更新时间的客户
            var oldUserCustormData = GetByFiltrate("1", dto.OldUserID, dto.HospitalID);//得到原开发人员客户信息
            var newUserCustormData = GetByFiltrate("1", dto.NewUserID, dto.HospitalID);//得到新的开发人员客户信息

            List<SingleCustormInfo> list = newUserCustormData.Data.ToList();
            if (oldUserCustormData.Data == null || oldUserCustormData.Data.Count() == 0)
            {
                result.Message = "原开发人员无客户信息！";
                return result;
            }
            else if (oldUserCustormData.Data != null && oldUserCustormData.Data.Count() > 0)
            {
                foreach (var item in oldUserCustormData.Data.ToList())
                {
                    //if (!list.Exists(o => o.CustomerID == item.CustomerID))
                    //{
                        listCustormID.Add(new BatchConsultantUserTemp()
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CustomerID = item.CustomerID,
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
                        CustomerID = item.CustomerID,
                        UserID = dto.OldUserID,
                        EndTime = DateTime.Now,
                        Type = 1,
                        HospitalID = dto.HospitalID
                    });
                }
            }

            if (Convert.ToInt64(dto.NewUserID) == 0)
            {
                result.Message = "请选择新的调拨用户！";
                return result;
            }

            #region 开启事物操作
            TryTransaction(() =>
            {
                #region 开始数据操作动作

                _connection.Execute("UPDATE SmartOwnerShip SET EndTime=@EndTime WHERE UserID=@UserID AND Type=@Type AND HospitalID=@HospitalID AND  GETDATE() BETWEEN StartTime and EndTime", listCustormUpdateID, _transaction);
                
                result.Data = _connection.Execute("insert into SmartOwnerShip(ID,CustomerID,UserID,StartTime,EndTime,Type,HospitalID,Remark) VALUES(@ID, @CustomerID, @UserID, @StartTime, @EndTime1, @Type, @HospitalID, @Remark)", listCustormID, _transaction);

                var temp = new { 编号 = result.Data, 新开发人员 = dto.NewUserID, 原开发人员 = dto.OldUserID, 医院 = dto.HospitalID };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SingleDeveLoperUserUpdateAdd,
                    Remark = LogType.SingleDeveLoperUserUpdateAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "调拨成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }
    }
}
