Ext.define('SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveLotEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.SparePartReceiveLotEditor',
    items: [{
        xtype: 'textfield',
        id: 'SparePartReceiveLotSnId',
        name: 'SparePartReceiveLotSnName',
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
                    var childView = form._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveLot"; });
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
                    var lotQty = fromEntity.getLotQty();
                    if (lotQty <= 0) {
                        fromEntity.setMessage("请输入批次数量后再扫描".t());
                        comp.setValue("");
                        return;
                    }
                    if (lotQty > fromEntity.getQty() - fromEntity.getRecivedQty()) {
                        fromEntity.setMessage("累计接收数量大于建单数量".t());
                        comp.setValue("");
                        return;
                    }
                    if (childView.getData().data.items.findIndex(function (p) { return p.data.LotNo == barcode }) !== -1) {
                        fromEntity.setMessage("批次号" + barcode + "已存在，请勿重复扫描".t());
                        comp.setValue("");
                        return;
                    }
                    SIE.invokeDataQuery({
                        type: "SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer",
                        method: "ReceiveLotExecute",
                        params: [barcode, fromEntity.data],
                        async: false,
                        token: form.token,
                        success: function (res) {
                            var info = res.Result;
                            if (info.Success) {
                                var childData = childView.getData();
                                info.LotInfo.LineNo = detail.data.LineNo;
                                info.LotInfo.PurchaseOrderNo = detail.data.PurchaseOrderNo;
                                info.LotInfo.PurchaseOrderItemLineNo = detail.data.PurchaseOrderLine;
                                info.LotInfo.SupplierCode = detail.data.SupplierCode;
                                info.LotInfo.SupplierName = detail.data.SupplierName;
                                info.LotInfo.PurchaseObjectType = detail.data.PurchaseObjectType;
                                info.LotInfo.SparePartCode = detail.data.SparePartCode;
                                info.LotInfo.SparePartName = detail.data.SparePartName;
                                info.LotInfo.ControlMethod = detail.data.ControlMethod;
                                childData.insert(0, info.LotInfo);
                                detail.setRecivedQty(detail.getRecivedQty() + lotQty);
                                fromEntity.setRecivedQty(detail.getRecivedQty());
                                if (fromEntity.getFixedQty() === false) {
                                    fromEntity.setLotQty(null);
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