SIE.defineCommand('SIE.Web.MES.TaskManagement.FeedingRecords.Commands.ScrapWeighingRecordEditCommand', {
    meta: { text: "修改", group: "edit", iconCls: "icon-EditEntity icon-red" },
    canExecute: function (view) {
        if (view == null)
            return false;
        if (view.getSelection() == null)
            return false;
        if (view.getSelection().length > 0)
            return true;
        return false;
    },
    execute: function (view, source) {

        var me = this;
        var list = view.getSelection().select(p => p.getData());
        var ids = view.getSelectionIds();
        //SIE.invokeDataQuery({
        //    type: "SIE.Web.MES.TaskManagement.FeedingRecords.DeductionRecordDataQueryer",
        //    method: "GetDeductionRecordsByIds",
        //    params: [ids],
        //    async: false,
        //    token: view.token,
        //    callback: function (res) {
        //        if (res.Success) {
        //            if (res.Result) {
        //                list = res.Result.data.items;
        //            }
        //        }
        //    }
        //});

        if (list == null || list.length < 1)
            SIE.Msg.showError("没有数据".t());

        SIE.AutoUI.getMeta({
            model: 'SIE.MES.TaskManagement.FeedingRecords.ScrapWeighingRecord',
            viewGroup: 'EditView',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: true,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var listView = SIE.AutoUI.createListView(mainBlock);

                listView.getData().add(list);

                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: '修改余料称重记录'.t(),
                    items: ui,
                    width: 800,
                    height: 500,
                    callback: function (btn) {
                        if (btn == "确定".t()) {

                            var selection = listView.getData().data.items.where(p => p.dirty == true).select(p => p.data);
                            me.view.execute({
                                data: selection,
                                success: function (res) {
                                    win.close();
                                    me.view.reloadData();
                                    SIE.Msg.showInstantMessage('修改成功'.L10N());
                                }
                            });
                        }
                    },
                });

            }
        });
    }
});