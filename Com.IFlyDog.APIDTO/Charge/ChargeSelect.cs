using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 收费项目查询
    /// </summary>
  public  class ChargeSelect
    {
        /// <summary>
        ///主键id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 收费项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目分类id
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 当前分页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 类型 等于1的时候需要拼接一个不能查询重复的的条件
        /// </summary>
        public string Type { get; set; }
    }
}
