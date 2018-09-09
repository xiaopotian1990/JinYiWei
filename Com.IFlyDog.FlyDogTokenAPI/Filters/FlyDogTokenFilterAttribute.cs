using System.Threading.Tasks;
using Com.JinYiWei.WebAPI.Filters;

namespace Com.IFlyDog.FlyDogTokenAPI.Filters
{
    /// <summary>
    ///     /
    /// </summary>
    public class FlyDogTokenFilterAttribute : TokenFilterAttribute
    {
        /// <summary>
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected override async Task<bool> IsExist(string token)
        {
            return true;
        }
    }
}