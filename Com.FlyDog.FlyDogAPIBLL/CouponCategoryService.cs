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
    /// 卷类型业务处理
    /// </summary>
    public class CouponCategoryService : BaseService, ICouponCategoryService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加卷类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(CouponCategoryAdd dto)
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

            if (dto.SmartCouponCategoryHospitalAdd == null || dto.SmartCouponCategoryHospitalAdd.Count <= 0)
            {//添加预收款类型时必须选择适用医院
                result.Message = "请选择可使用医院！";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }


            if (dto.ScopeLimit == "2")
            {//按照项目分类进行限制
                if (dto.SmartCouponCategoryChargeCategoryAdd == null || dto.SmartCouponCategoryChargeCategoryAdd.Count <= 0)
                {//添加预收款类型时必须选择适用医院
                    result.Message = "请指定项目分类！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return result;
                }

            }

            if (dto.ScopeLimit == "3")
            {//按照指定项目进行限制
                if (dto.SmartCouponCategoryChargeAdd == null || dto.SmartCouponCategoryChargeAdd.Count <= 0)
                {//添加预收款类型时必须选择适用医院
                    result.Message = "请指定项目！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return result;
                }

            }

            if (dto.TimeLimit == "2")
            {
                if (dto.EndDate.IsNullOrEmpty())
                {
                    result.Message = "指定日期不能为空！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return result;
                }
            }

            if (dto.TimeLimit == "3")
            {
                if (dto.Days.IsNullOrEmpty())
                {
                    result.Message = "生效之后N天不能为空！";
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
                result.Data = _connection.Execute(@"insert into SmartCouponCategory(ID,Name,ScopeLimit,ChargeID,ChargeCategoryID,TimeLimit,EndDate,Days,Remark,Status) 
VALUES(@ID, @Name, @ScopeLimit, @ChargeID, @ChargeCategoryID, @TimeLimit, @EndDate, @Days, @Remark, @Status)",
                    new { ID = id, Name = dto.Name, ScopeLimit = dto.ScopeLimit, ChargeID = "", ChargeCategoryID = "", TimeLimit = dto.TimeLimit, EndDate = dto.TimeLimit == "2" ? dto.EndDate : DateTime.Now.ToString("yyyy-MM-dd"), Days = dto.TimeLimit == "3" ? dto.Days : "0", Remark = dto.Remark, Status = dto.Status }, _transaction);


                foreach (var u in dto.SmartCouponCategoryHospitalAdd)
                {
                    _connection.Execute("insert into SmartCouponCategoryHospital(ID,CouponCategoryID,HospitalID) VALUES(@ID, @CouponCategoryID, @HospitalID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CouponCategoryID = id,
                            HospitalID = u.HospitalID
                        }, _transaction); //卷类型医院映射表
                };


                if (dto.ScopeLimit == "2")
                {//按照项目分类进行限制
                    foreach (var ud in dto.SmartCouponCategoryChargeCategoryAdd)
                    {
                        _connection.Execute(@"insert into SmartCouponCategoryChargeCategory(ID,CouponCategoryID,ChargeCategoryID) 
VALUES(@ID, @CouponCategoryID, @ChargeCategoryID)",
                                           new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), CouponCategoryID = id, ChargeCategoryID = ud.ChargeCategoryID }, _transaction);
                    }

                }

                if (dto.ScopeLimit == "3")
                {//收费项目映射

                    foreach (var udc in dto.SmartCouponCategoryChargeAdd)
                    {
                        _connection.Execute(@"insert into SmartCouponCategoryCharge(ID,CouponCategoryID,ChargeID) 
VALUES(@ID, @CouponCategoryID, @ChargeID)",
                  new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), CouponCategoryID = id, ChargeID = udc.ChargeID }, _transaction);
                    }

                }

                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.CouponCategoryAdd,
                    Remark = LogType.CouponCategoryAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion


            CacheDelete.CategoryChange(SelectType.CouponCategory, -1);

            return result;
        }



        /// <summary>
        /// 删除代金券类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(CouponCategoryDelete dto)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 获取全部卷类型数据
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CouponCategoryInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CouponCategoryInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<CouponCategoryInfo>("SELECT scc.ID,scc.Name,scc.Status,scc.Remark FROM SmartCouponCategory AS scc");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据医院id查询当前医院状态为可使用的所有卷类型s
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CouponCategoryInfo>> GetByHospitalID(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CouponCategoryInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<CouponCategoryInfo>(@"SELECT scc.ID,scc.Name FROM dbo.SmartCouponCategory AS scc LEFT JOIN SmartCouponCategoryHospital  AS scch ON scc.ID = scch.CouponCategoryID WHERE scch.HospitalID = @HospitalID AND scc.Status = 1", new { HospitalID = hospitalID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id获取卷类型详情 （此处也可以优化）
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CouponCategoryInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CouponCategoryInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<CouponCategoryInfo>(@"SELECT scc.ID,scc.Name,scc.Status,scc.Remark,scc.ScopeLimit, scc.TimeLimit,Convert(varchar(30),scc.EndDate,23) EndDate, scc.Days FROM dbo.SmartCouponCategory AS scc WHERE scc.ID = @ID", new { ID = id }).FirstOrDefault();

                result.Data.SmartCouponCategoryHospitalAdd = new List<SmartCouponCategoryHospitalAdd>();
                result.Data.SmartCouponCategoryHospitalAdd = _connection.Query<SmartCouponCategoryHospitalAdd>(@"SELECT scch.ID,scch.CouponCategoryID, scch.HospitalID,sh.Name AS HospitalName FROM SmartCouponCategoryHospital AS scch INNER JOIN dbo.SmartHospital AS sh ON scch.HospitalID=sh.ID WHERE scch.CouponCategoryID=@CouponCategoryID", new { CouponCategoryID = id }).ToList(); //得到选择的医院数据

                result.Data.SmartCouponCategoryChargeAdd = new List<SmartCouponCategoryChargeAdd>();
                result.Data.SmartCouponCategoryChargeAdd = _connection.Query<SmartCouponCategoryChargeAdd>(@"SELECT sccc.ID,sccc.CouponCategoryID,sccc.ChargeID,sc.Name AS ChargeName FROM dbo.SmartCouponCategoryCharge AS sccc INNER JOIN SmartCharge AS sc ON sccc.ChargeID=sc.ID WHERE sccc.CouponCategoryID=@CouponCategoryID", new { CouponCategoryID = id }).ToList(); //得到收费项目

                result.Data.SmartCouponCategoryChargeCategoryAdd = new List<SmartCouponCategoryChargeCategoryAdd>();
                result.Data.SmartCouponCategoryChargeCategoryAdd = _connection.Query<SmartCouponCategoryChargeCategoryAdd>(@"SELECT sdccc.ID,sdccc.CouponCategoryID,sdccc.ChargeCategoryID,scc.Name AS ChargeCategoryName FROM SmartCouponCategoryChargeCategory AS sdccc INNER JOIN SmartChargeCategory AS scc ON sdccc.ChargeCategoryID=scc.ID WHERE sdccc.CouponCategoryID=@CouponCategoryID", new { CouponCategoryID = id }).ToList(); //得到收费项目类型数据


                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新卷类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(CouponCategoryUpdate dto)
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

            if (dto.SmartCouponCategoryHospitalAdd == null || dto.SmartCouponCategoryHospitalAdd.Count <= 0)
            {//添加预收款类型时必须选择适用医院
                result.Message = "请选择可使用医院！";
                return result;
            }


            if (dto.ScopeLimit == "2")
            {//按照项目分类进行限制
                if (dto.SmartCouponCategoryChargeCategoryAdd == null || dto.SmartCouponCategoryChargeCategoryAdd.Count <= 0)
                {//添加预收款类型时必须选择适用医院
                    result.Message = "请指定项目分类！";
                    return result;
                }

            }

            if (dto.ScopeLimit == "3")
            {//按照指定项目进行限制
                if (dto.SmartCouponCategoryChargeAdd == null || dto.SmartCouponCategoryChargeAdd.Count <= 0)
                {//添加预收款类型时必须选择适用医院
                    result.Message = "请指定项目！";
                    return result;
                }

            }

            if (dto.TimeLimit == "2")
            {
                if (dto.EndDate.IsNullOrEmpty())
                {
                    result.Message = "指定日期不能为空！";
                    return result;
                }
            }

            if (dto.TimeLimit == "3")
            {
                if (dto.Days.IsNullOrEmpty())
                {
                    result.Message = "生效之后N天不能为空！";
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
                 DELETE SmartCouponCategoryCharge WHERE CouponCategoryID=@CouponCategoryID
                DELETE SmartCouponCategoryChargeCategory WHERE CouponCategoryID=@CouponCategoryID
                DELETE SmartCouponCategoryHospital WHERE CouponCategoryID=@CouponCategoryID
                ";

                _connection.Execute(sql,
                   new { CouponCategoryID = dto.ID }, _transaction); //删除3张映射表中的数据

                foreach (var u in dto.SmartCouponCategoryHospitalAdd)
                {
                    _connection.Execute("insert into SmartCouponCategoryHospital(ID,CouponCategoryID,HospitalID) VALUES(@ID, @CouponCategoryID, @HospitalID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CouponCategoryID = dto.ID,
                            HospitalID = u.HospitalID
                        }, _transaction); //卷类型医院映射表
                };


                if (dto.ScopeLimit == "2")
                {//按照项目分类进行限制
                    foreach (var ud in dto.SmartCouponCategoryChargeCategoryAdd)
                    {
                        _connection.Execute(@"insert into SmartCouponCategoryChargeCategory(ID,CouponCategoryID,ChargeCategoryID) 
                                            VALUES(@ID, @CouponCategoryID, @ChargeCategoryID)",
                                           new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), CouponCategoryID = dto.ID, ChargeCategoryID = ud.ChargeCategoryID }, _transaction);
                    }

                }

                if (dto.ScopeLimit == "3")
                {//收费项目映射

                    foreach (var udc in dto.SmartCouponCategoryChargeAdd)
                    {
                        _connection.Execute(@"insert into SmartCouponCategoryCharge(ID,CouponCategoryID,ChargeID) 
                                            VALUES(@ID, @CouponCategoryID, @ChargeID)",
                  new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), CouponCategoryID = dto.ID, ChargeID = udc.ChargeID }, _transaction);
                    }

                }

                result.Data = _connection.Execute("UPDATE SmartCouponCategory SET Name=@Name,ScopeLimit=@ScopeLimit,TimeLimit=@TimeLimit,Remark=@Remark,EndDate=@EndDate,Days=@Days,Status=@Status WHERE ID=@ID", new { ID = dto.ID, Name = dto.Name, ScopeLimit = dto.ScopeLimit, TimeLimit = dto.TimeLimit, EndDate = dto.TimeLimit == "2" ? dto.EndDate : DateTime.Now.ToString("yyyy-MM-dd"), Days = dto.TimeLimit == "3" ? dto.Days : "0", Remark = dto.Remark, Status = dto.Status }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.CouponCategoryUpdate,
                    Remark = LogType.CouponCategoryUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.CouponCategory, -1);

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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.CouponCategory + ":" + hospitalID);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("select distinct a.ID,a.Name from SmartCouponCategory a,SmartCouponCategoryHospital b where a.ID=b.CouponCategoryID and b.HospitalID=@HospitalID and a.Status=@Status",
                    new { HospitalID = hospitalID, Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.CouponCategory + ":" + hospitalID, result.Data);
            });

            return result;
        }
    }
}
