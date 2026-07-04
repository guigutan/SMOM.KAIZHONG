SIE.defineCommand('SIE.Web.EMS.Purchases.FixtureReceives.Commands.ScreenCommand', {
    meta: { text: "筛选", group: "edit", iconCls: "icon-Search icon-blue" },
    canExecute: function (view) {
        if (view._parent == null)
            return false;
        if (view._parent.getCurrent() == null)
            return false;
        return true;
    },
    execute: function (view, source) {
        var receiveId = view._parent.getCurrent().getId();
        view.loadData();
        SIE.AutoUI.getMeta({
            model: "SIE.EMS.Purchases.FixtureReceives.ReceiveSnScreenViewModel",
            ignoreCommands: false,
            isDetail: true,
            ignoreQuery: true,
            viewGroup: "DetailsView",
            callback: function (res) {
                var detailView = SIE.AutoUI.createDetailView(res);
                var entity = new detailView._model();
                entity.setFixtureReceiveId(receiveId);
                detailView.setData(entity);
                var win = SIE.Window.show({
                    title: "序列号筛选".t(),
                    width: 480,
                    height: 200,
                    items: detailView.getControl(),
                    id: "ScreenCommand001",
                    callback: function (btn) {
                        if (btn == "确定".t()) {
                            var reprintInfo = detailView.getData().data;
                            if (reprintInfo.FixtureReceiveDetailId <= 0) {
                                SIE.Msg.showError("请选择数据".t());
                                return false;
                            }
                            var store = view.getControl().getStore();
                            var filterData = view.getData().data.items.where(p => p.getFixtureReceiveDetailId() == reprintInfo.FixtureReceiveDetailId);
                            store.setData(filterData);
                            win.close();
                            return false;
                        }
                    }
                });
            }
        });
    }
});