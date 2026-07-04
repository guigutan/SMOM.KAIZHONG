SIE.defineCommand('SIE.Web.Barcodes.WipBatchs.GenerateCommand', {
    meta: { text: "批次生成", group: "edit", iconCls: "icon-TextRelease icon-blue" },
    canExecute: function (view) {
        var entity = view.getData();
        return entity != null && entity.data.GenerateingQty > 0 && entity.data.PlanQty > 0;
    },
    execute: function (view, source) {
        var me = view;
        var entity = view.getData();
        if (!view.validateData(view)) {
            SIE.Msg.showMessage("输入数据不正确，请重新输入".t());
            return;
        }
        if (entity) {
            Ext.MessageBox.show({
                msg: '正在生成'.t(),
                progressText: '...',
                width: 300,
                modal: true,
                wait: {
                    interval: 200
                }
            });
            me.timer = Ext.defer(function () {
                me.timer = null;
                Ext.MessageBox.hide();
            }, 15000);
            view.execute({
                data: entity.data,
                success: function (res) {
                    var data = res.Result;
                    if (data.errorMsg == "") {
                        entity.setGeneratedQty(data.GeneratedQty);
                        entity.setNotGenerateQty(data.NotGenerateQty);
                        entity.setGenerateingQty(data.GenerateingQty);
                        SIE.Msg.showMessage('生成成功！'.t());
                        view.mainView.reloadData();
                    }
                    else {
                        SIE.Msg.showMessage('生成失败'.t()+"!" + data.errorMsg);
                    }
                }
            });
        }
    }
});
