SIE.defineCommand('SIE.Web.ProductIntfc.FirstInsps.Commands.FirstInspRuleCommand', {
    meta: { text: "报检规则", group: "edit", iconCls: "icon-Module icon-blue" }, 
    execute: function (view, source) {
        var me = view;
        SIE.AutoUI.getMeta({
            model: "SIE.ProductIntfc.FirstInsps.FirstInspRule",
            module: "SIE.ProductIntfc.FirstInsps.FirstInsp,SIE.ProductIntfc",
            ignoreCommands: false,
            isDetail: false,
            ignoreQuery: true,
            callback: function (res) {
                var mainBolck;
                if (res.mainBolck)
                    mainBolck = res.mainBolck;
                else
                    mainBolck = res;
                var listView = SIE.AutoUI.createListView(mainBolck);
                var ui = listView.getControl();
                var win = SIE.Window.show({
                    title: "报检规则".t(),
                    width: 550,
                    height: 350,
                    buttons: [
                        { xtype: "button", text: "确定".t(), hidden: true },
                        { xtype: "button", text: "取消".t(), hidden: true }
                    ],
                    items: ui,
                    id: "FirstInspRule001",
                });
                
                var filter = {
                    Method: 'GetFirstInspRules',
                    Parameters: []
                };
                filter = Ext.encode(filter);
                listView.loadData({
                    filter: filter,
                    action: 'queryer',
                    token: me.token,
                    type: 'SIE.Web.ProductIntfc.FirstInsps.DataQuery.FirstInspsDataQuery',
                });
            }
        });
    },
});