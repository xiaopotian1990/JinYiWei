using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Dapper;
using Com.JinYiWei.Common.Extensions;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 排班管理业务逻辑
    /// </summary>
    public class ShiftService : BaseService, IShiftService
    {
        private ISmartUserService _userService;
        /// <summary>
        /// 添加排班信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(ShiftAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            if (dto.UserInfoList != null)
            {
                if (dto.UserInfoList.Count <= 0)
                {
                    result.Message = "排班用户不能为空！";
                    return result;
                }
            }

            if (dto.ShiftDateList != null)
            {
                if (dto.ShiftDateList.Count <= 0)
                {
                    result.Message = "排班日期不能为空！";
                    return result;
                }
            }

            #region 开启事物操作
            TryTransaction(() =>
            {
                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id

                foreach (var item1 in dto.UserInfoList)
                {
                    for (int i = 0; i < dto.ShiftDateList.Count; i++)
                    {
                        _connection.Execute("insert into SmartShift(ID,CategoryID,UserID,ShiftDate) VALUES(@ID, @CategoryID, @UserID, @ShiftDate)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CategoryID = dto.ShiftDateList[i].ShiftCategoryID,
                        UserID = item1.UserId,
                        ShiftDate = dto.ShiftDateList[i].ShiftDataTime
                    }, _transaction); //排班表
                    }
                }
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.ShiftAdd,
                    Remark = LogType.ShiftAdd.ToDescription()
                });
                #endregion


                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;

        }

        /// <summary>
        /// 删除排班信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(ShiftDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("delete SmartShift WHERE ID=@ID ", new { ID = dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.ShiftDelete,
                    Remark = LogType.ShiftDelete.ToDescription() + temp.ToJsonString()
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
        /// 获取所有排班信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Shift> Get(long hospitalID, int number)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Shift>();

            TryExecute(() =>
            {
                Shift sfi = new Shift();

                #region 排班表头集合
                DateTime dtWeekSt = new DateTime();
                DateTime dtWeekEd = new DateTime();
                //当前日期
                DateTime dtNow = DateTime.Now.Date;
                //今天是星期几
                int iNowOfWeek = (int)dtNow.DayOfWeek;
                if (iNowOfWeek == 0)
                {
                    //按中国的习惯，星期天是最后一天
                    iNowOfWeek = 7;
                }
                List<string> headerList = new List<string>();
                headerList.Add("部门");
                headerList.Add("用户");

                dtWeekSt = dtNow.AddDays(1 - iNowOfWeek);
                dtWeekEd = dtNow.AddDays(7 - iNowOfWeek);

              
                headerList.Add("" + dtWeekSt.AddDays(number * 7).ToString("yyyy-MM-dd") + "星期一");//星期一：
                headerList.Add("" + dtWeekSt.AddDays(1 + number * 7).ToString("yyyy-MM-dd") + "星期二");//星期二：
                headerList.Add("" + dtWeekSt.AddDays(2 + number * 7).ToString("yyyy-MM-dd") + "星期三");//星期三：
                headerList.Add("" + dtWeekSt.AddDays(3 + number * 7).ToString("yyyy-MM-dd") + "星期四");//星期四：
                headerList.Add("" + dtWeekSt.AddDays(4 + number * 7).ToString("yyyy-MM-dd") + "星期五");//星期五：
                headerList.Add("" + dtWeekSt.AddDays(5 + number * 7).ToString("yyyy-MM-dd") + "星期六");//星期六：
                headerList.Add("" + dtWeekEd.AddDays(number * 7).ToString("yyyy-MM-dd") + "星期日"); 
                sfi.HeaderList = headerList;
                #endregion

                #region 数据集合

                #endregion
                var list = new Dictionary<string, UserShiftTemp>();

                _connection.Query<UserShiftTemp, ShiftTemp, UserShiftTemp>(
                        @"SELECT DISTINCT a.ID,a.Name AS UserName,f.Name AS DeptName,b.ID AS ShiftID,b.CategoryID,c.Name AS CategoryName,b.ShiftDate
                    FROM dbo.SmartUser a 
                    left JOIN dbo.SmartShift b ON a.ID=b.UserID AND b.ShiftDate BETWEEN @StartTime AND @EndTime
                    left JOIN SmartShiftCategory c ON b.CategoryID=c.ID
                    left JOIN dbo.SmartUserRole d ON a.ID=d.UserID
                    left JOIN dbo.SmartRole e ON d.RoleID=e.ID AND e.CYPB=1
                    left JOIN dbo.SmartDept f ON a.DeptID=f.ID
                    WHERE a.HospitalID=@HospitalID",
                        (user, shift) =>
                            {
                                UserShiftTemp userTemp = new UserShiftTemp();
                                if (!list.TryGetValue(user.ID, out userTemp))
                                {
                                    list.Add(user.ID, userTemp = user);
                                }
                                if (shift != null)
                                    userTemp.Shifts.Add(shift);
                                return user;
                            },
                        new
                        {
                            HospitalID = hospitalID,
                            StartTime = dtWeekSt.AddDays(number * 7),
                            EndTime = dtWeekEd.AddDays(number * 7+1)
                        }, null, true, splitOn: "ShiftID");

                List<UserShiftTemp> shiftTemp = list.Values.ToList();


                sfi.UserInfoList = shiftTemp.Select(u => new UserShiftInfo
                {
                    UserID = u.ID,
                    DeptName = u.DeptName,
                    UserName = u.UserName,
                    Monday = u.Shifts.Where(m => m.ShiftDate == dtWeekSt.Date.AddDays(number * 7)).Select(n =>
                    {
                        if (n == null)
                            return new ShiftInfo()
                            {
                                UserID = u.ID,
                                ShiftDate = n.ShiftDate,
                            };
                        return new ShiftInfo()
                        {
                            ShiftID=n.ShiftID,
                            UserID = u.ID,
                            CategoryID = n.CategoryID,
                            ShiftDate = n.ShiftDate,
                            CategoryName = n.CategoryName
                        };
                    }).FirstOrDefault(),
                    Tuesday = u.Shifts.Where(m => m.ShiftDate == dtWeekSt.AddDays(number * 7+1).Date).Select(n =>
                    {
                        if (n == null)
                            return new ShiftInfo()
                            {
                                UserID = u.ID,
                                ShiftDate = n.ShiftDate,
                            };
                        return new ShiftInfo()
                        {
                            ShiftID = n.ShiftID,
                            UserID = u.ID,
                            CategoryID = n.CategoryID,
                            ShiftDate = n.ShiftDate,
                            CategoryName = n.CategoryName
                        };
                    }).FirstOrDefault(),
                    Wednesday = u.Shifts.Where(m => m.ShiftDate == dtWeekSt.AddDays(number * 7+2).Date).Select(n =>
                    {
                        if (n == null)
                            return new ShiftInfo()
                            {
                                UserID = u.ID,
                                ShiftDate = n.ShiftDate,
                            };
                        return new ShiftInfo()
                        {
                            ShiftID = n.ShiftID,
                            UserID = u.ID,
                            CategoryID = n.CategoryID,
                            ShiftDate = n.ShiftDate,
                            CategoryName = n.CategoryName
                        };
                    }).FirstOrDefault(),
                    Thursday = u.Shifts.Where(m => m.ShiftDate == dtWeekSt.AddDays(number * 7+3).Date).Select(n =>
                    {
                        if (n == null)
                            return new ShiftInfo()
                            {
                                UserID = u.ID,
                                ShiftDate = n.ShiftDate,
                            };
                        return new ShiftInfo()
                        {
                            ShiftID = n.ShiftID,
                            UserID = u.ID,
                            CategoryID = n.CategoryID,
                            ShiftDate = n.ShiftDate,
                            CategoryName = n.CategoryName
                        };
                    }).FirstOrDefault(),
                    Friday = u.Shifts.Where(m => m.ShiftDate == dtWeekSt.AddDays(number * 7+4).Date).Select(n =>
                    {
                        if (n == null)
                            return new ShiftInfo()
                            {
                                UserID = u.ID,
                                ShiftDate = n.ShiftDate,
                            };
                        return new ShiftInfo()
                        {
                            ShiftID = n.ShiftID,
                            UserID = u.ID,
                            CategoryID = n.CategoryID,
                            ShiftDate = n.ShiftDate,
                            CategoryName = n.CategoryName
                        };
                    }).FirstOrDefault(),
                    Saturday = u.Shifts.Where(m => m.ShiftDate == dtWeekSt.AddDays(number * 7+5).Date).Select(n =>
                    {
                        if (n == null)
                            return new ShiftInfo()
                            {
                                UserID = u.ID,
                                ShiftDate = n.ShiftDate,
                            };
                        return new ShiftInfo()
                        {
                            ShiftID = n.ShiftID,
                            UserID = u.ID,
                            CategoryID = n.CategoryID,
                            ShiftDate = n.ShiftDate,
                            CategoryName = n.CategoryName
                        };
                    }).FirstOrDefault(),
                    Sunday = u.Shifts.Where(m => m.ShiftDate == dtWeekSt.AddDays(number * 7+6).Date).Select(n =>
                    {
                        if (n == null)
                            return new ShiftInfo()
                            {
                                UserID = u.ID,
                                ShiftDate = n.ShiftDate,
                            };
                        return new ShiftInfo()
                        {
                            ShiftID = n.ShiftID,
                            UserID = u.ID,
                            CategoryID = n.CategoryID,
                            ShiftDate = n.ShiftDate,
                            CategoryName = n.CategoryName
                        };
                    }).FirstOrDefault()
                }).ToList();

                result.Data = sfi;

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 根据id获取排班详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ShiftInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ShiftInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<ShiftInfo>(@"SELECT ss.ID,ss.CategoryID AS ShiftCategoryID,ss.UserID,su.Name AS UserName,ss.ShiftDate AS ShiftDataTime FROM dbo.SmartShift AS ss LEFT JOIN dbo.SmartUser AS su
                   ON ss.UserID = su.ID WHERE ss.ID = @ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 更新排班信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(ShiftUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (string.IsNullOrWhiteSpace(dto.ShiftCategoryID) || dto.ShiftCategoryID == "-1")
            {
                result.Message = "请选择班次！";
                return result;
            }
            TryTransaction(() =>
            {
                result.Data = _connection.Execute("UPDATE SmartShift SET CategoryID=@CategoryID WHERE UserID = @UserID AND ShiftDate=@ShiftDates", new { CategoryID = dto.ShiftCategoryID, UserID = dto.UserID, ShiftDates=dto.DataTimeShif }, _transaction);

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.ShiftUpdate,
                    Remark = LogType.ShiftUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;

        }

        
    }
}
