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
    /// 客户病例模板业务逻辑
    /// </summary>
    public class CustomerMedicalRecordService : BaseService, ICustomerMedicalRecordService
    {
        /// <summary>
        /// 新增病例模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(CustomerMedicalRecordAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.CustomerID == 0 || dto.CustomerID == -1)
            {
                result.Message = "客户ID异常！";
                return result;
            }

            if (dto.MedicalRecordID == 0 || dto.MedicalRecordID == -1)
            {
                result.Message = "请选择病例模板！";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                result.Message = "诊断结果不能为空！";
                return result;
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {

                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute("insert into SmartCustomerMedicalRecord(ID,CustomerID,MedicalRecordID,CreateTime,CreateUserID,No,Location,Content,Remark) values(@ID, @CustomerID, @MedicalRecordID, @CreateTime, @CreateUserID, @No, @Location, @Content, @Remark)",
                    new
                    {
                        ID = id,
                        CustomerID = dto.CustomerID,
                        MedicalRecordID = dto.MedicalRecordID,
                        CreateTime = DateTime.Now,
                        CreateUserID = dto.CreateUserID,
                        No = dto.No,
                        Location = dto.Location,
                        Content = dto.Content,
                        Remark = dto.Remark
                    }, _transaction);

                var temp = new { 编号 = result.Data, 顾客id = dto.CustomerID,病例模板id=dto.MedicalRecordID,内容=dto.Content };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CustomerMedicalRecordAdd,
                    Remark = LogType.CustomerMedicalRecordAdd.ToDescription() + temp.ToJsonString()
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
        /// 列表查询客户病例信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerMedicalRecordInfo>> Get(CustomerMedicalRecordSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerMedicalRecordInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<CustomerMedicalRecordInfo>(@"SELECT scmr.ID,scmr.CreateTime,('【'+sh.Name+'】'+' '+'【'+sd.Name+'】'+'【'+su.Name+'】') AS CreateUserInfo,sct.Title, scmr.No, scmr.Location, scmr.Remark FROM dbo.SmartCustomerMedicalRecord AS scmr
                LEFT JOIN dbo.SmartUser AS su ON scmr.CreateUserID = su.ID
                LEFT JOIN dbo.SmartDept AS sd ON su.DeptID = sd.ID
                LEFT JOIN dbo.SmartHospital AS sh ON su.HospitalID = sh.ID
                LEFT JOIN dbo.SmartCaseTemplate AS sct ON scmr.MedicalRecordID = sct.ID
                WHERE scmr.CustomerID = @CustomerID",new { CustomerID=dto.CustomerID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询病例模板详情，客户病例模板列表使用
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CustomerMedicalRecordInfo> GetByPKID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CustomerMedicalRecordInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<CustomerMedicalRecordInfo>(@"SELECT scmr.ID,scmr.MedicalRecordID,scmr.CreateTime,('【'+sh.Name+'】'+' '+'【'+sd.Name+'】'+'【'+su.Name+'】') AS CreateUserInfo,sct.Title,scmr.Content, scmr.No,scmt.Name AS CustomerName,scmr.Location, scmr.Remark FROM dbo.SmartCustomerMedicalRecord AS scmr
                LEFT JOIN dbo.SmartUser AS su ON scmr.CreateUserID = su.ID
                LEFT JOIN dbo.SmartDept AS sd ON su.DeptID = sd.ID
                LEFT JOIN dbo.SmartHospital AS sh ON su.HospitalID = sh.ID
                LEFT JOIN dbo.SmartCaseTemplate AS sct ON scmr.MedicalRecordID = sct.ID
                LEFT JOIN dbo.SmartCustomer AS scmt ON scmr.CustomerID = scmt.ID
                WHERE scmr.ID = @ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据病例模板详情查询客户病例模板详情(添加客户病例功能使用)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CustomerMedicalRecordInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CustomerMedicalRecordInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<CustomerMedicalRecordInfo>("SELECT ID,CustomerID,MedicalRecordID,CreateTime,CreateUserID,No,Location,Content,Remark FROM dbo.SmartCustomerMedicalRecord WHERE ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新客户病例模板详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(CustomerMedicalRecordUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.MedicalRecordID == 0 || dto.MedicalRecordID == -1)
            {
                result.Message = "请选择病例模板！";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Content))
            {
                result.Message = "诊断结果不能为空！";
                return result;
            }
            #endregion
            #region 开启事物操作
            TryTransaction(() =>
            {

                #region 开始数据操作动作
                result.Data = _connection.Execute("UPDATE SmartCustomerMedicalRecord SET MedicalRecordID=@MedicalRecordID,No=@No,Location=@Location,Remark=@Remark,Content=@Content WHERE ID = @ID",
                    new
                    {
                        ID = dto.ID,
                        MedicalRecordID = dto.MedicalRecordID,
                        No = dto.No,
                        Location = dto.Location,
                        Content = dto.Content,
                        Remark = dto.Remark
                    }, _transaction);

                var temp = new { 编号 = result.Data,id = dto.ID, 病例模板id = dto.MedicalRecordID, 内容 = dto.Content };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CustomerMedicalRecordUpdate,
                    Remark = LogType.CustomerMedicalRecordUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 删除顾客病例模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(CustomerMedicalRecordDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("DELETE SmartCustomerMedicalRecord WHERE ID=@ID", new { ID=dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CustomerMedicalRecordDelete,
                    Remark = LogType.CustomerMedicalRecordDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion
                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }
    }
}
