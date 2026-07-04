SIE.defineCommand('SIE.Web.Items.Items.Commands.AddTreeCommand', {
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
        if (view.model == "SIE.Items.ItemCategoryLevel") {
            this.mon(result, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    },
    _onEntityPropertyChanged: function (e) {
        if (e.property == "Type") {
            var entity = e.entity;
            var children = entity.childNodes;
            if (children.length > 0) {
                this._setType(children, e.value);
            }
        }
    },
    _setType: function (children, value) {
        for (var i = 0; i < children.length; i++) {
            children[i].setType(value);
            if (children[i].childNodes.length > 0) {
                this._setType(children[i].childNodes, value);
            }
        }
    }
});