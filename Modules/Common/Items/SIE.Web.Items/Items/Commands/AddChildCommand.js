SIE.defineCommand('SIE.Web.Items.Items.Commands.AddChildCommand', {
    meta: { text: "添加子", group: "edit", iconCls: "icon-FileTree icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        var childNode = view.insertNewChild();
        childNode.data.ItemType = view.getSelection()[0].data.ItemType;
        childNode.generateId();
        view.getControl().setSelection(childNode);
    },
});