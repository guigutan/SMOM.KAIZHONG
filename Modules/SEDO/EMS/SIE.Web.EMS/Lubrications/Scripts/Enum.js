//润滑状态
Ext.define('SIE.EMS.Enums.LubricationStatus', {
    statics: {
		Pending: { value: 10, text: 'Pending', label: '待执行' },
		Doing: { value: 20, text: 'Doing', label: '执行中' },
		Done: { value: 30, text: 'Done', label: '已执行' },
    }
});

//单据来源
Ext.define('SIE.Equipments.Enums.BillSourceType', {
	statics: {
		Manual: { value: 10, text: 'Manual', label: '手工创建' },
		Automatically: { value: 20, text: 'Automatically', label: '自动创建' },
	}
});
