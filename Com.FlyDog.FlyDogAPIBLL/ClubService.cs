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
    /// 单项目管理业务逻辑
    /// </summary>
    public class ClubService : BaseService, IClubService
    {
        /// <summary>
        /// 添加单项目管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(ClubAdd dto)
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

            if (dto.ScopeLimit == "2")
            {//对单个项目有效
                if (dto.ChargeID.IsNullOrEmpty() || dto.ChargeID == "-1")
                {
                    result.Message = "请选择项目！";
                    return result;
                }
            }

            if (dto.ScopeLimit == "1")
            {//对一类项目有效
                if (dto.ChargeCategoryID.IsNullOrEmpty() || dto.ChargeCategoryID == "-1")
                {
                    result.Message = "请选择项目分类！";
                    return result;
                }
            }

            if (dto.Icon.IsNullOrEmpty())
            {
                result.Message = "请上传图标！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 100)
            {
                result.Message = "备注最多100个字符！";
                return result;
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {

                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute(@"  insert into SmartClub(ID,Name,ScopeLimit,ChargeCategoryID,ChargeID,CreateUserID,Icon,Remark,CreateTime,HospitalID)
                  values(@ID, @Name,@ScopeLimit,@ChargeCategoryID, @ChargeID, @CreateUserID, @Icon, @Remark, @CreateTime, @HospitalID)",
                    new
                    {
                        ID = id,
                        Name = dto.Name,
                        ScopeLimit = dto.ScopeLimit,
                        ChargeCategoryID = dto.ScopeLimit == "1" ? dto.ChargeCategoryID : "0",
                        ChargeID = dto.ScopeLimit == "2" ? dto.ChargeID : "0",
                        CreateUserID = dto.UserID,
                        Icon = dto.Icon,
                        Remark = dto.Remark,
                        CreateTime = DateTime.Now,
                        HospitalID = dto.HospitalID
                    }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID=Convert.ToInt64(dto.UserID),
                    Type = LogType.ClubAdd,
                    Remark = LogType.ClubAdd.ToDescription() + temp.ToJsonString()
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
        /// 删除单项目管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(ClubDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.ID.IsNullOrEmpty()) {
                result.Message = "删除失败，ID不能为空";
                return result;
            }

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute(" DELETE SmartClub WHERE ID=@ID", new { ID = dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.ClubDelete,
                    Remark = LogType.ClubDelete.ToDescription() + temp.ToJsonString()
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
        /// 查询全部单项目管理信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ClubInfo>> Get(string hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ClubInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<ClubInfo>(@"SELECT scl.ID,scl.Name,scl.ChargeID,
                            (CASE scl.ScopeLimit   WHEN 1 THEN(SELECT Name FROM dbo.SmartChargeCategory WHERE ID = scl.ChargeCategoryID) WHEN 2 THEN(SELECT Name FROM SmartCharge WHERE ID = scl.ChargeID) END) AS ChargeName,scl.Icon,su.Name AS UserName,Convert(varchar(30),scl.CreateTime,23) AS CreateTime,scl.Remark,scl.HospitalID  FROM SmartClub AS  scl 
                            LEFT JOIN SmartCharge AS sc  ON scl.ChargeID=sc.ID 
                            LEFT JOIN dbo.SmartUser AS su ON scl.CreateUserID=su.ID
                            LEFT JOIN SmartChargeCategory AS scc ON scl.ChargeCategoryID=scc.ID WHERE scl.HospitalID=@HospitalID",new { HospitalID = hospitalID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }
    }
}
