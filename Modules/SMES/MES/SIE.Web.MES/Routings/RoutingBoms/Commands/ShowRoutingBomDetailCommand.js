SIE.defineCommand('SIE.Web.MES.Routings.RoutingBoms.Commands.ShowRoutingBomDetailCommand', {
    meta: { text: "查看明细", group: "edit", iconCls: "icon-Search icon-blue" },
    canExecute: function (view) {
        if (view.getSelection() == null || view.getSelection().length != 1) {
            return false;
        }
        if (view.getCurrent() == null) return false;
        return true;
    },
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: 'SIE.MES.Routings.RoutingBoms.RoutingBomDetail',
            isAggt: true,
            async: false,
            ignoreQuery: true,
            viewGroup: "ImportView",
            token: me.token,
            callback: function (res) {
                res.mainBlock.storeConfig.pageSize = 99999;
                var listView = SIE.AutoUI.generateAggtControl(res);
                var attId = view.getCurrent().getAttachmentId();
                SIE.Window.show({
                    title: '查看工序bom导入明细'.t(),
                    width: "60%",
                    height: "60%",
                    buttons: [],
                    items: listView.getControl(),
                });
                var filter = Ext.encode({
                    "Method": "GetBomImportRecordByAttachment", "Parameters": [attId]
                });
                listView._view.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: me.token,
                    type: 'SIE.Web.MES.Routings.RoutingBoms.DataQueryers.RoutingBomDetailDataQuery',
                });
            }
        });
    }
});