using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
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
    /// <summary>
    /// 收费项目业务逻辑
    /// </summary>
    public class ChargeService : BaseService, IChargeService
    {
        /// <summary>
        /// 添加收费项目信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(ChargeAdd dto)
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
                result.Message = "名称不能超过20个字符!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.PinYin))
            {
                result.Message = "拼音码不能为空!";
                return result;
            }
            else if (!dto.PinYin.IsNullOrEmpty() && dto.PinYin.Length >= 20)
            {
                result.Message = "拼音码不能超过20个字符!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.CategoryID) || dto.CategoryID == "-1")
            {
                result.Message = "请选择类型!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.UnitID) || dto.UnitID == "-1")
            {
                result.Message = "请选择单位!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Price))
            {
                result.Message = "请输入价格!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Size))
            {
                result.Message = "请输入规格!";
                return result;
            }
            else if (!dto.Size.IsNullOrEmpty() && dto.Size.Length >= 20)
            {
                result.Message = "规格不能超过20个字符!";
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


            if (dto.SmartChargeProductDetailAdd == null || dto.SmartChargeProductDetailAdd.Count <= 0)
            {
                result.Message = "药物品详细不可为空!";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                //通过创建人ID查询一条数据
                #region 验证用户 先注释，等测试完成在放开 20170204
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();
                //判断是否有数据
                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }
                #endregion

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                _connection.Execute(@"insert into SmartCharge(ID,Name,CategoryID,PinYin,Price,Status,Remark,UnitID,Size,ProductAdd)
 values(@ID, @Name, @CategoryID, @PinYin, @Price, @Status, @Remark, @UnitID,@Size,@ProductAdd)",
                 new { ID = id, Name = dto.Name, CategoryID = dto.CategoryID, PinYin = dto.PinYin, Price = dto.Price, Status = dto.Status, Remark = dto.Remark, UnitID = dto.UnitID, Size = dto.Size, ProductAdd = dto.ProductAdd }, _transaction);  //收费项目

                foreach (var u in dto.SmartChargeProductDetailAdd)
                {
                    _connection.Execute(@"insert into SmartChargeProductDetail(ID,ChargeID,ProductID,MinNum,MaxNum) 
 VALUES(@ID, @ChargeID, @ProductID, @MinNum, @MaxNum)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            ChargeID = id,
                            ProductID = u.ProductID,
                            MinNum = u.MinNum,
                            MaxNum = u.MaxNum
                        }, _transaction);

                };//收费项目耗材映射表

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.ChargeAdd,
                    Remark = LogType.ChargeAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询所有收费项目信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeInfo>>> Get(ChargeSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<ChargeInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT sc.ID,sc.Name,sc.PinYin,sc.UnitID,su.Name AS UnitName,sc.Price,sc.CategoryID,scc.Name AS CategoryName,sc.Size,sc.ProductAdd,sc.Status FROM dbo.SmartCharge AS sc
                    left JOIN dbo.SmartUnit AS su ON sc.UnitID=su.ID
                    left JOIN SmartChargeCategory AS scc ON sc.CategoryID=scc.ID WHERE 1=1";

                sql2 = @"SELECT COUNT(sc.ID) FROM dbo.SmartCharge AS sc
                        left JOIN dbo.SmartUnit AS su ON sc.UnitID=su.ID
                        left JOIN SmartChargeCategory AS scc ON sc.CategoryID=scc.ID WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(dto.PinYin))
                {
                    sql += @" AND sc.PinYin LIKE '%" + dto.PinYin + "%'";
                    sql2 += @" AND sc.PinYin LIKE '%" + dto.PinYin + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql += @" AND sc.Name LIKE '%" + dto.Name + "%'";
                    sql2 += @" AND sc.Name LIKE '%" + dto.Name + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.CategoryID) && dto.CategoryID != "-1")
                {
                    sql += " And sc.CategoryID=" + dto.CategoryID + "";
                    sql2 += @" And sc.CategoryID=" + dto.CategoryID + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.Status) && dto.Status != "-1")
                {
                    sql += " And sc.Status=" + dto.Status + "";
                    sql2 += @" And sc.Status=" + dto.Status + "";
                }

                sql += " ORDER by sc.Status  desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<ChargeInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;

        }

        /// <summary>
        /// 检测收费项目
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeCheckItem>>> GetCheckCharge(ChargeSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeCheckItem>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<ChargeCheckItem>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT sc.Name AS ChargeName,sc.Status,si.Name AS ChargeItemName FROM SmartCharge AS sc
						LEFT JOIN SmartItemChargeDetail AS scd ON scd.ChargeID=sc.ID
						LEFT JOIN SmartItem AS si ON scd.ItemID=si.ID";

                sql2 = @"SELECT COUNT(sc.ID) AS Count FROM SmartCharge AS sc
						LEFT JOIN SmartItemChargeDetail AS scd ON scd.ChargeID=sc.ID
						LEFT JOIN SmartItem AS si ON scd.ItemID=si.ID WHERE 1=1 ";

                sql += " ORDER by sc.Status  desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<ChargeCheckItem>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 查询所有收费项目信息（不分页给弹窗使用）
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeInfo>> GetData(ChargeSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                string sql = @"SELECT sc.ID,sc.Name,sc.PinYin,sc.UnitID,su.Name AS UnitName,sc.Price,sc.CategoryID,scc.Name AS CategoryName,sc.Size,sc.ProductAdd,sc.Status FROM dbo.SmartCharge AS sc
                left JOIN dbo.SmartUnit AS su ON sc.UnitID=su.ID
                left JOIN SmartChargeCategory AS scc ON sc.CategoryID=scc.ID where 1=1";

                if (!string.IsNullOrWhiteSpace(dto.PinYin))
                {
                    sql += @" AND sc.PinYin LIKE '%" + dto.PinYin + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql += @" AND sc.Name LIKE '%" + dto.Name + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Type) && dto.Type == "1")
                {
                    sql += @" AND sc.ID NOT IN(SELECT ChargeID FROM dbo.SmartItemChargeDetail)";
                }
                sql += @" AND sc.Status=1";
                result.Data = _connection.Query<ChargeInfo>(sql);
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询收费项目详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ChargeInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ChargeInfo>();
            TryExecute(() =>
            {
                result.Data = _connection.Query<ChargeInfo>("select ID,Name,CategoryID,PinYin,Price,Status,Remark,UnitID,Size,ProductAdd from SmartCharge where ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.SmartChargeProductDetailAdd = new List<SmartChargeProductDetailAdd>();
                result.Data.SmartChargeProductDetailAdd = _connection.Query<SmartChargeProductDetailAdd>(@" SELECT scpd.ID,scpd.ChargeID,scpd.ProductID,sp.Name AS ProductName,sp.Size,su.Name AS UnitName,scpd.MinNum,scpd.MaxNum FROM dbo.SmartChargeProductDetail AS scpd 
                left JOIN dbo.SmartProduct AS sp ON scpd.ProductID=sp.ID
                left JOIN dbo.SmartUnit AS su ON sp.UnitID=su.ID WHERE scpd.ChargeID=@ChargeID", new { ChargeID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }



        /// <summary>
        /// 更新收费项目详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(ChargeUpdate dto)
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
                result.Message = "名称不能超过20个字符!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.PinYin))
            {
                result.Message = "拼音码不能为空!";
                return result;
            }
            else if (!dto.PinYin.IsNullOrEmpty() && dto.PinYin.Length >= 20)
            {
                result.Message = "拼音码不能超过20个字符!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.CategoryID) || dto.CategoryID == "-1")
            {
                result.Message = "请选择类型!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.UnitID) || dto.UnitID == "-1")
            {
                result.Message = "请选择单位!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Price))
            {
                result.Message = "请输入价格!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Size))
            {
                result.Message = "请输入规格!";
                return result;
            }
            else if (!dto.Size.IsNullOrEmpty() && dto.Size.Length >= 20)
            {
                result.Message = "规格不能超过20个字符!";
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


            if (dto.SmartChargeProductDetailAdd == null || dto.SmartChargeProductDetailAdd.Count <= 0)
            {
                result.Message = "药物品详细不可为空!";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                //通过创建人ID查询一条数据
                #region 验证用户
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();
                //判断是否有数据
                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }
                #endregion

                _connection.Execute(@"UPDATE SmartCharge SET Name=@Name,CategoryID=@CategoryID,PinYin=@PinYin,Price=@Price,Status=@Status,Remark=@Remark,UnitID=@UnitID,Size=@Size,ProductAdd=@ProductAdd WHERE ID=@ID", dto, _transaction);  //收费项目


                _connection.Execute(@"DELETE SmartChargeProductDetail WHERE ChargeID = @ChargeID",
                        new
                        {
                            ChargeID = dto.ID
                        }, _transaction); //先把关联表中的数据删掉

                foreach (var u in dto.SmartChargeProductDetailAdd)
                {
                    _connection.Execute(@"insert into SmartChargeProductDetail(ID,ChargeID,ProductID,MinNum,MaxNum) 
 VALUES(@ID, @ChargeID, @ProductID, @MinNum, @MaxNum)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            ChargeID = dto.ID,
                            ProductID = u.ProductID,
                            MinNum = u.MinNum,
                            MaxNum = u.MaxNum
                        }, _transaction);

                };//收费项目耗材映射表

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.ChargeUpdate,
                    Remark = LogType.ChargeUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "更新成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
