using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    public class SubMenuDTO
    {
        public SubMenuDTO(string id, string name, string url, MenuModuleDTO module)
        {
            this.ID = id;
            this.Name = name;
            this.Url = url;
            this.Moudle = module;
        }

        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 模块
        /// </summary>
        public MenuModuleDTO Moudle { get; set; }
    }

 
}
