SIE.defineCommand('SIE.Web.Fixtures.Abnormals.Commands.AddAbnormalCommand', {
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
                success: function (res) {
                    var code = res.Result;
                    entity.setCode(code);
                    entity.phantom = true;
                },
                error: function (res) {
                    SIE.Msg.showMessage(res.Message);
                }
            }, me.view);
        }
    }
});