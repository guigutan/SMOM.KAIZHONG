using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.RedCardManagment.RedCardApplyBills.Service;
using SIE.RedCardManagment.WorkFlow.Variables;
using SIE.WorkFlow.Base.Common;
using System;

namespace SIE.RedCardManagment.WorkFlow.Activities.RedCardApplyBill
{
    /// <summary>
    /// PDCA获取节点单据
    /// </summary>
    public class PdcaReportActivityEntityProvider : IActivityEntityProvider<SIE.RedCardManagment.RedCardApplyBills.RedCardApplyBill>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Entity GetActivityEntity(GetActivityEntityInput input)
        {
            //1.获取流程变量
            var model = input.VariablesJson.ToJsonObjectCore<RedCardApplyVariable>();
            if (model == null)
            {
                //新增单据
                return new SIE.RedCardManagment.RedCardApplyBills.RedCardApplyBill()
                {
                    No = RT.Service.Resolve<RedCardApplyBillService>().GenerateNo()
                };
            }
            else
            {
                var bill = RF.GetById<SIE.RedCardManagment.RedCardApplyBills.RedCardApplyBill>(model.BillId, new EagerLoadOptions().LoadWithViewProperty());
                if(bill == null)
                    throw new ValidationException("无法查询到该单据。".L10N());
                //RT.Service.Resolve<EmployeeAuthController>().CheckEmployeeAuth(RT.IdentityId, bill.WarehouseId, bill.FactoryId); //权限验证
                return bill;
            }
        }
    }
}
