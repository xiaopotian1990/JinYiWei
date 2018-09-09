using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 角色更新
    /// </summary>
    public class RoleUpdate
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 操作人所在医院ID
        /// </summary>
        public long UserHospitalID { get; set; }
        /// <summary>
        /// 角色所属医院
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 详细角色
        /// </summary>
        public IEnumerable<long> ActionIDS { get; set; }
        /// <summary>
        /// 是否可以接受分诊0:否1：是
        /// </summary>
        public int FZ { get; set; }
        /// <summary>
        /// 是否是医护人员
        /// </summary>
        public int YHRY { get; set; }
        /// <summary>
        /// 是否参与排班
        /// </summary>
        public int CYPB { get; set; }
        /// <summary>
        /// 是否可接受手术预约
        /// </summary>
        public int SSYY { get; set; }
        /// <summary>
        /// 是否允许查看联系方式
        /// </summary>
        public int CKLXFS { get; set; }
        /// <summary>
        /// 是否允许查看药品成本价
        /// </summary>
        public int CKYPCBJ { get; set; }
    }
}
