Ext.define('SIE.LES.StockOrder.StockState', {
    statics: {
        Created: { value: 0, text: 'Created', label: '已创建' },
        ReCall: { value: 5, text: 'ReCall', label: '已撤回' },
        Audit: { value: 10, text: 'Audit', label: '待审核' },
        Submitted: { value: 20, text: 'Submitted', label: '已提交' },
        PickStocking: { value: 25, text: 'PickStocking', label: '拣配中' },
        PickStocked: { value: 30, text: 'PickStocked', label: '已拣配' },
        PartShip: { value: 35, text: 'PartShip', label: '部分发货' },
        Shipped: { value: 40, text: 'Shipped', label: '已发货' },
        PartReceive: { value: 45, text: 'PartReceive', label: '部分接收' },
        Received: { value: 50, text: 'Received', label: '已接收' },
        Closed: { value: 60, text: 'Closed', label: '已关闭' },
        Finished: { value: 70, text: 'Finished', label: '已完成' },
    }
});
//SIE:classEnd
Ext.define('SIE.LES.StockOrder.BillSource', {
    statics: {
        Manual: { value: 0, text: 'Manual', label: '手动' },
        Automatic: { value: 1, text: 'Automatic', label: '自动' },
    }
});