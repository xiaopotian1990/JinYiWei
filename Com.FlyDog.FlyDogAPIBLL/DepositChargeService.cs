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
    /// 预收款类型业务处理
    /// </summary>
    public class DepositChargeService : BaseService, IDepositChargeService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加预收款类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(DepositChargeAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "名称最多20个字符！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            if (dto.SmartDepositChargeHospitalAdd == null || dto.SmartDepositChargeHospitalAdd.Count <= 0)
            {//添加预收款类型时必须选择适用医院
                result.Message = "请选择可使用医院！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            if (dto.ScopeLimit == "2")
            {//按照项目分类进行限制
                if (dto.SmartDepositChargeChargeCategoryAdd == null || dto.SmartDepositChargeChargeCategoryAdd.Count <= 0)
                {//添加预收款类型时必须选择适用医院
                    result.Message = "请指定项目分类！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return result;
                }

            }

            if (dto.ScopeLimit == "3")
            {//按照指定项目进行限制
                if (dto.SmartDepositChargeChargeAdd == null || dto.SmartDepositChargeChargeAdd.Count <= 0)
                {//添加预收款类型时必须选择适用医院
                    result.Message = "请指定项目！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return result;
                }

            }

            if (dto.HasCoupon == "1")
            {//如果是赠送代金券
                if (dto.CouponCategoryID.IsNullOrEmpty() || dto.CouponCategoryID == "-1")
                {
                    result.Message = "请选择卷类型！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return result;
                }

                if (dto.CouponAmount.IsNullOrEmpty())
                {
                    result.Message = "卷金额不能为空！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return result;
                }
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字符！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {
                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute(@"insert into SmartDepositCharge(ID,Name,Price,Status,ScopeLimit,ChargeID,ChargeCategoryID,HasCoupon,CouponCategoryID,CouponAmount,Remark) 
                    VALUES(@ID, @Name, @Price, @Status, @ScopeLimit, @ChargeID, @ChargeCategoryID, @HasCoupon, @CouponCategoryID, @CouponAmount,@Remark)",
                    new { ID = id, Name = dto.Name, Price = dto.Price, Status = dto.Status, ScopeLimit = dto.ScopeLimit, ChargeID = "", ChargeCategoryID = "", HasCoupon = dto.HasCoupon, CouponCategoryID = dto.HasCoupon == "1" ? dto.CouponCategoryID : "-1", CouponAmount = dto.HasCoupon == "1" ? dto.CouponAmount : "0", Remark = dto.Remark }, _transaction);

                foreach (var u in dto.SmartDepositChargeHospitalAdd)
                {
                    _connection.Execute("insert into SmartDepositChargeHospital(ID,DepositChargeID,HospitalID) VALUES(@ID, @DepositChargeID, @HospitalID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            DepositChargeID = id,
                            HospitalID = u.HospitalID
                        }, _transaction); //预收款适用医院映射表
                };


                if (dto.ScopeLimit == "2")
                {//：按照项目分类进行限制
                    foreach (var ud in dto.SmartDepositChargeChargeCategoryAdd)
                    {
                        _connection.Execute(@"insert into SmartDepositChargeChargeCategory(ID,DepositChargeID,ChargeCategoryID) VALUES(@ID, @DepositChargeID, @ChargeCategoryID)",
                   new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), DepositChargeID = id, ChargeCategoryID = ud.ChargeCategoryID }, _transaction);
                    };

                }

                if (dto.ScopeLimit == "3")
                {//预收款收费项目映射表

                    foreach (var udc in dto.SmartDepositChargeChargeAdd)
                    {
                        _connection.Execute(@"insert into SmartDepositChargeCharge(ID,DepositChargeID,ChargeID) VALUES(@ID, @DepositChargeID, @ChargeID)",
                 new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), DepositChargeID = id, ChargeID = udc.ChargeID }, _transaction);
                    };

                }

                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.DepositChargeAdd,
                    Remark = LogType.DepositChargeAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.DepositCharge, -1);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion


            return result;

        }

        /// <summary>
        /// 获取全部预收款类型
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<DepositChargeInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<DepositChargeInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<DepositChargeInfo>("SELECT ID,Name,Price,Remark,Status FROM dbo.SmartDepositCharge");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询预收款信息 (此处可以优化，20170218)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, DepositChargeInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, DepositChargeInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<DepositChargeInfo>(@"  SELECT sdc.ID,sdc.Name,sdc.Status,sdc.Remark,sdc.Price, sdc.ScopeLimit,sdc.HasCoupon,sdc.CouponCategoryID,scc.Name AS CouponCategoryName,
				  sdc.CouponAmount FROM dbo.SmartDepositCharge AS sdc LEFT JOIN dbo.SmartCouponCategory AS scc
				  ON sdc.CouponCategoryID=scc.ID WHERE sdc.ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.SmartDepositChargeHospitalAdd = new List<SmartDepositChargeHospitalAdd>();
                result.Data.SmartDepositChargeHospitalAdd = _connection.Query<SmartDepositChargeHospitalAdd>(@"SELECT sdch.ID,sdch.DepositChargeID, sdch.HospitalID,sh.Name AS HospitalName FROM SmartDepositChargeHospital AS sdch INNER JOIN dbo.SmartHospital AS sh ON sdch.HospitalID=sh.ID WHERE sdch.DepositChargeID=@DepositChargeID", new { DepositChargeID = id }).ToList(); //得到选择的医院数据

                result.Data.SmartDepositChargeChargeAdd = new List<SmartDepositChargeChargeAdd>();
                result.Data.SmartDepositChargeChargeAdd = _connection.Query<SmartDepositChargeChargeAdd>(@"SELECT sdcc.ID,sdcc.DepositChargeID,sdcc.ChargeID,sc.Name AS ChargeName FROM dbo.SmartDepositChargeCharge AS sdcc INNER JOIN SmartCharge AS sc ON sdcc.ChargeID=sc.ID WHERE sdcc.DepositChargeID=@DepositChargeID", new { DepositChargeID = id }).ToList(); //得到收费项目

                result.Data.SmartDepositChargeChargeCategoryAdd = new List<SmartDepositChargeChargeCategoryAdd>();
                result.Data.SmartDepositChargeChargeCategoryAdd = _connection.Query<SmartDepositChargeChargeCategoryAdd>(@"SELECT sdccc.ID,sdccc.DepositChargeID,sdccc.ChargeCategoryID,scc.Name AS ChargeCategoryName FROM SmartDepositChargeChargeCategory AS sdccc INNER JOIN SmartChargeCategory AS scc
                    ON sdccc.ChargeCategoryID=scc.ID WHERE sdccc.DepositChargeID=@DepositChargeID", new { DepositChargeID = id }).ToList(); //得到收费项目类型数据

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新预收款类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(DepositChargeUpdate dto)
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

            if (dto.SmartDepositChargeHospitalAdd == null || dto.SmartDepositChargeHospitalAdd.Count <= 0)
            {//添加预收款类型时必须选择适用医院
                result.Message = "请选择可使用医院！";
                return result;
            }

            if (dto.ScopeLimit == "2")
            {//按照项目分类进行限制
                if (dto.SmartDepositChargeChargeCategoryAdd == null || dto.SmartDepositChargeChargeCategoryAdd.Count <= 0)
                {//添加预收款类型时必须选择适用医院
                    result.Message = "请指定项目分类！";
                    return result;
                }

            }

            if (dto.ScopeLimit == "3")
            {//按照指定项目进行限制
                if (dto.SmartDepositChargeChargeAdd == null || dto.SmartDepositChargeChargeAdd.Count <= 0)
                {//添加预收款类型时必须选择适用医院
                    result.Message = "请指定项目！";
                    return result;
                }

            }

            if (dto.HasCoupon == "1")
            {//如果是赠送代金券
                if (dto.CouponCategoryID.IsNullOrEmpty() || dto.CouponCategoryID == "-1")
                {
                    result.Message = "请选择卷类型！";
                    return result;
                }

                if (dto.CouponAmount.IsNullOrEmpty())
                {
                    result.Message = "卷金额不能为空！";
                    return result;
                }
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

                string sql = @"
                DELETE SmartDepositChargeChargeCategory WHERE DepositChargeID=@DepositChargeID
                DELETE SmartDepositChargeCharge WHERE DepositChargeID=@DepositChargeID
                DELETE SmartDepositChargeHospital WHERE DepositChargeID=@DepositChargeID
                ";
                _connection.Execute(sql,
                    new { DepositChargeID = dto.ID }, _transaction); //删除3张映射表中的数据
                foreach (var u in dto.SmartDepositChargeHospitalAdd)
                {
                    _connection.Execute("insert into SmartDepositChargeHospital(ID,DepositChargeID,HospitalID) VALUES(@ID, @DepositChargeID, @HospitalID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            DepositChargeID = dto.ID,
                            HospitalID = u.HospitalID
                        }, _transaction); //预收款适用医院映射表
                };


                if (dto.ScopeLimit == "2")
                {//：按照项目分类进行限制
                    foreach (var ud in dto.SmartDepositChargeChargeCategoryAdd)
                    {
                        _connection.Execute(@"insert into SmartDepositChargeChargeCategory(ID,DepositChargeID,ChargeCategoryID) VALUES(@ID, @DepositChargeID, @ChargeCategoryID)",
                   new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), DepositChargeID = dto.ID, ChargeCategoryID = ud.ChargeCategoryID }, _transaction);
                    };

                }

                if (dto.ScopeLimit == "3")
                {//预收款收费项目映射表

                    if (dto.ScopeLimit == "2")
                    {//：按照项目分类进行限制
                        foreach (var udc in dto.SmartDepositChargeChargeAdd)
                        {
                            _connection.Execute(@"insert into SmartDepositChargeCharge(ID,DepositChargeID,ChargeID) 
                            VALUES(@ID, @DepositChargeID, @ChargeID)",
                     new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), DepositChargeID = dto.ID, ChargeID = udc.ChargeID }, _transaction);
                        };
                    }
                }

                result.Data = _connection.Execute(@"UPDATE SmartDepositCharge SET               Name=@Name,Price=@Price,Status=@Status,ScopeLimit=@ScopeLimit,HasCoupon=@HasCoupon,
                    CouponCategoryID = @CouponCategoryID, CouponAmount = @CouponAmount, Remark = @Remark WHERE ID = @ID", new { ID = dto.ID, Name = dto.Name, Price = dto.Price, Status = dto.Status, ScopeLimit = dto.ScopeLimit, HasCoupon = dto.HasCoupon, Remark = dto.Remark, CouponCategoryID = dto.HasCoupon == "1" ? dto.CouponCategoryID : "-1", CouponAmount = dto.HasCoupon == "1" ? dto.CouponAmount : "0" }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.DepositChargeUpdate,
                    Remark = LogType.DepositChargeUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.DepositCharge, -1);

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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.DepositCharge + ":" + hospitalID);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("select distinct a.ID,a.Name from SmartDepositCharge a,SmartDepositChargeHospital b where a.ID=b.DepositChargeID and b.HospitalID=@HospitalID and a.Status=@Status",
                    new { HospitalID = hospitalID, Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.DepositCharge + ":" + hospitalID, result.Data);
            });

            return result;
        }
    }
}
