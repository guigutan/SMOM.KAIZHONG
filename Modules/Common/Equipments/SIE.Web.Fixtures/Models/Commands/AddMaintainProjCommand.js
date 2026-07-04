SIE.defineCommand('SIE.Web.Fixtures.Models.Commands.AddMaintainProjCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.INV_ORG_ID !== null && !entity.isNew();
        }
        return false;
    }
});