using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class ReceptionService : BaseService, IReceptionService
    {
        /// <summary>
        /// 获取今日接待记录
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ReceptionTodayInfo>> GetReceptionTodayAsync(long hospitalID, long userID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ReceptionTodayInfo>();

            await TryExecuteAsync(async () =>
            {
                var clubTemp = await _connection.QueryAsync<ClubTemp>(
                        @"with tree as
                        (
                        select [ChargeCategoryID],[Icon] from SmartClub where HospitalID=@HospitalID and [ScopeLimit]=1 
                        union all
                        select a.ID,b.Icon from SmartChargeCategory a,tree b where a.ParentID=b.ChargeCategoryID
                        )

				    	select distinct a.ID as ChargeID,b.Icon from SmartCharge a
				    	inner join tree b on a.CategoryID=b.ChargeCategoryID
					    inner join SmartClub c on a.ID!=c.ChargeID and [ScopeLimit]=2 and HospitalID=@HospitalID
					    union 
					    select distinct [ChargeID],Icon from SmartClub where [ScopeLimit]=2 and HospitalID=@HospitalID", new { HospitalID = hospitalID });

                var temp = await _connection.QueryAsync<ReceptionToday>(
                        @"with treeUser as
                        (
                        select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@UserID
                        union 
                        select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@UserID
                        union 
                        select distinct OwinUserID from SmartUserUser where UserID=@UserID
                        ),
						treeCustomer as
						(
						select distinct a.CustomerID from SmartOwnerShip a,treeUser b where a.UserID=b.ID  and a.EndTime> getdate() 
						union
						select distinct CustomerID from SmartTriage where AssignUserID=@UserID and DateDiff(dd,CreateTime,getdate())=0
						)

						select a.CustomerID,c.Name as CustomerName,e.Name as ManagerName,a.CreateTime,f.Name as AssignUserName,
						a.VisitType,case when a.VisitType in (1,2) then 1 else 2 end as CustomerType,g.Name as SymptomName,
						case when c.FirstDealTime is null then 0 else 1 end as DealType,h.Icon as MemberIcon,i.Icon as ShareIcon
						from [SmartVisit] a
						inner join treeCustomer b on a.CustomerID=b.CustomerID
						inner join SmartCustomer c on a.CustomerID=c.ID
						left join SmartOwnerShip d on a.CustomerID=d.CustomerID and d.Type=2 and d.EndTime>getdate() and d.HospitalID=@HospitalID
						left join SmartUser e on d.UserID=e.ID
						left join SmartUser f on a.UserID=f.ID
						left join SmartSymptom g on c.CurrentConsultSymptomID=g.ID
						left join SmartMemberCategory h on c.MemberCategoryID=h.ID
                        left join SmartShareCategory i on i.ID=c.ShareMemberCategoryID
						where a.HospitalID=@HospitalID and DateDiff(dd,a.CreateTime,getdate())=0 order by f.CreateTime desc",
                        new
                        {
                            HospitalID = hospitalID,
                            UserID = userID
                        });

                var charges = await _connection.QueryAsync<ReceptionChargeTemp>(
                        @"select a.CustomerID,b.ChargeID,b.FinalPrice,c.Name as ChargeName,a.PaidTime
						from SmartOrder a
						inner join SmartOrderDetail b on a.ID=b.OrderID
						inner join SmartCharge c on b.ChargeID=c.ID
                        inner join SmartVisit d on a.CustomerID=d.CustomerID and d.HospitalID=@HospitalID and DateDiff(dd,d.CreateTime,getdate())=0
						where a.PaidStatus!=@PaidStatus and a.HospitalID=@HospitalID
						union all
						select a.CustomerID,b.ChargeID,b.Amount*-1,c.Name as ChargeName,a.PaidTime
						from SmartBackOrder a
						inner join SmartBackOrderDetail b on a.ID=b.OrderID
						inner join SmartCharge c on b.ChargeID=c.ID
                        inner join SmartVisit d on a.CustomerID=d.CustomerID and d.HospitalID=@HospitalID and DateDiff(dd,d.CreateTime,getdate())=0
						where DateDiff(dd,a.PaidTime,getdate())=0 and a.HospitalID=@HospitalID", new { PaidStatus = PaidStatus.NotPaid, HospitalID = hospitalID });

                var rebate = await _connection.QueryAsync<ReceptionBack>(
                        @"select a.CustomerID,sum(a.Amount) as Amount,1 as Type
						from SmartDepositOrder a
						where DateDiff(dd,a.PaidTime,getdate())=0 and a.HospitalID=@HospitalID group by a.CustomerID 
						union all
						select a.CustomerID,sum(a.Amount) as Amount,2 as Type
						from SmartDepositRebateOrder a
						where DateDiff(dd,a.PaidTime,getdate())=0 and a.HospitalID=@HospitalID group by a.CustomerID", new { HospitalID = hospitalID });

                var data = new ReceptionTodayInfo();

                foreach (var u in temp)
                {
                    foreach (var m in charges)
                    {
                        if (u.CustomerID == m.CustomerID)
                        {
                            if (m.PaidTime > DateTime.Today)
                            {
                                u.ChargeName += m.ChargeName + "($ " + m.FinalPrice + ")" + "<br/>";
                                u.FinalPrice += m.FinalPrice;
                                data.Amount += m.FinalPrice;
                            }

                            foreach (var o in clubTemp)
                            {
                                if (o.ChargeID == m.ChargeID)
                                {
                                    u.ClubIconList.Add(o.Icon);
                                }
                            }
                        }
                    }
                    foreach (var n in rebate)
                    {
                        if (u.CustomerID == n.CustomerID)
                        {
                            if (n.Type == 1)
                            {
                                u.ChargeName += "预收款($ " + n.Amount + ")" + "<br/>";
                            }
                            else if (n.Type == 2)
                            {
                                u.ChargeName += "退款($ " + n.Amount + ")" + "<br/>";
                            }
                        }
                    }

                    if (u.DealType == DealType.Yes)
                    {
                        data.Deal += 1;
                    }
                    else if (u.DealType == DealType.No)
                    {
                        data.NotDeal += 1;
                    }

                    switch (u.VisitType)
                    {
                        case VisitType.Again:
                            data.Again += 1;
                            break;
                        case VisitType.Check:
                            data.Check += 1;
                            break;
                        case VisitType.First:
                            data.First += 1;
                            break;
                        case VisitType.Twice:
                            data.Twice += 1;
                            break;
                    }

                    if (u.CustomerType == CustomerType.New)
                    {
                        data.New += 1;
                    }
                    else if (u.CustomerType == CustomerType.Old)
                    {
                        data.Old += 1;
                    }

                    data.AllPeople += 1;
                }

                foreach (var u in temp)
                {
                    u.ClubIconList = u.ClubIconList.Distinct().ToList();
                }

                data.Receptions = temp;
                result.Data = data;

                result.Message = "查询成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
    }
}
