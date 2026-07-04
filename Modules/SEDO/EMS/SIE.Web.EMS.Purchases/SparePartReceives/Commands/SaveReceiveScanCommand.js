SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartReceives.Commands.SaveReceiveScanCommand', {
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var detailChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveDetail"; });
        var lotChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveLot"; });
        var snChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveSn"; });
        if (!detailChildView || !lotChildView || !snChildView) {
            SIE.Msg.showError("界面子列表无权限，请配置".t());
            return;
        }
        var fromEntity = view.getCurrent();
        var detailList = [];
        SIE.each(detailChildView.getData().data.items, function (model) {
            detailList.push(model.data);
        });
        var lotList = [];
        SIE.each(lotChildView.getData().data.items, function (model) {
            model.data.CreateDate = null;
            model.data.UpdateDate = null;
            lotList.push(model.data);
        });
        var snList = [];
        SIE.each(snChildView.getData().data.items, function (model) {
            model.data.CreateDate = null;
            model.data.UpdateDate = null;
            snList.push(model.data);
        });
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer",
            method: "SaveReceiveScan",
            params: [fromEntity.data, detailList, lotList, snList],
            async: false,
            token: view.token,
            success: function (res) {
                SIE.Msg.showInstantMessage('保存成功！', '提示', 3);
                window.setTimeout(function () {
                    CRT.Event.fire("SIE.EMS.Purchases.SparePartReceives.SparePartReceive_refresh");
                    CRT.Workbench.closeCurrentTab();
                }, 3);
            }
        });
    }
});