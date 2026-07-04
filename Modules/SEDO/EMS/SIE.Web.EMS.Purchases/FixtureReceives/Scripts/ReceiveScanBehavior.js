Ext.define('SIE.Web.EMS.Purchases.FixtureReceives.ReceiveScanBehavior', {
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
            entity.setFixtureReceiveId(me.ReceiveId);
            entity.setCurrentQty(1);
            SIE.invokeDataQuery({
                method: 'GetReceiveScanInfo',
                params: [me.ReceiveId],
                action: 'queryer',
                type: 'SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDataQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Result) {
                        var info = res.Result.data.items[0].data;
                        entity.setReceiveNo(info.ReceiveNo);
                        entity.setFactoryName(info.FactoryName);
                        entity.setDepartmentName(info.DepartmentName);
                        entity.setReceiveType(info.ReceiveType);
                        if (info.ReceiveType === 50) {
                            entity.setScanSnCode(true);
                        } else {
                            entity.setScanSnCodeAndSn(true);
                        }
                    }
                }
            });
        }
        me.detailChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveDetail"; });
        me.snChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveSn"; });
        view.scanBehavior = me;
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
        view.mon(view, 'beforeClosewin', me.beforeClosewin, view);
        me.loadChildData(me.ReceiveId, view);

    },
    loadChildData: function (receiveId, view) {
        var mainView = view;
        SIE.invokeDataQuery({
            method: 'LoadScanChildData',
            params: [receiveId],
            action: 'queryer',
            type: 'SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Result) {
                    if (res.Result.Item1.length > 0) {

                        res.Result.Item1.forEach(item => {
                            item.PurchaseOrderId_Display = item.PurOrderNo;
                            item.FixtureEncodeId_Display = item.FixtureEncodeCode;
                            item.PurchaseOrderItemId_Display = item.PuOrderLineNo;
                            item.SupplierId_Display = item.SupplierCode;
                            item.WarehouseId_Display = item.WareHouseCode;
                            item.CustomerId_Display = item.CustomerCode;
                            item.UnitId_Display = item.UnitName;

                            mainView._children[0].getData().add(item);
                        })
                        res.Result.Item2.forEach(item => {
                            item.PurchaseOrderId_Display = item.PurOrderNo;
                            item.FixtureEncodeId_Display = item.FixtureEncodeCode;
                            item.PurchaseOrderItemId_Display = item.PuOrderLineNo;
                            item.SupplierId_Display = item.SupplierCode;
                            item.WarehouseId_Display = item.WareHouseCode;
                            item.CustomerId_Display = item.CustomerCode;
                            item.UnitId_Display = item.UnitName;

                            mainView._children[1].getData().add(item);
                        })
                    }
                }
            }
        });

    },

    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'CurrentQty' && e.value > 1 && e.entity.getFixtureEncodeId() > 0) {
            e.entity.setFixtureEncodeId_Display("");
            e.entity.setFixtureEncodeId(null);
            e.entity.setMessage("");
        }
        if (e.property === 'ScanSnCode' && e.value === true) {
            e.entity.setMessage("扫描序列号编码".t());
            e.entity.setScanSn(false);
            e.entity.setScanSnCodeAndSn(false);
        }
        if (e.property === 'ScanSn' && e.value === true) {
            e.entity.setMessage("扫码原厂序列号".t());
            e.entity.setScanSnCode(false);
            e.entity.setScanSnCodeAndSn(false);
        }
        if (e.property === 'ScanSnCodeAndSn' && e.value === true) {
            e.entity.setMessage("扫描序列号编码或原厂序列号".t());
            e.entity.setScanSnCode(false);
            e.entity.setScanSn(false);
        }

        if (e.property === 'FixtureReceiveDetailId') {
            if (e.value > 0) {
                var detail = me.scanBehavior.detailChildView.getData().data.items.find(function (p) { return p.data.Id == e.value });
                e.entity.setRecivedQty(detail.getRecivedQty());
                e.entity.setCurrentQty(detail.getQty() - detail.getRecivedQty());
            } else {
                e.entity.setPuOrderLineNo("");
                e.entity.setRecivedQty(null);
                e.entity.setCurrentQty(null);
            }
            if (e.entity.getScanSnCodeAndSn()) {
                e.entity.setMessage("扫描序列号编码或原厂序列号".t());
            }
            if (e.entity.getScanSn()) {
                e.entity.setMessage("扫码原厂序列号".t());
            }
            if (e.entity.getScanSnCode()) {
                e.entity.setMessage("扫描序列号编码".t());
            }
        }
    },



    beforeClosewin: function (returnObj) {
        this.mun(this, 'beforeClosewin');
    }
});