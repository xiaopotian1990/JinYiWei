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

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 打印设置业务处理
    /// </summary>
    public class HospitalPrintService : BaseService, IHospitalPrintService
    {
        /// <summary>
        /// 获取所有打印设置
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<HospitalPrintInfo>> Get(string hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<HospitalPrintInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<HospitalPrintInfo>("SELECT ID,HospitalID,Type,Width,Content,FontSize,FontFamily FROM dbo.SmartHospitalPrint WHERE HospitalID=@HospitalID",new { HospitalID= hospitalID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据医院id，类型查询打印设置
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, HospitalPrintInfo> GetByHospitalAndType(long hospitalID, string type)
        {
            var result = new IFlyDogResult<IFlyDogResultType, HospitalPrintInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<HospitalPrintInfo>("SELECT ID,HospitalID,PrintExplain,Type,Width,Content,FontSize,FontFamily FROM dbo.SmartHospitalPrint WHERE HospitalID = @HospitalID AND Type = @Type", new { HospitalID = hospitalID, Type= type }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id获取打印详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, HospitalPrintInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, HospitalPrintInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<HospitalPrintInfo>("SELECT ID,HospitalID,Type,Width,Content,FontSize,FontFamily,PrintExplain FROM dbo.SmartHospitalPrint WHERE ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }



        /// <summary>
        /// 更新打印设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(HospitalPrintUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "打印设置不能为空！";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("UPDATE SmartHospitalPrint SET Content=@Content,Width=@Width,FontSize=@FontSize,FontFamily=@FontFamily WHERE ID=@ID", new {
                    Content=dto.Content,
                    Width=dto.Width,
                    FontSize=dto.FontSize,
                    FontFamily=dto.FontFamily,
                    ID=dto.ID
                }, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Content };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.HospitalPrintUpdate,
                    Remark = LogType.HospitalPrintUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
