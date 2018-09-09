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
    /// 店家负责人业务处理类
    /// </summary>
    public class StoreManagerService : BaseService, IStoreManagerService
    {
        /// <summary>
        /// 店家店家负责人
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(StoreManagerAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (string.IsNullOrWhiteSpace(dto.UserID)) {
                result.Message = "请选择负责人!";
                return result;
            }

            if (dto.StoreManagerInfoData==null||dto.StoreManagerInfoData.Count==0) {
                result.Message = "请选择店铺!";
                return result;
            }
            var userStoreData = GetUserID(dto.UserID);//得到当前用户已经拥有的店铺
            TryTransaction(() =>
            {
              

            if (userStoreData == null|| userStoreData.Data.Count()==0) {
                    foreach (var u in dto.StoreManagerInfoData)
                    {
                        _connection.Execute("insert into SmartStoreManager(ID,StoreID,UserID) values(@ID, @StoreID, @UserID)",
                            new
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                StoreID = u.StoreID,
                                UserID = dto.UserID
                            }, _transaction); //店家负责人表
                    };
                }else
                {
                    List<StoreInfoManager> list =userStoreData.Data.ToList();
                    foreach (var u in dto.StoreManagerInfoData)
                    {
                        if (!list.Exists(o => o.StoreID == u.StoreID))
                        {
                            _connection.Execute("insert into SmartStoreManager(ID,StoreID,UserID) values(@ID, @StoreID, @UserID)",
                           new
                           {
                               ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                               StoreID = u.StoreID,
                               UserID = dto.UserID
                           }, _transaction); //店家负责人表
                           
                        }
                        //else
                        //{
                        //    result.Message = "当前用户已管理此店铺，不能重复添加!";
                        //}                                 
                    };
                }
         

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.StoreManagerAdd,
                    Remark = LogType.StoreManagerAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 删除店家负责人下的店铺
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(StoreManagerDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("delete SmartStoreManager where StoreID=@StoreID AND UserID=@UserID", new { StoreID=dto.StoreID, UserID=dto.UserID }, _transaction);

                var temp = new { 店铺id = dto.StoreID,负责人id=dto.UserID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.StoreManagerDelete,
                    Remark = LogType.StoreManagerDelete.ToDescription() + temp.ToJsonString()
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
        /// 删除当前负责人所负责的所有店铺
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> DeleteByUserID(StoreManagerDelete dto){
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("delete SmartStoreManager where  UserID=@UserID", new {UserID = dto.UserID }, _transaction);

                var temp = new { 店铺id = dto.StoreID, 负责人id = dto.UserID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.StoreManagerDelete,
                    Remark = LogType.StoreManagerDelete.ToDescription() + temp.ToJsonString()
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
        /// 根据医院id查询当前医院下的所有店家负责人
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreUserManager>> GetByHospitalID(string hospitalID,string userID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<StoreUserManager>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<StoreUserManager>("SELECT DISTINCT ssm.UserID,su.Name AS UserName,(SELECT Count(StoreID) FROM SmartStoreManager WHERE UserID=ssm.UserID) AS SumCount FROM SmartStoreManager AS ssm LEFT JOIN dbo.SmartUser AS su ON ssm.UserID = su.ID WHERE su.HospitalID = @HospitalID", new { HospitalID= hospitalID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据用户id查询当前用户所管辖的店铺信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreInfoManager>> GetUserID(string userID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<StoreInfoManager>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<StoreInfoManager>("SELECT ssm.StoreID,ss.Name,ss.Linkman,ss.Address FROM SmartStoreManager AS ssm LEFT JOIN dbo.SmartStore AS ss ON ssm.StoreID = ss.ID WHERE ssm.UserID = @UserID", new { UserID = userID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }
    }
}
