Ext.define('SIE.Web.Items.Common.Editors.MultiItemComboPopup', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.MultiItemComboPopup',
    pageSize: 5000,
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
    setQueryCriteria: function (dialogView, data)
    {
        var criteria = dialogView._relations[0]._target.getData();
        //criteria.setCategoryType(SIE.Items.Items.CategoryType.Item.value);
        if (data.ItemCategorys != null && data.ItemCategorys != "") {
            criteria.setItemCategorys(data.ItemCategorys);  
        }
    },
    
});