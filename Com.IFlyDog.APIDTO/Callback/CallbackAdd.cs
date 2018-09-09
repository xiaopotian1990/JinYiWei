using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回访添加
    /// </summary>
    public class CallbackAdd
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 回访类型
        /// </summary>
        public long CategoryID { get; set; }
        /// <summary>
        /// 回访工具
        /// </summary>
        public long Tool { get; set; }
        /// <summary>
        /// 回访内容
        /// </summary>
        public string Content { get; set; }
    }

    /// <summary>
    /// 回访添加
    /// </summary>
    public class CallbackAddByDesk
    {
        /// <summary>
        /// 回访记录ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 回访工具
        /// </summary>
        public long Tool { get; set; }
        /// <summary>
        /// 回访内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 是否增加下次回访提醒0：否1：是
        /// </summary>
        public int IsNext { get; set; }
        /// <summary>
        /// 回访人员（下次）
        /// </summary>
        public long NextUserID { get; set; }
        /// <summary>
        /// 回访类型（下次）
        /// </summary>
        public long NextCategoryID { get; set; }
        /// <summary>
        /// 回放日期，具体到天（下次）
        /// </summary>
        public DateTime NextTaskTime { get; set; }
        /// <summary>
        /// 回访计划（下次）
        /// </summary>
        public string NextName { get; set; }
    }
}
