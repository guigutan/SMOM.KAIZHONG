Ext.define('SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveScanBehavior', {
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        var entity = view.getCurrent();
        me.detailChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveDetail"; });
        me.lotChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveLot"; });
        me.snChildView = view._children.first(function (p) { return p.model === "SIE.EMS.Purchases.SparePartReceives.SparePartReceiveSn"; });
        var qty = view._children.length;
        var tabPanel = view._children[0].getControl().ownerCt.ownerCt;
        for (var i = 1; i < qty; i++) {
            tabPanel.setActiveTab(i);
        }
        tabPanel.setActiveTab(0);
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            me.ReceiveId = params.ReceiveId;
            entity.setSparePartReceiveId(me.ReceiveId);
            me.setReceiveScanInfo(view);
        }
        view.scanBehavior = me;
        view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, view);
        view.mon(view, 'beforeClosewin', me.beforeClosewin, view);
    },
    setReceiveScanInfo: function (view) {
        var me = this;
        var entity = view.getCurrent();
        SIE.invokeDataQuery({
            method: 'GetReceiveScanInfo',
            params: [me.ReceiveId],
            action: 'queryer',
            type: 'SIE.Web.EMS.Purchases.SparePartReceives.SparePartReceiveDataQueryer',
            token: view.token,
            success: function (res) {
                if (res.Result) {
                    var info = res.Result.Item1;
                    entity.setReceiveNo(info.ReceiveNo);
                    entity.setFactoryName(info.FactoryName);
                    entity.setDepartmentName(info.DepartmentName);
                    entity.setReceiveType(info.ReceiveType);
                    if (info.ReceiveType === 50) {
                        entity.setScanEquip(true);
                    } else {
                        entity.setScanEquipAndSn(true);
                    }
                    if (me.detailChildView) {
                        var detStore = me.detailChildView.getControl().getStore();
                        detStore.setData(res.Result.Item2);
                    }
                    if (me.lotChildView) {
                        var lotStore = me.lotChildView.getControl().getStore();
                        lotStore.setData(res.Result.Item3);
                    }
                    if (me.snChildView) {
                        var snStore = me.snChildView.getControl().getStore();
                        snStore.setData(res.Result.Item4);
                    }
                    entity.setControlMethod(10);
                }
            }
        });
    },
    onEntityPropertyChanged: function (e) {
        var me = this;
        if (e.property === 'SparePartReceiveDetailId') {
            if (e.value > 0) {
                var detail = me.scanBehavior.detailChildView.getData().data.items.find(function (p) { return p.data.Id == e.value });
                if (detail && detail.data.PurchaseOrderNo.length > 0) {
                    e.entity.setPurchaseOrderLineNo(detail.data.PurchaseOrderNo + "-" + detail.data.PurchaseOrderLine);
                }
                e.entity.setRecivedQty(detail.getRecivedQty());
                e.entity.setCurrentQty(detail.getQty() - detail.getRecivedQty());
            } else {
                e.entity.setPurchaseOrderLineNo("");
                e.entity.setRecivedQty(null);
                e.entity.setCurrentQty(null);
            }
        }
        if (e.property === 'ControlMethod') {
            //由于控件是被隐藏状态，页面打开无操作后一段时间，控件的dom会被销毁
            var sparePartReceiveLotSnId = Ext.getCmp('SparePartReceiveLotSnId');
            var sparePartReceiveSnId = Ext.getCmp('SparePartReceiveSnId');
            if (sparePartReceiveLotSnId == null || sparePartReceiveLotSnId.el == null || sparePartReceiveLotSnId.el.dom == null
                || sparePartReceiveSnId == null || sparePartReceiveSnId.el == null || sparePartReceiveSnId.el.dom == null) {
                SIE.Msg.showInstantMessage('当前页面长时间未操作，请关闭重新打开'.t());
                return;
            }
            if (e.value === 10) {
                e.entity.setMessage("");
                me.getControl().query('[name=LotSn]').first().hide();
                me.getControl().query('[name=FixedQty]').first().hide();
                me.getControl().query('[name=Sn]').first().hide();
            } else if (e.value === 20) {
                e.entity.setMessage("扫码批次号".t());
                me.getControl().query('[name=LotSn]').first().show();
                me.getControl().query('[name=FixedQty]').first().show();
                me.getControl().query('[name=Sn]').first().hide();
            } else if (e.value === 30) {
                e.entity.setMessage("扫码序列号编码".t());
                me.getControl().query('[name=LotSn]').first().hide();
                me.getControl().query('[name=FixedQty]').first().hide();
                me.getControl().query('[name=Sn]').first().show();
            }
        }
        if (e.property === 'ScanEquip' && e.value === true) {
            e.entity.setMessage("扫码序列号编码".t());
            e.entity.setScanSn(false);
            e.entity.setScanEquipAndSn(false);
        }
        if (e.property === 'ScanSn' && e.value === true) {
            e.entity.setMessage("扫码原厂序列号".t());
            e.entity.setScanEquip(false);
            e.entity.setScanEquipAndSn(false);
        }
        if (e.property === 'ScanEquipAndSn' && e.value === true) {
            e.entity.setMessage("扫码序列号编码".t());
            e.entity.setScanEquip(false);
            e.entity.setScanSn(false);
        }
    },
    beforeClosewin: function (returnObj) {
        this.mun(this, 'beforeClosewin');
    }
});