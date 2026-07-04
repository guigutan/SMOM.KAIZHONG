SIE.defineCommand('SIE.Web.EMS.AssetRequisitions.Commands.EditAssetRequisitionCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改", group: "edit", iconCls: "icon-TextEdit icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null) {
            return false;
        }
        return view.getSelection().length == 1
            && (view.getSelection()[0].data.ApprovalStatus == 10 || view.getSelection()[0].data.ApprovalStatus == 50);
    }
});