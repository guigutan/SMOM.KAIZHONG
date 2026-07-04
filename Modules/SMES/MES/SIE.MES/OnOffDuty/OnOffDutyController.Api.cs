using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.OnOffDuty.ApiModel;
using SIE.MES.PrepareProducts.ApiModels;
using SIE.MES.WIP;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.OnOffDuty
{
    /// <summary>
    /// 
    /// </summary>
    public partial class OnOffDutyController
    {

        /// <summary>
        /// 上下岗
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="onOffDutyType"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        [ApiService("设置上下岗信息")]
        public virtual OnOffDutyRecrodInfo SetOnOffDuty([ApiParameter("上下岗关键编码")] string keyCode,
            [ApiParameter("上下岗类型")] int onOffDutyType,
            [ApiParameter("工作单元")] Workcell workcell)
        {
            if (workcell == null)
            {
                throw new ValidationException("参数异常".L10N());

            }
            var onOffDutyRecrods = new OnOffDutyRecrods();
            onOffDutyRecrods.GenerateId();
            var staff = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(keyCode);
            if (staff == null)
            {
                staff = RT.Service.Resolve<EmployeeController>().GetEmployeeByName(keyCode);
            }
            if (staff == null)
            {
                throw new ValidationException("扫描员工工号或姓名不存在，请检查".L10N());
            }
            onOffDutyRecrods.EmployeeId = staff.Id;
            onOffDutyRecrods.OnOffDutyType =(OnOffDutyType)onOffDutyType;
            onOffDutyRecrods.ProcessId = workcell.ProcessId;
            onOffDutyRecrods.StationId= workcell.StationId;
            onOffDutyRecrods.ResourceId = workcell.ResourceId;

           OnOffDuty(onOffDutyRecrods, workcell, onOffDutyRecrods.OnOffDutyType == OnOffDutyType.OnDuty);
            var resultEntity = RF.GetById<OnOffDutyRecrods>(onOffDutyRecrods.Id, new EagerLoadOptions().LoadWithViewProperty());
            return new OnOffDutyRecrodInfo()
            {
                DutyType = resultEntity.OnOffDutyType.ToLabel().L10N(),
                OffDutyTime = resultEntity.OffDutyTime.HasValue ? resultEntity.OffDutyTime.Value.ToString("yyyy-MM-dd hh:mm:ss") : "",
                OnDutyTime = resultEntity.OnDutyTime.HasValue ? resultEntity.OnDutyTime.Value.ToString("yyyy-MM-dd hh:mm:ss") : "",
                StaffCode = resultEntity.EmployeeCode,
                StaffName = resultEntity.EmployeeName,
                Process = resultEntity.ProcessName,
                Resource = resultEntity.ResourceName,
                StaffGroup = resultEntity.EmployeeGroupName,
                Station = resultEntity.StationName
            };
        }



    }
}
