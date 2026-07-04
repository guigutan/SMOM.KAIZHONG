SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentReceives.Commands.SaveReceiveScanCommand', {
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var childView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.EquipmentReceives.ReceiveScanSnViewModel"; });
        if (!childView) {
            SIE.Msg.showError("界面子列表无权限，请配置".t());
            return;
        }
        if (childView.getData().data.items.length === 0) {
            SIE.Msg.showError("序列号子表数据为空".t());
            return;
        }
        var fromEntity = view.getCurrent();
        var snList = [];
        SIE.each(childView.getData().data.items, function (model) {
            snList.push(model.data);
        });
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Purchases.EquipmentReceives.EquipmentReceiveDataQueryer",
            method: "SaveReceiveScan",
            params: [fromEntity.data, snList],
            async: false,
            token: view.token,
            success: function (res) {
                SIE.Msg.showInstantMessage('保存成功'.t());
                CRT.Workbench.closeCurrentTab();
                CRT.Event.fire("SIE.EMS.Purchases.EquipmentReceives.EquipmentReceive_refresh");
            }
        });
    }
});