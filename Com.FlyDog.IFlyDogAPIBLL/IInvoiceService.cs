using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 采购发票接口
    /// </summary>
   public interface IInvoiceService
    {
        /// <summary>
        /// 添加采购发票
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(InvoiceAdd dto);

        /// <summary>
        /// 修改采购发票
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(InvoiceUpdate dto);

        /// <summary>
        /// 查询所有采购发票信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<InvoiceInfo>>> Get(InvoiceSelect dto);

        /// <summary>
        /// 根据ID获取采购发票信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, InvoiceInfo> GetByID(long id);

        /// <summary>
        /// 删除采购发票信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> InvoiceDel(InvoiceDelete dto);
    }
}
