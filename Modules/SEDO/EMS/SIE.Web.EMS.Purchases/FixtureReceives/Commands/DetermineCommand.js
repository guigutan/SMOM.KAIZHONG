SIE.defineCommand('SIE.Web.EMS.Purchases.FixtureReceives.Commands.DetermineCommand', {
    meta: { text: "确定", group: "edit", iconCls: "icon-Check icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var detailChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveDetail"; });
        var snChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveSn"; });
        if (!detailChildView || !snChildView) {
            SIE.Msg.showError("界面子列表无权限，请配置".t());
            return;
        }
        var fromEntity = view.getCurrent();
        if (fromEntity.getFixtureReceiveDetailId() === null) {
            SIE.Msg.showError("请选择接收明细".t());
            return;
        }
        me.itemCodeRecived(fromEntity, detailChildView);
        me.snRecived(fromEntity, view, detailChildView, snChildView);
    },
    itemCodeRecived: function (fromEntity, detailChildView) {
        if (fromEntity.getManageMode() === 10) {
            if (fromEntity.getCurrentQty() <= 0) {
                SIE.Msg.showError("请输入接收数量".t());
                return;
            }
            var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getFixtureReceiveDetailId() });
            var qty = detail.getRecivedQty() + fromEntity.getCurrentQty();
            if (qty > fromEntity.getQty()) {
                SIE.Msg.showError("【已接收数量】+【本次接收数量】不能大于【接收数量】".t());
                return;
            }
            detail.setRecivedQty(qty);
            fromEntity.setRecivedQty(qty);
        }
    },
    
    snRecived: function (fromEntity, view, detailChildView, snChildView) {
        if (fromEntity.getManageMode() === 5) {
            if (fromEntity.getCurrentQty() <= 0) {
                fromEntity.setMessage("请填写本次接收数量".t());
                return;
            }
            var detail = detailChildView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getFixtureReceiveDetailId() });
            var qty = detail.getRecivedQty();
            if (fromEntity.getReceiveType() === 50) {
                if (fromEntity.getSnCodeId_Display() === null) {
                    fromEntity.setMessage("请选择返厂的序列号编码".t());
                    return;
                }
                if (snChildView.getData().data.items.findIndex(function (p) { return p.data.Sn == fromEntity.getSnCodeId_Display() }) !== -1) {
                    fromEntity.setMessage("序列号编码" + fromEntity.getSnCodeId_Display_Display() + "已接收，请勿重复接收".t());
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
            var childData = snChildView.getData().data.items;
            if (childData.length > 0) {
                var max = childData[0].getLineNo();
                for (var i = 1; i < childData.length; i++) {
                    if (max < childData[i].getLineNo()) {
                        max = childData[i].getLineNo();
                    }
                }
            }
            SIE.invokeDataQuery({
                type: "SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDataQueryer",
                method: "SnDetermine",
                params: [fromEntity.data, max],
                async: false,
                token: view.token,
                success: function (res) {
                    snChildView.getData().insert(0, res.Result);
                    detail.setRecivedQty(qty);
                    fromEntity.setRecivedQty(qty);
                }
            });
        }
    }
});