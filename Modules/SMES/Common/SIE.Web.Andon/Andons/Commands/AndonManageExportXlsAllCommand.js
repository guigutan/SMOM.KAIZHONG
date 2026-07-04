SIE.defineCommand('SIE.Web.Andon.Andons.Commands.AndonManageExportXlsAllCommand', {
    meta: { text: "导出全部", splitTo: "导出", group: "business", iconCls: "icon-ExportData icon-blue" },
    myview: {}, // 当前视图对象
    fieldNames: [],//导出的数据
    pageSize: 500000,
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {

        var mask = SIE.ExportExcelHelper.showMask(Ext.getBody().component, '数据准备中...');

        myview = view;
        var me = this;

        var store = SIE.AutoUI.viewFactory._createStore(myview.getMeta());
        Ext.Object.merge(store.getFilters(), myview.getControl().store.getFilters());
        store.setPageSize(me.pageSize);
        var proxy = store.proxy;
        proxy.setExtraParams({});
        proxy.setExtraParam("token", myview.getToken());
        if (!(proxy.extraParams && proxy.extraParams.action)) proxy.setExtraParam("action", proxy.action || "entity");
        if (!(proxy.extraParams && proxy.extraParams.type)) proxy.setExtraParam("type", myview.model);
        proxy.setExtraParam("viewGroup", myview.viewGroup);
        proxy.setExtraParam("url", proxy.url);
        var parent = myview._parent;

        if (parent && parent._current) {
            var pName = myview._childProperty;
            if (!pName) {
                proxy.setExtraParam("action", "delegate");
                proxy.setExtraParam("parent", parent.model);
                proxy.setExtraParam("filter", Ext.encode([
                    {
                        property: SIE._KeyPropertyName,
                        value: parent._current.data[SIE._KeyPropertyName],
                        exactMatch: true
                    }]));
            }
        }
        store.load({
            scope: myview,
            callback: function (records, operation, success) {
                mask.hide();
                store._loaded = success;
                if (records.length === 0) {
                    SIE.Msg.showInstantMessage('没有需要导出的数据！'.t());
                    return false;
                }
                if (records.last().getAndonManageCode() == '合计') {
                    records.last().setUpdateDate(null);
                    records.last().setCreateDate(null);
                    records.last().setFaultTime(null);
                    records.last().setTriggerTime(null);
                }
                SIE.Signature.otherCheckIsNeedToSign("导出全部", view, function () {
                    SIE.ExportExcelHelper.exportXls(myview.gridConfig, records, myview.label.t());
                });
            }
        });
    },


});