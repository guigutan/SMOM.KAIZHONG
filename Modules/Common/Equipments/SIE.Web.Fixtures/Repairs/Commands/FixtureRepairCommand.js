SIE.defineCommand('SIE.Web.Fixtures.Repairs.Commands.FixtureRepairCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "维修", group: "edit", iconCls: "iconfont icon-Repair icon-blue" },
    canExecute: function (view) {
        var curEntity = view.getCurrent();
        if (curEntity == null)
            return false;
        var curData = curEntity.getData();
        if (curData != null && curData.RepairState == 15)
            return false;
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var curEntity = view.getCurrent();
        var data = curEntity.getData();
        var title = Ext.String.format('维修-工治具报修[{0}]'.t(), data.No);
        var entityId = curEntity.entityName + '-' + data.No;
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        CRT.Workbench.addPage({
            tabId: tabId,
            entityType: me.view.model,
            recordId: data.Id,
            title: title,
            isDetail: true,
            viewGroup: 'RepairDetails',
            pageClass: 'SIE.Web.Fixtures.Repairs.Script.FixRepairPage',
            params: {
                No: data.No,
                RepairState: data.RepairState,
                ApplyById: data.ApplyById,
                ApplyByName: data.ApplyByName,
                ApplyDate: data.ApplyDate,
                RepairById: data.RepairById,
                RepairByName: data.RepairByName,
                RepairDate: data.RepairDate,
                RepairId: data.Id,
                token: view.token,
                tabId: tabId,
            }
        });
    },
});