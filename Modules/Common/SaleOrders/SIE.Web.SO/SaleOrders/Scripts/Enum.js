//SIE:classEnd
Ext.define('SIE.Pcb.SO.SaleOrders.LineState', {
    statics: {
        NEW: { value: 0, text: 'NEW', label: '新建' },
        CONFIRMED: { value: 10, text: 'CONFIRMED', label: '确定' },
        PRODUCTION: { value: 20, text: 'PRODUCTION', label: '生产' },
        COMPLETE: { value: 30, text: 'COMPLETE', label: '完成' }
    }
});