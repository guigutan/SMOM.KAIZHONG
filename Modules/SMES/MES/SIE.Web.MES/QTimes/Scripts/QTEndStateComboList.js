Ext.define('SIE.Web.MES.QTimes.Scripts.QTEndStateComboList', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.QTEndStateComboList',
    listeners: {
        expand: function (field, eOpts) {
            var me = this;
            var entity = me.up('container').context.record;
            var token = entity.store.associateView.token;
            // 结束工序Id
            var endPId = entity.getEndProcessId();
            if (endPId === null || endPId === 0) {
                SIE.MessageBox.showMessage("请先选择结束工序！".t());
                return;
            }
            // 结束工序类型
            var endPType = entity.getEndProcessType();

            if (SIE.Web.MES.QTimes.Scripts.Common.singleProcessList.includes(endPType)) {
                me.getStore().filterBy(function (record, id) {
                    return record.data.value === 0 || record.data.value === 1;
                });
            }
            else if (SIE.Web.MES.QTimes.Scripts.Common.batchProcessList.includes(endPType)) {
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
