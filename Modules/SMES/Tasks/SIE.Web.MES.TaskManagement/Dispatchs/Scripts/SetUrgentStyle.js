Ext.define('SIE.Web.MES.TaskManagement.Dispatchs.Scripts.SetUrgentStyle', {
    extend: 'Ext.grid.column.Column',
    alias: 'widget.setUrgentStyle',
    renderer: function (value, meta, record) {
        var me = this;
        if (this.SIEView && value != null) {
            var task = record;
            var priority = task.getPriority();
            if (priority === 1) {
                meta.tdCls = "icon-red";
            }
        }
        return value;
    },
});