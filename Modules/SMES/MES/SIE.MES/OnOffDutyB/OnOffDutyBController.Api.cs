using SIE.Api;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.OnOffDutyB.ApiModel;
using SIE.MES.WIP;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.OnOffDutyB
{
    public partial class OnOffDutyBController
    {

        /// <summary>
        /// B设置上下岗信息
        /// </summary>
        /// <param name="keyCode"></param>
        /// <param name="onOffDutyType"></param>
        /// <param name="workcell"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        [ApiService("B设置上下岗信息")]
        public virtual OnOffDutyBRecrodInfo SetOnOffDuty([ApiParameter("上下岗关键编码")] string keyCode, [ApiParameter("上下岗类型")] int onOffDutyType, [ApiParameter("工作单元")] Workcell workcell)
        {
            if (workcell == null) { throw new ValidationException("参数异常".L10N()); }
            var onOffDutyBRecrods = new OnOffDutyBRecrods();
            onOffDutyBRecrods.GenerateId();
            var staff = RT.Service.Resolve<EmployeeController>().GetEmployeeByCode(keyCode);
            if (staff == null)
            {
                staff = RT.Service.Resolve<EmployeeController>().GetEmployeeByName(keyCode);
            }
            if (staff == null)
            {
                throw new ValidationException("扫描员工工号或姓名不存在，请检查".L10N());
            }
            onOffDutyBRecrods.EmployeeId = staff.Id;
            onOffDutyBRecrods.OnOffDutyType = (OnOffDutyBType)onOffDutyType;          
            onOffDutyBRecrods.ResourceId = workcell.ResourceId;

            OnOffDuty(onOffDutyBRecrods, onOffDutyBRecrods.OnOffDutyType == OnOffDutyBType.OnDuty);
            var resultEntity = RF.GetById<OnOffDutyBRecrods>(onOffDutyBRecrods.Id, new EagerLoadOptions().LoadWithViewProperty());
            return new OnOffDutyBRecrodInfo()
            {
                DutyType = resultEntity.OnOffDutyType.ToLabel().L10N(),
                OffDutyTime = resultEntity.OffDutyTime.HasValue ? resultEntity.OffDutyTime.Value.ToString("yyyy-MM-dd hh:mm:ss") : "",
                OnDutyTime = resultEntity.OnDutyTime.HasValue ? resultEntity.OnDutyTime.Value.ToString("yyyy-MM-dd hh:mm:ss") : "",
                StaffCode = resultEntity.EmployeeCode,
                StaffName = resultEntity.EmployeeName,              
                Resource = resultEntity.ResourceName,
                StaffGroup = resultEntity.EmployeeGroupName,
               
            };
        }



    }
}
