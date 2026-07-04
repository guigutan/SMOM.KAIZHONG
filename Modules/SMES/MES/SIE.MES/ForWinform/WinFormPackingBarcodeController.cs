using SIE.Api;
using SIE.Barcodes;
using SIE.Common.NumberRules;
using SIE.Common.Prints;
using SIE.Common.Sort;
using SIE.Domain;
using SIE.MES.ForWinform.ApiModels;
using SIE.MES.PackingPrints;
using SIE.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.MES.ForWinform
{
    /// <summary>
    /// 
    /// </summary>
    public class WinFormPackingBarcodeController : PackingBarcodeController
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="PageNum"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        [ApiService("包装采集-验证包装号")]
        public virtual Tuple<int, List<PackingWorkOrder>> GetWorkOrders([ApiParameter("搜索条件")] string keyword, [ApiParameter("页号")] int PageNum, [ApiParameter("每页条数")] int PageSize)
        {

            PageNum = PageNum <= 0 ? 1 : PageNum;
            PageSize = PageSize <= 0 ? int.MaxValue : PageSize;
            PagingInfo pagingInfo = new PagingInfo(PageNum, PageSize);
            pagingInfo.IsNeedCount = true;
            var result = Query<PackingWorkOrder>().WhereIf(keyword.IsNotEmpty(), p => p.No.Contains(keyword) || p.Product.Code == keyword || p.Product.Name == keyword).ToList(pagingInfo, new EagerLoadOptions().LoadWithViewProperty());
            return new Tuple<int, List<PackingWorkOrder>>(result.TotalCount, result.ToList());
        }

        /// <summary>
        /// 获取工单的包装规则明细
        /// </summary>
        /// <param name="woId"></param>
        /// <returns></returns>
        [ApiService("获取工单的包装规则明细")]
        public virtual List<XPWorkOrderPackageRuleDetail> GetWorkOrderPackageRuleDetails([ApiParameter("搜索工单Id")] double woId)
        {
            var workOrderRule = Query<WorkOrderPackageRuleDetail>().Where(p => p.WorkOrderId == woId && p.IsPrint).ToList(null, new EagerLoadOptions().LoadWithViewProperty()).OrderBy(p => SortExtension.GetIndex(p)).ToArray();

            List<XPWorkOrderPackageRuleDetail> packageRules = new List<XPWorkOrderPackageRuleDetail>();
            foreach (var item in workOrderRule)
            {
                packageRules.Add(new XPWorkOrderPackageRuleDetail(item));
            }
            return packageRules;
        }
        /// <summary>
        /// 获取工单记录打印模板
        /// </summary>
        /// <returns></returns>
        [ApiService("获取包装号打印模板")]
        [return: ApiReturn("获取包装号打印模板 不分页")]
        public virtual List<PrintTemplate> GetPrintTemplates()
        {
            var labelPrintName = typeof(PackingPrintable).GetQualifiedName();
            return RT.Service.Resolve<BarcodeController>().GetPrintTemplatesByType(labelPrintName, null, "").ToList();
        }

        /// <summary>
        /// 获取包装号打印规则数量信息
        /// </summary>
        /// <param name="packageRuleDetailId"></param>
        /// <param name="workOrderId"></param>
        /// <returns></returns>
        [ApiService("获取包装号打印规则数量信息")]
        [return: ApiReturn("获取包装号打印规则数量信息 ")]

        public virtual PackageRuleDetailInfo GetPackageRuleDetailInfo([ApiParameter("包装规则明细Id")] double? packageRuleDetailId, [ApiParameter("工单Id")] double workOrderId)
        {
            var wo = RF.GetById<WorkOrder>(workOrderId);
            var packageRuleDetail = RT.Service.Resolve<PackingBarcodeController>().GetWorkOrderPackageRuleDetail(Convert.ToDouble(packageRuleDetailId));
            if (packageRuleDetail.NumberRuleId == null)
                return null;
            PackageRuleDetailInfo packageRuleDetailInfo = new PackageRuleDetailInfo();
            var totalQty = (int)Math.Ceiling(Math.Round(wo.PlanQty / packageRuleDetail.Qty, 0));
            packageRuleDetailInfo.ProductQty = (int)packageRuleDetail.Qty;
            packageRuleDetailInfo.PrintedQty = RT.Service.Resolve<PackingBarcodeController>().GetPackingBarcodeCount(workOrderId, Convert.ToDouble(packageRuleDetailId));
            packageRuleDetailInfo.ResidualQty = totalQty - packageRuleDetailInfo.PrintedQty;
            if (packageRuleDetailInfo.ResidualQty < 1)
                packageRuleDetailInfo.PrintQty = 1;
            else
            {
                packageRuleDetailInfo.PrintQty = packageRuleDetailInfo.ResidualQty;

            }
            var NumberRule = RF.GetById<NumberRule>(packageRuleDetail.NumberRuleId.Value);
            StringBuilder sb = new StringBuilder();
            foreach (var ruleDtl in NumberRule.DetailList)
            {
                sb.Append("[" + ruleDtl.Segment.Name + "]" + " ");
            }
            packageRuleDetailInfo.BarcodeRuleDtl = sb.ToString();

            packageRuleDetailInfo.BeginSn = RT.Service.Resolve<NumberRuleController>().GetStartSegment(packageRuleDetail.NumberRuleId.Value, wo);
            packageRuleDetailInfo.EndSn = RT.Service.Resolve<NumberRuleController>().GetEndSegment(packageRuleDetail.NumberRuleId.Value, packageRuleDetailInfo.PrintQty, wo);
            return packageRuleDetailInfo;
        }

        /// <summary>
        /// 包装号打印
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        [ApiService("包装号打印")]
        [return: ApiReturn("包装号打印 ")]
        public virtual List<PackingBarcode> PackageNumberPrint([ApiParameter("打印信息")] PackingPrinterInfo info)
        {
            return RT.Service.Resolve<PackingBarcodeController>().Print(info).ToList();
        }

        /// <summary>
        /// 获取指定工单包装号打印的记录
        /// </summary>
        /// <param name="packingWorkOrderId"></param>
        /// <returns></returns>
        [ApiService("获取指定工单包装号打印的记录")]
        [return: ApiReturn("获取指定工单包装号打印的记录")]
        public virtual List<PackingBarcode> GetPackingBarcodeListByWorkOrder([ApiParameter("工单号")] double packingWorkOrderId)
        {
          return  RT.Service.Resolve<PackingBarcodeController>().GetPackingBarcodeListByWorkOrderId(packingWorkOrderId, null, new List<OrderInfo> { new OrderInfo() {
             Property=nameof(PackingBarcode.PrintDateProperty),
               SortIndex=0,
                SortOrder= System.ComponentModel.ListSortDirection.Descending
            } }).ToList();
        }

    }

    /// <summary>
    /// 包装规则明细信息
    /// </summary>
    public class PackageRuleDetailInfo
    {
        /// <summary>
        /// 产品数
        /// </summary>
        public decimal ProductQty { get; set; }

        /// <summary>
        /// 已打印数
        /// </summary>
        public int PrintedQty { get; set; }

        /// <summary>
        /// 打印数数量
        /// </summary>
        public int PrintQty { get; set; }

        /// <summary>
        /// 剩余数量
        /// </summary>
        public int ResidualQty { get; set; }

        /// <summary>
        /// 条码规则明细
        /// </summary>
        public string BarcodeRuleDtl { get; set; }

        /// <summary>
        /// 开始条码
        /// </summary>
        public string BeginSn { get; set; }

        /// <summary>
        /// 结束条码
        /// </summary>
        public string EndSn { get; set; }
    }
}
