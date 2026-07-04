Ext.define('SIE.Web.EMS.Purchases.EquipmentAcceptances.EquipmentAcceptanceSnEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.EquipmentAcceptanceSnEditor',
    items: [{
        xtype: 'textfield',
        id: 'EquipmentAcceptanceSn',
        name: 'EquipmentAcceptanceSn',
        hideLabel: true,
        style: 'width:100%;border-color:#3892D4;',
        fieldStyle: 'background-color:#90EE90;height:30px;',
        allowBlank: true,
        forceSelection: true,
        listeners: {
            specialkey: function (comp, e) {
                if (e.getKey() == e.ENTER) {
                    var barcode = comp.getValue();
                    if (barcode == "") return;
                    var form = this.up('form').SIEView;
                    var fromEntity = form.getData();
                    var childView = form._children.first(function (p) { return p.model === "SIE.EMS.Purchases.EquipmentAcceptances.EquipmentAcceptanceDetail"; });
                    if (!childView) {
                        fromEntity.setMessage("界面子列表无权限，请配置".t());
                        return;
                    }
                    var scanSn = childView.getData().data.items.find(function (p) { return p.data.EquipmentCode == barcode || p.data.OriginalSn == barcode });
                    if (!scanSn) {
                        fromEntity.setMessage("扫描的编码不存在设备明细中".t());
                        comp.setValue("");
                        return;
                    }
                    scanSn.setAcceptanceStatus(fromEntity.getAcceptanceStatus());
                    scanSn.setRemark(fromEntity.getRemark());
                    comp.setValue("");                    
                    fromEntity.setRemark("");
                    fromEntity.setMessage("扫描设备编码/原厂序列号");
                }
            }
        }
    }],
});