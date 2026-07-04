Ext.define('SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveSnEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.SparePartReceiveSnEditor',
    items: [{
        xtype: 'textfield',
        id: 'SparePartReceiveSnId',
        name: 'SparePartReceiveSnName',
        hideLabel: false,
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
                    var childView = form._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveSn"; });
                    var detailView = form._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveDetail"; });
                    if (!childView || !detailView) {
                        fromEntity.setMessage("界面子列表无权限，请配置".t());
                        return;
                    }
                    if (fromEntity.getSparePartReceiveDetailId() === null) {
                        fromEntity.setMessage("请选择接收明细后再扫码".t());
                        comp.setValue("");
                        return;
                    }
                    var detail = detailView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getSparePartReceiveDetailId() });
                    if (fromEntity.getSparePartId() === null) {
                        fromEntity.setMessage("备件编码不能为空".t());
                        comp.setValue("");
                        return;
                    }
                    if (fromEntity.getRecivedQty() >= fromEntity.getQty()) {
                        fromEntity.setMessage("累计接收数量大于建单数量".t());
                        comp.setValue("");
                        return;
                    }
                    if (fromEntity.data.ScanEquip || (fromEntity.data.ScanEquipAndSn && fromEntity.data.FirstSn == "")) {
                        if (childView.getData().data.items.findIndex(function (p) { return p.data.Sn == barcode }) !== -1) {
                            fromEntity.setMessage("序列号编码" + barcode + "已存在，请勿重复扫描".t());
                            comp.setValue("");
                            return;
                        }
                    }
                    if (fromEntity.data.ScanSn || (fromEntity.data.ScanEquipAndSn && fromEntity.data.FirstSn != "")) {
                        if (childView.getData().data.items.findIndex(function (p) { return p.data.OriginalSn == barcode }) !== -1) {
                            fromEntity.setMessage("原厂序列号" + barcode + "已存在，请勿重复扫描".t());
                            comp.setValue("");
                            return;
                        }
                    }
                    SIE.invokeDataQuery({
                        type: "SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer",
                        method: "ReceiveSnExecute",
                        params: [barcode, fromEntity.data],
                        async: false,
                        token: form.token,
                        success: function (res) {
                            var info = res.Result;
                            if (info.Success) {
                                if (info.IsFirstSn) {
                                    fromEntity.setFirstSn(barcode);
                                } else {
                                    var childData = childView.getData();
                                    info.SnInfo.LineNo = detail.data.LineNo;
                                    info.SnInfo.PurchaseOrderNo = detail.data.PurchaseOrderNo;
                                    info.SnInfo.PurchaseOrderItemLineNo = detail.data.PurchaseOrderLine;
                                    info.SnInfo.SupplierCode = detail.data.SupplierCode;
                                    info.SnInfo.SupplierName = detail.data.SupplierName;
                                    info.SnInfo.PurchaseObjectType = detail.data.PurchaseObjectType;
                                    info.SnInfo.SparePartCode = detail.data.SparePartCode;
                                    info.SnInfo.SparePartName = detail.data.SparePartName;
                                    info.SnInfo.ControlMethod = detail.data.ControlMethod;
                                    childData.insert(0, info.SnInfo);
                                    detail.setRecivedQty(detail.getRecivedQty() + 1);
                                    fromEntity.setRecivedQty(detail.getRecivedQty());
                                    fromEntity.setFirstSn("");
                                }
                            }
                            fromEntity.setMessage(info.Message);
                            comp.setValue("");
                        }
                    });
                }
            }
        }
    }],
});