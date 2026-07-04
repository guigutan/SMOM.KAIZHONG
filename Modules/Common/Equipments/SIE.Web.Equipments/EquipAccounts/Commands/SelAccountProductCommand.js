SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.SelAccountProductCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'ProductId', targetClassName: 'SIE.Items.Item' }
    },
    canExecute: function (view) {
        var entity = view.getParent().getCurrent();
        if (entity != null && entity.data) {
            return entity.data.CreateBy !== null;
        }
        return false;
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = this._targetView.getSelection();
        if (selections != null && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var productId = item.getId();
                if (me._sourceViewSelectItems.indexOf(productId) === -1) {
                    var equipProduct = { EquipAccountId: me._sourceId, ProductId: productId };
                    operationDatas.push(equipProduct);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close();  //关闭模态窗口
                    me._ownerView.loadChildData(true); //重载视图数据
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
});