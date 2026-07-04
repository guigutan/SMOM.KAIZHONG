using SIE.Api;
using SIE.Defects;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.ErpInfoDatas;
using SIE.ERPInterface.Common.Datas.ErpInfoDatas.InspStandards;
using SIE.ERPInterface.Common.Datas.SmomOrder;
using SIE.ERPInterface.Smom.Download.PurchaseOrders;
using System.Collections.Generic;

namespace SIE.ERPInterface.Api.WebApi
{
    #region 基础数据

    /// <summary>
    /// 单位API参数显示提
    /// </summary>
    class UnitDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<UnitData> { new UnitData() };
        }
    }

    /// <summary>
    /// 物料API参数显示提
    /// </summary>
    class ItemDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<ItemData> { new ItemData() };
        }
    }

    /// <summary>
    /// 物料分类API参数显示提
    /// </summary>
    class ItemCategoryDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<ItemCategoryData> { new ItemCategoryData() };
        }
    }

    /// <summary>
    /// 企业模型API参数显示提
    /// </summary>
    class EnterpriseDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<EnterpriseData> { new EnterpriseData() };
        }
    }

    /// <summary>
    /// 客户API参数显示提
    /// </summary>
    class CustomerDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<CustomerData> { new CustomerData() };
        }
    }

    /// <summary>
    /// 客户地址API参数显示提
    /// </summary>
    class CustomerAddressDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<CustomerAddressData> { new CustomerAddressData() };
        }
    }

    /// <summary>
    /// 供应商API参数显示提
    /// </summary>
    class SupplierDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<SupplierData> { new SupplierData() };
        }
    }

    /// <summary>
    /// 供应商地址API参数显示提
    /// </summary>
    class SupplierAddressDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<SupplierAddressData> { new SupplierAddressData() };
        }
    }

    /// <summary>
    /// 员工API参数显示提
    /// </summary>
    class EmployeeDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<EmployeeData> { new EmployeeData() };
        }
    }

    /// <summary>
    /// 产品BOM API参数显示提
    /// </summary>
    class ProductBomDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<ProductBomData> { new ProductBomData() };
        }
    }

    /// <summary>
    /// 产品BOM明细API参数显示提
    /// </summary>
    class ProductBomDetailDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<ProductBomDetailData> { new ProductBomDetailData() };
        }
    }

    /// <summary>
    /// 仓库API参数显示提
    /// </summary>
    class ErpWarehouseDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<WarehouseData> { new WarehouseData() };
        }
    }

    /// <summary>
    /// 库区API参数显示提
    /// </summary>
    class ErpStorageAreaDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<StorageAreaData> { new StorageAreaData() };
        }
    }

    /// <summary>
    /// 库位API参数显示提
    /// </summary>
    class ErpLocationDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<StorageLocationData> { new StorageLocationData() };
        }
    }

    #region 缺陷
    /// <summary>
    /// 缺陷分类API参数显示提示
    /// </summary>
    class DefectCategoryDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<DefectCategoryData> { new DefectCategoryData() };
        }
    }

    /// <summary>
    /// 缺陷等级API参数显示提示
    /// </summary>
    class DefectGradeDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<DefectGradeData> { new DefectGradeData() };
        }
    }

    /// <summary>
    /// 缺陷代码API参数显示提示
    /// </summary>
    class DefectCodeDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<DefectCodeData> { new DefectCodeData() };
        }
    }
    #endregion

    #region 检验标准
    /// <summary>
    /// 物料检验标准API参数显示提示
    /// </summary>
    class ItemInspStandardDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<ItemInspStandardData> { new ItemInspStandardData()
            {
                DetailList = new List<InspStandardDataDetailBase>(){
                    new ItemInspStandardDataDetail()  }
                }
            };
        }
    }

    /// <summary>
    /// 分类检验标准API参数显示提示
    /// </summary>
    class CategoryInspStandardDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<CategoryInspStandardData> { new CategoryInspStandardData()
            {
                DetailList = new List<InspStandardDataDetailBase>(){
                    new CategoryInspStandardDataDetail()  }
                }
            };
        }
    }
    #endregion

    #endregion

    #region 来源订单

    /// <summary>
    /// 工单API参数显示提
    /// </summary>
    class ErpWorkOrderDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<WorkOrderData> { new WorkOrderData() };
        }
    }

    /// <summary>
    /// 工单BOM API参数显示提
    /// </summary>
    class ErpWorkOrderBomDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<WorkOrderBomData> { new WorkOrderBomData() };
        }
    }

    /// <summary>
    /// 采购订单API参数显示提
    /// </summary>
    class PoDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<PoData> { new PoData() };
        }
    }

    /// <summary>
    /// 采购订单明细API参数显示提
    /// </summary>
    class PoDetailDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<PoDetailData> { new PoDetailData() };
        }
    }

    #endregion

    #region 业务单据

    /// <summary>
    /// ASN单API参数显示提
    /// </summary>
    internal class AsnDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<AsnData> { new AsnData() };
        }
    }

    /// <summary>
    /// ASN单明细API参数显示提
    /// </summary>
    class AsnDetailDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<AsnDetailData> { new AsnDetailData() };
        }
    }

    /// <summary>
    /// 发运单API参数显示提
    /// </summary>
    class ShippingOrderDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<ShippingOrderData> { new ShippingOrderData { DetailList = new List<ShippingOrderDetailData> { new ShippingOrderDetailData() } } };
        }
    }

    /// <summary>
    /// 发运单明细API参数显示提
    /// </summary>
    class ShippingOrderDetailDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<ShippingOrderDetailData> { new ShippingOrderDetailData() };
        }
    }

    /// <summary>
    /// 生产订单BOM API参数显示提
    /// </summary>
    class ProductOrderBomDataDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<ProductionOrderBomData> { new ProductionOrderBomData() };
        }
    }

    /// <summary>
    /// 生产订单 API参数显示提
    /// </summary>
    class ProductOrderDataDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<ProductOrderData> { new ProductOrderData() };
        }
    }

    /// <summary>
    /// 库存调拨 API参数显示提
    /// </summary>
    class InventoryAllocateDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<InventoryAllocateData> { new InventoryAllocateData() { RequireList = new List<InventoryAllocateRequireData>() { new InventoryAllocateRequireData() } } };
        }
    }

    /// <summary>
    /// 发货计划 API参数显示提
    /// </summary>
    class DeliveryPlanDataValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<DeliveryPlanData> { new DeliveryPlanData() {} };
        }
    }

    #endregion

    /// <summary>
    /// 接口API结果返回显示
    /// </summary>
    class ApiResultValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new ApiResult { ErpErrorDatas = new List<ErpErrorData>() { new ErpErrorData() } };
        }
    }

    /// <summary>
    /// Ebs发运单API参数显示提
    /// </summary>
    class EbsToSmomShippingOrderValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<EbsDeliveryPlanData> { new EbsDeliveryPlanData { } };
        }
    }

    /// <summary>
    /// Ebs发运单API参数显示提
    /// </summary>
    class EbsToSmomPurOrderValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<EbsPurOrderData> { new EbsPurOrderData { } };
        }
    }

    /// <summary>
    /// EbsASN单API参数显示提
    /// </summary>
    class EbsToSmomAsnValueProvider : IApiSampleValueProvider
    {
        public object GetValue()
        {
            return new List<EbsToSmomAsnDetailData> { new EbsToSmomAsnDetailData { OrderType=70 } };
        }
    }
}
