SIE.defineCommand("SIE.Web.LES.MaterialPreparations.Commands.EditPrepareCommand", {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null) {
            return false;
        }
        if (entity.getPrepareStatus() != 0) {
            return false;
        }
        return true;
    },
    showView: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            var type = entity.data.PrepareType;
            var viewGroup = type == 2 ? "WorkShopModeViewStr" : "WorkOrderModeViewStr";
            CRT.Workbench.addPage({
                entityType: me.view.model,
                title: me.getEditViewTitle(entity),
                recordId: entity.getId(),
                viewGroup: viewGroup,
                isDetail: true
            })
        }

    }
})