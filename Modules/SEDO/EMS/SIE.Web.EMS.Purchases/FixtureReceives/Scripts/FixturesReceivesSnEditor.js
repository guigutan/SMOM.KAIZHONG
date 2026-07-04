Ext.define('SIE.Web.EMS.Purchases.FixturesReceives.FixturesReceivesSnEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.FixturesReceivesSnEditor',
    items: [{
        xtype: 'textfield',
        id: 'FixturesReceivesSn',
        name: 'FixturesReceivesSn',
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
                    var childView = form._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveSn"; });
                    var detailView = form._children.first(function (p) { return p.model === "SIE.EMS.Purchases.FixtureReceives.FixtureReceiveDetail"; });
                    if (!childView || !detailView) {
                        fromEntity.setMessage("界面子列表无权限，请配置".t());
                        return;
                    }
                    if (fromEntity.getFixtureReceiveDetailId() === null) {
                        fromEntity.setMessage("请选择接收明细后再扫码".t());
                        comp.setValue("");
                        return;
                    }
                    if (fromEntity.getFixtureEncodeId() === null) {
                        fromEntity.setMessage("工治具编码不能为空".t());
                        comp.setValue("");
                        return;
                    }
                    if (fromEntity.data.ScanSnCode || (fromEntity.data.ScanSnCodeAndSn && fromEntity.data.FirstSn == "")) {
                        if (childView.getData().data.items.findIndex(function (p) { return p.data.Sn == barcode }) !== -1) {
                            fromEntity.setMessage("序列号编码" + barcode + "已存在，请勿重复扫描".t());
                            comp.setValue("");
                            return;
                        }
                    }

                    if (fromEntity.data.ScanSn || (fromEntity.data.ScanSnCodeAndSn && fromEntity.data.FirstSn != "")) {
                        if (childView.getData().data.items.findIndex(function (p) { return p.data.OriginalSn == barcode }) !== -1) {
                            fromEntity.setMessage("原厂序列号" + barcode + "已存在，请勿重复扫描".t());
                            comp.setValue("");
                            return;
                        }
                    }
                    if (fromEntity.data.RecivedQty + 1 > fromEntity.data.Qty) {
                        fromEntity.setMessage("已接收数量+本次接收数量不能大于接收数量".t());
                        comp.setValue("");
                        return;
                    }
                  
                    var detail = detailView.getData().data.items.find(function (p) { return p.data.Id == fromEntity.getFixtureReceiveDetailId() });
                    SIE.invokeDataQuery({
                        type: "SIE.Web.EMS.Purchases.FixtureReceives.FixtureReceiveDataQueryer",
                        method: "ReceiveExecute",
                        params: [barcode, fromEntity.data],
                        async: false,
                        token: form.token,
                        success: function (res) {
                            var info = res.Result;
                            if (info!=null&&info.Success) {
                                if (info.IsFirstSn) {
                                    fromEntity.setFirstSn(barcode);
                                } else {
                                    var childData = childView.getData();
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