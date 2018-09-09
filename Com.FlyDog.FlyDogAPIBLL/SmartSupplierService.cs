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
    public class SmartSupplierService : BaseService, ISmartSupplierService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加供应商信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartSupplierAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "供应商名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 50)
            {
                result.Message = "供应商名称最多50个字符！";
                return result;
            }

            if (dto.PinYin.IsNullOrEmpty())
            {
                dto.PinYin = " ";
            }
            else if (!dto.PinYin.IsNullOrEmpty() && dto.PinYin.Length >= 20)
            {
                result.Message = "拼音码最多20个字符！";
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
                result.Data = _connection.Execute("insert into SmartSupplier(ID,Name,LinkMan,Contact,Remark,PinYin,HospitalID) values (@ID,@Name,@LinkMan,@Contact,@Remark,@PinYin,@HospitalID)",
                    new { ID = id, Name = dto.Name, LinkMan = dto.LinkMan, Contact = dto.Contact, Remark = dto.Remark, PinYin = dto.PinYin, HospitalID = dto.HospitalID }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name, 联系人 = dto.LinkMan, 联系方式 = dto.Contact, 备注 = dto.Remark, 拼音码 = dto.PinYin, 所属医院 = dto.HospitalID };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartSupplierAdd,
                    Remark = LogType.SmartSupplierAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.Supplier, dto.HospitalID);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }


        #region 验证供应商是否存在引用 (现在先写三个方法，后期看看能不能优化成使用一条sql语句将结果返回回来 20170216)
        /// <summary>
        /// 根据供应商id查询进货记录表中此供应商是否被引用
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetPurchaseBySupplierIDData(string supplierID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(SmartPurchase.ID) FROM dbo.SmartPurchase WHERE SupplierID=@SupplierID", new { SupplierID = supplierID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据供应商id查询退货记录表中此供应商是否被引用
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetReturnBySupplierIDData(string supplierID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(SmartReturn.ID) FROM dbo.SmartReturn WHERE SupplierID=@SupplierID", new { SupplierID = supplierID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据供应商id查询开票记录表中此供应商是否被引用
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetInvoiceBySupplierIDData(string supplierID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(SmartInvoice.ID) FROM dbo.SmartInvoice WHERE SupplierID=@SupplierID", new { SupplierID = supplierID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }
        #endregion

        /// <summary>
        /// 删除供应商信息 (删除之前需要判断，在进货，退货，发票是否有引用，如果有则不能删除)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(SmartSupplierDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var purchaseNum = GetPurchaseBySupplierIDData(dto.ID);
            var returnNum = GetReturnBySupplierIDData(dto.ID);
            var invoiceNum = GetInvoiceBySupplierIDData(dto.ID);

            if (purchaseNum.Data > 0)
            {
                result.Message = "此供应商已存在进货记录不能删除！";
                return result;
            }

            if (returnNum.Data > 0)
            {
                result.Message = "此供应商已存在退货记录不能删除！";
                return result;
            }

            if (invoiceNum.Data > 0)
            {
                result.Message = "此供应商已存在开票记录不能删除！";
                return result;
            }

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作

                long hospitalID = _connection.Query<long>("select [HospitalID] from [SmartSupplier] where ID=@ID", dto, _transaction).FirstOrDefault();

                result.Data = _connection.Execute("delete SmartSupplier where ID=@ID", dto, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartSupplierDelete,
                    Remark = LogType.SmartSupplierDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.Supplier, hospitalID);

                result.Message = "供应商删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取全部供应商信息，不分页，主要给下拉列表使用 string hospitalID  where HospitalID = " + hospitalID + "
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartSupplierInfo>> GetAll(string hospitalIDD)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartSupplierInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartSupplierInfo>("SELECT * FROM dbo.SmartSupplier WHERE HospitalID=@HospitalID", new { HospitalID = hospitalIDD });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取全部供应商信息
        /// </summary>
        /// <param name="hospitalId">所属医院id，查询当前医院的供应商</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartSupplierInfo>>> Get(SmartSupplierSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartSupplierInfo>>>();
            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                //result.Data.PageNum = 1;// dto.PageNum;

                //result.Data.PageSize = dto.PageSize;
                string py = string.IsNullOrWhiteSpace(dto.PinYin) ? " " : dto.PinYin;
                string name = string.IsNullOrWhiteSpace(dto.Name) ? " " : dto.Name;
                result.Data = new Pages<IEnumerable<SmartSupplierInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;
                if (!string.IsNullOrWhiteSpace(dto.PinYin))
                {
                    sql = @"SELECT * FROM dbo.SmartSupplier WHERE PinYin LIKE '%" + py + "%' AND HospitalID = " + dto.HospitalID + " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";
                    sql2 = @"SELECT Count(ID) as Count FROM dbo.SmartSupplier WHERE PinYin LIKE '%" + py + "%' AND HospitalID = " + dto.HospitalID + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql = @"SELECT * FROM dbo.SmartSupplier WHERE  Name like '%" + name + "%' AND HospitalID = " + dto.HospitalID + " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";
                    sql2 = @"SELECT Count(ID) as Count FROM dbo.SmartSupplier WHERE  Name like '%" + name + "%' AND HospitalID = " + dto.HospitalID + "";
                }

                if (!string.IsNullOrWhiteSpace(dto.PinYin) && !string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql = @"SELECT * FROM dbo.SmartSupplier WHERE PinYin LIKE '%" + py + "%'AND Name like '%" + name + "%' AND HospitalID = " + dto.HospitalID + " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";
                    sql2 = "SELECT Count(ID) as Count FROM dbo.SmartSupplier WHERE PinYin LIKE '%" + py + "%'AND Name like '%" + name + "%' AND HospitalID = " + dto.HospitalID + "";
                }

                if (string.IsNullOrWhiteSpace(dto.PinYin) && string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql = "SELECT * FROM dbo.SmartSupplier WHERE HospitalID = " + dto.HospitalID + " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";
                    sql2 = "SELECT Count(ID) as Count FROM dbo.SmartSupplier WHERE HospitalID = " + dto.HospitalID + "";
                }
                result.Data.PageDatas = _connection.Query<SmartSupplierInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询单个供应商详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartSupplierInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartSupplierInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartSupplierInfo>("SELECT [ID],[Name],[LinkMan],[Contact],[Remark],[PinYin],[HospitalID] FROM [SmartSupplier] where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "供应商查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 修改供应商信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartSupplierUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "供应商名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 50)
            {
                result.Message = "供应商名称最多50个字符！";
                return result;
            }

            if (dto.PinYin.IsNullOrEmpty())
            {
                dto.PinYin = " ";
            }
            else if (!dto.PinYin.IsNullOrEmpty() && dto.PinYin.Length >= 20)
            {
                result.Message = "拼音码最多20个字符！";
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
                result.Data = _connection.Execute("UPDATE SmartSupplier SET Name=@Name,LinkMan=@LinkMan,Contact=@Contact,Remark=@Remark,PinYin=@PinYin WHERE ID=@ID", dto, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name, 联系人 = dto.LinkMan, 联系方式 = dto.Contact, 备注 = dto.Remark, 拼音码 = dto.PinYin };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartSupplierUpdate,
                    Remark = LogType.SmartSupplierUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.Supplier, dto.HospitalID);

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
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.Supplier + ":" + hospitalID);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartSupplier] where [HospitalID]=@HospitalID order by Name", new { HospitalID = hospitalID });

                _redis.StringSet(RedisPreKey.Category + SelectType.Supplier + ":" + hospitalID, result.Data);
            });

            return result;
        }
    }
}
