SIE.defineCommand('SIE.Web.CSM.Suppliers.Commands.SelectUserCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'UserId', targetClassName: 'SIE.Rbac.Users.User' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    _flag: true,
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems;
        if (me._flag) {
            if (selections && selections.items.length > 0) {
                me._flag = false;
                var operationDatas = [];
                SIE.each(selections.items, function (item) {
                    var userId = item.getId();
                    if (me._sourceViewSelectItems.indexOf(userId) === -1) {
                        var supplierUser = { SupplierId: me._sourceId, UserId: userId };
                        operationDatas.push(supplierUser);
                    }
                });
                indata = operationDatas;
                me._targetView.execute({
                    data: indata,
                    success: function (res) {
                        me._flag = true;
                        win.close();  //关闭模态窗口
                        me._ownerView.loadChildData(true); //重载视图数据                    
                    },
                    error: function () {
                        me._flag = true;
                    }
                }, me._ownerView);
            }
            else {
                SIE.Msg.showWarning('没有可提交的数据'.t());
            }
        }
    }
    // end 
});