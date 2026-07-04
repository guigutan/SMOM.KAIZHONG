Ext.define('SIE.Web.EMS.SpareParts.OutDepotHandovers.Behaviors.ScanHandoverDetailBehavior',
    {
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            var me = this;
            me.view = view;
            setTimeout(function () {
                var entity = view.getCurrent();
                view.mon(entity, "propertyChanged", me.onEntityPropertyChanged, me);
            }, 0);

        },
        onEntityPropertyChanged: function (e) {
            var me = this;

            setTimeout(function () {

                var dtlChildView = me.view.findChild('SIE.EMS.SpareParts.OutDepotHandovers.OutDepotHandoverDetail');

                if (e.property === 'OutDepotHandoverBillId') {

                    var childStore = dtlChildView.getData();
                    childStore.removeAll();

                    if (e.entity.data.OutDepotHandoverBillId != null) {
                        e.entity.setMessage("请扫描【序列号】/【批次号】/【备件编码】！".t());

                        SIE.invokeDataQuery({
                            method: 'GetOutDepotHandoverDetails',
                            params: [e.entity.data.OutDepotHandoverBillId, e.entity.data.SparePartId],
                            async: false,
                            action: 'queryer',
                            type: 'SIE.Web.EMS.SpareParts.OutDepotHandovers.DataQuerys.OutDepotHandoverDataQuery',
                            token: e.entity.belongsView.token,
                            success: function (res) {

                                if (res.Success) {
                                    for (var i = 0; i < res.Result.data.items.length; i++) {
                                        childStore.add(res.Result.data.items[i]);
                                    }
                                }
                            }
                        });
                    }
                    else {
                        e.entity.setMessage("请选择【交接单号】！".t());
                    }
                }

                if (e.property === 'SparePartId' && e.entity.data.IsSelectSparePart) {
                        if (e.entity.data.SparePartId != null) {
                            if (e.entity.data.ControlMethod == 10) {

                                var dtlStore = dtlChildView.getData();
                                var record = dtlStore.findRecord('SparePartId', e.entity.data.SparePartId, 0, false, true, true);

                                if (record != null) {
                                    if (record.data.HandOverStatus == 30) {
                                        e.entity.setBarcode(null);
                                        e.entity.setQty(null);
                                        e.entity.setReceiveQty(null);
                                        e.entity.setMessage("该备件行已被接收，请确认后重新扫描！".t());
                                    }
                                    else {
                                        e.entity.setBarcode(null);
                                        e.entity.setQty(record.data.Qty);
                                        e.entity.setReceiveQty(record.data.Qty);
                                        record.set('ReceiveQty', record.data.Qty);
                                        record.set('HandOverStatus', 30);
                                        e.entity.setMessage("接收成功，请继续扫描【序列号】/【批次号】/【备件编码】！".t());
                                    }
                                }
                            }
                            if (e.entity.data.ControlMethod == 20) {
                                e.entity.setBarcode(null);
                                e.entity.setQty(null);
                                e.entity.setReceiveQty(null);
                                e.entity.setMessage("请扫描【批次号】！".t());
                            }
                            if (e.entity.data.ControlMethod == 30) {
                                e.entity.setBarcode(null);
                                e.entity.setQty(null);
                                e.entity.setReceiveQty(null);
                                e.entity.setMessage("请扫描【序列号】！".t());
                            }
                        }
                        else {
                            e.entity.setBarcode(null);
                            e.entity.setQty(null);
                            e.entity.setReceiveQty(null);
                            e.entity.setMessage("请扫描【序列号】/【批次号】/【备件编码】！".t());
                        }
                }
            }, 0);
            
        }
    });
