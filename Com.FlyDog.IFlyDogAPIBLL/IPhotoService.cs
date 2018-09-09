using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IPhotoService
    {
        /// <summary>
        /// 图片批量上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> BatchAdd(BatchPhotoAdd dto);

        /// <summary>
        /// 查询顾客照片详细
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, CustomerPhoto>> GetByCustomerID(long userID, long customerID);

        /// <summary>
        /// 图片批量上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Delete(PhotoDelete dto);
    }
}
