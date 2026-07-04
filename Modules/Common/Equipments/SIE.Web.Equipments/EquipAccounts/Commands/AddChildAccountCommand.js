SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.AddChildAccountCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添子设备", group: "edit", iconCls: "iconfont icon-FileTree icon-green" },
    getEditEntity: function () {
        var newEntity = Ext.create(this.view.model);
        newEntity.generateId(); //默认添加按钮在树形结果弹窗时有BUG，需要重写实体获取逻辑：去掉列表的增加动作，new的实例生成ID
        var parent = this.view.getCurrent();//获取选择数据，作为父
        newEntity.setTreePId(parent.data.Id);

        this.onItemCreated(newEntity);
        return newEntity;
    },
    showView: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            CRT.Workbench.addPage({
                entityType: me.view.model,
                title: me.getEditViewTitle(entity),
                recordId: entity.getId(),
                params: {
                    TreePId: model.TreePId,
                    SourceEquipId: null
                },
                isDetail: true
            });
        }
    },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length !== 1) {
            return false;
        }
        var p = view.getCurrent();
        if (p == null) { return false; }
        if (p.isNew()) { return false; }
        return true;
    }
});