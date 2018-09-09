using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 银行卡相关接口
    /// </summary>
    public interface ICardCategoryService
    {
        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <param name="dto">银行卡信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(CardCategoryAdd dto);

        /// <summary>
        /// 更新银行卡信息
        /// </summary>
        /// <param name="dto">银行卡信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(CardCategoryUpdate dto);

        /// <summary>
        /// 银行卡使用停用
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(CardCategoryStopOrUse dto);

        /// <summary>
        /// 查询所有银行卡
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<CardCategory>> Get();

        /// <summary>
        /// 查询银行卡详细
        /// </summary>
        /// <param name="id">银行卡ID</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, CardCategory> GetByID(long id);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
