Ext.define('SIE.Web.Kit.Mes.SingleLabels.SingleLabelBehavior', {
    //requires: [
    //    'SIE.Web.MES.WorkOrders.AddWorkOrderCommand'
    //],
    /**
     * view生命周期函数--view准备完成
     * @param {ListLogicView} view 生成的view
     */
    onViewReady: function (view) {
        var me = this;
        CRT.Event.listen('singleLabelAdd', function () {
            var cmd = Ext.create('SIE.cmd.Add', {});
            cmd._setOwnerView(view);
            cmd.command = Ext.getClassName(cmd);
            cmd.tryExecute(cmd);
        });
    },
});