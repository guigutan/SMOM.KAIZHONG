SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.AddTurnOverRuleCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    getEditEntity: function () {
        var newEntity = Ext.create(this.view.model);
        if (this.view.isListView) {
            newEntity = this.createNewItem();
        }
        newEntity.phantom = false; //触发只读一下，自动显示编码
        this.onItemCreated(newEntity);
        return newEntity;
    },
    onItemCreated: function (entity) {
        if (entity) {
            var model = entity.data;
            var me = this;
            this.view.execute({
                data: model,
                isSubmmit: false,
                success: function (res) {
                    var data = res.Result;
                    entity.setCode(data.Code);
                    entity.phantom = true;
                    entity.setState(SIE.Domain.State.Enable.value);
                }
            }, me.view);
        }
    },
});