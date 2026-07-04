using SIE.ObjectModel;
using System.ComponentModel;

namespace SIE.Inventory.Transactions
{
    /// <summary>
    /// 事务类型(接收、拣货、IQC检验合格、IQC检验不合格类型不可修改）
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        /// 接收
        /// </summary>
        [Label("接收")]
        Receive = 0,

        /// <summary>
        /// 取消接收
        /// </summary>
        [Label("取消接收")]
        UnReceive = 1,

        /// <summary>
        /// 接收入库
        /// </summary>
        [Label("接收入库")]
        RecInStorage = 2,

        /// <summary>
        /// 码盘
        /// </summary>
        [Label("码盘")]
        Disk = 3,

        /// <summary>
        /// 拣货
        /// </summary>
        [Label("拣货")]
        Picking = 10,

        /// <summary>
        /// 反拣
        /// </summary>
        [Label("反拣")]
        UnPicking = 11,

        /// <summary>
        /// 超额拣货
        /// </summary>
        [Label("超额拣货")]
        OverPicking = 12,

        /// <summary>
        /// IQC检验合格
        /// </summary>
        [Label("IQC检验合格")]
        IqcQualified = 20,

        /// <summary>
        /// IQC检验不合格
        /// </summary>
        [Label("IQC检验不合格")]
        IqcUnQualified = 30,

        /// <summary>
        /// 调整
        /// </summary>
        [Label("调整")]
        adjust = 40,

        /// <summary>
        /// 入库
        /// </summary>
        [Label("入库")]
        InStorage = 50,

        /// <summary>
        /// 出库
        /// </summary>
        [Label("出库")]
        OutStorage = 60,

        /// <summary>
        /// 取消出库
        /// </summary>
        [Label("取消出库")]
        UnOutStorage = 61,

        /// <summary>
        /// 一键出库
        /// </summary>
        [Label("一键出库")]
        OneKeyOutStorage = 62,

        /// <summary>
        /// 移动
        /// </summary>
        [Label("移动")]
        Move = 70,

        /// 搬运
        /// </summary>
        [Label("搬运")]
        Carry = 71,

        /// <summary>
        /// 调拨
        /// </summary>
        [Label("调拨")]
        Allocate = 75,

        /// <summary>
        /// 分配
        /// </summary>
        [Label("分配")]
        Allot = 80,

        /// <summary>
        /// 取消分配
        /// </summary>
        [Label("取消分配")]
        CancelAllot = 81,

        /// <summary>
        /// 关闭分配
        /// </summary>
        [Label("关闭分配")]
        CloseAllot = 82,

        /// <summary>
        /// 取消调拨
        /// </summary>
        [Label("取消调拨")]
        CancelShipAllot = 83,

        /// <summary>
        /// 冻结
        /// </summary>
        [Label("冻结")]
        Frozen = 90,

        /// <summary>
        /// 解冻
        /// </summary>
        [Label("解冻")]
        UnFrozen = 91,

        /// <summary>
        /// 播种
        /// </summary>
        [Label("播种")]
        Sow = 92,

        /// <summary>
        /// 补货
        /// </summary>
        [Label("补货")]
        Replenish = 100,

        /// <summary>
        /// 释放分配
        /// </summary>
        [Label("释放分配")]
        ReleaseAllot = 101,

        /// <summary>
        /// 超额分配
        /// </summary>
        [Label("超额分配")]
        ExcessAllot = 102,

        /// <summary>
        /// 取消补货
        /// </summary>
        [Label("取消补货")]
        CancelReplenish = 103,

        /// <summary>
        /// 库存报检
        /// </summary>
        [Label("库存报检")]
        Inspection = 104,

        /// <summary>
        /// 取消报检
        /// </summary>
        [Label("取消报检")]
        CancelInspection = 105,

        /// <summary>
        /// IQC复检
        /// </summary>
        [Label("IQC复检")]
        IqcRecheck = 106,

        /// <summary>
        /// 取消报检
        /// </summary>
        [Label("取消质检")]
        CancelQuan = 108,

        /// <summary>
        /// 借出
        /// </summary>
        [Label("借出")]
        Lend = 110,

        /// <summary>
        /// 返还
        /// </summary>
        [Label("返还")]
        Return = 111,

        /// <summary>
        /// 返包移动
        /// </summary>
        [Label("返包移动")]
        WhReturnInsp = 112,

        /// <summary>
        /// 返包回库
        /// </summary>
        [Label("返包回库")]
        WhReturnIn = 113,

        /// <summary>
        /// 加工投入
        /// </summary>
        [Label("加工投入")]
        InvProcessInput = 114,

        /// <summary>
        /// 加工产出
        /// </summary>
        [Label("加工产出")]
        InvProcessOutput = 115,

        /// <summary>
        /// 物料上料
        /// </summary>
        [Label("物料上料")]
        MesMaterialUp = 120,

        /// <summary>
        /// 物料下料
        /// </summary>
        [Label("物料下料")]
        MesMaterialDown = 121,

        /// <summary>
        /// 物料倒扣
        /// </summary>
        [Label("物料倒扣")]
        MesMaterialReduce = 123,

        /// <summary>
        /// 工单报工
        /// </summary>
        [Category("KZ")]
        [Label("工单报工")]
        WoFinish = 124,

        /// <summary>
        /// 扣料
        /// </summary>
        [Category("KZ")]
        [Label("扣料")]
        Deduction = 125,

        /// <summary>
        /// 副产品
        /// </summary>
        [Category("KZ")]
        [Label("副产品")]
        OutputProduct = 126,

        /// <summary>
        /// 委外出库同步工厂数据
        /// </summary>
        [Category("KZ")]
        [Label("委外出库同步工厂数据")]
        Outsourcingouts = 127,

        /// <summary>
        /// 委外入库同步工厂数据
        /// </summary>
        [Category("KZ")]
        [Label("委外入库同步工厂数据")]
        OutsourcingIns = 128,

        /// <summary>
        /// 委外报工同步工厂数据
        /// </summary>
        [Category("KZ")]
        [Label("委外报工同步工厂数据")]
        OutsourcingReport = 129,

        /// <summary>
        /// 委外报工同步可疑品标签数据
        /// </summary>
        [Category("KZ")]
        [Label("委外报工同步可疑品标签数据")]
        OutsourcingSupWipBatch = 130,

        /// <summary>
        /// 委外收货报工
        /// </summary>
        [Category("KZ")]
        [Label("委外收货报工")]
        ProcessingInStockReport = 131,

        /// <summary>
        /// 差异扣料
        /// </summary>
        [Category("KZ")]
        [Label("差异扣料")]
        ScrapWeighing = 132,


        /// <summary>
        /// 发货确认
        /// </summary>
        [Category("KZ")]
        [Label("发货确认")]
        OutboundConfirm = 133,

        /// <summary>
        /// 返工信息
        /// </summary>
        [Category("KZ")]
        [Label("返工信息")]
        ReworkInfoRecord = 134,

    }
}