SIE.defineCommand('SIE.Web.EMS.Equipments.Boms.Commands.AddRootSparePartCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },
    _curUserId: null, //操作的用户id
    _ownerViewSelecteds: null, //主人视图已选择项
    _operationView: null,//操作弹窗视图
    _selects: null,//弹窗视图选择的项
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
        if (parent.isLeaf()) { parent.set('leaf', false); }
        var rootNode = view._createTreeNode(parent);
        rootNode.EquipBomId = view.getParent().getCurrent().data.Id;
        result = parent.insertChild(0, rootNode);
        result.generateId();
        result.set('leaf', false);
        result.set('TreePId', null);
        if (!parent.isExpanded()) { parent.expand(); }
        this.fireEvent('itemCreated', { item: rootNode });
        view.getControl().setSelection(result); 
        view.getControl().scrollTo(0, 0);
    }
});

