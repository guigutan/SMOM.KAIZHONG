using SIE.Api;
using SIE.ERPInterface.Api.WebApi;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.SmomOrder;
using SIE.ERPInterface.Ebs.Download.OnHands;
using SIE.ERPInterface.Smom.Download;
using SIE.ERPInterface.Smom.Download.PurchaseOrders;
using SIE.ERPInterface.Smom.Download.Receipts;
using SIE.ERPInterface.Smom.Download.WorkOrders;
using SIE.Security;
using System.Collections.Generic;

namespace SIE.ERPInterface.Api.EbsWebApi
{
    /// <summary>
    /// Erp返回数据
    /// </summary>
    public class EbsToSmomWebApiController : DomainController
    {
        /// <summary>
        /// 保存发货计划数据-销售发货
        /// </summary>
        /// <param name="orderDatas">销售发货</param>
        /// <param name="invOrgId">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>       
        [ApiService("保存发货计划数据-销售发货")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveShippingOrders([ApiParameter("销售发货数据", SampleValueProvider = typeof(EbsToSmomShippingOrderValueProvider))] List<EbsDeliveryPlanData> orderDatas, [ApiParameter("ERP库存组织Id")] string invOrgId)
        {
            var ctl = RT.Service.Resolve<EbsDownloadDeliveryPlanController>();
            return ctl.DownloadDeliveryPlanToBusiness(orderDatas, invOrgId);
        }

        /// <summary>
        /// 保存采购订单数据
        /// </summary>
        /// <param name="orderDatas">采购订单数据</param>
        /// <param name="invOrgId">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [ApiService("保存采购订单数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SavePurOrders([ApiParameter("采购订单数据", SampleValueProvider = typeof(EbsToSmomPurOrderValueProvider))] List<EbsPurOrderData> orderDatas, [ApiParameter("ERP库存组织Id")] string invOrgId)
        {
            var ctl = RT.Service.Resolve<EbsDownloadPoController>();
            return ctl.DownloadPurOrderToBusiness(orderDatas, invOrgId);
        }


        /// <summary>
        /// 保存发货计划数据-销售发货
        /// </summary>
        /// <param name="orderDatas">销售发货</param>
        /// <param name="invOrgId">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>       
        [ApiService("保存发货计划数据-工单发料")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveWorkFeed([ApiParameter("工单发料数据", SampleValueProvider = typeof(EbsToSmomShippingOrderValueProvider))] List<EbsDeliveryPlanData> orderDatas, [ApiParameter("ERP库存组织Id")] string invOrgId)
        {
            var ctl = RT.Service.Resolve<EbsDownloadDeliveryPlanController>();
            return ctl.DownloadSaveWrokFeedToBusiness(orderDatas, invOrgId);
        }

        /// <summary>
        /// 保存ASN数据-销售退货
        /// </summary>
        /// <param name="orderDatas">销售退货</param>
        /// <param name="invOrgId">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>       
        [ApiService("保存ASN数据-销售退货")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveSaleReturn([ApiParameter("销售退货数据", SampleValueProvider = typeof(EbsToSmomAsnValueProvider))] List<EbsToSmomAsnDetailData> orderDatas, [ApiParameter("ERP库存组织Id")] string invOrgId)
        {
            var ctl = RT.Service.Resolve<EbsDownloadAsnController>();
            return ctl.DownloadSaleReturnToBusiness(orderDatas, invOrgId);
        }

        /// <summary>
        /// 获取库存现有量
        /// </summary>
        [ApiService("获取库存现有量")]
        [return: ApiReturn("获取库存现有量")]
        public virtual void TextGetErpOnHandData()
        {
            //RT.Service.Resolve<EbsOnHandController>().Download();
        }

        /// <summary>
        /// 保存工单数据
        /// </summary>
        /// <param name="workOrderDatas">工单数据</param>
        /// <param name="invOrgId">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [ApiService("保存工单数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveWorkOrders([ApiParameter("工单数据", SampleValueProvider = typeof(ErpWorkOrderDataValueProvider))] List<EbsWorkOrderData> workOrderDatas, [ApiParameter("ERP库存组织Id")] string invOrgId)
        {
            var ctl = RT.Service.Resolve<EbsDownloadWorkOrderController>();
            return ctl.DownloadWorkOrderToBusiness(workOrderDatas, invOrgId);
        }
    }
}
