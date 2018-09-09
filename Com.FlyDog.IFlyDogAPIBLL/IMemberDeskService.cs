using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IMemberDeskService
    {
        /// <summary>
        /// 查询会员
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<MemberDeskCustomer>>> Get(MemberDeskCustomerSelect dto);

        /// <summary>
        /// 最近七日生日顾客
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<MemberDeskBirthdayCustomer>>> GetBirthday(long hospitalID);
    }
}
