using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.Common;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Cache;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 医院信息业务逻辑
    /// </summary>
    public class HospitalService : BaseService, IHospitalService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();

        /// <summary>
        /// 查询医院
        /// </summary>
        /// <param name="id">查询所有医院输入0，其他输入医院ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<HospitalInfo>> Get(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<HospitalInfo>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var hospitalInfos = _redis.StringGet<IEnumerable<HospitalInfo>>(RedisPreKey.Category + SelectType.Hospital + ":" + id);

            if (hospitalInfos != null)
            {
                result.Data = hospitalInfos;
                return result;
            }

            string sql_where = "";
            if (id == 0)
            {
                sql_where = " PID is null ";
            }
            else
            {
                sql_where = " ID=@ID ";
            }
            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<HospitalInfo>(string.Format(
                    @"with tree as 
                     ( 
                     select [ID],[Name],[Address],[Remark],SortNo,Cast(RANK() OVER(order by SortNo,Name) as nvarchar(4000)) Code,cast('' as varchar) as prex from SmartHospital where {0}
                     union all
                     select a.[ID],a.Name,a.[Address],a.[Remark],a.SortNo,b.Code +Cast(RANK() OVER(order by a.SortNo,a.Name) as nvarchar(4000)),cast(b.prex+'··' as varchar) from SmartHospital a,tree b where a.PID=b.ID 
                     ) 
                     select ID,prex+Name as Name,Address,Remark,SortNo from tree order by Code OPTION(MAXRECURSION 0)", sql_where),
                    new { ID = id }
                    );
                _redis.StringSet(RedisPreKey.Category + SelectType.Hospital + ":" + id, result.Data);
            });
            #endregion
            return result;
        }


        /// <summary>
        /// 是否拥有操作该医院数据的权限
        /// </summary>
        /// <param name="userHositalID">操作人所在医院</param>
        /// <param name="hositalID">选择的操作的医院</param>
        /// <returns></returns>
        public bool HasHospital(long userHositalID, long hositalID)
        {
            var result = this.Get(userHositalID);
            if (result.ResultType != IFlyDogResultType.Success)
            {
                return false;
            }
            foreach (var u in result.Data)
            {
                if (u.ID == hositalID.ToString())
                    return true;
            }

            return false;
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

            var hospitals = this.Get(hospitalID);
            if (hospitals.ResultType != IFlyDogResultType.Success)
            {
                result.Message = "查询失败";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            result.Data = hospitals.Data.Select(u => new Select() { ID = u.ID, Name = u.Name });

            return result;
        }
    }
}
