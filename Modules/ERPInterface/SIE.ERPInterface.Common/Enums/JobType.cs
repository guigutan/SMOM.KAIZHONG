using SIE.ObjectModel;

namespace SIE.ERPInterface.Common.Enums
{
    /// <summary>
    /// 任务类型
    /// </summary>
    public enum JobType
    {
        /// <summary>
        /// 企业层级
        /// </summary>
        [Label("企业层级")]
        EnterpriseLevel = 0,
        /// <summary>
        /// 企业模型
        /// </summary>
        [Label("企业模型")]
        Enterprise = 1,
        /// <summary>
        /// 物料分类
        /// </summary>
        [Label("物料分类")]
        ItemCategory = 2,
        /// <summary>
        /// 物料
        /// </summary>
        [Label("物料")]
        Item = 3,
        /// <summary>
        /// 产品BOM
        /// </summary>
        [Label("产品BOM")]
        ProductBom = 4,
        /// <summary>
        /// 供应商
        /// </summary>
        [Label("供应商")]
        Supplier = 5,
        /// <summary>
        /// 客户
        /// </summary>
        [Label("客户")]
        Customer = 6,
        /// <summary>
        /// 仓库
        /// </summary>
        [Label("仓库")]
        Warehouse = 7,
        /// <summary>
        /// 库区
        /// </summary>
        [Label("库区")]
        StorageArea = 8,
        /// <summary>
        /// 库位
        /// </summary>
        [Label("库位")]
        StorageLocation = 9,
        /// <summary>
        /// 工序
        /// </summary>
        [Label("工序")]
        Process = 10,
        /// <summary>
        /// 采购订单
        /// </summary>
        [Label("采购订单")]
        PurchaseOrder = 11,
        /// <summary>
        /// 采购订单明细
        /// </summary>
        [Label("采购订单明细")]
        PurchaseOrderDetail = 12,
        /// <summary>
        /// 功能
        /// </summary>
        [Label("功能")]
        Function = 13,
        /// <summary>
        /// 事务
        /// </summary>
        [Label("事务")]
        Transaction = 14,
        /// <summary>
        /// 单位
        /// </summary>
        [Label("单位")]
        Unit = 15,
        /// <summary>
        /// 工单
        /// </summary>
        [Label("工单")]
        WorkOrder = 16,
        /// <summary>
        /// 工单BOM
        /// </summary>
        [Label("工单BOM")]
        WorkOrderBom = 17,
        /// <summary>
        /// 产品BOM明细
        /// </summary>
        [Label("产品BOM明细")]
        ProductBomDtl = 18,
        /// <summary>
        /// 供应商地址
        /// </summary>
        [Label("供应商地址")]
        SupplierAddress = 19,
        /// <summary>
        /// 客户地址
        /// </summary>
        [Label("客户地址")]
        CustomerAddress = 20,
        /// <summary>
        /// 员工
        /// </summary>
        [Label("员工")]
        Employee = 21,
        /// <summary>
        /// ASN单
        /// </summary>
        [Label("ASN单")]
        Asn = 22,
        /// <summary>
        /// ASN明细
        /// </summary>
        [Label("ASN明细")]
        AsnDtl = 23,
        /// <summary>
        /// 发运单
        /// </summary>
        [Label("发运单")]
        ShippingOrder = 24,
        /// <summary>
        /// 发运单明细
        /// </summary>
        [Label("发运单明细")]
        ShippingOrderDtl = 25,
        /// <summary>
        /// 账户别名
        /// </summary>
        [Label("账户别名")]
        GenericDisposition = 26,
        /// <summary>
        /// 生产订单BOM
        /// </summary>
        [Label("生产订单BOM")]
        ProductOrderBom = 27,
        /// <summary>
        /// 生产订单
        /// </summary>
        [Label("生产订单")]
        ProductOrder = 28,
       
        /// <summary>
        /// 缺陷分类
        /// </summary>
        [Label("缺陷分类")]
        DefectCategory = 29,
        /// <summary>
        /// 缺陷等级
        /// </summary>
        [Label("缺陷等级")]
        DefectGrade = 30,
        /// <summary>
        /// 缺陷代码
        /// </summary>
        [Label("缺陷代码")]
        DefectCode = 31,

        /// <summary>
        /// 库存调拨
        /// </summary>
        [Label("库存调拨")]
        Allocate = 32,

        /// <summary>
        /// 发货计划
        /// </summary>
        [Label("发货计划")]
        DeliveryPlan = 33,

        /// <summary>
        /// 分类检验标准
        /// </summary>
        [Label("分类检验标准")]
        CategoryInspStandard = 34,

        /// <summary>
        /// 分类检验标准
        /// </summary>
        [Label("库存组织")]
        InvOrg = 35,


        /// <summary>
        /// 分类检验标准
        /// </summary>
        [Label("项目")]
        Project = 36,


        /// <summary>
        /// 分类检验标准
        /// </summary>
        [Label("任务")]
        Task = 37,

        /// <summary>
        /// 单位转换
        /// </summary>
        [Label("单位转换")]
        UnitChange = 38,

        /// <summary>
        /// 销售发货
        /// </summary>
        [Label("销售发货")]
        SaleOut = 39,

        /// <summary>
        /// 工单发料
        /// </summary>
        [Label("工单发料")]
        WorkFeed = 40,

        /// <summary>
        /// 销售退货
        /// </summary>
        [Label("销售退货")]
        SaleReturn = 41,

        /// <summary>
        /// 调拨入库
        /// </summary>
        [Label("调拨入库")]
        AllocateIn = 42,

        /// <summary>
        /// 直接调拨
        /// </summary>
        [Label("直接调拨")]
        DirectAllocate = 43,

        /// <summary>
        /// 跨组织调拨入库
        /// </summary>
        [Label("跨组织调拨入库")]
        CrossOrgTransferIn = 44,


        /// <summary>
        /// 跨组织调拨出库
        /// </summary>
        [Label("跨组织调拨出库")]
        CrossOrgTransferOut = 55,


        /// <summary>
        /// 其他入库
        /// </summary>
        [Label("其他入库")]
        OtherIn = 45,

        /// <summary>
        /// 其他出库
        /// </summary>
        [Label("其他出库")]
        OtherOut = 46,

        /// <summary>
        /// 生产退料
        /// </summary>
        [Label("生产退料")]
        MaterialReturn = 47,

        /// <summary>
        /// 采购入库
        /// </summary>
        [Label("采购入库(入库上架)")]
        PurchaseIn = 48,

       
        /// <summary>
        /// 供应商退货
        /// </summary>
        [Label("供应商退货")]
        SupplierReturn = 49,

        /// <summary>
        /// 账户别名
        /// </summary>
        [Label("账户别名")]
        AccountAliases = 50,

        /// <summary>
        /// 物料检验标准
        /// </summary>
        [Label("物料检验标准")]
        ItemInspStandard = 51,

      
        /// <summary>
        /// 暂收供应商退货
        /// </summary>
        [Label("暂收供应商退货")]
        TempSupplierReturn = 53,
        

        /// <summary>
        /// 来料暂收
        /// </summary>
        [Label("采购入库(来料暂收)")]
        Receive = 52,

        /// <summary>
        /// 完工入库(成品入库)
        /// </summary>
        [Label("完工入库(成品入库)")]
        Finished = 56,

        /// <summary>
        /// 完工入库(半成品入库)
        /// </summary>
        [Label("完工入库(半成品入库)")]
        PartedIn = 54,

        /// <summary>
        /// ERP子库
        /// </summary>
        [Label("ERP子库")]
        ErpWarehouse = 57,

    }
}