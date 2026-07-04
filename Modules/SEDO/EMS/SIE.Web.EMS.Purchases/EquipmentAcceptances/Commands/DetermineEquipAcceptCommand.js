SIE.defineCommand('SIE.Web.EMS.Purchases.EquipmentAcceptances.Commands.DetermineEquipAcceptCommand', {
    meta: { text: "确定", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var comp = Ext.getCmp('EquipmentAcceptanceSn');
        var entity = view.getCurrent();
        var barcode = comp.getValue();
        if (barcode == "") {
            SIE.Msg.showError("扫描框不能为空".t());
            return;
        }
        if (entity.getAcceptanceStatus() == null) {
            SIE.Msg.showError("验收状态不能为空".t());
            return;
        }
        var childView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.EquipmentAcceptances.EquipmentAcceptanceDetail"; });
        if (!childView) {
            SIE.Msg.showError("界面子列表无权限，请配置".t());
            return;
        }
        var scanSn = childView.getData().data.items.find(function (p) { return p.data.EquipmentCode == barcode || p.data.OriginalSn == barcode });
        if (!scanSn) {
            SIE.Msg.showError("扫描的编码不存在设备明细中".t());
            return;
        }
        view.execute({
            data: entity.data.Id,
            success: function (res) {
                scanSn.setAcceptanceStatus(entity.getAcceptanceStatus());
                scanSn.setRemark(entity.getRemark());
                comp.setValue("");                
                entity.setRemark("");
                entity.setMessage("扫描设备编码/原厂序列号");
            }
        });
    }
});