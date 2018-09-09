using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.Common;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Cache;
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
    public class MemberCategoryService : BaseService, IMemberCategoryService
    {

        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 根据id查询当前会员卡表中的最大值信息
        /// </summary>
        /// <param name="id">会员卡ID</param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, MemberCategory> GetMemberMaxInfo()
        {
            var result = new IFlyDogResult<IFlyDogResultType, MemberCategory>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<MemberCategory>("SELECT max([Level]) as MaxLevle,max([Amount]) as MaxAmount FROM [SmartMemberCategory]").FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 添加会员卡
        /// </summary>
        /// <param name="dto">会员卡信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(MemberCategoryAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length > 20)
            {
                result.Message = "名称最多20个字！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            if (dto.Icon.IsNullOrEmpty())
            {
                result.Message = "请上传图标！";
                return result;
            }

            var max = GetMemberMaxInfo().Data;
            
            if (dto.Level <= Convert.ToInt32(max.MaxLevle))
            {
                result.Message = "级别不能小于最高级别" + Convert.ToInt32(max.MaxLevle) + "！";
                return result;
            }
            if (dto.Amount <= Convert.ToDecimal(max.MaxAmount))
            {
                result.Message = "升级到达金额不能小于当前最高金额" + Convert.ToDecimal(max.MaxAmount) + "元！";
                return result;
            }

            TryTransaction(() =>
            {

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                result.Data = _connection.Execute("insert into [SmartMemberCategory]([ID],[Name],[Remark],[Level],[Amount],[Icon]) values (@ID,@Name,@Remark,@Level,@Amount,@Icon)",
                    new { ID = id, Name = dto.Name, Level = dto.Level, Remark = dto.Remark, Amount = dto.Amount, Icon = dto.Icon }, _transaction);

                if (dto.MemberCategoryEquityAdd != null)
                {
                    if (dto.MemberCategoryEquityAdd.Count > 0)
                    {//只有添加会员权益数组里有值时才需要插入权益表
                        foreach (var item in dto.MemberCategoryEquityAdd)
                        {
                            _connection.Execute("insert into SmartMemberCategoryEquity(ID,CategoryID,EquityID) values(@ID, @CategoryID, @EquityID)",
                                                new
                                                {
                                                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                                    CategoryID = id,
                                                    EquityID = item.EquityID
                                                }, _transaction); //会员类型权益映射表
                        }
                    }
                }


                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.MemberCategoryAdd,
                    Remark = LogType.MemberCategoryAdd.ToDescription() + dto.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.MemberCategory);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }


        /// <summary>
        /// 根据id查询当前会员卡表中的最大值信息(不包含当前id) 【此方法可以优化】
        /// </summary>
        /// <param name="id">会员卡ID</param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, MemberCategory> GetMemberMaxNotIdInfo(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, MemberCategory>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<MemberCategory>("SELECT max([Level]) as MaxLevle,max([Amount]) as MaxAmount FROM [SmartMemberCategory] WHERE ID NOT IN (@ID)", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }


        /// <summary>
        /// 更新会员卡信息
        /// </summary>
        /// <param name="dto">会员卡信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(MemberCategoryUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length > 20)
            {
                result.Message = "名称最多20个字！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }
            if (dto.Icon.IsNullOrEmpty())
            {
                result.Message = "请上传图片！";
                return result;
            }

            //var max = GetMemberMaxNotIdInfo(dto.ID).Data;
            //if (!string.IsNullOrWhiteSpace(max.MaxLevle)) {
            //    if (dto.Level <= Convert.ToInt32(max.MaxLevle))
            //    {
            //        result.Message = "级别不能小于最高级别" + Convert.ToInt32(max.MaxLevle) + "！";
            //        return result;
            //    }
            //}

            //if (!string.IsNullOrWhiteSpace(max.MaxAmount)) {
            //    if (dto.Amount <= Convert.ToDecimal(max.MaxAmount))
            //    {
            //        result.Message = "升级到达金额不能小于当前最高金额" + Convert.ToDecimal(max.MaxAmount) + "元！";
            //        return result;
            //    }
            //}                    

            TryTransaction(() =>
            {

                string sql = "update [SmartMemberCategory] set Name=@Name,Remark=@Remark,Level=@Level,Amount=@Amount,Icon=@Icon where ID = @ID";

                if (dto.ID == 1 || dto.ID == 0)
                {
                    sql = "update [SmartMemberCategory] set Name=@Name,Icon=@Icon where ID = @ID";
                }

                result.Data = _connection.Execute(sql, dto, _transaction);

                if (dto.MemberCategoryEquityAdd != null)
                {
                    if (dto.MemberCategoryEquityAdd.Count > 0)
                    {//只有添加会员权益数组里有值时才需要插入权益表
                        _connection.Execute("DELETE SmartMemberCategoryEquity WHERE CategoryID=@CategoryID", new { CategoryID = dto.ID }, _transaction);

                        foreach (var item in dto.MemberCategoryEquityAdd)
                        {
                            _connection.Execute("insert into SmartMemberCategoryEquity(ID,CategoryID,EquityID) values(@ID, @CategoryID, @EquityID)",
                                                new
                                                {
                                                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                                    CategoryID = dto.ID,
                                                    EquityID = item.EquityID
                                                }, _transaction); //会员类型权益映射表
                        }
                    }
                }

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.MemberCategoryUpdate,
                    Remark = LogType.MemberCategoryUpdate.ToDescription() + dto.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.MemberCategory);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 会员卡删除
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(MemberCategoryDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.ID == 1 || dto.ID == 0)
            {
                result.Message = "该会员等级不允许删除！";
                return result;
            }

            TryTransaction(() =>
            {
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }

                _connection.Execute("DELETE SmartMemberCategoryEquity WHERE CategoryID=@CategoryID", new { CategoryID = dto.ID }, _transaction);

                result.Data = _connection.Execute("delete from SmartMemberCategory where ID = @ID", new { ID = dto.ID }, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.MemberCategoryDelete,
                    Remark = LogType.MemberCategoryDelete + "ID：" + dto.ID
                });

                CacheDelete.CategoryChange(SelectType.MemberCategory);

                result.Message = LogType.MemberCategoryDelete.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询所有会员卡
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<MemberCategory>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<MemberCategory>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<MemberCategory>("select ID,Name,Remark,Level,Amount,Icon from SmartMemberCategory order by Level");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询会员卡详细
        /// </summary>
        /// <param name="id">会员卡ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, MemberCategory> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, MemberCategory>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<MemberCategory>("SELECT ID,Name,Remark,Level,Amount,Icon FROM dbo.SmartMemberCategory WHERE ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.MemberCategoryEquityAdd = new List<MemberCategoryEquityAdd>();
                result.Data.MemberCategoryEquityAdd = _connection.Query<MemberCategoryEquityAdd>(@"SELECT sce.ID,se.ID as EquityID,se.Name as EquityName,se.Remark FROM dbo.SmartMemberCategoryEquity AS sce INNER JOIN SmartEquity AS se
ON sce.EquityID=se.ID WHERE sce.CategoryID=@CategoryID", new { CategoryID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        public IFlyDogResult<IFlyDogResultType, MemberCategoryEquity> GetMemberCategoryEquitysByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, MemberCategoryEquity>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<MemberCategoryEquity>("select ID,Name,Remark,Level,Amount,Icon from SmartMemberCategory  where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        public IFlyDogResult<IFlyDogResultType, int> AddMemberCategoryEquity(MemberCategoryEquityAdd dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.MemberCategory);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartMemberCategory] order by Level");

                _redis.StringSet(RedisPreKey.Category + SelectType.MemberCategory , result.Data);
            });

            return result;
        }
    }
}
