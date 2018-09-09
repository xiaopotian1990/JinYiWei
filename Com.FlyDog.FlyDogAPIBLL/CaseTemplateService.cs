using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.DataAccess;
using Dapper;
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 病例模板业务逻辑s
    /// </summary>
    public class CaseTemplateService : BaseService, ICaseTemplateService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加病例模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(CaseTemplateAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 数据验证
            if (dto.Title.IsNullOrEmpty())
            {
                result.Message = "标题不能为空！";              
                return result;
            }
            else if (!dto.Title.IsNullOrEmpty() && dto.Title.Length >= 50)
            {
                result.Message = "标题最多50个字符！";
                return result;
            }

            if (dto.RtfContent.IsNullOrEmpty())
            {
                result.Message = "病例模板内容不能为空！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字符！";
                return result;
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {

                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute("insert into SmartCaseTemplate(ID,Title,Remark,RtfContent,OpenStatus) values (@ID,@Title,@Remark,@RtfContent,@OpenStatus)",
                    new { ID = id, Title = dto.Title, Remark = dto.Remark, RtfContent = dto.RtfContent, OpenStatus = dto.OpenStatus }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Title };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.CaseTemplateAdd,
                    Remark = LogType.CaseTemplateAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.CaseTemplate);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 查询全部病例模板信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CaseTemplateInfo>> Get(CaseTemplateSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CaseTemplateInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                string sql = "SELECT ID,Title,Remark,RtfContent,OpenStatus FROM SmartCaseTemplate where 1=1 ";

                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    sql += @" AND Title LIKE '%" + dto.Title + "%'";
                }

                sql += " ORDER BY OpenStatus DESC";

                result.Data = _connection.Query<CaseTemplateInfo>(sql);
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询病例模板详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CaseTemplateInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CaseTemplateInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<CaseTemplateInfo>("SELECT ID,Title,Remark,RtfContent,OpenStatus FROM SmartCaseTemplate WHERE ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 修改病例模板s
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(CaseTemplateUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Title.IsNullOrEmpty())
            {
                result.Message = "标题不能为空！";
                return result;
            }
            else if (!dto.Title.IsNullOrEmpty() && dto.Title.Length >= 50)
            {
                result.Message = "标题最多50个字符！";
                return result;
            }

            if (dto.RtfContent.IsNullOrEmpty())
            {
                result.Message = "病例模板内容不能为空！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字符！";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {

                #region 开始更新操作
                result.Data = _connection.Execute("update SmartCaseTemplate set Title = @Title,Remark=@Remark,RtfContent=@RtfContent,OpenStatus=@OpenStatus where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Title };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.CaseTemplateUpdate,
                    Remark = LogType.CaseTemplateUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.CaseTemplate);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.CaseTemplate);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Title] as Name FROM [SmartCaseTemplate] where [OpenStatus]=@Status order by Title", new { Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.CaseTemplate, result.Data);
            });

            return result;
        }

    }
}
