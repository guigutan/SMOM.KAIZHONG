using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Core.Enums
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 采购入库
        /// </summary>
        [Category("RECEIPT")]
        [Label("采购入库")]
        PurchaseIn = 0,

        /// <summary>
        /// 成品入库
        /// </summary>
        [Category("RECEIPT")]
        [Label("成品入库")]
        Finished = 10,

        /// <summary>
        /// 半成品入库
        /// </summary>
        [Category("RECEIPT")]
        [Label("半成品入库")]
        PartedIn = 20,

        /// <summary>
        /// 生产退料
        /// </summary>
        [Category("RECEIPT")]
        [Label("生产退料")]
        MaterialReturn = 30,

        /// <summary>
        /// 委外退料
        /// </summary>
        [Category("RECEIPT")]
        [Label("委外退料")]
        OutMaterialReturn = 31,

        /// <summary>
        /// 销售退货
        /// </summary>
        [Category("RECEIPT")]
        [Label("销售退货")]
        SaleReturn = 40,

        /// <summary>
        /// 客供入库
        /// </summary>
        [Category("RECEIPT")]
        [Label("客供入库")]
        CustomerIn = 41,

        /// <summary>
        /// VMI入库
        /// </summary>
        [Label("VMI入库")]
        [Category("RECEIPT")]
        VMIIN = 50,

        /// <summary>
        /// 其他入库
        /// </summary>
        [Category("RECEIPT")]
        [Label("其他入库")]
        OtherIn = 60,

        /// <summary>
        /// 转仓入库
        /// </summary>
        [Category("RECEIPT")]
        [Label("转仓入库")]
        WhTransferIn =61,

        /// <summary>
        /// 销售出库
        /// </summary>
        [Category("SHIPMENT")]
        [Label("销售出库")]
        SaleOut = 70,

        /// <summary>
        /// 工单发料
        /// </summary>
        [Category("SHIPMENT")]
        [Label("工单发料")]
        WorkFeed = 80,

        /// <summary>
        /// 委外工单发料
        /// </summary>
        [Category("SHIPMENT")]
        [Label("委外工单发料")]
        OutWorkFeed = 81,

        /// <summary>
        /// 委外工单耗料
        /// </summary>
        [Category("SHIPMENT")]
        [Label("委外工单耗料")]
        OutWorkFeedUse = 82,

        /// <summary>
        /// 委外调拨退料
        /// </summary>
        [Category("SHIPMENT")]
        [Label("委外调拨退料")]
        OutAllotReturn = 84,

        /// <summary>
        /// 完工工单退回
        /// </summary>
        [Category("SHIPMENT")]
        [Label("完工工单退回")]
        WoFinishReturn = 83,

        /// <summary>
        /// 其他出库
        /// </summary>
        [Category("SHIPMENT")]
        [Label("其他出库")]
        OtherOut = 90,

        /// <summary>
        /// 转仓出库
        /// </summary>
        [Category("SHIPMENT")]
        [Label("转仓出库")]
        WhTransferOut =91,

        /// <summary>
        /// 供应商退货
        /// </summary>
        [Category("SHIPMENT")]
        [Label("供应商退货")]
        SupplierReturn = 100,

        /// <summary>
        /// 直接移动
        /// </summary>
        [Label("直接移动")]
        DirectMove = 110,

        /// <summary>
        /// 直接调拨
        /// </summary>
        [Category("ALLOCATE")]
        [Label("直接调拨")]
        DirectAllocate = 120,

        /// <summary>
        /// 两次调拨
        /// </summary>
        [Category("ALLOCATE")]
        [Label("两步调拨")]
        TwoAllocate = 121,

        /// <summary>
        /// 调拨入库
        /// </summary>
        [Label("调拨入库")]
        AllocateIn = 122,

        /// <summary>
        /// 库存调整
        /// </summary>
        [Category("ADJUST")]
        [Label("库存调整")]
        InventoryAdjust = 130,

        /// <summary>
		/// 标准盘点
		/// </summary>
        [Category("STOCKCOUNT")]
        [Label("标准盘点")]
        StandardCount = 140,

        /// <summary>
        /// 动账盘点
        /// </summary>
        [Category("STOCKCOUNT")]
        [Label("动账盘点")]
        AccountCount = 150,

        /// <summary>
        /// 随机盘点
        /// </summary>
        [Category("STOCKCOUNT")]
        [Label("随机盘点")]
        RandomCount = 160,

        /// <summary>
        /// 差异盘点
        /// </summary>
        [Label("差异盘点")]
        DifferenceCount = 170,

        /// <summary>
        /// 循环盘点
        /// </summary>
        [Label("循环盘点")]
        CycleCount = 171,

        /// <summary>
		/// 冻结
		/// </summary>
        [Category("FROZEN")]
        [Label("冻结")]
        Frozen = 180,

        /// <summary>
        /// 解冻
        /// </summary>
        [Category("FROZEN")]
        [Label("解冻")]
        UnFrozen = 190,

        /// <summary>
		/// 批次变更
		/// </summary>
        [Category("LOTADJUST")]
        [Label("批次变更")]
        LotAdjust = 200,

        /// <summary>
        /// 货主变更
        /// </summary>
        [Category("ONHANDADJUST")]
        [Label("货主变更")]
        StorerAdjust = 210,

        /// <summary>
        /// 库存状态变更
        /// </summary>
        [Category("ONHANDADJUST")]
        [Label("库存状态变更")]
        OnhandStateAdjust = 220,

        /// <summary>
        /// 项目变更
        /// </summary>
        [Category("ONHANDADJUST")]
        [Label("项目变更")]
        ProjectAdjust = 221,

        /// <summary>
        /// 任务号变更
        /// </summary>
        [Category("ONHANDADJUST")]
        [Label("任务号变更")]
        TaskNoAdjust = 222,

        /// <summary>
        /// 波次分配
        /// </summary>
        [Category("WAVE")]
        [Label("波次分配")]
        WaveAssign = 230,

        /// <summary>
        /// 波次拣货
        /// </summary>
        [Category("WAVE")]
        [Label("波次拣货")]
        WavePick = 240,

        /// <summary>
        /// 波次补货
        /// </summary>
        [Label("波次补货")]
        WaveReplenish = 250,

        /// <summary>
        /// 直接补货
        /// </summary>
        [Category("REPLENISH")]
        [Label("直接补货")]
        DirectReplenish = 260,

        /// <summary>
        /// 两步补货
        /// </summary>
        [Category("REPLENISH")]
        [Label("两步补货")]
        TwoReplenish = 261,

        /// <summary>
        /// 补货入库
        /// </summary>
        [Label("补货入库")]
        ReplenishIn = 262,

        /// <summary>
        /// 库存报检
        /// </summary>
        [Category("INSPECTION")]
        [Label("库存报检")]
        Inspection = 263,

        /// <summary>
        /// 组合板合并
        /// </summary>
        [Label("组合板合并")]
        CombineMerge = 270,

        /// <summary>
        /// 借入借出
        /// </summary>
        [Label("借入借出")]
        BorrowLend = 280,

        /// <summary>
        /// 提仓返检
        /// </summary>
        [Label("提仓返检")]
        WhReturn = 281,

        /// <summary>
        /// 库内加工
        /// </summary>
        [Label("库内加工")]
        InvProcess = 282,

        /// <summary>
        /// MES上料
        /// </summary>
        [Label("上料")]
        MaterialUp = 283,

        /// <summary>
        /// MES物料接收
        /// </summary>
        [Label("物料接收")]
        MaterialRecive = 284,

        /// <summary>
        /// MES下料
        /// </summary>
        [Label("下料")]
        MaterialDown = 285,

        /// <summary>
        /// MES倒扣非工序BOM物料
        /// </summary>
        [Label("倒扣料")]
        MaterialReduce = 286,

        /// <summary>
        /// 自动加入线边仓
        /// </summary>
        [Label("自动入线边仓")]
        AutoJoinLineWarehouse=287,

        /// <summary>
        /// 跨组织调拨入库
        /// </summary>
        [Category("RECEIPT")]
        [Label("跨组织调拨入库")]
        CrossOrgTransferIn = 288,

        /// <summary>
        /// 工单报工
        /// </summary>
        [Category("KZ")]
        [Label("工单报工")]
        WoFinish = 300,

        /// <summary>
        /// 扣料
        /// </summary>
        [Category("KZ")]
        [Label("扣料")]
        Deduction = 310,

        /// <summary>
        /// 副产品
        /// </summary>
        [Category("KZ")]
        [Label("副产品")]
        OutputProduct = 320,

        /// <summary>
        /// 委外出库同步工厂数据
        /// </summary>
        [Category("KZ")]
        [Label("委外出库同步工厂数据")]
        Outsourcingouts = 330,

        /// <summary>
        /// 委外入库同步工厂数据
        /// </summary>
        [Category("KZ")]
        [Label("委外入库同步工厂数据")]
        OutsourcingIns = 340,

        /// <summary>
        /// 委外报工同步工厂数据
        /// </summary>
        [Category("KZ")]
        [Label("委外报工同步工厂数据")]
        OutsourcingReport = 350,

        /// <summary>
        /// 委外报工同步可疑品标签数据
        /// </summary>
        [Category("KZ")]
        [Label("委外报工同步可疑品标签数据")]
        OutsourcingSupWipBatch = 360,

        /// <summary>
        /// 委外收货报工
        /// </summary>
        [Category("KZ")]
        [Label("委外收货报工")]
        ProcessingInStockReport = 370,

        /// <summary>
        /// 余料称重上传
        /// </summary>
        //[Category("KZ")]
        //[Label("余料称重上传")]
        //ScrapWeighing = 380,

        /// <summary>
        /// 发货确认
        /// </summary>
        [Category("KZ")]
        [Label("发货确认")]
        OutboundConfirm = 390,

        /// <summary>
        /// 返工信息
        /// </summary>
        [Category("KZ")]
        [Label("返工信息")]
        ReworkInfoRecord = 400,
    }
}