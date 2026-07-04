SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.DeleteFixedAssetAccountCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {

        if (view.hasSelectedEntities()) {
            var flag = true;
            Ext.each(view.getSelection(), function (item) {
                if (item.getReviewStatus() != 10 && item.getReviewStatus() != 50) {
                    flag = false;
                    return false;
                }
            });
            return flag;
        }
        else {
            return false;
        }
    }
});