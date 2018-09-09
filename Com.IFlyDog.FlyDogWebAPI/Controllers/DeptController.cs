using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 部门相关API接口
    /// </summary>
    public class DeptController : ApiController
    {
        private IDeptService _deptService;
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="deptService"></param>
        public DeptController(IDeptService deptService)
        {
            _deptService = deptService;
        }

        /// <summary>
        /// 添加部门[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">部门信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]SmartDeptAdd dto)
        {
            return _deptService.Add(dto);
        }

        /// <summary>
        /// 部门修改[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">部门信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]SmartDeptUpdate dto)
        {
            return _deptService.Update(dto);
        }

        /// <summary>
        /// 查询所有部门[所属角色("CRM")]
        /// </summary>
        /// <param name="hospitalID">所属医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartDept>> Get(long hospitalID)
        {
            return _deptService.Get(hospitalID);
        }

        /// <summary>
        /// 根据ID获取部门信息
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, SmartDept> GetByID(long id)
        {
            return _deptService.GetByID(id);
        }

        /// <summary>
        /// 删除部门（删除之前要先判断有没有引用）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete(DeptDelete dto)
        {
            return _deptService.Delete(dto);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            return _deptService.GetSelect(hospitalID);
        }

        /// <summary>
        /// 治疗部门下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetTreatDeptSelect(long hospitalID)
        {
            return _deptService.GetTreatDeptSelect(hospitalID);
        }
    }
}
