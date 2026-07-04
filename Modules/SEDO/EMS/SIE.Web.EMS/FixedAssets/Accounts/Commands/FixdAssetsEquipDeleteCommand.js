SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.FixdAssetsEquipDeleteCommand', {
    extend: 'SIE.Web.Core.Common.Commands.ImmediateDeleteCommand',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();

        //待提交Draft = 10, 驳回 Reject = 50,
        if (entity != null && entity.data) {
            return entity.data.INV_ORG_ID !== null && !entity.isNew()
                && (entity.data.ReviewStatus == 10 || entity.data.ReviewStatus == 50);
        }
        return false;
    }
})