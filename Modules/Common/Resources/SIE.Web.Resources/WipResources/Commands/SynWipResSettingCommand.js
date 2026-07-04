SIE.defineCommand('SIE.Web.Resources.WipResources.Commands.SynWipResSettingCommand', {
    meta: { text: "资源同步设置", group: "business", iconCls: "icon-ListConfig icon-blue" },
    execute: function (view, source) {
        var me = this;
        var commit = function (settings, win) {
            var me = this;
            var datas = [];
            settings.items.forEach(function (item) { datas.push(item.data); });
            view.execute({
                data: datas,
                success: function (res) {
                    var errMsg = res.Result;
                    if (errMsg == true) {
                        win.close();
                    }
                }
            });
        };
        me.showSetingDialog(commit);
    },
    showSetingDialog: function (commit) {
        SIE.AutoUI.getMeta({
            model: 'SIE.Resources.WipResources.SynWipResSetting',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: true,
            callback: function (res) {
                var listView = SIE.AutoUI.createListView(res);
                var filter = {
                    Method: 'GetSynWipResSettings',
                    Parameters: []
                };
                var win = SIE.Window.show({
                    title: '资源同步设置'.L10N(),
                    width: 800,
                    height: 450,
                    items: listView.getControl(),
                    callback: function (btn) {
                        if (btn === "确定".t()) {
                            commit(listView.getData().data, win);
                            view.loadData();
                            return false;
                        }
                    }
                });
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    type: 'SIE.Web.Resources.WipResources.WipResourceDataQueryer'
                });
            }
        });
    }
});