SIE.defineCommand('SIE.Web.EMS.Equipments.Boms.Commands.InsertChildSparePartCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加子", group: "edit", iconCls: "icon-FileTree icon-green" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null)
            return false;
        return true;
    },
    execute: function (view, source) {
        var childNode = view.insertNewChild();
        childNode.generateId();
        childNode.setEquipBomId(view.getParent().getCurrent().data.Id);
        view.getControl().setSelection(childNode); 
    }
});

