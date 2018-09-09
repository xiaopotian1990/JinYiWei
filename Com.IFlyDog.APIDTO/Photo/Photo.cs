using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 图片
    /// </summary>
    public class Photo
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 症状
        /// </summary>
        public string SymptomName { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public PhotoType Type { get; set; }
        /// <summary>
        /// 原始图片
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 缩略图
        /// </summary>
        public string ReducedImage { get; set; }
    }

    /// <summary>
    /// 顾客照片
    /// </summary>
    public class CustomerPhoto
    {
        /// <summary>
        /// g构造函数
        /// </summary>
        public CustomerPhoto()
        {
            Consult = new List<Photo>();
            Before = new List<Photo>();
            Under = new List<Photo>();
            After = new List<Photo>();
            Other = new List<Photo>();
        }
        /// <summary>
        /// 咨询
        /// </summary>
        public virtual IList<Photo> Consult { get; set; }
        /// <summary>
        /// 治疗前
        /// </summary>
        public virtual IList<Photo> Before { get; set; }
        /// <summary>
        /// 治疗中
        /// </summary>
        public virtual IList<Photo> Under { get; set; }
        /// <summary>
        /// 治疗后
        /// </summary>
        public virtual IList<Photo> After { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public virtual IList<Photo> Other { get; set; }
    }
}
