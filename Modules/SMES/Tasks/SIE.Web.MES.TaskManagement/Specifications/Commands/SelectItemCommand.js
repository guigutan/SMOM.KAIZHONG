SIE.defineCommand('SIE.Web.MES.TaskManagement.Specifications.Commands.SelectItemCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'ProductId', targetClassName: 'SIE.Items.Item' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        /* post数据结构*/
        var indata = {};
        /* post数据结构*/
        var selections = this._targetView.getSelection();
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var productId = item.getId();
                if (me._sourceViewSelectItems.indexOf(productId) === -1) {
                    var productSpecification = { ProductId: productId };
                    operationDatas.push(productSpecification);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close();  //关闭模态窗口
                    me._ownerView.loadData(true); //重载视图数据
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
    // end 
});