SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.AddAccountCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    getEditEntity: function () {
        var newEntity = Ext.create(this.view.model);
        newEntity.generateId(); //默认添加按钮在树形结果弹窗时有BUG，需要重写实体获取逻辑：去掉列表的增加动作，new的实例生成ID
        this.onItemCreated(newEntity);
        return newEntity;
    },
});