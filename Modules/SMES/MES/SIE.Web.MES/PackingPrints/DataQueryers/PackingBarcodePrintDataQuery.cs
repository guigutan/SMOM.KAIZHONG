using SIE.Common.NumberRules;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.PackingPrints;
using SIE.MES.PackingPrints.ViewModels;
using SIE.MES.WorkOrders;
using SIE.Web.Data;
using SIE.Web.MES.PackingPrints.Commands;
using System;
using System.Text;

namespace SIE.Web.MES.PackingPrints.DataQueryers
{
    /// <summary>
    /// 包装号打印查询器
    /// </summary>
    public class PackingBarcodePrintDataQuery : DataQueryer
    {
        /// <summary>
        /// 获取包装号打印数据
        /// </summary>
        /// <param name="vm">包装号打印ViewModel</param>
        /// <param name="flag">是否重算打印数</param>
        /// <returns>包装号打印数据</returns>
        public PackingPrintData GetPrintData(PackingBarcodePrintViewModel vm, bool flag)
        {
            var printData = new PackingPrintData();
            var workOrder = RF.GetById<PackingWorkOrder>(vm.WorkOrderId);
            if (vm.PackageRuleDetailId.IsNotEmpty())
            {
                var packageRuleDetail = RT.Service.Resolve<PackingBarcodeController>().GetWorkOrderPackageRuleDetail(Convert.ToDouble(vm.PackageRuleDetailId));
                if (packageRuleDetail.NumberRuleId == null)
                    throw new ValidationException("编码规则未设置，请检查".L10N());
                var totalQty = (int)Math.Ceiling(Math.Round(workOrder.PlanQty / packageRuleDetail.Qty, 0));
                printData.ProductQty = (int)packageRuleDetail.Qty;
                printData.PrintedQty = RT.Service.Resolve<PackingBarcodeController>().GetPackingBarcodeCount(vm.WorkOrderId, Convert.ToDouble(vm.PackageRuleDetailId));
                printData.ResidualQty = totalQty - printData.PrintedQty;
                if (flag)
                {
                    if (printData.ResidualQty < 1)
                        printData.PrintQty = 1;
                    else printData.PrintQty = printData.ResidualQty;
                }
                else printData.PrintQty = vm.PrintQty;

                if (packageRuleDetail.PrintTemplate != null)
                    printData.TemplateEntityType = packageRuleDetail.PrintTemplate.EntityType;
                var numberRule = RF.GetById<NumberRule>(packageRuleDetail.NumberRuleId.Value);
                printData.NumberRuleId = packageRuleDetail.NumberRuleId;
                printData.NumberRuleName = numberRule.Name;
                StringBuilder sb = new StringBuilder();
                foreach (var ruleDtl in numberRule.DetailList)
                {
                    sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
                }
                printData.BarcodeRuleDtl += sb;
                if (printData.PrintQty > 0)
                {
                    GetPrintData(printData.PrintQty, vm.PrintControl, packageRuleDetail.NumberRuleId.Value, packageRuleDetail, printData);
                }
            }
            return printData;
        }

        /// <summary>
        /// 获取拼板码打印数据
        /// </summary>
        /// <param name="printQty">可打印数量</param>
        /// <param name="printControl">是否反打</param>
        /// <param name="numberRuleId">条码规则Id</param>
        /// <param name="packageRuleDetail">工单包装规则</param>
        /// <param name="printData">打印数据</param>
        private void GetPrintData(int printQty, bool printControl, double numberRuleId, WorkOrderPackageRuleDetail packageRuleDetail, PackingPrintData printData)
        {
            var ruleCt = RT.Service.Resolve<NumberRuleController>();
            if (!printControl) //默认为正打
            {
                printData.BeginSn = ruleCt.GetStartSegment(numberRuleId, packageRuleDetail);
                printData.EndSn = ruleCt.GetEndSegment(numberRuleId, printQty, packageRuleDetail);
            }
            else
            {
                printData.EndSn = ruleCt.GetStartSegment(numberRuleId, packageRuleDetail);
                printData.BeginSn = ruleCt.GetEndSegment(numberRuleId, printQty, packageRuleDetail);
            }
        }
    }
}
