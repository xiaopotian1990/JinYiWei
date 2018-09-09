using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class DeptService : BaseService, IDeptService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        private IHospitalService _hospitalService;
        public DeptService(IHospitalService hospitalService)
        {
            _hospitalService = hospitalService;
        }
        /// <summary>
        /// 添加部门
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartDeptAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "部门名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 20)
            {
                result.Message = "部门名称最多20个字！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty()) {
                dto.Remark = " ";
            }else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            if (!_hospitalService.HasHospital(dto.UserHospitalID, dto.HospitalID))
            {
                result.Message = "对不起，您无权操作其他家的医院！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            TryTransaction(() =>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                result.Data = _connection.Execute("insert into SmartDept(ID,Name,Remark,OpenStatus,SortNo,HospitalID) values (@ID,@Name,@Remark,@OpenStatus,@SortNo,@HospitalID)",
                    new { ID = id, Name = dto.Name, OpenStatus = CommonStatus.Use, SortNo = dto.SortNo, Remark = dto.Remark, HospitalID = dto.HospitalID }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name, 排序 = dto.SortNo, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.DeptAdd,
                    Remark = LogType.DeptAdd.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.DeptDelete(id, dto.UserHospitalID); //部门添加成功之后更新当前医院所拥有的部门缓存
                result.Message = "部门添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 部门修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartDeptUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "部门名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 20)
            {
                result.Message = "部门名称最多20个字！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty()) {
                dto.Remark = " ";
            }else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            if (!_hospitalService.HasHospital(dto.UserHospitalID, dto.HospitalID))
            {
                result.Message = "对不起，您无权操作其他家的医院！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            TryTransaction(() =>
            {

                result.Data = _connection.Execute("update SmartDept set Name = @Name, SortNo = @SortNo, Remark = @Remark,OpenStatus=@OpenStatus, HospitalID=@HospitalID where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 排序 = dto.SortNo, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.DeptUpdate,
                    Remark = LogType.DeptUpdate.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.DeptUpdate(dto.ID, dto.UserHospitalID); //更新部门信息之后更新部门缓存

                result.Message = "部门修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }



        /// <summary>
        /// 查询当前医院下的所有直接部门
        /// </summary>
        /// <param name="hospitalID">所属医院ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartDept>> Get(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartDept>>();
            result.Message = "部门查询成功";
            result.ResultType = IFlyDogResultType.Success;

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartDept>("SELECT ID,Name,Remark,OpenStatus,SortNo,HospitalID FROM dbo.SmartDept WHERE HospitalID=@HospitalID", new { HospitalID = hospitalID });
            });

            return result;
        }

        /// <summary>
        /// 根据ID获取部门信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartDept> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartDept>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartDept>("SELECT a.[ID],a.[Name],a.[Remark],[OpenStatus],a.[SortNo],a.[HospitalID],b.Name as HospitalName FROM [SmartDept] a,SmartHospital b where a.HospitalID=b.ID and a.ID=@ID order by a.OpenStatus desc,a.SortNo,a.Name", new { ID = id }).FirstOrDefault();
                result.Message = "部门查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.Dept + ":" + hospitalID);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartDept] where HospitalID=@HospitalID order by SortNo,Name",
                    new { HospitalID = hospitalID });

                _redis.StringSet(RedisPreKey.Category + SelectType.Dept + ":" + hospitalID, result.Data);
            });

            return result;
        }

        /// <summary>
        /// 治疗部门下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetTreatDeptSelect(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.TreatDept + ":" + hospitalID);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartDept] where [OpenStatus]=@Status and HospitalID=@HospitalID order by SortNo,Name",
                    new { Status = DeptStatus.Treat, HospitalID = hospitalID });

                _redis.StringSet(RedisPreKey.Category + SelectType.TreatDept + ":" + hospitalID, result.Data);
            });

            return result;
        }

        /// <summary>
        /// 根据id查询部门是否被引用
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetByDeptIDData(string deptID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) FROM dbo.SmartUser WHERE DeptID=@DeptID", new { DeptID = deptID}, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }


        /// <summary>
        /// 删除部门（删除之前要先判断有没有引用）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(DeptDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var deptNum = GetByDeptIDData(dto.ID.ToString());

            if (deptNum.Data > 0)
            {
                result.Message = "当前部门已被使用，不能删除!";
                return result;
            }

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("delete SmartDept where ID=@ID", dto, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.SmartUnitDelete,
                    Remark = LogType.SmartUnitDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.Dept);

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;

        }
    }
}
