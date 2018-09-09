using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using Com.JinYiWei.Common.DataAccess;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 药物品设置
    /// </summary>
    public class SmartProductService : BaseService, ISmartProductService
    {
        /// <summary>
        /// 添加药物品设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartProductAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "名称最多20个字符！";
                return result;
            }

            if (dto.PinYin.IsNullOrEmpty())
            {
                result.Message = "拼音码不能为空！";
                return result;
            }
            else if (!dto.PinYin.IsNullOrEmpty() && dto.PinYin.Length >= 10)
            {
                result.Message = "拼音码最多10个字符！";
                return result;
            }

            if (dto.CategoryID.IsNullOrEmpty() || dto.CategoryID == "-1")
            {
                result.Message = "请选择类型！";
                return result;
            }

            if (dto.Size.IsNullOrEmpty())
            {
                result.Message = "规格不能为空！";
                return result;
            }
            else if (!dto.Size.IsNullOrEmpty() && dto.Size.Length >= 10)
            {
                result.Message = "规格最多10个字符！";
                return result;
            }

            if (dto.Price.IsNullOrEmpty())
            {
                result.Message = "默认价格不能为空！";
                return result;
            }

            if (dto.UnitID.IsNullOrEmpty())
            {
                result.Message = "请选择库存单位！";
                return result;
            }

            if (dto.MiniUnitID.IsNullOrEmpty())
            {
                result.Message = "请选择使用单位！";
                return result;
            }

            if (dto.Scale.IsNullOrEmpty())
            {
                result.Message = "进制不能为空！";
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
                result.Data = _connection.Execute(@"insert into SmartProduct(ID,Name,PinYin,CategoryID,Size,Price,[Status],Remark,UnitID,MiniUnitID,Scale)
 values(@ID, @Name, @PinYin, @CategoryID, @Size, @Price, @Status, @Remark, @UnitID, @MiniUnitID, @Scale)",
                    new { ID = id, Name = dto.Name, PinYin = dto.PinYin, CategoryID = dto.CategoryID, Size = dto.Size, Price = dto.Price, Status = dto.Status, Remark = dto.Remark, UnitID = dto.UnitID, MiniUnitID = dto.MiniUnitID, Scale = dto.Scale }, _transaction);

                var temp = new
                {
                    编号 = result.Data,
                    名称 = dto.Name,
                    拼音码 = dto.PinYin,
                    药物品类型 = dto.CategoryID,
                    规格 = dto.Size,
                    价格 = dto.Price,
                    状态 = dto.Status,
                    库存单位 = dto.UnitID,
                    使用单位 = dto.MiniUnitID,
                    进制 = dto.Scale
                };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartProductAdd,
                    Remark = LogType.SmartProductAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "药物品添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取全部药物品信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartProductInfo>>> Get(SmartProductSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartProductInfo>>>();
            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<SmartProductInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT sp.ID,sp.ID AS ProductID,sp.Name,sp.Status,sp.Size,sp.PinYin,spc.Name AS CategoryName,sp.CategoryID,sp.UnitID,sp.MiniUnitID,sp.Price,su.Name AS KcName,sUnit.Name AS SYName,sp.Scale FROM dbo.SmartProduct AS sp INNER JOIN SmartProductCategory AS spc ON sp.CategoryID=spc.ID INNER JOIN dbo.SmartUnit AS su ON sp.UnitID=su.ID INNER JOIN SmartUnit AS sUnit ON sUnit.ID = sp.MiniUnitID where 1=1";

                sql2 = @"SELECT COUNT(sp.ID) AS Count FROM dbo.SmartProduct AS sp INNER JOIN SmartProductCategory AS spc ON sp.CategoryID = spc.ID INNER JOIN dbo.SmartUnit AS su ON sp.UnitID = su.ID INNER JOIN SmartUnit AS sUnit ON sUnit.ID = sp.MiniUnitID WHERE 1 = 1";

                if (!string.IsNullOrWhiteSpace(dto.PinYin))
                {
                    sql += @" AND sp.PinYin LIKE '%" + dto.PinYin + "%'";
                    sql2 += @" AND sp.PinYin LIKE '%" + dto.PinYin + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql += @" AND sp.Name LIKE '%" + dto.Name + "%'";
                    sql2 += @" AND sp.Name LIKE '%" + dto.Name + "%'";
                }
                //" ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only
                if (!string.IsNullOrWhiteSpace(dto.CategoryId) && dto.CategoryId != "-1")
                {
                    sql += @" AND sp.CategoryID =" + dto.CategoryId + "";
                    sql2 += @" AND sp.CategoryID =" + dto.CategoryId + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.Status) && dto.Status != "-1")
                {
                    sql += " AND sp.Status =" + dto.Status + "";
                    sql2 += @" AND sp.Status =" + dto.Status + "";
                }

                sql += " ORDER by sp.Status desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<SmartProductInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取所有药物品信息不分页SmartProductSelect dto
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartProductInfo>> GetAll(SmartProductSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartProductInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                string sql = @"SELECT sp.ID,sp.ID AS ProductID,sp.Name,sp.Status,sp.Size,sp.PinYin,spc.Name AS CategoryName,sp.CategoryID,sp.UnitID,sp.MiniUnitID,sp.Price,su.Name AS KcName,sUnit.Name AS SYName,sp.Scale FROM dbo.SmartProduct AS sp LEFT JOIN SmartProductCategory AS spc ON sp.CategoryID = spc.ID LEFT JOIN dbo.SmartUnit AS su ON sp.UnitID = su.ID　LEFT JOIN SmartUnit AS sUnit ON sUnit.ID = sp.MiniUnitID where 1 = 1";

                if (!string.IsNullOrWhiteSpace(dto.PinYin))
                {
                    sql += @" AND sp.PinYin LIKE '%" + dto.PinYin + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql += @" AND sp.Name LIKE '%" + dto.Name + "%'";
                }
                //" ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only
                if (!string.IsNullOrWhiteSpace(dto.CategoryId) && dto.CategoryId != "-1")
                {
                    sql += @" AND sp.CategoryID =" + dto.CategoryId + "";
                }

                result.Data = _connection.Query<SmartProductInfo>(sql);
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据仓库id查询仓库内的商品
        /// </summary>
        /// <param name="warehouseID">仓库id</param>
        /// <param name="type">类型1：入库2：调拨3：盘点4：科室领用</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartProductInfo>> GetByWarehouseIDDataAll(SmartProductSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartProductInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                string sql = @"SELECT sp.ID,sp.Name,sp.Status,sp.Size,sp.PinYin,spc.Name AS CategoryName,sp.CategoryID,sp.UnitID,sp.MiniUnitID,ss.Price,ss.Amount,ss.Num,su.Name AS KcName,sUnit.Name AS SYName,sp.Scale 
                    FROM dbo.SmartProduct AS sp left JOIN SmartProductCategory AS spc 
                    ON sp.CategoryID = spc.ID left JOIN dbo.SmartUnit AS su 
                    ON sp.UnitID = su.ID  left JOIN  dbo.SmartPurchaseDetail AS spd 
                    ON sp.ID=spd.ProductID left JOIN dbo.SmartStock AS ss
                    ON ss.ProductID=sp.ID left JOIN SmartUnit AS sUnit ON sUnit.ID = sp.MiniUnitID where 1 = 1";

                if (!string.IsNullOrWhiteSpace(dto.PinYin))
                {
                    sql += @" AND sp.PinYin LIKE '%" + dto.PinYin + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql += @" AND sp.Name LIKE '%" + dto.Name + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.CategoryId) && dto.CategoryId != "")
                {
                    sql += @" AND sp.CategoryID =" + dto.CategoryId + "";
                }

                sql += @" AND sp.ID IN(SELECT ProductID FROM SmartStock WHERE WarehouseID = " + dto.WarehouseID + " AND Type = " + dto.Type + ")";
                result.Data = _connection.Query<SmartProductInfo>(sql);
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }


        /// <summary>
        /// 根据id查询信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartProductInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartProductInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartProductInfo>(" SELECT [ID],[Name],[PinYin],[CategoryID],[Size],[Price],[Status],[Remark],[UnitID],[MiniUnitID],[Scale] FROM SmartProduct where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }



        /// <summary>
        /// 启用停用药物品设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> SmartProductDispose(SmartProductStopOrUse dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 开始状态动作
            TryTransaction(() =>
            {
                #region 验证用户合法性
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }
                #endregion

                #region 开始修改班次状态
                result.Data = _connection.Execute(" update SmartProduct set [Status] = @Status where ID = @ID", dto, _transaction);

                AddOperationLog(new SmartOperationLog() { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), CreateTime = DateTime.Now, CreateUserID = dto.CreateUserID, Type = LogType.SmartProductDispose });

                result.Message = dto.Status.ToString() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
                #endregion
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新药物品设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartProductUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "名称最多20个字符！";
                return result;
            }

            if (dto.PinYin.IsNullOrEmpty())
            {
                result.Message = "拼音码不能为空！";
                return result;
            }
            else if (!dto.PinYin.IsNullOrEmpty() && dto.PinYin.Length >= 10)
            {
                result.Message = "拼音码最多10个字符！";
                return result;
            }

            if (dto.CategoryID.IsNullOrEmpty() || dto.CategoryID == "-1")
            {
                result.Message = "请选择类型！";
                return result;
            }

            if (dto.Size.IsNullOrEmpty())
            {
                result.Message = "规格不能为空！";
                return result;
            }
            else if (!dto.Size.IsNullOrEmpty() && dto.Size.Length >= 10)
            {
                result.Message = "规格最多10个字符！";
                return result;
            }

            if (dto.Price.IsNullOrEmpty())
            {
                result.Message = "默认价格不能为空！";
                return result;
            }

            if (dto.UnitID.IsNullOrEmpty())
            {
                result.Message = "请选择库存单位！";
                return result;
            }

            if (dto.MiniUnitID.IsNullOrEmpty())
            {
                result.Message = "请选择使用单位！";
                return result;
            }

            if (dto.Scale.IsNullOrEmpty())
            {
                result.Message = "进制不能为空！";
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
                result.Data = _connection.Execute(@"UPDATE SmartProduct SET Name=@Name,PinYin=@PinYin,CategoryID=@CategoryID,Size=@Size,Price=@Price,
 Status = @Status, Remark = @Remark, UnitID = @UnitID, MiniUnitID = @MiniUnitID, Scale = @Scale WHERE ID = @ID", dto, _transaction);

                var temp = new
                {
                    编号 = result.Data,
                    名称 = dto.Name,
                    拼音码 = dto.PinYin,
                    药物品类型 = dto.CategoryID,
                    规格 = dto.Size,
                    价格 = dto.Price,
                    状态 = dto.Status,
                    库存单位 = dto.UnitID,
                    使用单位 = dto.MiniUnitID,
                    进制 = dto.Scale
                };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartProductUpdate,
                    Remark = LogType.SmartProductUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 根据药品分类id查询是否有药品使用该分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>

        public IFlyDogResult<IFlyDogResultType, int> SmartProductGetByCategoryID(string categoryID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 开始状态动作
            TryTransaction(() =>
            {
                #region 开始修改班次状态
                result.Data = _connection.Execute("SELECT COUNT(ID) FROM dbo.SmartProduct WHERE CategoryID=@CategoryID", new { CategoryID = categoryID }, _transaction);
                result.ResultType = IFlyDogResultType.Success;
                return true;
                #endregion
            });
            #endregion
            return result;
        }
    }
}
