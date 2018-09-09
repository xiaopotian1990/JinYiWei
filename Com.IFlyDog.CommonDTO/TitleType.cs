using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 题目类型
    /// </summary>
    public enum TitleType
    {
        /// <summary>
        /// 单选题
        /// </summary>
        SingleChoice = 1,
        /// <summary>
        /// 多选题
        /// </summary>
        MultipleChoice = 2,
        /// <summary>
        /// 填空题
        /// </summary>
        Completion = 3,
    }
}
