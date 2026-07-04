Ext.define('SIE.Web.MES.WorkOrders.Editors.ReworkSnEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.ReworkSnEditor',
    items: [{
        xtype: 'textfield',
        name: 'WoReworkSn',
        hideLabel: true,
        style: 'width:100%;border-color:#3892D4;',
        fieldStyle: 'background-color:#90EE90;height:30px;',
        allowBlank: true,
        forceSelection: true,
        listeners: {
            afterRender: function (comp) {
                var formBody = this.up('form').SIEView.getControl().body;
                formBody.getFirstChild().setStyle("max-width", '800px')
            },
            specialkey: function (comp, e) {
                // e.HOME, e.END, e.PAGE_UP, e.PAGE_DOWN,
                // e.TAB, e.ESC, arrow keys: e.LEFT, e.RIGHT, e.UP, e.DOWN
                if (e.getKey() == e.ENTER) {
                    var barcode = comp.getValue();
                    if (barcode == "") return;
                    var form = this.up('form').SIEView;
                    var fromEntity = form.getData();
                    if (form.getChildren().length > 0) {
                        var ubView = form.getChildren().first(function (m) {
                            return m.model === 'SIE.MES.WorkOrders.Reworks.UnionBarcode'
                        });
                        if (ubView) {
                            var leftChildItems = ubView.getData().data.items.select(function (p) { return p.data; });//todo null ref
                            if (leftChildItems.any(function (p) { return p.OriginalBarcode == barcode; })) {
                                SIE.Msg.showError(Ext.String.format('条码[{0}]已存在'.L10N(), barcode));
                                comp.setValue("");
                                return;
                            }
                            var entityData = fromEntity.data;
                            if (entityData.PlanQty < leftChildItems.length + 1) {
                                SIE.Msg.showError(Ext.String.format('已超过工单计划数量：{0}'.L10N(), entityData.PlanQty));
                                comp.setValue("");
                                return;
                            }
                            else {
                                //leftChildItems.forEach(function (p) {
                                //    p.CreateDate = SIE.Web.MES.CommonFuns.dateTypeChange(p.CreateDate);
                                //    p.UpdateDate = SIE.Web.MES.CommonFuns.dateTypeChange(p.UpdateDate);
                                //});
                                //form._children[1].getData().data.items.select(function (p) { return p.data; }).forEach(function (p) {
                                //    p.CreateDate = SIE.Web.MES.CommonFuns.dateTypeChange(p.CreateDate);
                                //    p.UpdateDate = SIE.Web.MES.CommonFuns.dateTypeChange(p.UpdateDate);
                                //});
                                SIE.invokeDataQuery({
                                    method: 'GetUnionBarcode',
                                    params: [entityData, leftChildItems, form._children[1].getData().data.items.select(function (p) { return p.data; }), barcode],
                                    action: 'queryer',
                                    type: 'SIE.Web.MES.WorkOrders.Reworks.ReworkDataQueryer',
                                    token: form.token,
                                    success: function (res) {
                                        var re = res.Result;
                                        var barList = form._children[0].getControl();
                                        var store = barList.getStore();
                                        store.setData(re.BarcodeList);
                                        barList.setStore(store);

                                        var keyList = form._children[1].getControl();
                                        var keyStore = keyList.getStore();
                                        keyStore.setData(re.KeyItemList)
                                        keyList.setStore(keyStore);

                                        fromEntity.setScanQty(re.ScanQty);
                                        comp.setValue('');
                                        return true;

                                    },
                                    error: function (res) {
                                        SIE.Msg.showError(res.Message);
                                        comp.setValue('');
                                        return false;
                                    }
                                });
                            }
                        }
                        
                    }
                }
            }
        }
    }],
});