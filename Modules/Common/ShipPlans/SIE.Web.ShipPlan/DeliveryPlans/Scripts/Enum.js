Ext.define('SIE.Web.ShipPlan.DeliveryState', {
    statics: {
        Created: { value: 10, text: 'Created', label: '创建' },
        Aduited: { value: 20, text: 'Aduited', label: '审核' },
        Executing: { value: 30, text: 'Executing', label: '执行中' },
        Finished: { value: 40, text: 'Finished', label: '已完成' },
        Cancel: { value: 50, text: 'Cancel', label: '取消' },
    }
});