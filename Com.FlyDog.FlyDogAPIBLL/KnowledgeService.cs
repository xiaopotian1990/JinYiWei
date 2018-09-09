using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.APIDTO.Knowledge;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class KnowledgeService : BaseService, IKnowledgeService
    {
        /// <summary>
        /// 添加知识管理
        /// </summary>
        /// <param name="dto">知识管理</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(KnowledgeAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Title.IsNullOrEmpty())
            {
                result.Message = "标题不能为空！";
                return result;
            }
            else if (dto.Title.Length > 50)
            {
                result.Message = "标题最多50个字！";
                return result;
            }

            if (dto.RtfContent.IsNullOrEmpty())
            {
                result.Message = "内容不能为空！";
                return result;
            }

            if (dto.CategoryID.IsNullOrEmpty()|| dto.CategoryID=="-1")
            {
                result.Message = "请选择知识分类！";
                return result;
            }

            TryTransaction(() =>
            {

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                result.Data = _connection.Execute("insert into SmartKnowledge(ID,CategoryID,Title,RtfContent,[OpenStatus]) values (@ID,@CategoryID,@Title,@RtfContent,@OpenStatus)",
                    new { ID = id, CategoryID = dto.CategoryID, Title = dto.Title, RtfContent = dto.RtfContent , OpenStatus = dto.OpenStatus, }, _transaction);

                var temp = new { 编号 = result.Data, 标题 = dto.Title, 分类ID = dto.CategoryID };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.KnowledgeAdd,
                    Remark = LogType.KnowledgeAdd.ToDescription() + temp.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 知识管理修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(KnowledgeUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Title.IsNullOrEmpty())
            {
                result.Message = "标题不能为空！";
                return result;
            }
            else if (dto.Title.Length > 50)
            {
                result.Message = "标题最多50个字！";
                return result;
            }

            if (dto.RtfContent.IsNullOrEmpty())
            {
                result.Message = "内容不能为空！";
                return result;
            }

            if (dto.CategoryID.IsNullOrEmpty() || dto.CategoryID == "-1")
            {
                result.Message = "请选择知识分类！";
                return result;
            }


            TryTransaction(() =>
            {

                result.Data = _connection.Execute("update SmartKnowledge set CategoryID=@CategoryID,Title=@Title,RtfContent=@RtfContent,OpenStatus=@OpenStatus where ID = @ID", new { ID = dto.ID, CategoryID = dto.CategoryID, Title = dto.Title, RtfContent = dto.RtfContent, OpenStatus = dto.OpenStatus, }, _transaction);

                var temp = new { 编号 = dto.ID, 标题 = dto.Title, 分类ID = dto.CategoryID };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.KnowledgeUpdate,
                    Remark = LogType.KnowledgeUpdate.ToDescription() + temp.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }


        /// <summary>
        /// 查询所有知识管理
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<KnowledgeInfo>>> Get(KnowledgeSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<KnowledgeInfo>>>();

            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;
                result.Data = new Pages<IEnumerable<KnowledgeInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT sk.ID,sk.CategoryID,skc.Name AS CategoryName,sk.OpenStatus,sk.Title,sk.RtfContent FROM SmartKnowledge AS sk INNER JOIN SmartKnowledgeCategory AS skc ON sk.CategoryID=skc.ID where 1=1";

                sql2 = @"SELECT COUNT(sk.ID) AS Count FROM SmartKnowledge AS sk INNER JOIN SmartKnowledgeCategory AS skc
                            ON sk.CategoryID=skc.ID WHERE 1=1 ";

                if (!string.IsNullOrWhiteSpace(dto.CategoryID) && dto.CategoryID != "-1")
                {
                    sql += @" And sk.CategoryID=" + dto.CategoryID + "";
                    sql2 += @" And sk.CategoryID=" + dto.CategoryID + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.Title))
                {
                    sql += @" AND sk.Title LIKE '%" + dto.Title + "%'";
                    sql2 += @" AND sk.Title LIKE '%" + dto.Title + "%'";
                }

                sql += " ORDER BY sk.OpenStatus DESC OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<KnowledgeInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result; ;
        }

        /// <summary>
        /// 查询ID知识管理
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, KnowledgeInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, KnowledgeInfo>();
            TryExecute(() =>
            {
                result.Data = _connection.Query<KnowledgeInfo>(" SELECT ID,CategoryID,Title,RtfContent,OpenStatus FROM SmartKnowledge WHERE ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
    }
}
