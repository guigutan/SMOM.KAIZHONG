Ext.define('SIE.Web.Warehouses.Control.WarehouseInvorgEditor', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.warehouseInvorgEditor',
    triggerCls: "x-form-arrow-trigger",
    _onSearchBoxTriggerClick: function (pageNum) {
        pageNum = pageNum || 1;
        var me = this;
        me._searchByItem(pageNum);
    },

    _isQuerySelectItems: true,
    catalogReloadData: true,
    _searchByItem: function (pageNum) {
        var me = this;
        //继承时发现_isQuerySelectItems偶尔会未定义，而基类又会直接使用_isQuerySelectItems，使得报错。所以这里再定义一次。
        if (typeof (_isQuerySelectItems) === "undefined")
            _isQuerySelectItems = this._isQuerySelectItems;
        var me = this,
            dsp = this.dataSourceProperty;

        var sieView = me._getSIEView();
        var isOnlyCurOrg = true;
        var AllotModel = null;
        var orderType = null;
        if (sieView.getCurrent()) {
            if (sieView.getCurrent().data && sieView.getCurrent().data.AllotModel == 2) {
                isOnlyCurOrg = false;
            }
            if (sieView.getCurrent().data && sieView.getCurrent().data.AllotModel >= 0) {
                AllotModel = sieView.getCurrent().data.AllotModel;
            }
            orderType = sieView.getCurrent().data.OrderType;
        }
        //console.log(sieView);
        if (!sieView) {
            me._searchByRawValue();
            return;
            //viewGroup = view.viewGroup;
        }
        var filter = {};

        var rec = me._getContainerRecord();
        var searchValue = me.cbSearch.getRawValue();
        if (orderType == SIE.Inventory.Commom.OrderType.WhTransferIn.value || orderType == SIE.Inventory.Commom.OrderType.CrossOrgTransferIn.value) {
            me._view.loadData({
                action: 'queryer',
                type: 'SIE.Web.Warehouses.DataQueryer.WarehouseDataQueryer',
                filter: Ext.encode({ Method: 'GetWarehouseInvorgByAsn', Parameters: [orderType, (searchValue ? '%' + searchValue + '%' : '')] })
            });
        }
        else {
            me._view.loadData({
                action: 'queryer',
                type: 'SIE.Web.Warehouses.DataQueryer.WarehouseDataQueryer',
                filter: Ext.encode({ Method: 'GetWarehouseInvorg', Parameters: [isOnlyCurOrg, AllotModel, (searchValue ? '%' + searchValue + '%' : '')] })
            });
        }
        me._lastSearchValue = searchValue;
    },
    _getViewMeta: function () {
        var me = this;
        var sieView = me._getSIEView();
        var isOnlyCurOrg = true;
        if (sieView.getCurrent()) {
            if (sieView.getCurrent().data && sieView.getCurrent().data.AllotModel == 2) {
                isOnlyCurOrg = false;
            }

        }
        var model = me.model;
        SIE.AutoUI.getMeta({
            async: false, //同步
            model: model, viewGroup: 'WarehouseInvorgView', isLookup: true, isReadonly: true, ignoreCommands: true,
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

        Ext.applyIf(meta.gridConfig, {
            frame: false,
            //width: 450,
            columnLines: true,
            focusOnToFront: false,
            ownerCt: this.up('[floating]')
        });

        if (me.ischeckbox) {
            meta.gridConfig.selModel = {
                injectCheckbox: 0,
                //checkbox位于哪一列，默认值为0
                selType: 'checkboxmodel',
                //checkbox
                checkOnly: true,
                //只能通过checkbox选择
                mode: "MULTI" //(multiSelect ? 'MULTI' : 'SINGLE'), //是否多选
            };

            me._targetSelectItems = {
                items: [],
                keys: []
            };
        }


        meta.gridConfig.viewConfig = {
            enableTextSelection: false,
            getRowClass: function (record, index, rowParams, store) {
                if (me.lastSelectionRecord && me.lastSelectionRecord.value) {
                    if (me.lastSelectionRecord.value == record.get(me.valueField)) {
                        me.grid.getSelectionModel().select(record, true);
                        if (!me.ischeckbox)
                            return 'gridRowLock';
                    }
                }
            }
        };

        if (me._isTree) {
            meta.gridConfig.useArrows = true;
        }
        meta.gridConfig.pagingBarConfig = {
            _displayInfoOnSimple: true,
            afterPageText: '/&nbsp{0}页'.t(),
            displayMsg: '共{2}条'.t(),
            _pageSize: me.pageSize
        };

        if (!me.catalogReloadData && me.store && me.store.data) {
            meta.storeConfig = me.store;
            if (typeof me.store.data == "string")
                meta.storeConfig.data = JSON.parse(me.store.data);
            meta.gridConfig.pagingBarConfig._pageSize = 100000;  //本地不分页
            me.pageSize = 100000;

        }

        meta.gridConfig.pagingBarConfig._pageSize = 100000;  //本地不分页
        me.pageSize = 100000;

        Ext.apply(meta.storeConfig, { pageSize: me.pageSize });
        return meta;
    },
});