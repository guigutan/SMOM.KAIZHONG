Ext.define('SIE.Web.DIST.Editors.GoodsIssueSnEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.GoodsIssueSnEditor',
    items: [{
        xtype: 'textfield',
        name: 'GoodsIssueSn',
        hideLabel: true,
        bind: '{p.Barcode}',
        style: 'width:100%;',
        fieldStyle: "background-color:#90EE90;height:30px;",
        allowBlank: true,
        forceSelection: true,
        listeners: {
            specialkey: function (comp, e) {
                // e.HOME, e.END, e.PAGE_UP, e.PAGE_DOWN,
                // e.TAB, e.ESC, arrow keys: e.LEFT, e.RIGHT, e.UP, e.DOWN
                if (e.getKey() == e.ENTER) {
                    var barcode = comp.getValue();
                    if (barcode == "") return;
                    var form = this.up('form').SIEView;
                    var fromEntity = form.getData();
                    var entityData = fromEntity;
                    entityData.setTips('');
                    entityData.setError('');
                    entityData.setBarcode(barcode);
                    SIE.invokeDataQuery({
                        method: 'BarcodeChange',
                        params: [entityData.data, entityData.data.ItemLabelList],
                        action: 'queryer',
                        type: 'SIE.Web.DIST.GoodsIssueDataQueryer',
                        token: form.token,
                        success: function (res) {
                            var goodEntity = res.Result;
                            if (goodEntity != null && res.Result.GoodsModel != null) {                               
                                var goodsModel = res.Result.GoodsModel;
                                if (goodsModel.Error != "") {
                                    fromEntity.setTips(goodsModel.Error);                                                                 
                                    SIE.Web.DIST.GoodsIssueCommonFun.setTipsCtlColor("red");
                                }
                                else {
                                    fromEntity.setTurnoverBox(res.Result.GoodsModel.TurnoverBox);
                                    fromEntity.data.ItemLabelList = res.Result.ItemLabelList;
                                    fromEntity.setQty(goodsModel.Qty);
                                    fromEntity.setTips(goodsModel.Tips);
                                    fromEntity.setQtyReadOnly(goodsModel.QtyReadOnly);
                                    fromEntity.setBarcode('');
                                    SIE.Web.DIST.GoodsIssueCommonFun.setTipsCtlColor("#008000");                                     
                                }
                            }
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
    }],
});