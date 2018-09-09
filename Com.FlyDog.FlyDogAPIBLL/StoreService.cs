using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.Common;
using Com.IFlyDog.CommonDTO;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class StoreService : BaseService, IStoreService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();

        /// <summary>
        /// 添加店铺
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(StoreAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 数据验证
            //var storeCount = GetByHCCount(dto.ID, dto.CreateUserID);

            //if (storeCount.Data == 0)
            //{
            //    result.Message = "您不是该店铺管理员，不能操作该店铺！";
            //    return result;
            //}
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "名称最多20个字符！";
                return result;
            }

            if (dto.Mobile.IsNullOrEmpty())
            {
                result.Message = "电话不能为空！";
                return result;
            }

            if (dto.Linkman.IsNullOrEmpty())
            {
                result.Message = "联系人不能为空！";
                return result;
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {

                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute(@"insert into SmartStore(ID,Name,Linkman,Mobile,Address,OwnerName,Bank,CardNo,Remark,HospitalID)
                    values(@ID, @Name, @Linkman, @Mobile, @Address, @OwnerName, @Bank, @CardNo, @Remark, @HospitalID)",
                    new
                    {
                        ID = id,
                        Name = dto.Name,
                        Linkman = dto.Linkman,
                        Mobile = dto.Mobile,
                        Address = dto.Address,
                        OwnerName = dto.OwnerName,
                        Bank = dto.Bank,
                        CardNo = dto.CardNo,
                        Remark = dto.Remark,
                        HospitalID = dto.HospitalID
                    }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID =Convert.ToInt64(dto.CreateUserID),
                    Type = LogType.StoreAdd,
                    Remark = LogType.StoreAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.StoreCategory, long.Parse(dto.HospitalID));
                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据店铺id查询客户表看当前店铺是否有数据
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetByStoreIDData(string storeID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) FROM  dbo.SmartCustomer WHERE StoreID=@StoreID", new { StoreID = storeID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        ///  根据医院id，用户id查询当前用户是否有权限操作更新等方法
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> GetByHCCount(string storeID, string crateUserID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>(" SELECT COUNT(ID) FROM  dbo.SmartStoreManager WHERE StoreID=@StoreID AND UserID=@UserID", new { StoreID = storeID, UserID = crateUserID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 删除店家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(StoreDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var storeCount = GetByHCCount(dto.ID, dto.CreateUserID);

            if (storeCount.Data == 0)
            {
                result.Message = "您不是该店铺管理员，不能操作该店铺！";
                return result;
            }

            var storeCustoreNum = GetByStoreIDData(dto.ID);
            if (storeCustoreNum.Data > 0)
            {
                result.Message = "当前店铺存在客户，不能删除!";
                return result;
            }

            #region 开始事物操作
            TryTransaction(() =>
            {
                string sql = @"DELETE SmartStore WHERE ID=@ID
                              DELETE dbo.SmartStoreManager WHERE StoreID = @StoreID";
                #region 开始更新操作
                result.Data = _connection.Execute(sql, new { ID = dto.ID, StoreID = dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=Convert.ToInt64(dto.CreateUserID),
                    Type = LogType.StoreDelete,
                    Remark = LogType.StoreDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.StoreCategory, dto.HospitalID);
                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        ///查询所有店家信息（不分页，主要给弹窗使用）
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreInfo>> GetNoPageStoreInfo(StoreSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<StoreInfo>>();
            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT DISTINCT ss.ID,ss.Name,ss.Linkman,ss.Mobile,ss.Address,ss.OwnerName,ss.Bank,ss.CardNo,ss.Remark,ss.HospitalID
                         FROM dbo.SmartStore AS ss LEFT JOIN dbo.SmartStoreManager AS ssm ON ss.ID=ssm.StoreID WHERE 1=1";
                if (!string.IsNullOrWhiteSpace(dto.Linkman))
                {
                    sql += @" AND ss.Linkman LIKE '%" + dto.Linkman + "%'";
                }
                sql += " And ss.HospitalID='" + dto.HospitalID + "'";
                if (!string.IsNullOrWhiteSpace(dto.Type) && dto.Type == "1")
                {//查询当前用户负责的
                    sql += " AND ssm.UserID='" + dto.CrateUserID + "'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Name)) {
                    sql += @" AND ss.Name LIKE '%" + dto.Name + "%'";
                }
                result.Data = _connection.Query<StoreInfo>(sql);

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;

        }

        /// <summary>
        /// 查询全部店家信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<StoreInfo>>> Get(StoreSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<StoreInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<StoreInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT ss.ID,ss.Name,ss.Linkman,ss.Mobile,ss.Address,ss.OwnerName,ss.Bank,ss.CardNo,ss.Remark,ss.HospitalID FROM dbo.SmartStore AS ss left JOIN dbo.SmartStoreManager AS ssm ON ss.ID=ssm.StoreID WHERE 1=1";

                sql2 = @" SELECT COUNT(ss.ID) as Count FROM dbo.SmartStore AS ss left JOIN dbo.SmartStoreManager AS ssm ON ss.ID=ssm.StoreID WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql += @" AND ss.Name LIKE '%" + dto.Name + "%'";
                    sql2 += @" AND ss.Name LIKE '%" + dto.Name + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Linkman))
                {
                    sql += @" AND ss.Linkman LIKE '%" + dto.Linkman + "%'";
                    sql2 += @" AND ss.Linkman LIKE '%" + dto.Linkman + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Mobile))
                {
                    sql += @" AND ss.Mobile LIKE '%" + dto.Mobile + "%'";
                    sql2 += @" AND ss.Mobile LIKE '%" + dto.Mobile + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.OwnerName))
                {
                    sql += @" AND ss.OwnerName LIKE '%" + dto.OwnerName + "%'";
                    sql2 += @" AND ss.OwnerName LIKE '%" + dto.OwnerName + "%'";
                }

                sql += " And ss.HospitalID='" + dto.HospitalID + "'";
                sql += " And ssm.UserID='" + dto.CrateUserID + "'";
                sql2 += " And ss.HospitalID='" + dto.HospitalID + "'";
                sql2 += " And ssm.UserID='" + dto.CrateUserID + "'";

                sql += " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<StoreInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, StoreInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, StoreInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<StoreInfo>("SELECT ID,Name,Linkman,Mobile,Address,OwnerName,Bank,CardNo,Remark,HospitalID FROM dbo.SmartStore where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据店家id查询店家基础信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, StoreBasicInfo> GetStoreBasicInfo(string id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, StoreBasicInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<StoreBasicInfo>(@" SELECT ss.ID,ss.Name,ss.Linkman,ss.Mobile,ss.Address,ss.OwnerName,ss.Bank,ss.CardNo,ss.Remark, COUNT(sc.ID) AS CustormerNum,
                    SUM(scom.Amount) AS CommissionNum, SUM(ssb.Amount) AS SaleBackNum
                    FROM dbo.SmartStore AS ss
                    LEFT JOIN SmartCustomer AS sc ON ss.ID = sc.StoreID
                    LEFT JOIN SmartCommission AS scom ON ss.ID = scom.StoreID
                    LEFT JOIN SmartSaleBack AS ssb ON ss.ID = ssb.StoreID
                    WHERE ss.ID = @ID GROUP  BY ss.ID,ss.Name,ss.Linkman,ss.Mobile,ss.Address,ss.OwnerName,ss.Bank,ss.CardNo,ss.Remark", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据店家id查询店家管理员信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreManager>> GetStoreManagerInfo(string id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<StoreManager>>();
            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<StoreManager>(@" SELECT ssm.UserID,su.Name,su.Gender,su.Phone FROM SmartStoreManager AS ssm LEFT JOIN dbo.SmartUser AS su
                   ON ssm.UserID=su.ID WHERE ssm.StoreID=@StoreID", new { StoreID = id });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }


        /// <summary>
        /// 根据店家id获取店家基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, StoreBasicInfo> GetByIDStoreBasicData(long id)
        {
            StoreBasicInfo storeBasicInfo = new StoreBasicInfo();
            var result = new IFlyDogResult<IFlyDogResultType, StoreBasicInfo>();
            var getStoreBasicInfoData = GetStoreBasicInfo(id.ToString());//获取店铺信息
            var getStoreManagerInfoData = GetStoreManagerInfo(id.ToString());//获取店家管理员

            if (getStoreBasicInfoData.Data == null)
            {
                result.Message = "查询店家信息异常";
                return result;
            }
            else
            {
                storeBasicInfo.ID = getStoreBasicInfoData.Data.ID;
                storeBasicInfo.Name = getStoreBasicInfoData.Data.Name;
                storeBasicInfo.Linkman = getStoreBasicInfoData.Data.Linkman;
                storeBasicInfo.Mobile = getStoreBasicInfoData.Data.Mobile;
                storeBasicInfo.Address = getStoreBasicInfoData.Data.Address;
                storeBasicInfo.OwnerName = getStoreBasicInfoData.Data.OwnerName;
                storeBasicInfo.Bank = getStoreBasicInfoData.Data.Bank;
                storeBasicInfo.CardNo = getStoreBasicInfoData.Data.CardNo;
                storeBasicInfo.Remark = getStoreBasicInfoData.Data.Remark;
                storeBasicInfo.CustormerNum = getStoreBasicInfoData.Data.CustormerNum;
                storeBasicInfo.CommissionNum = getStoreBasicInfoData.Data.CommissionNum;
                storeBasicInfo.SaleBackNum = getStoreBasicInfoData.Data.SaleBackNum;
            }

            List<StoreManager> storeManagerList = new List<StoreManager>();

            if (getStoreManagerInfoData.Data != null && getStoreManagerInfoData.Data.Count() > 0)
            {
                foreach (var item in getStoreManagerInfoData.Data)
                {
                    StoreManager sm = new StoreManager();
                    sm.UserID = item.UserID;
                    sm.Name = item.Name;
                    sm.Gender = item.Gender;
                    sm.Phone = item.Phone;
                    storeManagerList.Add(sm);
                }
            }
            storeBasicInfo.StoreManagerDateil = storeManagerList;
            result.Data = storeBasicInfo;
            return result;
        }

        /// <summary>
        /// 根据店家id获取店家佣金记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreCommissionInfo>> GetByIDStoreCommissionData(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<StoreCommissionInfo>>();
            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<StoreCommissionInfo>(@" SELECT sh.Name AS HospitalName,sc.CreateTime,su.Name AS CreateUserName,su.Name AS CustomerName,sc.Amount,sc.Remark FROM dbo.SmartCommission AS sc
                    LEFT JOIN dbo.SmartHospital AS sh ON sc.HospitalID=sh.ID
                    LEFT JOIN dbo.SmartUser AS su ON sc.CreateUserID=su.ID
                    LEFT JOIN dbo.SmartUser AS smu ON sc.CustomerID=su.ID
                    WHERE sc.StoreID=@StoreID", new { StoreID = id });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据店家id获取店家客户列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreManagerInfo>> GetByIDStoreManagerData(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<StoreManagerInfo>>();
            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<StoreManagerInfo>(@"SELECT sc.ID,sc.Name,sc.Gender, Convert(varchar(30),sc.CreateTime,23) AS CreateTime,ssy.Name AS SymptomName, Convert(varchar(30),sc.FirstConsultTime,23) AS FirstConsultTime,Convert(varchar(30),sc.FirstDealTime,23) AS FirstDealTime FROM dbo.SmartCustomer AS sc LEFT JOIN SmartSymptom AS ssy ON sc.CurrentConsultSymptomID=ssy.ID WHERE sc.StoreID=@StoreID", new { StoreID = id });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据店家id获取店家回款记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreSaleBackInfo>> GetByIDStoreSaleBackData(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<StoreSaleBackInfo>>();
            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<StoreSaleBackInfo>(@"SELECT sh.Name AS HospitalName,ss.CreateTime,su.Name AS CreateUserName,ss.CreateDate,ss.Amount,ss.Remark  FROM dbo.SmartSaleBack AS ss 
                    LEFT JOIN dbo.SmartHospital AS sh ON ss.HospitalID=sh.ID
                    LEFT JOIN dbo.SmartUser AS su ON ss.CreateUserID=su.ID
                    WHERE ss.StoreID=@StoreID", new { StoreID = id });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }



        /// <summary>
        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.StoreCategory + ":" + hospitalID);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartStore] where HospitalID=@HospitalID order by Name",
                    new { HospitalID = hospitalID });

                _redis.StringSet(RedisPreKey.Category + SelectType.StoreCategory + ":" + hospitalID, result.Data);
            });

            return result;
        }

        /// <summary>
        /// 修改店家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(StoreUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var storeCount = GetByHCCount(dto.ID, dto.CrateUserID);

            if (storeCount.Data == 0)
            {
                result.Message = "您不是该店铺管理员，不能操作该店铺！";
                return result;
            }

            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "名称最多20个字符！";
                return result;
            }

            if (dto.Mobile.IsNullOrEmpty())
            {
                result.Message = "电话不能为空！";
                return result;
            }

            if (dto.Linkman.IsNullOrEmpty())
            {
                result.Message = "联系人不能为空！";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("update SmartStore set Name = @Name,Linkman=@Linkman,Mobile=@Mobile,Address=@Address,OwnerName=@OwnerName,Bank=@Bank,CardNo=@CardNo,Remark=@Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=Convert.ToInt64(dto.CrateUserID),
                    Type = LogType.StoreUpdate,
                    Remark = LogType.StoreUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.StoreCategory, dto.HospitalID);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }


    }
}
