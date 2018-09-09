using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using Com.IFlyDog.APIDTO.CallbackGroup;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Dapper;
using Com.IFlyDog.APIDTO;
using Com.JinYiWei.Common.Extensions;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 回访组设置
    /// </summary>
    public class CallbackGroupService : BaseService, ICallbackGroupService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        /// 

        public IFlyDogResult<IFlyDogResultType, int> Add(SmartCallbackGroupAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            ///判断DTO是否为空
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "回访组不可为空";
                return result;
            }
            else if (dto.Name.Length > 20)
            {
                result.Message = "回访组名称不可超过20字!";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty()) {
                dto.Remark = " ";
            }else  if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "描述最多50字!";
                return result;
            }


            if (dto.CallbackSetDetailAdd.Count <= 0)
            {
                result.Message = "详细不可为空!";
                return result;
            }

            TryTransaction(() =>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                 _connection.Execute("insert into [SmartCallbackSet](ID,Name,[Status],Remark) values (@ID,@Name,@Status,@Remark)",
                  new {ID=id, Name = dto.Name, Status = CallbackGroupStatusType.Normal, Remark = dto.Remark }, _transaction);

                foreach (var u in dto.CallbackSetDetailAdd)
                {
                    _connection.Execute("insert into [SmartCallbackSetDetail](ID,[SetID],[CategoryID],[Name],[Days]) values (@ID,@SetID,@CategoryID,@Name,@Days)",
                        new {ID= SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), SetID = id, CategoryID = u.CategoryID, Name = u.Name, Days = u.Days }, _transaction);
                };

                AddOperationLog(new SmartOperationLog()
                {
                    ID=id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CallbackGroupAdd,
                    Remark = LogType.CallbackGroupAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }



        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartCallbackGroup>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartCallbackGroup>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartCallbackGroup>("SELECT  [ID],[Name],[Status],[Remark] FROM [SmartCallbackSet] order by Status desc,Name");
                result.Message = "回访组查询成功";

                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 回访类型通过ID查询一条数据
        /// </summary>
        public IFlyDogResult<IFlyDogResultType, SmartCallbackGroup> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartCallbackGroup>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartCallbackGroup>("SELECT ID,Name,Remark,Status FROM dbo.SmartCallbackSet WHERE ID=@ID", new { ID = id }).FirstOrDefault();
                result.Data.CallbackSetDetailGet = new List<SmartCallbackSetDetail>();
                result.Data.CallbackSetDetailGet = _connection.Query<SmartCallbackSetDetail>(@"SELECT scsd.SetID,scsd.Name AS DetailRemark,scsd.Days AS DetailSetDays,scsd.CategoryID,scc.Name AS CategoryName FROM dbo.SmartCallbackSetDetail AS scsd LEFT JOIN SmartCallbackCategory AS scc ON scsd.CategoryID=scc.ID WHERE scsd.SetID=@SetID", new { SetID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result; 
        }


        /// <summary>
        /// 使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(SmartCallbackGroupStopOrUse dto)
        {

            var result = new IFlyDogResult<IFlyDogResultType, int>();

            result.ResultType = IFlyDogResultType.Failed;


            TryTransaction(() =>
            {
                int num = _connection.Query<int>("SELECT COUNT(ID) FROM SmartUser Where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID },_transaction).FirstOrDefault();

                ///是否存在数据
                if (num == 0)
                {
                    result.Message = "操作人ID不存在";
                    return false;
                }

                result.Data = _connection.Execute("update [SmartCallbackSet] set [Status]=@Status where ID = @CallbackGroupID", dto, _transaction);

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CallbackGroupStopOrUse,
                    Remark = dto.Status.ToDescription() + "类型ID：" + dto.CallbackGroupID
                });

                result.Message = dto.Status.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>               
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartCallbackGroupUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 20)
            {
                result.Message = "名称最多20个字！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty()) {
                dto.Remark = " ";
            }else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "描述最多50个字！";
                return result;
            }

            if (dto.CallbackSetDetailUpdate.Count <= 0)
            {
                result.Message = "详细不可为空!";
                return result;
            }

            TryTransaction(() =>
            {

                result.Data = _connection.Execute("update SmartCallbackSet set Name = @Name,  Remark = @Remark where ID = @ID", dto, _transaction);

                if (result.Data > 0)
                {
                    int delDetail = _connection.Execute("delete from SmartCallbackSetDetail where SetID = @ID", new { ID = dto.ID }, _transaction);                       
                        foreach (var u in dto.CallbackSetDetailUpdate)
                        {
                            _connection.Execute("insert into [SmartCallbackSetDetail](ID,[SetID],[CategoryID],[Name],[Days]) values (@ID,@SetID,@CategoryID,@Name,@Days)",
                                new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), SetID = dto.ID, CategoryID = u.CategoryID, Name = u.DetailRemark, Days = u.Days }, _transaction);
                        };

                }

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CallbackGroupUpdate,
                    Remark = LogType.CallbackGroupUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
