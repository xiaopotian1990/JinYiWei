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
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 仓库管理
    /// </summary>
    public class SmartWarehouseService : BaseService, ISmartWarehouseService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加仓库管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartWarehouseAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            ///判断DTO是否为空
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "仓库名称不能为空!";
                return result;
            }
            else if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name.Length > 20)
            {
                result.Message = "仓库名称不可超过20字!";
                return result;
            }
            if (dto.DeptID.IsNullOrEmpty() || dto.DeptID == "-1")
            {
                result.Message = "所属部门不能为空!";
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

            if (dto.SmartWarehouseSetDetail == null || dto.SmartWarehouseSetDetail.Count <= 0)
            {
                result.Message = "仓库管理员详细不可为空!";
                return result;
            }

            #endregion

            TryTransaction(() =>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                _connection.Execute("insert into [SmartWarehouse](ID,Name,Remark,DeptID,UnitType,HospitalID) values (@ID,@Name,@Remark,@DeptID,@UnitType,@HospitalID)",
                 new { ID = id, Name = dto.Name, Remark = dto.Remark, DeptID = dto.DeptID, UnitType = dto.UnitType, HospitalID = dto.HospitalID }, _transaction);

                foreach (var u in dto.SmartWarehouseSetDetail)
                {
                    _connection.Execute("insert into [SmartWarehouseManager](ID,[WarehouseID],[UserID]) values (@ID,@WarehouseID,@UserID)",
                        new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), WarehouseID = id, UserID = u.UserID }, _transaction);
                };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartWarehouseAdd,
                    Remark = LogType.SmartWarehouseAdd.ToDescription() + dto.ToJsonString()
                });

                CacheDelete.WarehouseChange(id, long.Parse(dto.HospitalID), dto.SmartWarehouseSetDetail.Select(u => long.Parse(u.UserID)));

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartWarehouseInfo>> Get(string hospitalId)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartWarehouseInfo>>();
            result.Message = "仓库查询成功";
            result.ResultType = IFlyDogResultType.Success;

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartWarehouseInfo>(@"SELECT sw.ID,sw.Name,dept.ID AS DeptIDValue,dept.Name AS DeptName,sw.UnitType,sw.HospitalID,sw.Remark FROM dbo.SmartWarehouse AS sw INNER JOIN dbo.SmartDept AS dept ON sw.DeptID = dept.ID WHERE sw.HospitalID=@HospitalID", new { HospitalID = hospitalId });
            });

            return result;
        }

        /// <summary>
        /// 查询所有(根据用户id查询用户所能操作的仓库)
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartWarehouseInfo>> GetByUserId(string userId, string hospitalId)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartWarehouseInfo>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartWarehouseInfo>(@"SELECT sw.ID,sw.Name,dept.ID AS DeptIDValue,dept.Name AS DeptName,sw.UnitType,sw.HospitalID,sw.Remark 
                FROM dbo.SmartWarehouse AS sw INNER JOIN dbo.SmartDept AS dept ON sw.DeptID = dept.ID
                WHERE sw.ID IN(SELECT WarehouseID FROM dbo.SmartWarehouseManager WHERE UserID=@UserID) and sw.HospitalID=@HospitalID", 
                new { UserID = userId, HospitalID = hospitalId });
            });

            return result;
        }

        /// <summary>
        /// 通过id查询一条仓库数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartWarehouseInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartWarehouseInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartWarehouseInfo>("SELECT ID,Name,Remark,DeptID as DeptIDValue,UnitType,HospitalID FROM dbo.SmartWarehouse where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Data.SmartWarehouseSetDetail = new List<SmartWarehouseManagerAdd>();
                result.Data.SmartWarehouseSetDetail = _connection.Query<SmartWarehouseManagerAdd>(@" SELECT swm.ID,swm.WarehouseID,swm.UserID,su.Name FROM dbo.SmartWarehouseManager AS swm INNER JOIN dbo.SmartUser AS su ON swm.UserID=su.ID WHERE swm.WarehouseID = @WarehouseID", new { WarehouseID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 这个使用停用因为现在没有字段，所以先不做
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> SmartWarehouseDispose(SmartWarehouseStopOrUse dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartWarehouseUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            ///判断DTO是否为空
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "仓库名称不能为空!";
                return result;
            }
            else if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name.Length > 20)
            {
                result.Message = "仓库名称不可超过20字!";
                return result;
            }
            if (dto.DeptID.IsNullOrEmpty() || dto.DeptID == "-1")
            {
                result.Message = "所属部门不能为空!";
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

            if (dto.SmartWarehouseSetDetail == null || dto.SmartWarehouseSetDetail.Count <= 0)
            {
                result.Message = "仓库管理员详细不可为空!";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {

                int resultData = _connection.Execute("UPDATE SmartWarehouse SET Name=@Name,Remark=@Remark,DeptID=@DeptID,UnitType=@UnitType WHERE ID=@ID", dto, _transaction);

                if (resultData > 0)
                {
                    int delDetail = _connection.Execute("delete from SmartWarehouseManager where WarehouseID = @WarehouseID", new { WarehouseID = dto.ID }, _transaction);//根据仓库id删除仓库详情表中相关数据

                    foreach (var u in dto.SmartWarehouseSetDetail)
                    {
                        _connection.Execute("insert into [SmartWarehouseManager](ID,[WarehouseID],[UserID]) values (@ID,@WarehouseID,@UserID)",
                            new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), WarehouseID = dto.ID, UserID = u.UserID }, _transaction);
                    };
                }

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartWarehouseUpdate,
                    Remark = LogType.SmartWarehouseUpdate.ToDescription() + dto.ToJsonString()
                });

                CacheDelete.WarehouseChange(long.Parse(dto.ID), long.Parse(dto.HospitalID), dto.SmartWarehouseSetDetail.Select(u => long.Parse(u.UserID)));

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 根据仓库id查询库存表中此仓库是否有药物品存在
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetByWarehouseIDData(string warehouseID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(dbo.SmartStock.ID) FROM dbo.SmartStock WHERE WarehouseID=@WarehouseID", new { WarehouseID = warehouseID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 删除仓库信息（注 如果仓库里有商品了就不能再删除了）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(SmartWarehouseDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            var productNum = GetByWarehouseIDData(dto.ID);
            if (productNum.Data > 0)
            {
                result.Message = "当前仓库已经存在药物品，不能删除!";
                return result;
            }
            #region 开始事物操作
            TryTransaction(() =>
            {

                #region 开始更新操作

                var userIDs = _connection.Query<long>("select [UserID] from SmartWarehouseManager WHERE WarehouseID=@WarehouseID", new { WarehouseID = dto.ID }, _transaction);

                result.Data = _connection.Execute("DELETE dbo.SmartWarehouse WHERE ID=@ID", new { ID = dto.ID }, _transaction);
                result.Data = _connection.Execute("DELETE dbo.SmartWarehouseManager WHERE WarehouseID=@WarehouseID", new { WarehouseID = dto.ID }, _transaction);
                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartWarehouseDelete,
                    Remark = LogType.SmartWarehouseDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion
                CacheDelete.WarehouseChange(long.Parse(dto.ID), long.Parse(dto.HospitalID), userIDs);

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.Warehouse + ":" + hospitalID);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartWarehouse] where [HospitalID]=@HospitalID", new { HospitalID = hospitalID });

                _redis.StringSet(RedisPreKey.Category + SelectType.Warehouse + ":" + hospitalID, result.Data);
            });

            return result;
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelectByUserID(long hospitalID, long userID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.WarehouseOfUser + ":" + userID);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT distinct a.ID,a.Name FROM [SmartWarehouse] a,SmartWarehouseManager b where b.WarehouseID=a.ID and b.UserID=@UserID and a.HospitalID=@HospitalID",
                    new { UserID = userID, HospitalID = hospitalID });

                _redis.StringSet(RedisPreKey.Category + SelectType.WarehouseOfUser + ":" + userID, result.Data);
            });

            return result;
        }
    }
}
