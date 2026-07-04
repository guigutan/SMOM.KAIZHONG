Ext.define('SIE.Web.MES.QTimes.Scripts.QTStartStateComboList', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.QTStartStateComboList',
    listeners: {
        expand: function (field, eOpts) {
            var me = this;
            var entity = me.up('container').context.record;
            var token = entity.store.associateView.token;
            // 开始工序Id
            var startPId = entity.getStartProcessId();
            if (startPId === null || startPId === 0) {
                SIE.MessageBox.showMessage("请先选择开始工序！".t());
                return;
            }
            // 开始工序类型
            var startPType = entity.getStartProcessType();
            
            if (SIE.Web.MES.QTimes.Scripts.Common.singleProcessList.includes(startPType)) {
                me.getStore().filterBy(function (record, id) {
                    return record.data.value === 0 || record.data.value === 1;
                });
            }
            else if (SIE.Web.MES.QTimes.Scripts.Common.batchProcessList.includes(startPType)) {
                me.getStore().filterBy(function (record, id) {
                    return record.data.value === 2 || record.data.value === 3;
                });
            }

        },
        select: function (combo, record, index) {
            var me = this;
        },

        // 下拉框收起时清空过滤器
        collapse: function (combo) {
            var me = this;
            me.getStore().clearFilter();
        }
    }
});
