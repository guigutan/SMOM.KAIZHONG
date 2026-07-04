Ext.define('SIE.Web.Inventory.Scripts.InvMultiWhComboPopup', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.InvMultiWhComboPopup',
   
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;                   
        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) { //存在查询面板时
                    me.setQueryCriteria(dialogView);
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
    setQueryCriteria: function (dialogView) {
        var criteria = dialogView._relations[0]._target.getData();
        try {
            criteria.setIsEmployeeWarehouse(true);
           /* criteria.setLibraryType(0);*/
        } catch (err) {

        }     
    },
    _queryBlockProcess: function (block) {
        /// <summary>
        /// 查询块处理-只读为false
        /// </summary>
        /// <param name="block" type="type"></param>
        if (block.surrounders) {
            var surround = block.surrounders["0"];
            if (surround) {
                var items = surround.mainBlock.formConfig.items;
                for (var i = 0, len = items.length; i < len; i++) {
                    var item = items[i];
                    item.readOnly = false;
                    //if (item.name == "LibraryType") {
                    //    item.readOnly = true;
                    //} else {
                    //    item.readOnly = false;
                    //}
                }
            }
        }
    },

    /**
 * grid 处理
 * @param block 块配置
 */
    _gridBlockProcess: function (block) {
        var me = this;
        me.multiSelect = 'Multi';
           
        var multiSelect = me.multiSelect;
        //var multiSelect='MULTI';
        var gridConfig = block.gridConfig || block.mainBlock.gridConfig;
        gridConfig.selModel = {
            injectCheckbox: 0,
            //checkbox位于哪一列，默认值为0
            selType: 'checkboxmodel',
            //checkbox
            checkOnly: true,
            //只能通过checkbox选择
            mode: multiSelect //(multiSelect ? 'MULTI' : 'SINGLE'), //是否多选
        };
        gridConfig.viewConfig = {
            enableTextSelection: true,
            //启用文本选中
            getRowClass: function (record, index, rowParams, store) {
                if (me.lastSelectionRecord && me.lastSelectionRecord.value && me.multiSelect != "Multi") {
                    if (me.lastSelectionRecord.value == record.get(me.valueField)) {
                        me.grid.getSelectionModel().select(record, true);
                        return 'gridRowLock';
                    }
                }
            }
        };

        gridConfig.pagingBarConfig = {
            _displayInfoOnSimple: false,
            afterPageText: '/&nbsp{0}页'.t(),
            displayMsg: '共{2}条'.t(),
            _pageSize: me.pageSize
        };

    },
});