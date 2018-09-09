using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IConsultService
    {
        /// <summary>
        /// 获取顾客咨列表
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Consult>>> GetConsult(long customerID);

        /// <summary>
        /// 添加咨询
        /// </summary>
        /// <param name="dto">咨询内容</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> ConsultAdd(ConsultAddUpdate dto);

        /// <summary>
        /// 咨询修改
        /// </summary>
        /// <param name="dto">咨询内容</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> ConsultUpdate(ConsultAddUpdate dto);

        /// <summary>
        /// 咨询删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> ConsultDelete(ConsultDelete dto);

        /// <summary>
        /// 获取咨询详细信息
        /// </summary>
        /// <param name="ID">咨询记录ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ConsultDetail>> GetConsultDetail(long ID);
    }
}
