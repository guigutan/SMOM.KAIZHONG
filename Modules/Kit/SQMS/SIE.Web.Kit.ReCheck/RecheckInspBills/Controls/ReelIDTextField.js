Ext.define('SIE.Web.Kit.Recheck.RecheckInspBills.Controls.ReelIDTextField', {
    extend: 'Ext.form.field.Text',
    alias: 'widget.recheckEeelIDTextField',

    /**失去焦点 */
    onBlur: function () {
        var view = this.up("grid").SIEView;
        if (view) {
            var parent = view.getParent();
            var asnNo = parent.getData().getAsnNo();
            var reelID = this.value;
            var entity = view.getCurrent();
            if (!Ext.isEmpty(asnNo) && !Ext.isEmpty(reelID)) {
                //校验ReelID是否存在于ASN号中,如果存在,则带出对应数量
                var billId = parent.getData().getId();
                SIE.invokeDataQuery({
                    type: "SIE.Web.Kit.Recheck.RecheckInspBills.DataQueryers.KitRecheckInspBillsQueryer",
                    method: "GetReelIDInAsnNo",
                    params: [reelID, billId],
                    token: view.getToken(),
                    success: function (res) {
                        if (res.Result) {
                            if (entity && Ext.isEmpty(entity.getQuannity()))
                                entity.setQuannity(res.Result.Qty);
                        }
                    }
                });
            }
        }
        this.callParent(arguments);
    }
});