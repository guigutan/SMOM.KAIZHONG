SIE.defineCommand('SIE.Web.EMS.SpareParts.OutDepots.Commands.PickOutDepotDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "拣货", group: "edit", iconCls: "icon-TextEdit icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1
            && view.getSelection()[0].data.IsNeedPick
            && (view.getSelection()[0].data.OutDepotState == 0 || view.getSelection()[0].data.OutDepotState == 2);
    },
    execute: function (view, source) {

        var me = this;
        var editEntity = me.getEditEntity();

        CRT.Workbench.addPage({
            entityType: me.view.model,
            recordId: editEntity.data.Id,
            title: '拣货-备件出库单'.t(),
            viewGroup: "PickOutDepotDetailsViewGroup",
            isDetail: true,
            ignoreQuery: true,
            token: view.token,
            params: {
                Id: editEntity.data.Id,
                No: editEntity.data.No,
                OutDepotType: editEntity.data.OutDepotType,
                WarehouseId: editEntity.data.WarehouseId,
                WarehouseId_Display: editEntity.data.WarehouseId_Display,
                GetDepartmentId: editEntity.data.GetDepartmentId,
                GetDepartmentId_Display: editEntity.data.GetDepartmentId_Display,
                QualityStatus: editEntity.data.QualityStatus,
                CreateDate: editEntity.data.CreateDate,
            }
        });
    }
});