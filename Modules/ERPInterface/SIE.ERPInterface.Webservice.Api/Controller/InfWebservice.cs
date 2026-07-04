using SIE.ERPInterface.Api.WebApi;
using SIE.ERPInterface.Common.Datas;
using System.Collections.Generic;
using System.ServiceModel;

namespace SIE.ERPInterface.Webservice.Api.Controller
{
    /// <summary>
    /// 数据写中间表接口 控制器
    /// </summary>
    [ServiceContract]
    public class InfWebservice
    {
        #region 基础数据

        /// <summary>
        /// 保存物料数据
        /// </summary>
        /// <param name="itemDatas">物料数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns></returns>
        [OperationContract]
        public virtual ApiResult SaveItems(List<ItemData> itemDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveItems(itemDatas, invOrg);
        }

        /// <summary>
        /// 保存物料分类数据
        /// </summary>
        /// <param name="itemCategoryDatas">物料分类数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveItemCategorys(List<ItemCategoryData> itemCategoryDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveItemCategorys(itemCategoryDatas, invOrg);
        }

        /// <summary>
        /// 保存企业模型数据
        /// </summary>
        /// <param name="enterpriseDatas">企业模型数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveEnterprises(List<EnterpriseData> enterpriseDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveEnterprises(enterpriseDatas, invOrg);
        }

        /// <summary>
        /// 保存客户数据
        /// </summary>
        /// <param name="customerDatas">客户数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveCustomers(List<CustomerData> customerDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveCustomers(customerDatas, invOrg);
        }

        /// <summary>
        /// 保存客户地址数据
        /// </summary>
        /// <param name="customerAddressDatas">客户地址数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveCustomerAddresss(List<CustomerAddressData> customerAddressDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveCustomerAddresss(customerAddressDatas, invOrg);
        }

        /// <summary>
        /// 保存供应商数据
        /// </summary>
        /// <param name="supplierDatas">供应商数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveSuppliers(List<SupplierData> supplierDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveSuppliers(supplierDatas, invOrg);
        }

        /// <summary>
        /// 保存供应商地址数据
        /// </summary>
        /// <param name="supplierAddressDatas">供应商地址数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveSupplierAddresss(List<SupplierAddressData> supplierAddressDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveSupplierAddresss(supplierAddressDatas, invOrg);
        }

        /// <summary>
        /// 保存员工数据
        /// </summary>
        /// <param name="employeeDatas">员工数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveEmployees(List<EmployeeData> employeeDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveEmployees(employeeDatas, invOrg);
        }

        /// <summary>
        /// 保存产品BOM数据
        /// </summary>
        /// <param name="productBomDatas">产品BOM数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveProductBoms(List<ProductBomData> productBomDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveProductBoms(productBomDatas, invOrg);
        }

        /// <summary>
        /// 保存产品BOM明细数据
        /// </summary>
        /// <param name="productBomDetailDatas">产品BOM明细数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveProductBomDetails(List<ProductBomDetailData> productBomDetailDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveProductBomDetails(productBomDetailDatas, invOrg);
        }

        /// <summary>
        /// 保存仓库数据
        /// </summary>
        /// <param name="warehouseDatas">仓库数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveWarehouses(List<WarehouseData> warehouseDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveWarehouses(warehouseDatas, invOrg);
        }

        /// <summary>
        /// 保存库区数据
        /// </summary>
        /// <param name="storageAreaDatas">库区数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveStorageAreas(List<StorageAreaData> storageAreaDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveStorageAreas(storageAreaDatas, invOrg);
        }

        /// <summary>
        /// 保存库位数据
        /// </summary>
        /// <param name="storageLocDatas">库位数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveStorageLocations(List<StorageLocationData> storageLocDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveStorageLocations(storageLocDatas, invOrg);
        }

        #endregion

        #region 来源订单

        /// <summary>
        /// 保存工单数据
        /// </summary>
        /// <param name="workOrderDatas">工单数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveWorkOrders(List<WorkOrderData> workOrderDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveWorkOrders(workOrderDatas, invOrg);
        }

        /// <summary>
        /// 保存工单Bom数据
        /// </summary>
        /// <param name="workOrderBomDatas">工单Bom数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveWorkOrderBoms(List<WorkOrderBomData> workOrderBomDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveWorkOrderBoms(workOrderBomDatas, invOrg);
        }

        /// <summary>
        /// 保存采购订单数据
        /// </summary>
        /// <param name="poDatas">采购订单数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SavePurchaseOrders(List<PoData> poDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SavePurchaseOrders(poDatas, invOrg);
        }

        /// <summary>
        /// 保存采购订单明细数据
        /// </summary>
        /// <param name="poDtlDatas">采购订单明细数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SavePurchaseOrderDtls(List<PoDetailData> poDtlDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SavePurchaseOrderDtls(poDtlDatas, invOrg);
        }

        #endregion

        #region 业务单据

        /// <summary>
        /// 保存ASN数据
        /// </summary>
        /// <param name="asnDatas">ASN数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveAsns(List<AsnData> asnDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveAsns(asnDatas, invOrg);
        }

        /// <summary>
        /// 保存ASN明细数据
        /// </summary>
        /// <param name="asnDtlDatas">ASN明细数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveAsnDtls(List<AsnDetailData> asnDtlDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveAsnDtls(asnDtlDatas, invOrg);
        }

        /// <summary>
        /// 保存发运单数据
        /// </summary>
        /// <param name="orderDatas">发运单数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveShippingOrders(List<ShippingOrderData> orderDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveShippingOrders(orderDatas, invOrg);
        }

        /// <summary>
        /// 保存发运单明细数据
        /// </summary>
        /// <param name="orderDtlDatas">发运单明细数据</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveShippingOrderDtls(List<ShippingOrderDetailData> orderDtlDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveShippingOrderDtls(orderDtlDatas, invOrg);
        }

        /// <summary>
        /// 保存生产订单BOM数据
        /// </summary>
        /// <param name="poBomDatas">生产订单BOM</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveProductionOrderBoms(List<ProductionOrderBomData> poBomDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveProductionOrderBoms(poBomDatas, invOrg);
        }

        /// <summary>
        /// 保存生产订单数据
        /// </summary>
        /// <param name="poDatas">生产订单</param>
        /// <param name="invOrg">组织ID，大于0有效</param>
        /// <returns>处理结果</returns>
        [OperationContract]
        public virtual ApiResult SaveProductOrders(List<ProductOrderData> poDatas, int invOrg = 0)
        {
            var ctl = RT.Service.Resolve<InfWebApiController>();
            return ctl.SaveProductOrders(poDatas, invOrg);
        }

        #endregion
    }
}