SIE.defineCommand('SIE.Web.Packages.Boxs.Commands.AddCpaacityCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'ItemId',
            targetClassName: 'SIE.Items.Item'
        },
    },
    meta: {
        text: "选择".t(),
        group: "edit",
        iconCls: "icon-PlaylistCheck icon-blue"
    },
    /**
     * override 保存方法
     * @param {} win 
     * @returns {} 
     */
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems;
        if (selections && selections.items.length > 0) {
            var operationDatas = [];
            SIE.each(selections.items, function (item) {
                var itemId = item.getId();
                //获得周转箱的默认容量
                var defaultCap = me.view.getParent().getCurrent().getCapacity();
                if (me._sourceViewSelectItems.indexOf(itemId) === -1) {
                    var productCapacity = {
                        TurnoverBoxId: me._sourceId,
                        ItemId: itemId
                    };
                    productCapacity.Capacity = defaultCap;
                    operationDatas.push(productCapacity);
                }

            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close(); //关闭模态窗口
                    me._ownerView.loadChildData(true); //重载视图数据
                }
            },
                me._ownerView);
        } else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
});