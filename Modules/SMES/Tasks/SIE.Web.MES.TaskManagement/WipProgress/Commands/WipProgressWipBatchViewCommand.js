SIE.defineCommand('SIE.Web.MES.TaskManagement.WipProgress.Commands.WipProgressWipBatchViewCommand', {
    //extend: 'SIE.cmd.Edit',
    meta: { text: "查看详情", group: "edit", iconCls: "icon-PageSearch icon-blue" },
    canExecute: function (listView) {
        return listView != null && listView.getSelection().length == 1;
    },
    execute: function (view, source) {

        var me = this;
        var indata = {};
        var editEntity = view.getCurrent();
        data = editEntity.data;

        var entityId = editEntity.entityName + '_' + "WipProgressBatchView" + '_' + editEntity.getId();
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        CRT.Workbench.addPage({
            entityType: "SIE.MES.TaskManagement.WipProgress.WipProgressWipBatch",
            module: view.module,
            recordId: entityId,
            tabId: tabId,
            viewGroup: "WipProgressBatchView",
            title: Ext.String.format('查看工序在制详情-{0}-{1} {2}'.L10N(), data.WorkOrderNo, data.ProcessCode, data.BatchNo),
            //isDetail: true,
            params: {
                WorkOrderNo: data.WorkOrderNo,
                ProcessId: data.ProcessId,
                PreProcessCode: data.PreProcessCode,
                ProcessCode: data.ProcessCode,
                ProcessName: data.ProcessName,
                BatchNo: data.BatchNo
            }
        });
    },
});