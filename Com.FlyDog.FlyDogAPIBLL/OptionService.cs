using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Dapper;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.DataAccess;
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 系统设置业务处理
    /// </summary>
    public class OptionService : BaseService, IOptionService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 查询全部系统设置
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, OptionInfo> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, OptionInfo>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;
            var temp = _redis.StringGet<OptionInfo>(RedisPreKey.Option);

            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            #region 开始查询数据动作
            TryExecute(() =>
            {
                var option = _connection.Query<OptionInfo>("SELECT Code,Value FROM dbo.SmartOption");

                //List<OptionInfo> list = new List<OptionInfo>();
                OptionInfo oi = new OptionInfo();
              
                if (option != null)
                {
                    foreach (var item in option)
                    {
                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.Customer1)) {
                            oi.Customer1 = item.Code;
                            oi.Customer1Value = item.Value;
                        }
                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.Customer2)) {
                            oi.Customer2 = item.Code;
                            oi.Customer2Value = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) ==Convert.ToInt32(Option.Customer3)) {
                            oi.Customer3 = item.Code;
                            oi.Customer3Value = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.Customer4)) {
                            oi.Customer4 = item.Code;
                            oi.Customer4Value = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.Customer5)) {
                            oi.Customer5 = item.Code;
                            oi.Customer5Value = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.Customer6))
                        {
                            oi.Customer6 = item.Code;
                            oi.Customer6Value = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.Customer7)) {
                            oi.Customer7 = item.Code;
                            oi.Customer7Value = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.Customer8)) {
                            oi.Customer8 = item.Code;
                            oi.Customer8Value = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.Customer9)) {
                            oi.Customer9 = item.Code;
                            oi.Customer9Value = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.Customer10)) {
                            oi.Customer10 = item.Code;
                            oi.Customer10Value = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.ContentTemplate)) {
                            oi.ContentTemplateCode = item.Code;
                            oi.ContentTemplateCodeValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.AdvanceSettingsCode))
                        {
                            oi.AdvanceSettingsCode = item.Code; oi.AdvanceSettingsValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.MakeBeginTime)) {
                            oi.MakeBeginTimeCode = item.Code;
                            oi.MakeBeginTimeValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.MakeEndTime)) {
                            oi.MakeEndTimeCode = item.Code;
                            oi.MakeEndTimeValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.MakeTimeInterval)) {
                            oi.MakeTimeIntervalCode = item.Code;
                            oi.MakeTimeIntervalValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.AllowArrearsCode)) {
                            oi.AllowArrearsCode = item.Code;
                            oi.AllowArrearsValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.IntegralNum)) {
                            oi.IntegralNumCode = item.Code;
                            oi.IntegralNumValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.PrivacyProtectionCode)) {
                            oi.PrivacyProtectionCode = item.Code;
                            oi.PrivacyProtectionValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.RegistrationCode)) {
                            oi.RegistrationCode = item.Code;
                            oi.RegistrationValue = item.Value;
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.RegistrationChargeCode)) {
                            ChargeService smartCharge = new ChargeService();
                            if (item.Value!="-1") {
                                var SmartChargeData = smartCharge.GetByID(Convert.ToInt64(item.Value));

                                oi.RegistrationChargeCode = item.Code;
                                oi.RegistrationChargeValue = item.Value;
                                oi.RegistrationChargeName = SmartChargeData.Data == null ? "请选择" : SmartChargeData.Data.Name;
                            }else
                            {
                                oi.RegistrationChargeCode = item.Code;
                                oi.RegistrationChargeValue = item.Value;
                                oi.RegistrationChargeName = "请选择";
                            }
                           
                        }

                        if (Convert.ToInt32(item.Code) == Convert.ToInt32(Option.WaitingDiagnosisCode)) {
                            oi.WaitingDiagnosisCode = item.Code;
                            oi.WaitingDiagnosisValue = item.Value;
                        }
                           
                    }
                    //list.Add(oi);
                }
                result.Data = oi;

                _redis.StringSet(RedisPreKey.Option, oi);

            });
            #endregion
            return result;
        }

        /// <summary>
        /// 修改预收款成交设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> UpdateAdvanceSettings(OptionUpdateAdvanceSettings dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("UPDATE SmartOption SET Value=@Value WHERE Code=@Code",new { Value= dto.AdvanceSettingsCodeValue, Code= dto.Option17 }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.UpdateAdvanceSettings,
                    Remark = LogType.UpdateAdvanceSettings.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.Option);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 修改是否允许欠款
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> UpdateAllowArrears(OptionUpdateAllowArrears dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("UPDATE SmartOption SET Value=@Value WHERE Code=@Code",new { Value= dto.AllowArrearsCodeValue, Code= dto.Option18 }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.UpdateAllowArrears,
                    Remark = LogType.UpdateAllowArrears.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.Option);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 修改咨询模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> UpdateContentTemplate(OptionUpdateContentTemplate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("UPDATE SmartOption SET Value=@Value WHERE Code=@Code",new { Value= dto.ContentTemplateCodeValue, Code= dto.Option11 }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.UpdateAllowArrears,
                    Remark = LogType.UpdateAllowArrears.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.Option);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 修改客户自定义字段
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> UpdateCustomer(OptionUpdateCustomer dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                string sql = @"UPDATE SmartOption SET Value=@Value1 WHERE Code=@Code1 UPDATE SmartOption SET Value =@Value2  WHERE Code =@Code2 UPDATE SmartOption SET Value=@Value3 WHERE Code=@Code3 UPDATE SmartOption SET Value=@Value4 WHERE Code=@Code4   UPDATE SmartOption SET Value=@Value5 WHERE Code=@Code5 UPDATE SmartOption SET Value=@Value6 WHERE Code=@Code6 UPDATE SmartOption SET Value=@Value7 WHERE Code=@Code7 UPDATE SmartOption SET Value=@Value8 WHERE Code=@Code8 UPDATE SmartOption SET Value=@Value9 WHERE Code=@Code9 UPDATE SmartOption SET Value=@Value10 WHERE Code=@Code10";

                #region 开始更新操作
                result.Data = _connection.Execute(sql,new { Value1= dto.Customer1Value, Code1= dto.Option1, Value2= dto.Customer2Value, Code2= dto.Option2, Value3= dto.Customer3Value, Code3= dto.Option3, Value4= dto.Customer4Value, Code4= dto.Option4, Value5= dto.Customer5Value, Code5= dto.Option5, Value6= dto.Customer6Value, Code6= dto.Option6, Value7= dto.Customer7Value, Code7= dto.Option7, Value8= dto.Customer8Value, Code8= dto.Option8, Value9= dto.Customer9Value, Code9= dto.Option9, Value10= dto.Customer10Value, Code10= dto.Option10 }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.UpdateAllowArrears,
                    Remark = LogType.UpdateAllowArrears.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.Option);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 修改积分比例
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> UpdateIntegralNum(OptionUpdateIntegralNum dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                string sql = @"UPDATE SmartOption SET Value=@Value WHERE Code=@Code";

                #region 开始更新操作
                result.Data = _connection.Execute(sql,new { Value= dto.IntegralNumCodeValue, Code= dto.Option16 }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.UpdateIntegralNum,
                    Remark = LogType.UpdateIntegralNum.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.Option);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 修改预约设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> UpdateMakeTime(OptionUpdateMakeTime dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                string sql = @"UPDATE SmartOption SET Value=@Value1 WHERE Code=@Code1 UPDATE SmartOption SET Value=@Value2 WHERE Code=@Code2 UPDATE SmartOption SET Value=@Value3 WHERE Code=@Code3";

                #region 开始更新操作
                result.Data = _connection.Execute(sql,new { Value1 = dto.MakeBeginTimeCodeValue, Code1= dto.Option13, Value2= dto.MakeEndTimeCodeValue, Code2= dto.Option14, Value3= dto.MakeTimeIntervalCodeValue, Code3= dto.Option15 }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.UpdateIntegralNum,
                    Remark = LogType.UpdateIntegralNum.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.Option);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 修改隐私保护
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> UpdatePrivacyProtection(OptionUpdatePrivacyProtection dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                string sql = @"UPDATE SmartOption SET Value=@Value WHERE Code=@Code";

                #region 开始更新操作
                result.Data = _connection.Execute(sql,new { Value= dto.PrivacyProtectionCodeValue, Code= dto.Option19 }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.UpdateIntegralNum,
                    Remark = LogType.UpdateIntegralNum.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.Option);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }


        /// <summary>
        /// 修改挂号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> UpdateRegistration(OptionUpdateRegistration dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                string sql = @"UPDATE SmartOption SET Value=@Value WHERE Code=@Code UPDATE SmartOption SET Value=@Value1 WHERE Code=@Code1";

                #region 开始更新操作
                result.Data = _connection.Execute(sql,new { Value = dto.RegistrationCodeValue ,Code= dto.Option20, Value1= dto.RegistrationChargeCodeValue, Code1= dto.Option21 }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.UpdateRegistration,
                    Remark = LogType.UpdateRegistration.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.Option);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 修改等候是否开启
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> UpdateWaitingDiagnosis(OptionUpdateWaitingDiagnosis dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                string sql = @"UPDATE SmartOption SET Value=@Value WHERE Code=@Code";

                #region 开始更新操作
                result.Data = _connection.Execute(sql,new { Value= dto.WaitingDiagnosisCodeValue, Code= dto.Option22 }, _transaction);
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.UpdateRegistration,
                    Remark = LogType.UpdateRegistration.ToDescription()
                });
                #endregion

                CacheDelete.OptionUpdate(RedisPreKey.Option);
                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }
    }
}
