Ext.define('SIE.Web.EMS.Purchases.EquipmentReceives.ReceiveScanBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            me.ReceiveId = params.ReceiveId;
            entity.setEquipmentReceiveId(me.ReceiveId);
            entity.setCurrentQty(1);
            SIE.invokeDataQuery({
                method: 'GetReceiveScanInfo',
                params: [me.ReceiveId],
                action: 'queryer',
                type: 'SIE.Web.EMS.Purchases.EquipmentReceives.EquipmentReceiveDataQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Result) {
                        var info = res.Result.data.items[0].data;
                        entity.setReceiveNo(info.ReceiveNo);
                        entity.setFactoryName(info.FactoryName);
                        entity.setDepartmentName(info.DepartmentName);
                        entity.setReceiveType(info.ReceiveType);
                        if (info.ReceiveType === 50) {
                            entity.setScanEquip(true);
                        } else {
                            entity.setScanEquipAndSn(true);
                        }
                    }
                }
            });
        }
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
        view.mon(view, 'beforeClosewin', me.beforeClosewin, view);
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'CurrentQty' && e.value > 1 && e.entity.getEquipAccountId() > 0) {
            e.entity.setEquipAccountId_Display("");
            e.entity.setEquipAccountId(null);
        }
        if (e.property === 'ScanEquip' && e.value === true) {
            e.entity.setMessage("扫描设备编码".t());
            e.entity.setScanSn(false);
            e.entity.setScanEquipAndSn(false);
        }
        if (e.property === 'ScanSn' && e.value === true) {
            e.entity.setMessage("扫码原厂序列号".t());
            e.entity.setScanEquip(false);
            e.entity.setScanEquipAndSn(false);
        }
        if (e.property === 'ScanEquipAndSn' && e.value === true) {
            e.entity.setMessage("扫描设备编码".t());
            e.entity.setScanEquip(false);
            e.entity.setScanSn(false);
        }
        if (e.property === 'ReceiveLineNo') {
            if (e.value > 0) {
                let qty = e.entity.getOldRecivedQty();
                me._children[0].getData().data.items.forEach(function (p) {
                    if (p.data.ReceiveLineNo === e.value) {
                        qty++;
                    }
                });
                e.entity.setRecivedQty(qty);
            } else {
                e.entity.setRecivedQty(null);
            }
        }
    },
    beforeClosewin: function (returnObj) {
        this.mun(this, 'beforeClosewin');
    }
});