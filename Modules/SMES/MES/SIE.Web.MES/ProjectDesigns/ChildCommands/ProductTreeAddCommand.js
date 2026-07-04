SIE.defineCommand("SIE.Web.MES.ProjectDesigns.ChildCommands.ProductTreeAddCommand", {
    extend: "SIE.cmd.Add",
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    getEditEntity: function () {
        var newEntity = Ext.create(this.view.model);
        if (this.view.isListView) {
            newEntity = this.createNewItem();
        }
        newEntity.setTreeLevel(1);
        this.onItemCreated(newEntity);
        return newEntity;
    },
    execute: function (view, source) {
        var parent = view.getParent().getData();

        // 添加
        if (view.getCurrent() == null) {
            var root = view._getTreeRoot();
            if (root.isLeaf()) { root.set('leaf', false); }
            var rootNode = view._createTreeNode(root);
            result = root.insertChild(0, rootNode);
            result.generateId();
            result.set('leaf', false);
            result.set('TreePId', null);
            result.set("ProjectDesignId", parent.getId());
            result.set("TreeLevel", 1);
        }
        // 添加子
        else {
            var childNode = view.insertNewChild();
            childNode.setProjectDesignId(parent.getId());
            childNode.setTreeLevel(view.getCurrent().getTreeLevel() + 1);
            childNode.generateId();
            view.getControl().setSelection(childNode);
        }
    },
})