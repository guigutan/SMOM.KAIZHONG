using SIE.Api;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Smom.Download;
using SIE.Security;
using System.Collections.Generic;

namespace SIE.ERPInterface.Api.WebApi
{
    /// <summary>
    /// 数据下载API接口
    /// ERP下载数据到中间表，需另外通过SMOM的调度，把中间表数据执行到业务表
    /// </summary>
    public partial class InfWebApiController : DomainController
    {
        #region 基础数据

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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<ItemData>(
                itemDatas.OrderByLastUpdateDate(),
                p => this.SaveItem(p),
                JobType.Item,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<ItemCategoryData>(
                itemCategoryDatas.OrderByLastUpdateDate(),
                p => this.SaveItemCategory(p),
                JobType.ItemCategory,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<EnterpriseData>(
                enterpriseDatas.OrderByLastUpdateDate(),
                p => this.SaveEnterprise(p),
                JobType.Enterprise,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<CustomerData>(
                customerDatas.OrderByLastUpdateDate(),
                p => this.SaveCustomer(p),
                JobType.Customer,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<CustomerAddressData>(
                customerAddressDatas.OrderByLastUpdateDate(),
                p => this.SaveCustomerAddress(p),
                JobType.CustomerAddress,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<SupplierData>(
                supplierDatas.OrderByLastUpdateDate(),
                p => this.SaveSupplier(p),
                JobType.Supplier,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<SupplierAddressData>(
                supplierAddressDatas.OrderByLastUpdateDate(),
                p => this.SaveSupplierAddress(p),
                JobType.Supplier,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<EmployeeData>(
                employeeDatas.OrderByLastUpdateDate(),
                p => this.SaveEmployee(p),
                JobType.Employee,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<ProductBomData>(
                productBomDatas.OrderByLastUpdateDate(),
                p => this.SaveProductBom(p),
                JobType.ProductBom,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<ProductBomDetailData>(
                productBomDetailDatas.OrderByLastUpdateDate(),
                p => this.SaveProductBomDetail(p),
                JobType.ProductBomDtl,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<WarehouseData>(
                warehouseDatas.OrderByLastUpdateDate(),
                p => this.SaveWarehouse(p),
                JobType.Warehouse,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<StorageAreaData>(
                storageAreaDatas.OrderByLastUpdateDate(),
                p => this.SaveStorageArea(p),
                JobType.StorageArea,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<StorageLocationData>(
                storageLocDatas.OrderByLastUpdateDate(),
                p => this.SaveStorageLocation(p),
                JobType.StorageLocation,
                invOrg);
        }

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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<WorkOrderData>(
                workOrderDatas.OrderByLastUpdateDate(),
                p => this.SaveWorkOrder(p),
                JobType.WorkOrder,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<WorkOrderBomData>(
                workOrderBomDatas.OrderByLastUpdateDate(),
                p => this.SaveWorkOrderBom(p),
                JobType.WorkOrderBom,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<PoData>(
                poDatas.OrderByLastUpdateDate(),
                p => this.SavePurchaseOrder(p),
                JobType.PurchaseOrder,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<PoDetailData>(
                poDtlDatas.OrderByLastUpdateDate(),
                p => this.SavePurchaseOrderDtl(p),
                JobType.PurchaseOrderDetail,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<AsnData>(
                asnDatas.OrderByLastUpdateDate(),
                p => this.SaveAsn(p),
                JobType.Asn,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<AsnDetailData>(
                asnDtlDatas.OrderByLastUpdateDate(),
                p => this.SaveAsnDtl(p),
                JobType.AsnDtl,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<ShippingOrderData>(
                orderDatas.OrderByLastUpdateDate(),
                p => this.SaveShippingOrder(p),
                JobType.ShippingOrder,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<ShippingOrderDetailData>(
                orderDtlDatas.OrderByLastUpdateDate(),
                p => this.SaveShippingOrderDtl(p),
                JobType.ShippingOrderDtl,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<ProductionOrderBomData>(
                poBomDatas.OrderByLastUpdateDate(),
                p => this.SaveProductionOrderBom(p),
                JobType.ProductOrderBom,
                invOrg);
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
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            return ctl.ApiSaveInfData<ProductOrderData>(
                poDatas.OrderByLastUpdateDate(),
                p => this.SaveProductOrder(p),
                JobType.ProductOrder,
                invOrg);
        }
        #endregion
    }
}
