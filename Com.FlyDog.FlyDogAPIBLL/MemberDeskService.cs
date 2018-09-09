using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Com.JinYiWei.Common.Extensions;
using Com.FlyDog.IFlyDogAPIBLL;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 会员工作台
    /// </summary>
    public class MemberDeskService : BaseService, IMemberDeskService
    {
        /// <summary>
        /// 查询会员
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<MemberDeskCustomer>>> Get(MemberDeskCustomerSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<MemberDeskCustomer>>();
            //resu

            string sql_where = " where 1=1 ";
            if (dto.CustomerID > 0)
            {
                sql_where += " and a.ID=@CustomerID ";
            }

            if (!dto.CustomerName.IsNullOrEmpty())
            {
                sql_where += " and a.Name like @CustomerName ";
                dto.CustomerName = "%" + dto.CustomerName + "%";
            }

            if (dto.Mobile.IsMobileNumber())
            {
                sql_where += " and (a.Mobile=@Mobile or a.MobileBackup=@Mobile )";
            }

            if (dto.Gender != GenderEnum.All)
            {
                sql_where += " and a.Gender=@Gender";
            }

            if (dto.MemberCategoryID > 0)
            {
                sql_where += " and a.MemberCategoryID=@MemberCategoryID ";
            }

            if (dto.ShareCategoryID > 0)
            {
                sql_where += " and a.ShareMemberCategoryID=@ShareCategoryID ";
            }

            if (dto.BirthdayStart != null && dto.BirthdayEnd != null)
            {
                sql_where += " and a.Birthday between @BirthdayStart and @BirthdayEnd ";
                dto.BirthdayEnd = Convert.ToDateTime(dto.BirthdayEnd).AddDays(1);
                // Convert.
            }

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<MemberDeskCustomer>(
                        string.Format(@"with tree as
                        (
                        select distinct CustomerID from SmartOwnerShip where HospitalID=@HospitalID and EndTime>GETDATE()
                        )
                        select a.ID as CustomerID,a.Name as CustomerName,a.Gender,datediff(year,a.Birthday,getdate()) as Age,a.Birthday,d.Icon as MemberCategoryImage,
                        e.Icon as ShareCategoryImage,f.Name as ManagerUserName
                        from SmartCustomer a
                        inner join tree b on a.ID=b.CustomerID 
                        left join SmartOwnerShip c on a.ID=c.CustomerID and c.Type=2 and c.EndTime>GETDATE() and c.HospitalID=@HospitalID
                        left join SmartUser f on c.UserID=f.ID
                        left join SmartMemberCategory d on a.MemberCategoryID=d.ID
                        left join SmartShareCategory e on a.ShareMemberCategoryID=e.ID {0}", sql_where), dto);

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 最近七日生日顾客
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<MemberDeskBirthdayCustomer>>> GetBirthday(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<MemberDeskBirthdayCustomer>>();


            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<MemberDeskBirthdayCustomer>(
                        @"with tree as
                        (
                        select distinct CustomerID from SmartOwnerShip where HospitalID=@HospitalID and EndTime>GETDATE()
                        )
                        select a.ID as CustomerID,a.Name as CustomerName,a.Gender,datediff(year,a.Birthday,getdate()) as Age,a.Birthday,d.Icon as MemberCategoryImage,
                        e.Icon as ShareCategoryImage,f.Name as ManagerUserName,a.Mobile
                        from SmartCustomer a
                        inner join tree b on a.ID=b.CustomerID 
                        left join SmartOwnerShip c on a.ID=c.CustomerID and c.Type=2 and c.EndTime>GETDATE() and c.HospitalID=@HospitalID
                        left join SmartUser f on c.UserID=f.ID
                        left join SmartMemberCategory d on a.MemberCategoryID=d.ID
                        left join SmartShareCategory e on a.ShareMemberCategoryID=e.ID where DATEDIFF(day, GETDATE(),DATEADD(year, DATEDIFF(year, a.Birthday, GETDATE()), a.Birthday))  between 0 and 6 order by a.Birthday",  
                        new { HospitalID=hospitalID });

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
    }
}
