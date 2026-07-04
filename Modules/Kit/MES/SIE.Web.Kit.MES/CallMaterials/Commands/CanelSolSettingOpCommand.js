SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.CanelSolSettingOpCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "取消", group: "edit", iconCls: "icon-NetworkError icon-blue" },
    canExecute: function (view) {
        var items = view.getData().data.items;
        if (items != null && items.length > 0 && view.getData().isDirty()) {
            return true;
        }        
        return false;
    },
    execute: function (view, source) {
        var current = view.getCurrent();
        view.reloadData();       
        view.setCurrent(current, true);
    }
});