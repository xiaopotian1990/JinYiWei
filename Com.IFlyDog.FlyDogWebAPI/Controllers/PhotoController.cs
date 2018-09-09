using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 图片上传接口
    /// </summary>
    public class PhotoController : ApiController
    {
        private IPhotoService _photoService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="photoService"></param>
        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        /// <summary>
        /// 图片批量上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> BatchAdd(BatchPhotoAdd dto)
        {
            return await _photoService.BatchAdd(dto);
        }

        /// <summary>
        /// 查询顾客照片详细
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, CustomerPhoto>> GetByCustomerID(long userID, long customerID)
        {
            return await _photoService.GetByCustomerID(userID, customerID);
        }

        /// <summary>
        /// 图片批量上传
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(PhotoDelete dto)
        {
            return await _photoService.Delete(dto);
        }
    }
}
