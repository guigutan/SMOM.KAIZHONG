using SIE.Api;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.ErpInfoDatas;
using SIE.ERPInterface.Common.Datas.ErpInfoDatas.InspStandards;
using SIE.ERPInterface.Smom.Download;
using SIE.ERPInterface.Smom.Download.Defects;
using SIE.ERPInterface.Smom.Download.QmsStandards;
using SIE.Security;
using System.Collections.Generic;

namespace SIE.ERPInterface.Api.WebApi
{
    /// <summary>
    /// 数据下载API接口
    /// 不经过中间表，ERP下载数据到业务表
    /// </summary>
    public class SmomWebApiController : DomainController
    {
        #region 基础数据

        /// <summary>
        /// 保存物料数据
        /// </summary>
        /// <param name="unitDatas">单位数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存单位数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveUnits([ApiParameter("单位数据", SampleValueProvider = typeof(UnitDataValueProvider))] List<UnitData> unitDatas, int invOrg = 0)
        {
            //库存组织赋值
            if (invOrg > 0)
                RT.InvOrg = invOrg;            
            var ctl = RT.Service.Resolve<DownloadItemController>();
            var erpErrors = ctl.SaveUnits(unitDatas);
            ApiResult apiResult = new ApiResult();
            apiResult.ErpErrorDatas.AddRange(erpErrors);
            return apiResult;
        }

        /// <summary>
        /// 保存物料数据
        /// </summary>
        /// <param name="itemDatas">物料数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存物料数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveItems([ApiParameter("物料数据", SampleValueProvider = typeof(ItemDataValueProvider))] List<ItemData> itemDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadItemController>();
            return ctl.DownloadItemToBusiness(itemDatas, invOrg);
        }

