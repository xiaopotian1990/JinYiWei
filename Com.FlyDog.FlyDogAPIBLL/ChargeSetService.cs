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
    /// 套餐管理业务逻辑
    /// </summary>
    public class ChargeSetService : BaseService, IChargeSetService
    {
        /// <summary>
        /// 添加套餐管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(ChargeSetAdd dto)
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
            else if (!dto.PinYin.IsNullOrEmpty() && dto.PinYin.Length >= 20)
            {
                result.Message = "拼音码最多20个字符！";
                return result;
            }
            int day = 0;
            if (dto.TimeLimit == "1")
            {//如果有时间限制
                if (dto.Days.IsNullOrEmpty())
                {
                    result.Message = "天数不能为空！";
                    return result;
                }
                else if (!string.IsNullOrWhiteSpace(dto.Days) && !int.TryParse(dto.Days, out day))
                {
                    result.Message = "请输入有效的天数！";
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

            if (dto.SmartChargeSetDetailAdd==null||dto.SmartChargeSetDetailAdd.Count <= 0)
            {
                result.Message = "请添加套餐详细！";
                return result;
            }

            #endregion

            TryTransaction(() =>
            {

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                double price = 0;
                foreach (var u in dto.SmartChargeSetDetailAdd)
                {
                    _connection.Execute(@"insert into SmartChargeSetDetail(ID,SetID,ChargeID,Num,Amount) 
                                            VALUES(@ID, @SetID, @ChargeID, @Num, @Amount)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            SetID = id,
                            ChargeID = u.ChargeID,
                            Num = u.Num,
                            Amount = u.Amount
                        }, _transaction); //退货记录详情表
                    price += Convert.ToDouble(u.Amount);//计算套餐的总价格
                };

                _connection.Execute(@"insert into SmartChargeSet(ID,Name,Price,Status,Remark,PinYin,TimeLimit,TimeStart,Days) 
                                    VALUES(@ID, @Name, @Price, @Status, @Remark, @PinYin, @TimeLimit, @TimeStart, @Days)",
                 new { ID = id, Name = dto.Name, Price = price, Status = dto.Status, Remark = dto.Remark, PinYin = dto.PinYin, TimeLimit = dto.TimeLimit, TimeStart = dto.TimeLimit == "1" ? dto.TimeStart : "0", Days = dto.TimeLimit == "1" ? dto.Days : "0" }, _transaction);  //套餐



                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.ChargeSetAdd,
                    Remark = LogType.ChargeSetAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;

        }

        /// <summary>
        /// 获取全部套餐管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeSetInfo>>> Get(ChargeSetSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeSetInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<ChargeSetInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT ID,Name,Price,Status,Remark,PinYin,TimeLimit,TimeStart,Days FROM SmartChargeSet  WHERE 1=1 ";

                sql2 = @"SELECT COUNT(ID) AS Count FROM SmartChargeSet  WHERE 1=1 ";

                if (!string.IsNullOrWhiteSpace(dto.PinYin))
                {
                    sql += @" AND PinYin LIKE '%" + dto.PinYin + "%'";
                    sql2 += @" AND PinYin LIKE '%" + dto.PinYin + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql += @" AND Name LIKE '%" + dto.Name + "%'";
                    sql2 += " AND Name LIKE '%" + dto.Name + "%'";
                }
                //" ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only
                if (!string.IsNullOrWhiteSpace(dto.Status) && dto.Status != "-1")
                {
                    sql += @" AND Status =" + dto.Status + "";
                    sql2 += @" AND Status =" + dto.Status + "";
                }

                sql += " ORDER BY Status,ID DESC OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<ChargeSetInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询套餐详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ChargeSetInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ChargeSetInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<ChargeSetInfo>("SELECT ID,Name,Price,Status,Remark,PinYin,TimeLimit,TimeStart,Days FROM SmartChargeSet where ID=@ID", new { ID = id }).FirstOrDefault();

                result.Data.SmartChargeSetDetailAdd = new List<SmartChargeSetDetail>();
                result.Data.SmartChargeSetDetailAdd = _connection.Query<SmartChargeSetDetail>(@"SELECT scsd.ID,scsd.SetID,scsd.ChargeID,sc.Name AS ChargeName,scsd.Num,scsd.Amount FROM SmartChargeSetDetail AS scsd INNER JOIN SmartCharge AS sc ON scsd.ChargeID=sc.ID WHERE scsd.SetID=@SetID", new { SetID = id }).ToList();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 更新套餐
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(ChargeSetUpdate dto)
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
            else if (!dto.PinYin.IsNullOrEmpty() && dto.PinYin.Length >= 20)
            {
                result.Message = "拼音码最多20个字符！";
                return result;
            }
            int day = 0;
            if (dto.TimeLimit == "1")
            {//如果有时间限制
                if (dto.Days.IsNullOrEmpty())
                {
                    result.Message = "天数不能为空！";
                    return result;
                }
                else if (!string.IsNullOrWhiteSpace(dto.Days) && !int.TryParse(dto.Days, out day))
                {
                    result.Message = "请输入有效的天数！";
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

            if (dto.SmartChargeSetDetailAdd==null||dto.SmartChargeSetDetailAdd.Count <= 0)
            {
                result.Message = "请添加套餐详细！";
                return result;
            }

            #endregion

            TryTransaction(() =>
            {

                _connection.Execute(@"DELETE SmartChargeSetDetail WHERE SetID=@SetID",
                 new { SetID = dto.ID }, _transaction);  //先删除套餐映射表中的数据

                double price = 0;
                foreach (var u in dto.SmartChargeSetDetailAdd)
                {
                    _connection.Execute(@"insert into SmartChargeSetDetail(ID,SetID,ChargeID,Num,Amount) 
                        VALUES(@ID, @SetID, @ChargeID, @Num, @Amount)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            SetID = dto.ID,
                            ChargeID = u.ChargeID,
                            Num = u.Num,
                            Amount = u.Amount
                        }, _transaction); //套餐映射表
                    price += Convert.ToDouble(u.Amount);//计算套餐的总价格
                };

                _connection.Execute(@"UPDATE SmartChargeSet SET Name=@Name,Price=@Price,Status=@Status,Remark=@Remark,PinYin=@PinYin,TimeLimit=@TimeLimit,
                  TimeStart=@TimeStart,Days=@Days WHERE ID=@ID",
                 new { ID = dto.ID, Name = dto.Name, Price = price, Status = dto.Status, Remark = dto.Remark, PinYin = dto.PinYin, TimeLimit = dto.TimeLimit, TimeStart = dto.TimeLimit == "1" ? dto.TimeStart : "0", Days = dto.TimeLimit == "1" ? dto.Days : "0" }, _transaction);  //套餐

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.ChargeSetUpdate,
                    Remark = LogType.ChargeSetUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
