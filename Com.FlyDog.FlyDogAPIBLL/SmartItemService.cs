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
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 报表项目业务处理
    /// </summary>
    public class SmartItemService : BaseService, ISmartItemService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();

        /// <summary>
        /// 新增报表项目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartItemAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region
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

            if (dto.SortNo.IsNullOrEmpty()) {
                result.Message = "排序不能为空！";
                return result;
            }

            if (dto.GroupID.IsNullOrEmpty()|| dto.GroupID=="-1") {
                result.Message = "请选择报表项目组！";
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

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                _connection.Execute("insert into SmartItem(ID,Name,SortNo,Remark,GroupID) values(@ID, @Name,@SortNo,@Remark,@GroupID)",
                 new { ID = id, Name = dto.Name, SortNo=dto.SortNo, Remark = dto.Remark, GroupID=dto.GroupID }, _transaction);  //报表项目表

                if (dto.SymptomDetailInfoAdd!=null&&dto.SymptomDetailInfoAdd.Count>0) {
                    //说明选择了报表项目症状
                    foreach (var u in dto.SymptomDetailInfoAdd)
                    {
                        _connection.Execute("insert into SmartItemSymptomDetail(ID,ItemID,SymptomID) values(@ID, @ItemID, @SymptomID)",
                            new
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                ItemID = id,
                                SymptomID = u.ID
                            }, _transaction); //报表项目症状映射
                    };
                }

                if (dto.ChargeDetailInfoAdd != null && dto.ChargeDetailInfoAdd.Count > 0)
                {
                    //说明选择了报表项目收费项目
                    foreach (var u in dto.ChargeDetailInfoAdd)
                    {
                        _connection.Execute("insert into SmartItemChargeDetail(ID,ItemID,ChargeID) values(@ID, @ItemID, @ChargeID)",
                            new
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                ItemID = id,
                                ChargeID = u.ID
                            }, _transaction); //报表项目收费项目
                    };
                }
                CacheDelete.CategoryChange(SelectType.Item);
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.SmartItemAdd,
                    Remark = LogType.SmartItemAdd.ToDescription() + dto.ToJsonString()
                });
                
                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 删除报表项目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(SmartItemDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 开始事物操作
            TryTransaction(() =>
            {
                string sql = @"DELETE SmartItem WHERE ID=@ID
                                DELETE SmartItemChargeDetail WHERE ItemID=@ItemID
                                DELETE SmartItemSymptomDetail WHERE ItemID=@ItemID1";
                #region 开始更新操作
                result.Data = _connection.Execute(sql, new { ID = dto.ID, ItemID=dto.ID, ItemID1=dto.ID }, _transaction);
                CacheDelete.CategoryChange(SelectType.Item);
                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.SmartItemDelete,
                    Remark = LogType.SmartItemDelete.ToDescription() + temp.ToJsonString()
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
        /// 查询所有报表项目
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartItemInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartItemInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartItemInfo>("SELECT si.ID,si.Name,si.GroupID,sig.Name AS GrpupName,si.SortNo,si.Remark FROM dbo.SmartItem AS si LEFT JOIN SmartItemGroup AS sig ON si.GroupID = sig.ID");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id获取报表项目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartItemInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartItemInfo>();
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartItemInfo>(@"SELECT si.ID,si.Name,si.GroupID,sig.Name AS GrpupName,si.SortNo,si.Remark FROM dbo.SmartItem AS si LEFT JOIN SmartItemGroup AS sig ON si.GroupID = sig.ID WHERE si.ID = @ID", new { ID = id }).FirstOrDefault();

                result.Data.SymptomDetailInfoAdd = new List<SymptomDetailInfo>();
                result.Data.SymptomDetailInfoAdd = _connection.Query<SymptomDetailInfo>(@"SELECT sisd.SymptomID AS ID,ss.Name FROM dbo.SmartItemSymptomDetail AS sisd  LEFT JOIN SmartSymptom AS ss ON sisd.SymptomID=ss.ID WHERE sisd.ItemID=@ItemID", new { ItemID = id }).ToList();//获取报表项目症状集合

                result.Data.ChargeDetailInfoAdd = new List<ChargeDetailInfo>();
                result.Data.ChargeDetailInfoAdd = _connection.Query<ChargeDetailInfo>(@"SELECT sicd.ChargeID AS ID,sc.Name FROM dbo.SmartItemChargeDetail AS sicd 
                    LEFT JOIN SmartCharge AS sc ON sicd.ChargeID=sc.ID WHERE sicd.ItemID=@ItemID", new { ItemID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 更新报表项目
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartItemUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region
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

            if (dto.SortNo.IsNullOrEmpty())
            {
                result.Message = "排序不能为空！";
                return result;
            }

            if (dto.GroupID.IsNullOrEmpty()|| dto.GroupID=="-1")
            {
                result.Message = "请选择报表项目组！";
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
                string sql = @"DELETE SmartItemChargeDetail WHERE ItemID=@ItemID
                                DELETE SmartItemSymptomDetail WHERE ItemID=@ItemID1";
                _connection.Execute(sql,
               new { ItemID = dto.ID, ItemID1=dto.ID }, _transaction);  //先把报表收费项目表和报表症状表相关数据删除

                if (dto.SymptomDetailInfoAdd != null && dto.SymptomDetailInfoAdd.Count > 0)
                {
                    //说明选择了报表项目症状
                    foreach (var u in dto.SymptomDetailInfoAdd)
                    {
                        _connection.Execute("insert into SmartItemSymptomDetail(ID,ItemID,SymptomID) values(@ID, @ItemID, @SymptomID)",
                            new
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                ItemID = dto.ID,
                                SymptomID = u.ID
                            }, _transaction); //报表项目症状映射
                    };
                }

                if (dto.ChargeDetailInfoAdd != null && dto.ChargeDetailInfoAdd.Count > 0)
                {
                    //说明选择了报表项目收费项目
                    foreach (var u in dto.ChargeDetailInfoAdd)
                    {
                        _connection.Execute("insert into SmartItemChargeDetail(ID,ItemID,ChargeID) values(@ID, @ItemID, @ChargeID)",
                            new
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                ItemID = dto.ID,
                                ChargeID = u.ID
                            }, _transaction); //报表项目收费项目
                    };
                }

                result.Data = _connection.Execute("UPDATE SmartItem SET Name=@Name,SortNo=@SortNo,Remark=@Remark,GroupID=@GroupID WHERE ID=@ID",
                new { ID = dto.ID, Name = dto.Name, SortNo=dto.SortNo, Remark = dto.Remark, GroupID=dto.GroupID }, _transaction);  //标签记录表

                CacheDelete.CategoryChange(SelectType.Item);

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.SmartItemUpdate,
                    Remark = LogType.SmartItemUpdate.ToDescription() + dto.ToJsonString()
                });

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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.Item);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT ID,Name FROM dbo.SmartItem ORDER BY Name");

                _redis.StringSet(RedisPreKey.Category + SelectType.Item, result.Data);
            });

            return result;
        }
    }
}