        /// <summary>
        /// 保存物料分类数据
        /// </summary>
        /// <param name="itemCategoryDatas">物料分类数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存物料分类数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveItemCategorys([ApiParameter("物料分类数据", SampleValueProvider = typeof(ItemCategoryDataValueProvider))] List<ItemCategoryData> itemCategoryDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadItemCateController>();
            return ctl.DownloadItemCateToBusiness(itemCategoryDatas, invOrg);
        }

        /// <summary>
        /// 保存企业模型数据
        /// </summary>
        /// <param name="enterpriseDatas">企业模型数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存企业模型数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveEnterprises([ApiParameter("企业模型数据", SampleValueProvider = typeof(EnterpriseDataValueProvider))] List<EnterpriseData> enterpriseDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadEnterpriseController>();
            return ctl.DownloadEnterpriseToBusiness(enterpriseDatas, invOrg);
        }

        /// <summary>
        /// 保存客户数据
        /// </summary>
        /// <param name="customerDatas">客户数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存客户数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveCustomers([ApiParameter("客户数据", SampleValueProvider = typeof(CustomerDataValueProvider))] List<CustomerData> customerDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadCustomerController>();
            return ctl.DownloadCustomerToBusiness(customerDatas, invOrg);
        }

        /// <summary>
        /// 保存客户地址数据
        /// </summary>
        /// <param name="customerAddressDatas">客户地址数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存客户地址数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveCustomerAddresss([ApiParameter("客户地址数据", SampleValueProvider = typeof(CustomerAddressDataValueProvider))] List<CustomerAddressData> customerAddressDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadCustomerAddrController>();
            return ctl.DownloadCustomerAddrToBusiness(customerAddressDatas, invOrg);
        }

        /// <summary>
        /// 保存供应商数据
        /// </summary>
        /// <param name="supplierDatas">供应商数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存供应商数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveSuppliers([ApiParameter("供应商数据", SampleValueProvider = typeof(SupplierDataValueProvider))] List<SupplierData> supplierDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadSupplierController>();
            return ctl.DownloadSupplierToBusiness(supplierDatas, invOrg);
        }

        /// <summary>
        /// 保存供应商地址数据
        /// </summary>
        /// <param name="supplierAddressDatas">供应商地址数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存供应商地址数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveSupplierAddresss([ApiParameter("供应商地址数据", SampleValueProvider = typeof(SupplierAddressDataValueProvider))] List<SupplierAddressData> supplierAddressDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadSupplierAddrController>();
            return ctl.DownloadSupplierAddrToBusiness(supplierAddressDatas, invOrg);
        }

        /// <summary>
        /// 保存员工数据
        /// </summary>
        /// <param name="employeeDatas">员工数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存员工数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveEmployees([ApiParameter("员工数据", SampleValueProvider = typeof(EmployeeDataValueProvider))] List<EmployeeData> employeeDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadEmployeeController>();
            return ctl.DownloadEmployeeToBusiness(employeeDatas, invOrg);
        }

        /// <summary>
        /// 保存产品BOM数据
        /// </summary>
        /// <param name="productBomDatas">产品BOM数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存产品BOM数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveProductBoms([ApiParameter("产品BOM数据", SampleValueProvider = typeof(ProductBomDataValueProvider))] List<ProductBomData> productBomDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadProductBomController>();
            return ctl.DownloadProductBomToBusiness(productBomDatas, invOrg);
        }

        /// <summary>
        /// 保存产品BOM明细数据
        /// </summary>
        /// <param name="productBomDetailDatas">产品BOM明细数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存产品BOM明细数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveProductBomDetails([ApiParameter("产品BOM明细数据", SampleValueProvider = typeof(ProductBomDetailDataValueProvider))] List<ProductBomDetailData> productBomDetailDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadProductBomDtlController>();
            return ctl.DownloadProductBomDtlToBusiness(productBomDetailDatas, invOrg);
        }

        /// <summary>
        /// 保存仓库数据
        /// </summary>
        /// <param name="warehouseDatas">仓库数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存仓库数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveWarehouses([ApiParameter("仓库数据", SampleValueProvider = typeof(ErpWarehouseDataValueProvider))] List<WarehouseData> warehouseDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadWarehouseController>();
            return ctl.DownloadWarehouseToBusiness(warehouseDatas, invOrg);
        }

        /// <summary>
        /// 保存库区数据
        /// </summary>
        /// <param name="storageAreaDatas">库区数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存库区数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveStorageAreas([ApiParameter("库区数据", SampleValueProvider = typeof(ErpStorageAreaDataValueProvider))] List<StorageAreaData> storageAreaDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadStorageAreaController>();
            return ctl.DownloadStorageAreaToBusiness(storageAreaDatas, invOrg);
        }

        /// <summary>
        /// 保存库位数据
        /// </summary>
        /// <param name="storageLocDatas">库位数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存库位数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveStorageLocations([ApiParameter("库位数据", SampleValueProvider = typeof(ErpLocationDataValueProvider))] List<StorageLocationData> storageLocDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadStorageLocationController>();
            return ctl.DownloadStorageLocToBusiness(storageLocDatas, invOrg);
        }

        #region 缺陷代码
        /// <summary>
        /// 保存缺陷分类
        /// </summary>
        /// <param name="defectCategoryList">缺陷分类数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存缺陷分类")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveDefectCategory([ApiParameter("缺陷分类", SampleValueProvider = typeof(DefectCategoryDataValueProvider))] List<DefectCategoryData> defectCategoryList, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadDefectController>();
            return ctl.DownloadDefectCategoryToBusiness(defectCategoryList, invOrg);
        }

        /// <summary>
        /// 保存缺陷等级
        /// </summary>
        /// <param name="defectGradeList">缺陷等级数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存缺陷等级")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveDefectGrade([ApiParameter("缺陷等级", SampleValueProvider = typeof(DefectGradeDataValueProvider))] List<DefectGradeData> defectGradeList, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadDefectController>();
            return ctl.DownloadDefectGradeToBusiness(defectGradeList, invOrg);
        }

        /// <summary>
        /// 保存缺陷代码
        /// </summary>
        /// <param name="defectCodeList">缺陷代码数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存缺陷代码")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveDefectCode([ApiParameter("缺陷代码", SampleValueProvider = typeof(DefectCodeDataValueProvider))] List<DefectCodeData> defectCodeList, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadDefectController>();
            return ctl.DownloadDefectCodeToBusiness(defectCodeList, invOrg);
        }
        #endregion

        #region 检验标准
        /// <summary>
        /// 保存物料检验标准
        /// </summary>
        /// <param name="itemInspStandardList">物料检验标准数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存物料检验标准")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveInspectionItemStandards([ApiParameter("物料检验标准", SampleValueProvider = typeof(ItemInspStandardDataValueProvider))] List<ItemInspStandardData> itemInspStandardList, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadQmsStandardController>();
            return ctl.DownloadItemInspStandardsToBusiness(itemInspStandardList, invOrg);
        }

        /// <summary>
        /// 保存分类检验标准
        /// </summary>
        /// <param name="categoryInspStandardList">分类检验标准数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存分类检验标准")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveInspectionCategoryStandards([ApiParameter("分类检验标准", SampleValueProvider = typeof(CategoryInspStandardDataValueProvider))] List<CategoryInspStandardData> categoryInspStandardList, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadQmsStandardController>();
            return ctl.DownloadCategoryInspStandardsToBusiness(categoryInspStandardList, invOrg);
        }
        #endregion
        #endregion

        #region 来源订单

        /// <summary>
        /// 保存工单数据
        /// </summary>
        /// <param name="workOrderDatas">工单数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存工单数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveWorkOrders([ApiParameter("工单数据", SampleValueProvider = typeof(ErpWorkOrderDataValueProvider))] List<WorkOrderData> workOrderDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadWorkOrderController>();
            return ctl.DownloadWorkOrderToBusiness(workOrderDatas, invOrg);
        }

        /// <summary>
        /// 保存工单Bom数据
        /// </summary>
        /// <param name="workOrderBomDatas">工单Bom数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存工单Bom数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveWorkOrderBoms([ApiParameter("工单Bom数据", SampleValueProvider = typeof(ErpWorkOrderBomDataValueProvider))] List<WorkOrderBomData> workOrderBomDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadWorkOrderBomController>();
            return ctl.DownloadWorkOrderBomToBusiness(workOrderBomDatas, invOrg);
        }

        /// <summary>
        /// 保存采购订单数据
        /// </summary>
        /// <param name="poDatas">采购订单数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存采购订单数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SavePurchaseOrders([ApiParameter("采购订单数据", SampleValueProvider = typeof(PoDataValueProvider))] List<PoData> poDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadPoController>();
            return ctl.DownloadPoToBusiness(poDatas, invOrg);
        }

        /// <summary>
        /// 保存采购订单明细数据
        /// </summary>
        /// <param name="poDtlDatas">采购订单明细数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存采购订单明细数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SavePurchaseOrderDtls([ApiParameter("采购订单明细数据", SampleValueProvider = typeof(PoDetailDataValueProvider))] List<PoDetailData> poDtlDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadPoDtlController>();
            return ctl.DownloadPoDtlToBusiness(poDtlDatas, invOrg);
        }

        #endregion

        #region 业务单据

        /// <summary>
        /// 保存ASN数据
        /// </summary>
        /// <param name="asnDatas">ASN数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存ASN数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveAsns([ApiParameter("ASN数据", SampleValueProvider = typeof(AsnDataValueProvider))] List<AsnData> asnDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadAsnController>();
            return ctl.DownloadAsnToBusiness(asnDatas, invOrg);
        }

        /// <summary>
        /// 保存ASN明细数据
        /// </summary>
        /// <param name="asnDtlDatas">ASN明细数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存ASN明细数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveAsnDtls([ApiParameter("ASN明细数据", SampleValueProvider = typeof(AsnDetailDataValueProvider))] List<AsnDetailData> asnDtlDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadAsnDtlController>();
            return ctl.DownloadAsnDtlToBusiness(asnDtlDatas, invOrg);
        }

        /// <summary>
        /// 保存发运单数据
        /// </summary>
        /// <param name="orderDatas">发运单数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存发运单数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveShippingOrders([ApiParameter("发运单数据", SampleValueProvider = typeof(ShippingOrderDataValueProvider))] List<ShippingOrderData> orderDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadShipmentController>();
            return ctl.DownloadOrderToBusiness(orderDatas, invOrg);
        }

        /// <summary>
        /// 保存发运单明细数据
        /// </summary>
        /// <param name="orderDtlDatas">发运单明细数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存发运单明细数据")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveShippingOrderDtls([ApiParameter("发运单明细数据", SampleValueProvider = typeof(ShippingOrderDetailDataValueProvider))] List<ShippingOrderDetailData> orderDtlDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadShipmentDtlController>();
            return ctl.DownloadOrderDtlToBusiness(orderDtlDatas, invOrg);
        }

        /// <summary>
        /// 保存生产订单BOM数据
        /// </summary>
        /// <param name="poBomDatas">生产订单BOM</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存生产订单BOM")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveProductionOrderBoms([ApiParameter("生产订单BOM数据", SampleValueProvider = typeof(ProductOrderBomDataDataValueProvider))] List<ProductionOrderBomData> poBomDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadPoBomController>();
            return ctl.DownloadPoBomToBusiness(poBomDatas, invOrg);
        }

        /// <summary>
        /// 保存生产订单数据
        /// </summary>
        /// <param name="poDatas">生产订单</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("保存生产订单")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveProductOrders([ApiParameter("生产订单数据", SampleValueProvider = typeof(ProductOrderDataDataValueProvider))] List<ProductOrderData> poDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadProductOrderController>();
            return ctl.DownloadProOrderToBusiness(poDatas, invOrg);
        }

        /// <summary>
        /// 保存库存调拨数据
        /// </summary>
        /// <param name="allocateDatas">库存调拨</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("创建库存调拨")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveInventoryAllocateDatas([ApiParameter("库存调拨数据", SampleValueProvider = typeof(InventoryAllocateDataValueProvider))] List<InventoryAllocateData> allocateDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadInventoryAllocateController>();
            return ctl.DownloadAllocateToBusiness(allocateDatas, invOrg);
        }

        /// <summary>
        /// 保存发货计划数据
        /// </summary>
        /// <param name="planDatas">发货计划</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [AllowAnonymous]
        [ApiService("创建发货计划")]
        [return: ApiReturn("处理结果 ApiResult", SampleValueProvider = typeof(ApiResultValueProvider))]
        public virtual ApiResult SaveDeliveryPlanDatas([ApiParameter("发货计划数据", SampleValueProvider = typeof(DeliveryPlanDataValueProvider))] List<DeliveryPlanData> planDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<DownloadDeliveryPlanController>();
            return ctl.DownloadDeliveryPlanToBusiness(planDatas, invOrg);
        }

        #endregion
    }
}
