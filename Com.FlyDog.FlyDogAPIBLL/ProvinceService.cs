using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.Common;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Cache;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class ProvinceService : BaseService, IProvinceService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.Province);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartProvince] order by ID");

                _redis.StringSet(RedisPreKey.Category + SelectType.Province, result.Data);
            });

            return result;
        }

        /// <summary>
        /// 根据省查询市
        /// </summary>
        /// <param name="provinceID">省ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetCity(int provinceID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartCity] where [ProvinceID]=@ProvinceID order by ID", new { ProvinceID = provinceID });
            });

            return result;
        }

        /// <summary>
        /// 根据手机号自动识别省市
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ProvinceCity> GetProvinceCity(string phone)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ProvinceCity>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            if (!phone.IsMobileNumber())
            {
                result.Data = new ProvinceCity() { CityID = "-1", Phone = phone, ProvinceID = "-1" };
                return result;
            }

            var phoneNew = phone.Substring(0, 7);

            TryExecute(() =>
            {
                var temp= _connection.Query<ProvinceCity>("SELECT a.[CityID],b.ProvinceID FROM [SmartMobileInfo] a, SmartCity b where MobileNum=@MobileNum and a.CityID=b.ID", new { MobileNum = phoneNew }).FirstOrDefault();
                if (temp == null)
                {
                    result.Data = new ProvinceCity() { CityID = "-1", Phone = phone, ProvinceID = "-1" };
                }
                else
                {
                    temp.Phone = phone;
                    result.Data = temp;
                }
            });

            return result;
        }
    }
}
