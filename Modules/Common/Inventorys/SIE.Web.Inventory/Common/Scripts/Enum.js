Ext.define('SIE.Inventory.Commom.OrderType', {
    statics: {
        PurchaseIn: { value: 0, text: 'PurchaseIn', label: '采购入库' },
        Finished: { value: 10, text: 'Finished', label: '成品入库' },
        PartedIn: { value: 20, text: 'PartedIn', label: '半成品入库' },
        MaterialReturn: { value: 30, text: 'MaterialReturn', label: '生产退料' },
        OutRetAsn: { value: 31, text: 'OutMaterialReturn', label:'委外退料' },
        SaleReturn: { value: 40, text: 'SaleReturn', label: '销售退货' },
        CustomerIn: { value: 41, text: 'CustomerIn', label: '客供入库' },
        VMIIN: { value: 50, text: 'VMIIN', label: 'VMI入库' },
        OtherIn: { value: 60, text: 'OtherIn', label: '其他入库' },
        SaleOut: { value: 70, text: 'SaleOut', label: '销售出库' },
        WorkFeed: { value: 80, text: 'WorkFeed', label: '工单发料' },
        OutWorkFeed: { value: 81, text: 'OutWorkFeed', label: '委外工单发料' },
        OutWorkFeedUse: { value: 82, text: 'OutWorkFeedUse', label: '委外工单耗料' },
        OutAllotReturn: { value: 84, text: 'OutAllotReturn', label: '委外调拨退料' },
        WhTransferIn: { value: 61, text: 'WhTransferIn', label: '转仓入库' },
        CrossOrgTransferIn: { value: 288, text: 'CrossOrgTransferIn', label: '跨组织调拨入库' },
        WoFinishReturn: { value: 83, text: 'WoFinishReturn', label: '工单完工退回' },
        OtherOut: { value: 90, text: 'OtherOut', label: '其他出库' },
        WhTransferOut: { value: 91, text: 'WhTransferOut', label: '转仓出库' },
        SupplierReturn: { value: 100, text: 'SupplierReturn', label: '供应商退货' },
        DirectMove: { value: 110, text: 'DirectMove', label: '直接移动' },
        DirectAllocate: { value: 120, text: 'DirectAllocate', label: '直接调拨' },
        TwoAllocate: { value: 121, text: 'TwoAllocate', label: '两步调拨' },
        AllocateIn: { value: 122, text: 'AllocateIn', label: '调拨入库' },
        InventoryAdjust: { value: 130, text: 'InventoryAdjust', label: '库存调整' },
        StandardCount: { value: 140, text: 'StandardCount', label: '标准盘点' },
        AccountCount: { value: 150, text: 'AccountCount', label: '动账盘点' },
        RandomCount: { value: 160, text: 'RandomCount', label: '随机盘点' },
        DifferenceCount: { value: 170, text: 'DifferenceCount', label: '差异盘点' },
        CycleCount: { value: 171, text: 'CycleCount', label: '循环盘点' },
        Frozen: { value: 180, text: 'Frozen', label: '冻结' },
        UnFrozen: { value: 190, text: 'UnFrozen', label: '解冻' },
        LotAdjust: { value: 200, text: 'LotAdjust', label: '批次变更' },
        StorerAdjust: { value: 210, text: 'StorerAdjust', label: '货主变更' },
        OnhandStateAdjust: { value: 220, text: 'OnhandStateAdjust', label: '库存状态变更' },
        ProjectAdjust: { value: 221, text: 'ProjectAdjust', label: '项目变更' },
        TaskNoAdjust: { value: 222, text: 'TaskNoAdjust', label: '任务号变更' },
        WaveAssign: { value: 230, text: 'WaveAssign', label: '波次分配' },
        WavePick: { value: 240, text: 'WavePick', label: '波次拣货' },
        WaveReplenish: { value: 250, text: 'WaveReplenish', label: '波次补货' },
        DirectReplenish: { value: 260, text: 'DirectReplenish', label: '直接补货' },
        TwoReplenish: { value: 261, text: 'TwoReplenish', label: '两步补货' },
        ReplenishIn: { value: 262, text: 'ReplenishIn', label: '补货入库' },
        Inspection: { value: 263, text: 'Inspection', label: '库存报检' },
    }
});
//SIE:classEnd
Ext.define('SIE.Inventory.Commom.TaskLevel', {
    statics: {
        Low: { value: 0, text: 'Low', label: '低' },
        Middle: { value: 1, text: 'Middle', label: '中' },
        High: { value: 2, text: 'High', label: '高' },
        Urgent: { value: 3, text: 'Urgent', label: '加急' }
    }
});
//SIE:classEnd
//盘点细度
Ext.define('SIE.Inventory.Commom.CountDimension', {
    statics: {
        Default: { value: 0, text: 'Default', label: '默认' },
        Warehouse: { value: 10, text: 'Warehouse', label: '仓库' },
        Area: { value: 20, text: 'Area', label: '库区' },
        Location: { value: 30, text: 'Location', label: '库位' },
        WarehouseLot: { value: 40, text: 'WarehouseLot', label: '仓库+批次' },
        LocationLot: { value: 50, text: 'LocationLot', label: '库位+批次' }
    }
});
//SIE:classEnd
//库存状态
Ext.define('SIE.Inventory.Commom.OnhandState', {
    statics: {
        None: { value: 1, text: 'None', label: '未质检' },
        Ok: { value: 10, text: 'Ok', label: '合格' },
        Ng: { value: 20, text: 'Ng', label: '不合格' }
    }
});
//SIE:classEnd
//批次属性数据类型
Ext.define('SIE.Inventory.Commom.DataType', {
    statics: {
        Date: { value: 0, text: 'Date', label: '日期' },
        Text: { value: 1, text: 'Text', label: '文本' },
        Numerical: { value: 2, text: 'Numerical', label: '数值' },
    }
});
/*物料扩展属性功能类型*/
Ext.define('SIE.Inventory.Commom.FunctionType', {
    statics: {
        ASN: { value: 0, text: 'ASN', label: 'ASN' },
        PO: { value: 1, text: 'PO', label: '采购单' },
        MOVE: { value: 2, text: 'MOVE', label: '库存移动' },
        ALLOCATE: { value: 3, text: 'ALLOCATE', label: '库存调拨' },
        ADJUST: { value: 4, text: 'ADJUST', label: '库存调整' },
        FROZEN: { value: 5, text: 'FROZEN', label: '库存冻结' },
        COUNT: { value: 6, text: 'COUNT', label: '库存盘点' },
        SHIPMENT: { value: 7, text: 'SHIPMENT', label: '发运单' },
        LOT: { value: 8, text: 'LOT', label: '批次库存' },
        INSPECTION: { value: 9, text: 'INSPECTION', label: '库存报检' },
        IQCRECHECK: { value: 10, text: 'IQCRECHECK', label: 'IQC检验' },
        REPLENISHPLAN: { value: 11, text: 'REPLENISHPLAN', label: '补货计划' },
        WAVEPLAN: { value: 12, text: 'WAVEPLAN', label: '波次计划' },
        ITEMIOLIMIT: { value: 13, text: 'ITEMIOLIMIT', label: '收发控制' },
        REPLENISHRULE: { value: 14, text: 'REPLENISHRULE', label: '补货规则' },
        SOWPOOL: { value: 15, text: 'SOWPOOL', label: '播种池' },
    }
});