using SIE.Common.InvOrg;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.WIP;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.OnOffDuty
{

    /// <summary>
    /// 上下岗
    /// </summary>
    public partial class OnOffDutyController : WipController
    {
        /// <summary>
        /// 上下岗
        /// </summary>
        /// <param name="onOffDutyRecrod"></param>
        /// <param name="workcell"></param>
        /// <param name="isOnDuty"></param>
        public virtual void OnOffDuty(OnOffDutyRecrods onOffDutyRecrod, Workcell workcell, bool isOnDuty)
        {
            CheckedWorkcellParas(workcell);
            var onDuty = Query<OnOffDutyRecrods>().Where(p => p.EmployeeId == onOffDutyRecrod.EmployeeId && p.ResourceId == onOffDutyRecrod.ResourceId
                && p.ProcessId == onOffDutyRecrod.ProcessId && p.StationId == onOffDutyRecrod.StationId && !p.IsAdditionalRecording && p.OnOffDutyType == OnOffDutyType.OnDuty)
                .FirstOrDefault();
            var dbTime = RF.Find<OnOffDutyRecrods>().GetDbTime();
            if (onOffDutyRecrod.Employee.EmployeeStatus == EmployeeStatus.UnJob)
            {
                new ValidationException("员工【{0}】已离职，无法上下岗，请检查".L10nFormat(onOffDutyRecrod.Employee.Name));
            }
            if (!isOnDuty)//下岗
            {
                if (onDuty == null)
                {
                    throw new ValidationException("员工【{0}】在资源【{1}】、工序【{2}】、工位【{3}】上不存在上岗信息，下岗失败！请先上岗！".L10nFormat(
                        onOffDutyRecrod.Employee.Name, onOffDutyRecrod.Resource.Name, onOffDutyRecrod.Process.Name, onOffDutyRecrod.Station.Name
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
                    throw new ValidationException("员工【{0}】在资源【{1}】、工序【{2}】、工位【{3}】上已存在上岗信息，上岗失败，请先下岗！".L10nFormat(
                        onOffDutyRecrod.Employee.Name, onOffDutyRecrod.Resource.Name, onOffDutyRecrod.Process.Name, onOffDutyRecrod.Station.Name
                        ));
                }
                onOffDutyRecrod.OnOffDutyType = OnOffDutyType.OnDuty;
                onOffDutyRecrod.OnDutyTime = dbTime;
                onOffDutyRecrod.PersistenceStatus = PersistenceStatus.New;
                RF.Save(onOffDutyRecrod);
            }


        }
        /// <summary>
        /// 检查工作台
        /// </summary>
        /// <param name="workcell"></param>
        /// <returns></returns>
        public virtual void CheckedWorkcellParas(Workcell workcell)
        {
            if (workcell.ProcessId <= 0)
            {
                throw new ValidationException("请选择工序".L10N());
            }
            if (workcell.StationId <= 0)
            {
                throw new ValidationException("请选择工位".L10N());
            }
            if (workcell.ResourceId <= 0)
            {
                throw new ValidationException("请选择资源".L10N());
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="onOffDutyRecrodsCriteria"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public virtual EntityList<OnOffDutyRecrods> Fetch(OnOffDutyRecrodsCriteria onOffDutyRecrodsCriteria)
        {
            var q = Query<OnOffDutyRecrods>();
            //员工
            if (onOffDutyRecrodsCriteria.Staff.IsNotEmpty())
            {
                q.Where(p => p.Employee.Code.Contains(onOffDutyRecrodsCriteria.Staff) || p.Employee.Name.Contains(onOffDutyRecrodsCriteria.Staff));
            }
            //工序
            if (onOffDutyRecrodsCriteria.ProcessId.HasValue)
            {
                q.Where(p => p.ProcessId == onOffDutyRecrodsCriteria.ProcessId);
            }
            //工位
            if (onOffDutyRecrodsCriteria.StationId.HasValue)
            {
                q.Where(p => p.StationId == onOffDutyRecrodsCriteria.StationId);
            }
            //资源
            if (onOffDutyRecrodsCriteria.ResourceId.HasValue)
            {
                q.Where(p => p.ResourceId == onOffDutyRecrodsCriteria.ResourceId);
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
        public virtual EntityList<OnOffDutyRecrods> FetchOnOffDutyRecrodsInput(OnOffDutyRecrodsInputCriteria onOffDutyRecrodsInputCriteria)
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
            EntityList<OnOffDutyRecrods> onOffDutyRecrodsList = new EntityList<OnOffDutyRecrods>();
            foreach (var item in employeeReuslt)
            {
                onOffDutyRecrodsList.Add(new OnOffDutyRecrods
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
        public virtual void SetOnOffDuty(EntityList<OnOffDutyRecrods> selectedList)
        {
            RF.BatchInsert(selectedList);
        }
    }
}
