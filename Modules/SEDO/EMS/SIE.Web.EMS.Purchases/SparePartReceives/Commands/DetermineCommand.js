SIE.defineCommand('SIE.Web.EMS.Purchases.SparePartReceives.Commands.DetermineCommand', {
    meta: { text: "确定", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var detailChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveDetail"; });
        var lotChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveLot"; });
        var snChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveSn"; });
        if (!detailChildView || !lotChildView || !snChildView) {
            SIE.Msg.showError("界面子列表无权限，请配置".t());
            return;
        }
        var fromEntity = view.getCurrent();
        if (fromEntity.getSparePartReceiveDetailId() === null) {
            SIE.Msg.showError("请选择接收明细".t());
            return;
        }
        if (fromEntity.getSparePartId() === null) {
            fromEntity.setMessage("备件编码不能为空".t());
            return;
        }
        me.itemCodeRecived(fromEntity, detailChildView);
        me.lotRecived(fromEntity, view, detailChildView, lotChildView);
        me.snRecived(fromEntity, view, detailChildView, snChildView);
    },
    itemCodeRecived: function (fromEntity, detailChildView) {
        if (fromEntity.getControlMethod() === 10) {
            if (fromEntity.getCurrentQty() <= 0) {
                SIE.Msg.showError("请输入接收数量".t());
                return;
            }
            var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getSparePartReceiveDetailId() });
            var qty = detail.getRecivedQty() + fromEntity.getCurrentQty();
            if (qty > fromEntity.getQty()) {
                SIE.Msg.showError("【已接收数量】+【本次接收数量】不能大于【接收数量】".t());
                return;
            }
            detail.setRecivedQty(qty);
            fromEntity.setRecivedQty(qty);
        }
    },
    lotRecived: function (fromEntity, view, detailChildView, lotChildView) {
        if (fromEntity.getControlMethod() === 20) {
            if (fromEntity.getLotCount() <= 0 || fromEntity.getLotQty() <= 0) {
                fromEntity.setMessage("请填写批次个数和批次数量".t());
                return;
            }
            var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getSparePartReceiveDetailId() });
            var qty = detail.getRecivedQty() + (fromEntity.getLotCount() * fromEntity.getLotQty());
            if (qty > fromEntity.getQty()) {
                fromEntity.setMessage("【已接收数量】+【本次接收数量】不能大于【接收数量】".t());
                return;
            }
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer",
                method: "LotDetermine",
                params: [fromEntity.data],
                async: false,
                token: view.token,
                success: function (res) {
                    var childData = lotChildView.getData();
                    SIE.each(res.Result, function (model) {
                        model.LineNo = detail.data.LineNo;
                        model.PurchaseOrderNo = detail.data.PurchaseOrderNo;
                        model.PurchaseOrderItemLineNo = detail.data.PurchaseOrderLine;
                        model.SupplierCode = detail.data.SupplierCode;
                        model.SupplierName = detail.data.SupplierName;
                        model.PurchaseObjectType = detail.data.PurchaseObjectType;
                        model.SparePartCode = detail.data.SparePartCode;
                        model.SparePartName = detail.data.SparePartName;
                        model.ControlMethod = detail.data.ControlMethod;
                    });
                    childData.insert(0, res.Result);
                    detail.setRecivedQty(qty);
                    fromEntity.setRecivedQty(qty);
                }
            });
        }
    },
    snRecived: function (fromEntity, view, detailChildView, snChildView) {
        if (fromEntity.getControlMethod() === 30) {
            if (fromEntity.getCurrentQty() <= 0) {
                fromEntity.setMessage("请填写本次接收数量".t());
                return;
            }
            var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getSparePartReceiveDetailId() });
            var qty = detail.getRecivedQty();
            if (fromEntity.getReceiveType() === 50) {
                if (fromEntity.getStoreSummaryDetailId() === null) {
                    fromEntity.setMessage("请选择返厂的序列号编码".t());
                    return;
                }
                if (snChildView.getData().data.items.findIndex(function (p) { return p.data.Sn == fromEntity.getStoreSummaryDetailId_Display() }) !== -1) {
                    fromEntity.setMessage("序列号编码" + fromEntity.getStoreSummaryDetailId_Display() + "已接收，请勿重复接收".t());
                    return;
                }
                qty++;
            } else {
                qty = qty + fromEntity.getCurrentQty();
            }
            if (qty > fromEntity.getQty()) {
                fromEntity.setMessage("【已接收数量】+【本次接收数量】不能大于【接收数量】".t());
                return;
            }
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer",
                method: "SnDetermine",
                params: [fromEntity.data],
                async: false,
                token: view.token,
                success: function (res) {
                    var childData = snChildView.getData();
                    SIE.each(res.Result, function (model) {
                        model.LineNo = detail.data.LineNo;
                        model.PurchaseOrderNo = detail.data.PurchaseOrderNo;
                        model.PurchaseOrderItemLineNo = detail.data.PurchaseOrderLine;
                        model.SupplierCode = detail.data.SupplierCode;
                        model.SupplierName = detail.data.SupplierName;
                        model.PurchaseObjectType = detail.data.PurchaseObjectType;
                        model.SparePartCode = detail.data.SparePartCode;
                        model.ControlMethod = detail.data.ControlMethod;
                        model.SparePartName = detail.data.SparePartName;
                    });
                    childData.insert(0, res.Result);
                    detail.setRecivedQty(qty);
                    fromEntity.setRecivedQty(qty);
                }
            });
        }
    }
});