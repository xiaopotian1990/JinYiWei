using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 批量图片上传
    /// </summary>
    public class BatchPhotoAdd
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public long? ChargeID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 症状ID
        /// </summary>
        public long? SymptomID { get; set; }
        /// <summary>
        /// 照片类型
        /// </summary>
        public PhotoType Type { get; set; }
        /// <summary>
        /// 上传时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 图片
        /// </summary>
        public IEnumerable<BatchImage> Images { get; set; }
    }
}
