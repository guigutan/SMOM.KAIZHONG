SIE.defineCommand('SIE.Web.Fixtures.Demands.Commands.UnLoadCommand', {
    meta: { text: "出库", group: "edit", iconCls:"icon-WarehouseExport icon-blue"},
    canExecute: function (view) {
        var curEntity = view.getCurrent();
        if (curEntity == null)
            return false;
        var curData = curEntity.getData();

        if (_golble_use_approval)//启用了审批
        {
            if (curData.DemandState == 15 || curData.ApprovalStatus != 40 || curData.Close ==1)
                return false;
        }
        else {
            if (curData.DemandState == 15 || curData.Close ==1)
                return false;
        }
        return true;

    },
    execute: function (view, source) {
        var me = this;
        var curEntity = view.getCurrent();
        var data = curEntity.getData();
        var title = Ext.String.format('出库-工治具需求清单[{0}]'.t(), data.No);
        var entityId = curEntity.entityName + '-' + data.No;
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        CRT.Workbench.addPage({
            entityType: 'SIE.Fixtures.FixtureDemands.ViewModels.FixtureDemandViewModel',
            title: title,
            tabId: tabId,
            isDetail: true,
            pageClass: 'SIE.Web.Fixtures.Demands.Scripts.UnloadPage',
            module: 'SIE.Fixtures.FixtureDemands.FixtureDemand,SIE.Fixtures',
            params: {
                No: data.No,
                WorkShopName: data.WorkShopName,
                ResourceName: data.ResourceName,
                WorkOrderNo: data.WorkOrderNo,
                ProductCode: data.WorkOrderProductCode,
                FixDemandId: data.Id,
                token: view.token,
            }
        });
    }
});