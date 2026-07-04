Ext.define('SIE.Web.EMS.MainenanceProjects.Scripts.CycleTypeComboList', {
    extend: 'SIE.control.PagingLookUpCustom',
    alias: 'widget.cycleTypeComboList',
    triggerCls: "x-form-arrow-trigger",

    /**
    * 处理单击触发（输入框）
    * @param {type} comboBox 控件
    * @param {type} trigger 
    * @param {type} e
    */
    onTriggerClick: function () {
        var me = this;
        var paginglookup = me.control;
        me.callParent();
        if (!paginglookup.readOnly && !paginglookup.disabled && !paginglookup.isExpanded) {
            paginglookup.expand();
            paginglookup.cbSearch.setRawValue("");
            if (paginglookup._isdeferTrue) {
                me._onSearchBoxTriggerClick();
            } else {
                me._onBlurAsynSearch(paginglookup.inputEl.dom.value);
            }
            paginglookup._isdeferTrue = true;
        }
    },

    /**
    * Ctrl+V查询
    * @param {type} text
    */
    _onBlurAsynSearch: function (text) {
        var me = this;
        var paginglookup = me.control;
        var criteriaData = {};
        criteriaData[paginglookup.displayField] = text;
        me._AsynSearch(criteriaData, function (result) {
            if (result[0] && result[0].length > 0) {
                paginglookup._SelectItems = [];
                me.method.setValue(result[0][0]);
                paginglookup._SelectItems.push(result[0][0]);
                me.method._setEntityLink(result[0][0]);
            } else {
                me.method.setValue(null);
                me.method._setEntityLink(null);
                paginglookup._SelectItems = [];
            }
            //if (!paginglookup.up("form")) {
            //    paginglookup.up('container').context.view.refresh();
            //}
            if (paginglookup.grid) {
                paginglookup.grid.store.reload();
            }
        });
    },

    /**
    *  获取设置视图元数据（ViewMeta）
    * @returns {type} 
    */
    _getViewMeta: function () {
        var me = this.control;
        var model = me.model;
        SIE.AutoUI.getMeta({
            async: false, //同步
            model: model, viewGroup: 'SelectionView', isLookup: true, isReadonly: true, ignoreCommands: true,
            callback: function (res) {
                if (res.mainBlock)
                    meta = res.mainBlock;
                else
                    meta = res;
            }
        });
        if (me.token)
            meta.token = me.token;
        me._isTree = SIE.getModel(model).isTree;
        if (me._isTree)
            meta.gridConfig.useArrows = true;

        Ext.applyIf(meta.gridConfig, {
            frame: false,
            //width: 450,
            columnLines: true,
            focusOnToFront: false,
            ownerCt: me.up('[floating]')
        });

        meta.gridConfig.viewConfig = {
            enableTextSelection: false,
            getRowClass: function (record, index, rowParams, store) {
                if (me.lastSelectionRecord && me.lastSelectionRecord.value) {
                    if (me.lastSelectionRecord.value == record.get(me.valueField)) {
                        me.grid.getSelectionModel().select(record, true);
                        return 'gridRowLock';
                    }
                }
            }
        };

        meta.gridConfig.pagingBarConfig = {
            _displayInfoOnSimple: true,
            afterPageText: '/&nbsp{0}页'.t(),
            displayMsg: '共{2}条'.t(),
            _pageSize: me.pageSize
        };

        if (me.store && me.store.data) {
            meta.storeConfig = me.store;
            if (typeof me.store.data == "string")
                meta.storeConfig.data = JSON.parse(me.store.data);

            meta.gridConfig.pagingBarConfig._pageSize = 100000;  //本地不分页
            me.pageSize = 100000;
        }

        Ext.apply(meta.storeConfig, { pageSize: me.pageSize });
        return meta;
    },

    /**
     * 分页控件事件监听
     * @returns {type} 
     */
    _pagingBarListeners: function (pagingBar) {
        return null;
    },

    /**
     * 设置Grid列表事件监听
     */
    _setGridListeners: function () {
        var paginglookup = this.control;
        var me = this,
            grid = paginglookup.grid,
            store = grid.getStore();

        paginglookup.mon(grid.getView(), {
            rowdblclick: function (vthis, record, element, rowIndex, e, eOpts) {
                me._onRowdblClick(vthis, record, element, rowIndex, e, eOpts);
            }
        });

        paginglookup.mon(store, {
            load: function (evObj, records, successful, operation, eOpts) {
                me._SetCurToFirst(records, store);
            }
        })
    },

    /**
     * 设置当前选择项在第一行
     * @param {type} items
     * @param {type} store
     */
    _SetCurToFirst: function (items, store) {
        var me = this.control;
        var tableView = me.grid.getView();
        if (me.value && items && items.length > 0 && items[0].get(me.valueField) != me.value) {
            var isContains = false;
            for (var index = 0; index < items.length; index++) {
                var item = items[index];
                if (item.get(me.displayField) == me.value) {
                    //items.splice(0, 0, item);
                    //items.splice(index + 1, 1);
                    me._isQuerySelectItems = false;
                    isContains = true;
                    if (store) {
                        store.loadRecords(items);
                    }
                    tableView.refresh();
                    break;
                }
            }

            if (!isContains) {
                if (me._isQuerySelectItems) {
                    var criteriaData = {};
                    criteriaData[me.displayField] = me.getRawValue();
                    me._isQuerySelectItems = false;
                    //异步请求
                    this._AsynSearch(criteriaData,
                        function (result) {
                            if (result[0] && result[0].length > 0) {
                                //me._SelectItems = [];
                                //me._SelectItems.push(result[0][0]);
                                //items.splice(0, 0, me._SelectItems[0]);
                                if (store) {
                                    store.loadRecords(items);
                                }
                                tableView.refresh();
                            }
                        });
                } else {
                    if (me._SelectItems.length > 0) {
                        //items.splice(0, 0, me._SelectItems[0]);
                        if (store) {
                            store.loadRecords(items);
                        }
                        tableView.refresh();
                    }
                }
            }
        }
    },

    /**
     * 异步查询请求
     * @param {type} criteriaData 查询实体
     * @param {type} callback 回调函数
     */
    _AsynSearch: function (criteriaData, callback) {
        var me = this.control;
        var view;
        if (!view) {
            var meta = this._getViewMeta();
            view = SIE.AutoUI.createListView(meta);
        }
        var rec = this.method._getContainerRecord();
        var filter = {
            Parameters: {
                EntityType: !me.up("form") ? me.up().context.grid.SIEView.model : me.up("form").SIEView.model,
                Entity: rec.data,
                DataSourceProperty: me.dataSourceProperty
            }
        };
        var searchValue = criteriaData[me.displayField];
        view.loadData({
            action: 'lookup',
            filter: SIE.data.Utils.seriaizeRequest(filter),
            searchKeyWord: (searchValue ? searchValue : ''),
            page: 1,
            criteria: criteriaData,
            callback: function (result) {
                if (callback && Ext.isFunction(callback)) {
                    callback(result);
                }
            }
        });
    },

    /**
     * 下拉展开事件
     */
    onExpand: function () {
        var me = this.control;
        var tableView = me.grid.getView();
        if (me.reloadDataOnPopping === true) {
            me.cbSearch.setRawValue("");
            this._onSearchBoxTriggerClick();
        } else {
            me.cbSearch.setRawValue(me.inputEl.dom.value)
            if (!me.up("form")) {
                if (me._currentRowId != me.up().context.record.getId() && me._currentRowId != -1) {
                    me.cbSearch.setRawValue("");
                    me._isQuerySelectItems = true;
                    me._onSearchBoxTriggerClick();
                }
                me._currentRowIndex = me.up().context.record.getId();
            }
        }
        this.method._viewScrollTo();
        tableView.refresh();
    },

    /**
    * 下拉列表失去焦点事件
    * @param {type} e
    */
    onBlur: function (e) {
        var me = this.control;
        var tclass = this;
        var inputEl = me.inputEl.dom;
        if (me.up("grid") && me.revertInvalid === false) {
            me.up('container').revertInvalid = me.revertInvalid;
        }
        var text = inputEl && inputEl.value;
        this.method._verificaGrid(text);
        if (text && me.lastSelectionRecord && me.lastSelectionRecord.rawValue !== text) {
            me.markInvalid('输入['.t() + text + ']无效，未找到数据'.t());
            if (me.up("form"))
                me.value = null;
        }
    },

    /**
     * 获取控件绑定实体
     */
    _getbindEntity: function () {
        var me = this.control;
        var bindEntity;
        if (!me.bind || !me.bind.value) {
            var contet = me.up('container').context;
            var data = contet.record;
            bindEntity = data;
        } else {
            bindEntity = me.bind.value.owner.data;
            if (!(bindEntity instanceof Ext.data.Model)) {
                bindEntity = bindEntity.p;
            }
        }
        return bindEntity;
    },

    /**
     * 获取焦点事件
     * @param {type} e
     */
    onFocusLeave: function (e) {
        this._onFocusLeaveIntegrate();
    },

    /**
     * 获取焦点处理事件
     */
    _onFocusLeaveIntegrate: function () {
        var me = this.control;
        var rawValue = me.getRawValue();

        if (!me.grid || (me.lastSelectionRecord && me.lastSelectionRecord.rawValue == rawValue)) {
            if (!rawValue && rawValue.length >= 0) {
                this.method.setValue(null);
                this.method._setEntityLink(null);
                me._SelectItems = [];
            }
            return;
        }
        if (rawValue == "") {
            this.method.setValue(null);
            this.method._setEntityLink(null);
            me._SelectItems = [];
        }
    }

});