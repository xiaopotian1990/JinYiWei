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
    /// 渠道组业务处理
    /// </summary>
    public class ChannelGroupService : BaseService, IChannelGroupService
    {
        /// <summary>
        /// 添加渠道组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(ChannelGroupAdd dto)
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

            if (string.IsNullOrWhiteSpace(dto.SortNo))
            {
                result.Message = "排序号不能为空!";
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

            if (dto.ChannelGroupDetailAdd == null || dto.ChannelGroupDetailAdd.Count <= 0)
            {
                result.Message = "渠道组详细不能为空!";
                return result;
            }

            #endregion

            TryTransaction(() =>
            {

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                _connection.Execute("insert into SmartChannelGroup(ID,Name,SortNo,Remark) values(@ID, @Name,@SortNo, @Remark)",
                 new { ID = id, Name = dto.Name, SortNo=dto.SortNo, Remark = dto.Remark }, _transaction);  //渠道组组记录表

                foreach (var u in dto.ChannelGroupDetailAdd)
                {
                    _connection.Execute("insert into SmartChannelGroupDetail(ID,GroupID,ChannelID) values(@ID, @GroupID, @ChannelID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            GroupID = id,
                            ChannelID = u.ChannelID
                        }, _transaction); //渠道组映射
                };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.ChannelGroupAdd,
                    Remark = LogType.ChannelGroupAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 删除渠道组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(ChannelGroupDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 开始事物操作
            TryTransaction(() =>
            {
                _connection.Execute("DELETE SmartChannelGroup WHERE ID=@ID", new { ID = dto.ID }, _transaction);
                #region 开始更新操作
                result.Data = _connection.Execute("DELETE SmartChannelGroupDetail WHERE GroupID=@GroupID", new { GroupID = dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.ChannelGroupDelete,
                    Remark = LogType.ChannelGroupDelete.ToDescription() + temp.ToJsonString()
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
        /// 查询全部渠道组
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ChannelGroupInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ChannelGroupInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<ChannelGroupInfo>("SELECT ID,Name,SortNo,Remark FROM SmartChannelGroup");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询渠道组详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ChannelGroupInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ChannelGroupInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<ChannelGroupInfo>("SELECT ID,Name,SortNo,Remark FROM SmartChannelGroup WHERE ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.ChannelGroupDetailAdd = new List<ChannelGroupDetail>();
                result.Data.ChannelGroupDetailAdd = _connection.Query<ChannelGroupDetail>(@"SELECT scg.ChannelID,sc.Name AS ChannelName FROM SmartChannelGroupDetail AS scg LEFT JOIN dbo.SmartChannel AS sc ON scg.ChannelID=sc.ID WHERE scg.GroupID=@GroupID", new { GroupID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 检测所有渠道
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ChannelGroupCheck>> GetChannelGroupCheck()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ChannelGroupCheck>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<ChannelGroupCheck>(@"SELECT sc.ID,sc.Name AS ChannelName,sc.Status,scg.Name AS ChannelGroupName  FROM SmartChannel AS sc LEFT JOIN SmartChannelGroupDetail AS scgd ON scgd.ChannelID=sc.ID
                  LEFT JOIN SmartChannelGroup AS scg ON scg.ID=scgd.GroupID ORDER BY sc.Status DESC");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新渠道组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(ChannelGroupUpdate dto)
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

            if (string.IsNullOrWhiteSpace(dto.SortNo))
            {
                result.Message = "排序号不能为空!";
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

            if (dto.ChannelGroupDetailAdd == null || dto.ChannelGroupDetailAdd.Count <= 0)
            {
                result.Message = "渠道组详细不能为空!";
                return result;
            }

            #endregion

            TryTransaction(() =>
            {
                _connection.Execute("DELETE SmartChannelGroupDetail WHERE GroupID=@GroupID",
               new { GroupID = dto.ID }, _transaction);  //先把渠道记录映射表中相关数据删除

                foreach (var u in dto.ChannelGroupDetailAdd)
                {
                    _connection.Execute("insert into SmartChannelGroupDetail(ID,GroupID,ChannelID) values(@ID, @GroupID, @ChannelID)",
                       new
                       {
                           ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                           GroupID = dto.ID,
                           ChannelID = u.ChannelID
                       }, _transaction); //渠道组映射
                };

                result.Data = _connection.Execute("UPDATE SmartChannelGroup SET Name=@Name,Remark=@Remark,SortNo=@SortNo WHERE ID=@ID",
                new { ID = dto.ID, Name = dto.Name, Remark = dto.Remark, SortNo=dto.SortNo }, _transaction);  //渠道记录表

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.ChannelGroupUpdate,
                    Remark = LogType.ChannelGroupUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
