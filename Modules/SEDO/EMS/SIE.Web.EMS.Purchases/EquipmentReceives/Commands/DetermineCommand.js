SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentReceives.Commands.DetermineCommand', {
    meta: { text: "接收", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var childView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.EquipmentReceives.ReceiveScanSnViewModel"; });
        if (!childView) {
            SIE.Msg.showError("界面子列表无权限，请配置".t());
            return;
        }
        var fromEntity = view.getCurrent();
        if (fromEntity.getEquipmentReceiveDetailId() === null) {
            SIE.Msg.showError("请选择接收明细".t());
            return;
        }
        if (fromEntity.getEquipModelId() === null) {
            SIE.Msg.showError("设备型号不能为空".t());
            return;
        }
        if (fromEntity.getCurrentQty() === null) {
            SIE.Msg.showError("请输入接收数量".t());
            return;
        }
        if (fromEntity.data.RecivedQty + fromEntity.data.CurrentQty > fromEntity.data.Qty) {
            SIE.Msg.showError("已接收数量+本次接收数量不能大于接收数量".t());
            return;
        }
        if (fromEntity.getReceiveType() === 50) {
            if (fromEntity.getEquipAccountId() === null) {
                SIE.Msg.showError("请选择返厂的设备编码".t());
                return;
            }
            if (childView.getData().data.items.findIndex(function (p) { return p.data.EquipmentCode == fromEntity.getEquipAccountId_Display() }) !== -1) {
                SIE.Msg.showError("设备编码" + fromEntity.getEquipAccountId_Display() + "已接收，请勿重复接收".t());
                return;
            }
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Purchases.EquipmentReceives.EquipmentReceiveDataQueryer",
                method: "Determine",
                params: [fromEntity.data],
                async: false,
                token: view.token,
                success: function (res) {
                    var childData = childView.getData();
                    childData.add(res.Result.SnInfo);
                    fromEntity.setRecivedQty(fromEntity.data.RecivedQty + 1);
                }
            });
        } else {
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Purchases.EquipmentReceives.EquipmentReceiveDataQueryer",
                method: "DetermineOnQty",
                params: [fromEntity.data],
                async: false,
                token: view.token,
                success: function (res) {
                    var childData = childView.getData();
                    childData.add(res.Result);
                    fromEntity.setRecivedQty(fromEntity.data.RecivedQty + fromEntity.data.CurrentQty);
                }
            });
        }
    }
});