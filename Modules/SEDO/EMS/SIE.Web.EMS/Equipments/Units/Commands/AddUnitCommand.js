SIE.defineCommand('SIE.Web.EMS.Equipments.Units.Commands.AddUnitCommand', {
    meta: { text: "添加".t(), group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        var isListView = view.isListView;
        if (isListView) {
            return view.canAddItem();
        }
        else
            return this.callParent();
    },
    execute: function (view, source) {
        var parent = view._getTreeRoot();
        var rootNode = view._createTreeNode(parent);
        result = parent.insertChild(0, rootNode);
        result.generateId();
        result.setTreePId(null);
        view.getControl().setSelection(result);
    },
});