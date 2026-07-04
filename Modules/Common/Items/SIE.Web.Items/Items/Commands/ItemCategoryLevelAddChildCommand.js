SIE.defineCommand('SIE.Web.Items.Items.Commands.ItemCategoryLevelAddChildCommand', {
    meta: { text: "添加子", group: "edit", iconCls: "icon-FileTree icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var childNode = view.insertNewChild();
        childNode.generateId();
        var type = view.getCurrent().getType();
        childNode.setType(type);
        view.getControl().setSelection(childNode);
    },
});