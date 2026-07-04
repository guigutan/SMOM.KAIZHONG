SIE.defineCommand('SIE.Web.Tech.Routings.Commands.SelectItemCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'ItemId', targetClassName: 'SIE.Items.Item' },
    },
    meta: { text: "物料添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;

        var indata = {};
        var addCount = 0;
        var selections = this._targetView.getSelection();
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var itemId = item.getId();
                var code = item.getCode();
                var name = item.getName();
                var isHas = false;
                Ext.Array.each(me.ownerCtrl.store.getData().items, function (record) {
                    if (record.data.ItemId === itemId) {
                        isHas = true;
                        return;
                    }
                });
                if (!isHas) {
                    addCount++;
                    var bomItem = { ItemId: itemId, Code: code, Name: name };
                    operationDatas.push(bomItem);

                }
            });
            me.ownerCtrl.store.add(operationDatas);
            var bomData = [];
            Ext.Array.forEach(me.ownerCtrl.store.getData().items, function (record) {
                bomData.push(record.data);
            });
            me.ownerCtrl.CurNode.designerData.Boms = bomData;
            SIE.Msg.showInstantMessage(Ext.String.format('已选择{0}条物料，其中过滤已存在的物料后添加{1}条'.t(), selections.length, addCount), '提示'.t(), 3);
            win.close();  //关闭模态窗口
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
});