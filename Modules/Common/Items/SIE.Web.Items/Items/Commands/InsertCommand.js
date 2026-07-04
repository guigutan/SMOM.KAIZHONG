SIE.defineCommand('SIE.Web.Items.Items.Commands.InsertCommand', {
    meta: { text: "插入", group: "edit", iconCls: "iconfont icon-Import icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var isListView = view.isListView;
        if (isListView) {
            return view.canAddItem();
        }
        else
            return this.callParent();
    },
    execute: function (view, source) {
        var me = this;
        var entity = view.getCurrent();
        var parentNode = entity.parentNode;
        var newNode = {
            leaf: true,
            TreePId: entity.data.TreePId,
            Type: entity.data.Type
        };
        var result = parentNode.insertBefore(newNode, entity);
        result.generateId();
        view.getControl().setSelection(result);
    },
});