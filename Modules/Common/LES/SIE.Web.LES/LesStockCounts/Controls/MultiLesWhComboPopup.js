Ext.define('SIE.Web.LES.LesStockCounts.Controls.MultiLesWhComboPopup', {
    extend: 'SIE.control.GridComboPopup',
    alias: 'widget.MultiLesWhComboPopup',
    listeners: {
        change: function (field, newValue, oldValue, eOpts) {
            var me = this;
            entity = me.up("form").SIEView.getData();
        }
    },
    _createLayout: function () {
        var me = this;
        me.model || SIE.Msg.showWarning("请设置数据关联实体".L10N());
        SIE.AutoUI.getMeta({
            model: me.model,
            viewGroup:"ReadonlyView",
            ignoreChild: !0,
            ignoreCommands: !0,
            isReadonly: !0,
            ignoreQuery: !1,
            isAggt: !0,
            callback: function (blocks) {
                me._queryBlockProcess(blocks);
                me._gridBlockProcess(blocks);
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                me._popupWin(ui, me.inputEl);
                me._reloadTargetViewData();
                me._layouted = !0
            }
        })
    },
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        var view = me.up("form").SIEView;
        var entity = view.getData();
        var bill = view.getParent().getData();
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
                    me.setQueryCriteria(dialogView, bill);
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
        criteria.setIsEmployeeWarehouse(true);
        criteria.setLibraryType(0);
        criteria.setIsLine(true);
        if (data.data.SceneType && data.data.SceneType == 1) {
            criteria.setIsAutomated(false);
        } else if (data.data.SceneType && data.data.SceneType == 2) {
            criteria.setIsAutomated(true);
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
                    if (item.name == "LibraryType") {
                        item.readOnly = true;
                    } else {
                        item.readOnly = false;
                    }
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
        var view = me.up("form").SIEView;
        var bill =  view.getParent().getCurrent();
        if (bill && bill.data.SceneType && bill.data.SceneType == 2) {
            me.multiSelect = 'Select';
        }

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