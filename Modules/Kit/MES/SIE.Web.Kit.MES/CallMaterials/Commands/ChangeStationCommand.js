SIE.defineCommand('SIE.Web.Kit.MES.CallMaterials.Commands.ChangeStationCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "转移工位", group: "edit", iconCls: "icon-ArrowLongRight icon-blue" },
    canExecute: function (view) {
        var curEntity = this.view.getCurrent();
        if (curEntity == null) { return false; }
        if (view.getSelection().length != 1) { return false; }
        var curData = curEntity.getData();
        if (curData != null && curData.Priority == 0 && curData.Status == 0) { return true; }
        return false;
    },
    execute: function (view, source) {
        var me = this;
        var editEntity = this.getEditEntity();
        SIE.AutoUI.getMeta({
            model: me.view.model,
            ignoreCommands: true,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: 'ChangeStationView',
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                detailView.setData(editEntity);

                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: "转移工位".t(),
                    width: 300,
                    height: 200,
                    items: ui,
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var indata = {};
                            var editEntity = detailView.getData().data;
                            indata.Data = Ext.encode(editEntity);
                            view.execute({
                                data: indata,
                                success: function (res) {
                                    var errMsg = res.Result;
                                    if (errMsg == '操作成功'.t()) {
                                        SIE.Msg.showInstantMessage('转移成功!'.t(), "操作提示".t(), 3);
                                        win.close();
                                        me.view.loadData();
                                    }
                                    else {
                                        SIE.Msg.showMessage(errMsg);
                                        return false;
                                    }                                        
                                },
                                error: function (res) {
                                    SIE.Msg.showMessage(res.Result);
                                    return false;
                                }
                            });
                            return false;
                        }
                    }
                });
            },
        });
    }
});