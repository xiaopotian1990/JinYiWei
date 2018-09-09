using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IMemberCategoryService
    {
        /// <summary>
        /// 添加会员卡
        /// </summary>
        /// <param name="dto">会员卡信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(MemberCategoryAdd dto);
        /// <summary>
        /// 更新会员卡信息
        /// </summary>
        /// <param name="dto">会员卡信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(MemberCategoryUpdate dto);
        /// <summary>
        /// 会员卡删除
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(MemberCategoryDelete dto);
        /// <summary>
        /// 查询所有会员卡
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<MemberCategory>> Get();
        /// <summary>
        /// 查询会员卡详细
        /// </summary>
        /// <param name="id">会员卡ID</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, MemberCategory> GetByID(long id);
        /// <summary>
        /// 添加会员权益
        /// </summary>
        /// <param name="dto">会员权益列表</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> AddMemberCategoryEquity(MemberCategoryEquityAdd dto);
        /// <summary>
        /// 查询详细会员权益
        /// </summary>
        /// <param name="id">会员ID</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, MemberCategoryEquity> GetMemberCategoryEquitysByID(long id);

        /// <summary>
        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
