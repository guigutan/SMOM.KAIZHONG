using SIE.Api;
using SIE.Common.InvOrg;
using SIE.Common.Messages;

using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.OnOffDutyA
{

    /// <summary>
    /// 上下岗
    /// </summary>
    [ApiNameAttribute("OnOffDutyAController")] 
    public partial class OnOffDutyController : WipController
    {
        /// <summary>
        /// 上下岗
        /// </summary>
        /// <param name="onOffDutyRecrod"></param>      
        /// <param name="isOnDuty"></param>        
        public virtual void OnOffDuty(OnOffDutyRecrodsA onOffDutyRecrod, bool isOnDuty)
        {
             var onDuty = Query<OnOffDutyRecrodsA>().Where(p => p.EmployeeId == onOffDutyRecrod.EmployeeId  && !p.IsAdditionalRecording && p.OnOffDutyType == OnOffDutyType.OnDuty)
                .FirstOrDefault();


            var dbTime = RF.Find<OnOffDutyRecrodsA>().GetDbTime();
            if (onOffDutyRecrod.Employee.EmployeeStatus == EmployeeStatus.UnJob)
            {
                new ValidationException("员工【{0}】已离职，无法上下岗，请检查".L10nFormat(onOffDutyRecrod.Employee.Name));
            }
            if (!isOnDuty)//下岗
            {
                if (onDuty == null)
                {                    
                    throw new ValidationException("员工【{0}】不存在上岗信息，下岗失败！请先上岗！".L10nFormat(
                       onOffDutyRecrod.Employee.Name
                       ));
                }
                onDuty.OnOffDutyType = OnOffDutyType.OffDuty;
                onDuty.OffDutyTime = dbTime;
                onDuty.OnDutyDuration = (onDuty.OffDutyTime - onDuty.OnDutyTime).Value.TotalMinutes;               
                RF.Save(onDuty);
                onOffDutyRecrod.Id = onDuty.Id;
            }
            else//上岗
            {
                if (onDuty != null)
                {                   
                    throw new ValidationException("员工【{0}】已存在上岗信息，上岗失败，请先下岗！".L10nFormat(
                       onOffDutyRecrod.Employee.Name
                       ));

                }
                onOffDutyRecrod.OnOffDutyType = OnOffDutyType.OnDuty;
                onOffDutyRecrod.OnDutyTime = dbTime;
                onOffDutyRecrod.PersistenceStatus = PersistenceStatus.New;               


                RF.Save(onOffDutyRecrod); 
            }


        }
       

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="onOffDutyRecrodsCriteria"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<OnOffDutyRecrodsA> Fetch(OnOffDutyRecrodsCriteria onOffDutyRecrodsCriteria)
        {
            var q = Query<OnOffDutyRecrodsA>();
            //员工
            if (onOffDutyRecrodsCriteria.Staff.IsNotEmpty())
            {
                q.Where(p => p.Employee.Code.Contains(onOffDutyRecrodsCriteria.Staff) || p.Employee.Name.Contains(onOffDutyRecrodsCriteria.Staff));
            }
           
            //是否补录
            if (onOffDutyRecrodsCriteria.IsAdditionalRecording.HasValue)
            {
                q.Where(p => p.IsAdditionalRecording == (onOffDutyRecrodsCriteria.IsAdditionalRecording == YesNo.Yes));
            }
            //下岗时间
            if (onOffDutyRecrodsCriteria.OffDutyTime.BeginValue.HasValue)
            {
                q.Where(p => p.OffDutyTime >= onOffDutyRecrodsCriteria.OffDutyTime.BeginValue);
            }
            if (onOffDutyRecrodsCriteria.OffDutyTime.EndValue.HasValue)
            {
                q.Where(p => p.OffDutyTime <= onOffDutyRecrodsCriteria.OffDutyTime.EndValue);
            }
            //上岗时间
            if (onOffDutyRecrodsCriteria.OnDutyTime.BeginValue.HasValue)
            {
                q.Where(p => p.OnDutyTime >= onOffDutyRecrodsCriteria.OnDutyTime.BeginValue);
            }
            if (onOffDutyRecrodsCriteria.OnDutyTime.EndValue.HasValue)
            {
                q.Where(p => p.OnDutyTime <= onOffDutyRecrodsCriteria.OnDutyTime.EndValue);
            }
            //创建时间
            if (onOffDutyRecrodsCriteria.CreateTime.BeginValue.HasValue)
            {
                q.Where(p => p.CreateDate >= onOffDutyRecrodsCriteria.CreateTime.BeginValue);
            }
            if (onOffDutyRecrodsCriteria.CreateTime.EndValue.HasValue)
            {
                q.Where(p => p.CreateDate <= onOffDutyRecrodsCriteria.CreateTime.EndValue);
            }

            if (onOffDutyRecrodsCriteria.OnOffDutyType.HasValue)
            {
                q.Where(p => p.OnOffDutyType == onOffDutyRecrodsCriteria.OnOffDutyType);
            }
            return q.OrderBy(onOffDutyRecrodsCriteria.OrderInfoList).ToList(onOffDutyRecrodsCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());



        }





        /// <summary>
        /// 查询员工
        /// </summary>
        /// <param name="onOffDutyRecrodsInputCriteria"></param>
        /// <returns></returns>
        public virtual EntityList<OnOffDutyRecrodsA> FetchOnOffDutyRecrodsInput(OnOffDutyRecrodsInputCriteria onOffDutyRecrodsInputCriteria)
        {
            var q = Query<Employee>().LeftJoin<EmployeeGroup>((x, y) => x.EmployeeGroupId == y.Id);
            //员工
            if (onOffDutyRecrodsInputCriteria.StaffCode.IsNotEmpty())
            {
                q.Where(p => p.Code.Contains(onOffDutyRecrodsInputCriteria.StaffCode));
            }
            if (onOffDutyRecrodsInputCriteria.StaffName.IsNotEmpty())
            {
                q.Where(p => p.Name.Contains(onOffDutyRecrodsInputCriteria.StaffName));
            }
            if (onOffDutyRecrodsInputCriteria.StaffGroupName.IsNotEmpty())
            {
                q.Where<EmployeeGroup>((p, z) => z.Code.Contains(onOffDutyRecrodsInputCriteria.StaffGroupName) || z.Name.Contains(onOffDutyRecrodsInputCriteria.StaffGroupName));
            }
            EntityList<Employee> employeeReuslt;
            using (InvOrgs.WithAll())
            {
                employeeReuslt = q.OrderBy(onOffDutyRecrodsInputCriteria.OrderInfoList).ToList(onOffDutyRecrodsInputCriteria.PagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            }
            EntityList<OnOffDutyRecrodsA> onOffDutyRecrodsList = new EntityList<OnOffDutyRecrodsA>();
            foreach (var item in employeeReuslt)
            {
                onOffDutyRecrodsList.Add(new OnOffDutyRecrodsA
                {
                    EmployeeId = item.Id,
                    EmployeeCode = item.Code,
                    EmployeeName = item.Name,
                    EmployeeGroupName = item.EmployeeGroupName,
                    UserCode = item.UserCode,
                });
            }
            onOffDutyRecrodsList.SetTotalCount(employeeReuslt.TotalCount);
            return onOffDutyRecrodsList;
        }

        /// <summary>
        /// 补录上下岗
        /// </summary>
        /// <param name="selectedList"></param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void SetOnOffDuty(EntityList<OnOffDutyRecrodsA> selectedList)
        {
            RF.BatchInsert(selectedList);
        }
    }
}
