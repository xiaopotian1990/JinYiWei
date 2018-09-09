using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IStoreService
    {

        /// <summary>
        /// 添加店家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(StoreAdd dto);

        /// <summary>
        /// 修改店家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(StoreUpdate dto);

        /// <summary>
        /// 查询所有店家信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<StoreInfo>>> Get(StoreSelect dto);

        /// <summary>
        ///查询所有店家信息（不分页，主要给弹窗使用）
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<StoreInfo>> GetNoPageStoreInfo(StoreSelect dto);

        /// <summary>
        /// 根据ID获取店家详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, StoreInfo> GetByID(long id);

        /// <summary>
        /// 删除店家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(StoreDelete dto);

        /// <summary>
        /// 根据店家id获取店家基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, StoreBasicInfo> GetByIDStoreBasicData(long id);

        /// <summary>
        /// 根据店家id获取店家客户列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<StoreManagerInfo>> GetByIDStoreManagerData(long id);

        /// <summary>
        /// 根据店家id获取店家佣金记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<StoreCommissionInfo>> GetByIDStoreCommissionData(long id);

        /// <summary>
        /// 根据店家id获取店家回款记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<StoreSaleBackInfo>> GetByIDStoreSaleBackData(long id);

        /// <summary>
        ///  根据医院id，用户id查询当前用户是否有权限操作更新等方法
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> GetByHCCount(string storeID, string crateUserID);

        /// <summary>
        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID);
    }
}
