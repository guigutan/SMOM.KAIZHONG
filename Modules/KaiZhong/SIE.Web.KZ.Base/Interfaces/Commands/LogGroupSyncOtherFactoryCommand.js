SIE.defineCommand("SIE.Web.KZ.Base.Interfaces.Commands.LogGroupSyncOtherFactoryCommand", {
    meta: { text: "同步到其他工厂", group: "edit", iconCls: "icon-Sync icon-yellow" },//Delete.icon - Delete 黄色


    //自定义命令
    //是否可执行
    canExecute: function (view) {
        //var entity = view.getCurrent();
        //if (entity == null) {
        //    return false;
        //}


        this.selectedItems = view.getSelection();
        if (this.selectedItems.length > 0) {
            return true;
        }
        //for (i = 0, len = this.selectedItems.length; i < len; i++) {
        //    var item = this.selectedItems[i];
        //    if (item.data.SyncState >= 2) {
        //        return false;
        //    }
        //}
        return false;
    },

    execute: function (ListView, source) {
        var me = this;
        var selections = ListView.getSelection();
        var ids = selections.select(p => p.data.Id).join(',');
        debugger
        SIE.AutoUI.getMeta({
            model: "SIE.KZ.Base.Interfaces.ViewModels.LogGroupSyncOtherFactoryViewModel",
            //module: "SIE.Fixtures.Fixtures.Accounts.FixtureAccountModel,SIE.Fixtures",
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var detailView = SIE.AutoUI.createDetailView(mainBlock);
                var entity = new detailView._model();
                entity.setIds(ids);
                detailView._setDefaultValue(entity);
                detailView.setData(entity);

                var ui = detailView.getControl();
                var win = SIE.Window.show({
                    title: '同步到其他工厂',
                    width: 400,
                    height: 200,
                    items: ui,
                    buttons: ['确定'],
                    callback: function (btn) {
                        if (btn == "确定") {
                            SIE.Msg.wait("正在处理，请稍等...".t());
                            me.view.execute({
                                data: entity.data,
                                success: function (res) {
                                    SIE.Msg.showMessage(res.Result);
                                    me.view.reloadData();
                                }
                            });
                        }
                    }
                });

            }
        });
        //var ids = selections.map(function (item) { return item.getId(); });
        //SIE.Msg.wait("正在处理，请稍等...".t());
        //ListView.execute({
        //    data: ids,
        //    success: function (res) {
        //        SIE.Msg.showMessage(res.Result);
        //        ListView.reloadData();
        //    }
        //});
    }
});