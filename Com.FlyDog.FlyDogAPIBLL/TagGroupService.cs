using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.DataAccess;
using Dapper;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 标签组业务处理
    /// </summary>
    public class TagGroupService : BaseService, ITagGroupService
    {
        /// <summary>
        /// 添加标签组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(TagGroupAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            ///判断DTO是否为空
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                result.Message = "名称不能为空!";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "名称最多20个字符！";
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

            if (dto.TagGroupDetailAdd==null||dto.TagGroupDetailAdd.Count <= 0)
            {
                result.Message = "标签项目不能为空!";
                return result;
            }

            #endregion


            TryTransaction(() =>
            {

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                _connection.Execute("insert into SmartTagGroup(ID,Name,Remark) values(@ID, @Name, @Remark)",
                 new { ID = id, Name = dto.Name, Remark = dto.Remark }, _transaction);  //标签记录表

                foreach (var u in dto.TagGroupDetailAdd)
                {
                    _connection.Execute("insert into SmartTagGroupDetail(ID,GroupID,TagID) values(@ID, @GroupID, @TagID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            GroupID = id,
                            TagID = u.TagID
                        }, _transaction); //标签组映射
                };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.TagGroupAdd,
                    Remark = LogType.TagGroupAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 删除标签组信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(TagGroupDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;


            #region 开始事物操作
            TryTransaction(() =>
            {
                _connection.Execute("DELETE SmartTagGroupDetail WHERE GroupID=@GroupID", new { GroupID=dto.ID }, _transaction);
                #region 开始更新操作
                result.Data = _connection.Execute("DELETE SmartTagGroup WHERE ID=@ID", new { ID = dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.TagGroupDelete,
                    Remark = LogType.TagGroupDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 查询所有标签组信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<TagGroupInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<TagGroupInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<TagGroupInfo>("SELECT ID,Name,Remark FROM SmartTagGroup");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;

        }

        /// <summary>
        /// 根据id获取标签组详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, TagGroupInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, TagGroupInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<TagGroupInfo>("SELECT ID,Name,Remark FROM SmartTagGroup WHERE ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.TagGroupDetailAdd = new List<TagGroupDetailAdd>();
                result.Data.TagGroupDetailAdd = _connection.Query<TagGroupDetailAdd>(@"SELECT std.ID,std.GroupID,std.TagID,st.Content AS TagName FROM dbo.SmartTagGroupDetail AS std INNER JOIN dbo.SmartTag AS st
ON std.TagID=st.ID WHERE std.GroupID=@GroupID", new { GroupID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 更新标签组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(TagGroupUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            ///判断DTO是否为空
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                result.Message = "名称不能为空!";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "名称最多20个字符！";
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

            if (dto.TagGroupDetailAdd == null||dto.TagGroupDetailAdd.Count <= 0)
            {
                result.Message = "标签项目不能为空!";
                return result;
            }

            #endregion

            TryTransaction(() =>
            {
                _connection.Execute("DELETE SmartTagGroupDetail WHERE GroupID=@GroupID",
               new { GroupID = dto.ID}, _transaction);  //先把标签记录映射表中相关数据删除

                foreach (var u in dto.TagGroupDetailAdd)
                {
                    _connection.Execute("insert into SmartTagGroupDetail(ID,GroupID,TagID) values(@ID, @GroupID, @TagID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            GroupID = dto.ID,
                            TagID = u.TagID
                        }, _transaction); //标签组映射
                };

                result.Data = _connection.Execute("UPDATE SmartTagGroup SET Name=@Name,Remark=@Remark WHERE ID=@ID",
                new { ID = dto.ID, Name = dto.Name, Remark = dto.Remark }, _transaction);  //标签记录表

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.TagGroupUpdate,
                    Remark = LogType.TagGroupUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
