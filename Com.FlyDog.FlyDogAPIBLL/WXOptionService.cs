using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.Common;
using Dapper;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 微信系统设置业务逻辑类
    /// </summary>
    public class WXOptionService : BaseService, IWXOptionService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 获取微信系统所有设置
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, WXOptionInfo> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, WXOptionInfo>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;
            var temp = _redis.StringGet<WXOptionInfo>(RedisPreKey.WXOption);

            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            var pd = GetPromoteLevel();
            var channel = GetChannelInfo();
            #region 开始查询数据动作
            TryExecute(() =>
            {
                var option = _connection.Query<WXOptionInfo>("SELECT Code,Value FROM dbo.SmartOption");

                //List<OptionInfo> list = new List<OptionInfo>();
                WXOptionInfo oi = new WXOptionInfo();

                if (option != null)
                {
                    foreach (var item in option)
                    {
                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(WXOption.UserSendVolumeCode))//被推荐用户送卷是否开启
                        {
                            oi.SendVolumeCode = item.Code;
                            oi.SendVolumeValue = item.Value;
                        }
                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(WXOption.UserCouponCategoryCode)) //被推荐用户送券，卷类型Code
                        {
                            oi.UserCouponCategoryCode = item.Code;
                            oi.UserCouponCategoryValue = item.Value;

                            if (oi.UserCouponCategoryValue != "-1") {
                                CouponCategoryService ccs = new CouponCategoryService();
                                var couPonInfo = ccs.GetByID(Convert.ToInt64(oi.UserCouponCategoryValue));
                                if (couPonInfo != null) {
                                    oi.UserCouponCategoryName = couPonInfo.Data.Name;
                                }
                            }
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(WXOption.CouponCategoryMoneyCode))//被推荐用户送卷卷金额
                        {
                            oi.CouponCategoryMoneyCode = item.Code;
                            oi.CouponCategoryMoneyValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(WXOption.RecommendNumberDayCode))//推荐时限
                        {
                            oi.RecommendNumberDayCode = item.Code;
                            oi.RecommendNumberDayValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(WXOption.NoDiscountCode))//不提点折扣
                        {
                            oi.NoDiscountCode = item.Code;
                            oi.NoDiscountValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(WXOption.OpenCommissionCode))//是否开启佣金提成
                        {
                            oi.OpenCommissionCode = item.Code;
                            oi.OpenCommissionValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(WXOption.CommissionLevelCode))//佣金提成等级code
                        {
                            oi.CommissionLevelCode = item.Code;
                            oi.CommissionLevelValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(WXOption.CommissionRemindedCode))//佣金提成提点code
                        {
                            oi.CommissionRemindedCode = item.Code;
                            oi.CommissionRemindedValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(WXOption.ChannelCode))//默认渠道code
                        {
                            oi.ChannelCode = item.Code;
                            oi.ChannelValue = item.Value;
                        }

                        List<PromoteLevelInfo> promoteLevelList = new List<PromoteLevelInfo>();
                        if (pd.Data != null && pd.Data.Count() > 0)
                        {
                            foreach (var items in pd.Data)
                            {
                                PromoteLevelInfo pl = new PromoteLevelInfo();
                                pl.ID = items.ID;
                                pl.Level = items.Level;
                                pl.Rate = items.Rate;
                                promoteLevelList.Add(pl);
                            }
                        }

                        List<ChannelInfo> channelList = new List<ChannelInfo>();
                        if (channel.Data != null && channel.Data.Count() > 0)
                        {
                            foreach (var itemChannel in channel.Data)
                            {
                                ChannelInfo cl = new ChannelInfo();
                                cl.ID = itemChannel.ID;
                                cl.Name = itemChannel.Name;
                                channelList.Add(cl);
                            }
                        }
                        oi.ChannelInfoList = channelList;
                        oi.PromoteLevelList = promoteLevelList;
                    }
                }
                result.Data = oi;
                _redis.StringSet(RedisPreKey.WXOption, oi);
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 查询级别提点信息
        /// </summary>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, IEnumerable<PromoteLevelInfo>> GetPromoteLevel()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<PromoteLevelInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<PromoteLevelInfo>("SELECT ID,Level,Rate FROM dbo.SmartPromoteLevel");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 查询特殊渠道信息
        /// </summary>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, IEnumerable<ChannelInfo>> GetChannelInfo()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ChannelInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<ChannelInfo>("SELECT swc.ChanelID as ID,sc.Name FROM SmartWeChatChannel AS swc LEFT JOIN dbo.SmartChannel AS sc ON swc.ChanelID = sc.ID");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }


        /// <summary>
        /// 删除级别提点
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> DeletePromoteLevel()
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("DELETE FROM SmartPromoteLevel WHERE 1=1", null, _transaction);

                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    Type = LogType.SmartUnitDelete,
                    Remark = "删除级别提点"
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
        ///  微信系统设置级别提点
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> WXOptionInsertPromoteLevel(WXOptionUpdatePromoteLevel dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            DeletePromoteLevel();//插入之前先删除

            if (dto.PromoteLevelAddInfo == null || dto.PromoteLevelAddInfo.Count == 0) {
                result.Message = "级别提点信息不能为空！";
                return result;
            }

            //去除重复的级别信息
            for (int i = 0; i < dto.PromoteLevelAddInfo.Count; i++)  //外循环是循环的次数
            {
                for (int j = dto.PromoteLevelAddInfo.Count - 1; j > i; j--)  //内循环是 外循环一次比较的次数
                {

                    if (dto.PromoteLevelAddInfo[i].Level == dto.PromoteLevelAddInfo[j].Level)
                    {
                        dto.PromoteLevelAddInfo.RemoveAt(j);
                    }

                }
            }

            #region 开启事物操作
            TryTransaction(() =>
            {
                #region 开始数据操作动作
                if (dto.PromoteLevelAddInfo != null && dto.PromoteLevelAddInfo.Count > 0)
                {
                    foreach (var itemAdd in dto.PromoteLevelAddInfo)
                    {
                        result.Data = _connection.Execute("insert into SmartPromoteLevel(ID,Level,Rate) values (@ID,@Level,@Rate)",
                new
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    Level = itemAdd.Level,
                    Rate = itemAdd.Rate
                }, _transaction);
                    }
                }

                var temp = new { 编号 = result.Data };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.WXOptionUpdatePromoteLevelAdd,
                    Remark = LogType.WXOptionUpdatePromoteLevelAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.WXOption);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 微信系统设置佣金提成
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> WXOptionOpenCommission(WXOptionOpenCommission dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                #region 开始更新操作
                string sql = @"UPDATE SmartOption SET Value=@Value1 WHERE Code=@Code1
                                UPDATE SmartOption SET Value=@Value2 WHERE Code=@Code2
                                UPDATE SmartOption SET Value=@Value3 WHERE Code=@Code3";

                result.Data = _connection.Execute(sql, new {
                    Value1 = dto.OpenCommissionValue,
                    Code1 = dto.OpenCommissionCode,
                    Value2 = dto.CommissionLevelValue,
                    Code2 = dto.CommissionLevelCode,
                    Value3 = dto.CommissionRemindedValue,
                    Code3 = dto.CommissionRemindedCode
                }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.WXOptionOpenCommission,
                    Remark = LogType.WXOptionOpenCommission.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.WXOption);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 微信系统设置修改不提点折扣小于
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> WXOptionUpdateNoDiscount(WXOptionUpdateNoDiscount dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("UPDATE SmartOption SET Value=@Value WHERE Code=@Code", new { Value = dto.NoDiscountValue, Code = dto.NoDiscountCode }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.WXOptionUpdateNoDiscount,
                    Remark = LogType.WXOptionUpdateNoDiscount.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.WXOption);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        ///  微信系统设置修改推荐时限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> WXOptionUpdateRecommenDay(WXOptionUpdateRecommenDay dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("UPDATE SmartOption SET Value=@Value WHERE Code=@Code", new { Value = dto.RecommendNumberDayValue, Code = dto.RecommendNumberDayCode }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.WXOptionUpdateRecommenDay,
                    Remark = LogType.WXOptionUpdateRecommenDay.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.WXOption);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 更新被推荐用户送券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> WXOptionUpdateUserSendVolume(WXOptionUpdateUserSendVolume dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;


            TryTransaction(() =>
            {
                string sql = @"UPDATE SmartOption SET Value=@Value1 WHERE Code=@Code1
                                UPDATE SmartOption SET Value=@Value2 WHERE Code=@Code2
                                UPDATE SmartOption SET Value=@Value3 WHERE Code=@Code3";

                #region 开始更新操作
                result.Data = _connection.Execute(sql, new {
                    Value1 = dto.UserSendVolumeValue,
                    Code1 = dto.UserSendVolumeCode,
                    Value2 = dto.UserCouponCategoryValue,
                    Code2 = dto.UserCouponCategoryCode,
                    Value3 = dto.CouponCategoryMoneyValue,
                    Code3 = dto.CouponCategoryMoneyCode
                }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.WXOptionUpdateUserSendVolume,
                    Remark = LogType.WXOptionUpdateUserSendVolume.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.WXOption);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 微信系统设置默认渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> WXOptionDefaultChannel(WXOptionDefaultChannel dto) {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("UPDATE SmartOption SET Value=@Value WHERE Code=@Code", new { Value = dto.ChannelValue, Code = dto.ChannelCode }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.WXOptionDefaultChannel,
                    Remark = LogType.WXOptionDefaultChannel.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.WXOption);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 删除微信设置特殊渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> DeleteWeChatChannel()
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("DELETE SmartWeChatChannel WHERE 1=1", null, _transaction);
                #endregion
                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 微信系统设置特殊渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> WXOptionSpecialChannel(WXOptionSpecialChannel dto) {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            DeleteWeChatChannel();//插入之前先删除

            #region 开启事物操作
            TryTransaction(() =>
            {
                #region 开始数据操作动作
                if (dto.SpecialChannelAddInfoList != null && dto.SpecialChannelAddInfoList.Count > 0)
                {
                    foreach (var itemAdd in dto.SpecialChannelAddInfoList)
                    {
                        result.Data = _connection.Execute("insert into SmartWeChatChannel(ID,ChanelID) values (@ID,@ChanelID)",
                new
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    ChanelID = itemAdd.ID
                }, _transaction);
                    }
                }

                var temp = new { 编号 = result.Data };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.WXOptionSpecialChannel,
                    Remark = LogType.WXOptionSpecialChannel.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.WXOption);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }
    }
}
