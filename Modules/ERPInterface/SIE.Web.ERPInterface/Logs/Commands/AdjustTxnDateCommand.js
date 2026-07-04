SIE.defineCommand('SIE.Web.ERPInterface.Logs.Commands.AdjustTxnDateCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "调整交易日期", group: "business", iconCls: "icon-NetworkNormal icon-green" },

    canExecute: function (view) {
        var transList = view.getSelection();
        if (transList == null || transList.length == 0) {
            return false;
        }

        var trans = view.getCurrent();
        if (trans == null) {
            return false;
        }

        for (var i = 0; i < transList.length; i++) {
            if (transList[i].data.State != trans.data.State) return false;
        }
        //2:失败状态
        if (trans.data.State != 2 || view.getData().isDirty()) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        SIE.AutoUI.getMeta({
            model: 'SIE.ERPInterface.Common.Logs.UploadTransaction',
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                entity.setTransactionDate(new Date());
                detailView.setData(entity);
                var ui = detailView.getControl();

                var win = SIE.Window.show({
                    title: "调整交易日期".t(),
                    width: 500,
                    height: 200,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {

                            if (entity.data.TransactionDate == null)
                                SIE.Msg.showMessage("交易日期不能为空".t());
                            else
                                SIE.Msg.askQuestion(Ext.String.format('确定[调整]选中事务的交易日期?'.t()), function () {
                                    //调用后台逻辑
                                    view.execute({
                                        data: entity.data.TransactionDate,
                                        withIds: true,
                                        selectIds: view.getSelectionIds(),
                                        success: function (res) { 
                                            //成功后关闭窗口，刷新数据
                                            win.close();
                                            view.reloadData();
                                        }
                                    });
                                });

                            //返回false，确定后不关闭窗口
                            return false;
                        }
                    }
                });
            },
        });
    }
});