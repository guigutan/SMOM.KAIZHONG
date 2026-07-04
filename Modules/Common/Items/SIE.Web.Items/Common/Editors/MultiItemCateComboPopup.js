Ext.define('SIE.Web.Items.Common.Editors.MultiItemCateComboPopup', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.MultiItemCateComboPopup',
    pageSize: 5000,
    listeners: {
        change: function (field, newValue, oldValue, eOpts) {
            var me = this;
            entity = me.up("form").SIEView.getData();
            var editor = SIE.Web.WMS.INV.StockCountRangeAction.findEditor(me.up("form"), "Items");
            if (editor.getValue() != "" && editor.getValue() != null) {
                entity.setItems(null);
                editor.setRawValue(null);
                editor.setValue(null);
            }
        }
    },
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        var view = me.up("form").SIEView;
        var entity = view.getData();
        var dialogView = me._targetView;
        var clearCommand = dialogView._relations[0]._target._commands.items.first(function (p) { return p.meta.command == "SIE.cmd.ClearCondition"; });
        if (clearCommand) {
            var Id = clearCommand.meta.id;
            document.getElementById(Id).style.display = "none";
        }
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
    setQueryCriteria: function (dialogView, data) {
        var criteria = dialogView._relations[0]._target.getData();
        criteria.setType(SIE.Items.Items.CategoryType.Item.value);
    },
    _queryBlockProcess: function (block) {
        /// <summary>
        /// 查询块处理-只读为false
        /// </summary>
        /// <param name="block" type="type"></param>
        if (block.surrounders && block.surrounders.length) {
            var surround = block.surrounders[0];
            var items = surround.mainBlock.formConfig.items;
            for (var i = 0; i < items.length; ++i) {
                if (items[i].name == "Type") {
                    items[i].readOnly = true;
                }
                else {
                    items[i].readOnly = false;
                }
            }
        }
    },
});