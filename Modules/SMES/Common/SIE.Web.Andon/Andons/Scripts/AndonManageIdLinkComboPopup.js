Ext.define('SIE.Web.Andon.Andons.Scripts.AndonManageIdLinkComboPopup', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.AndonManageIdLinkComboPopup',

    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        var view = me.up("form").SIEView;
        var entity = view.getData();
        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) { //存在查询面板时
                    me.setQueryCriteria(dialogView, entity.data);
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
    onLoad: function (store, records, successful, operation, eOpts) {
        var me = this;
        if ((me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0)) {
            var selModel = me._targetView.getSelectionModel();
            if (records && records.length > 0) {
                for (var i = 0, len = records.length; i < len; i++) {
                    var record = records[i];
                    if (me._sourceViewSelectItems.indexOf(record.getId()) > -1) {
                        selModel.select(record, true, true);
                        if (Ext.Array.indexOf(me._targetSelectItems.keys, record.getId(), 0) === -1) {
                            me._targetSelectItems.keys.push(record.getId());
                            me._targetSelectItems.items.push(record);
                        }
                    }
                }
            }
        }
    },
    setQueryCriteria: function (dialogView, data) {

    },
});
