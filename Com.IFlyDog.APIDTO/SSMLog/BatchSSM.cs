using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 批量提交
    /// </summary>
    public class BatchSubmit
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BatchSubmit()
        {
            Data = new List<BatchTemp>();
        }
        /// <summary>
        /// appid
        /// </summary>
        public string Appid { get; set; }
        /// <summary>
        /// 批量提交的数据
        /// </summary>
        public List<BatchTemp> Data { get; set; }
    }
    /// <summary>
    /// 临时表
    /// </summary>
    public class BatchTemp
    {
        /// <summary>
        /// 电话，隔开，最多500
        /// </summary>
        public string phones { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 签名
        /// </summary>
        public string sign { get; set; }
    }
}
