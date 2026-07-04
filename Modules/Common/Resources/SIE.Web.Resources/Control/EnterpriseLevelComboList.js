Ext.define('SIE.Web.Resources.Control.EnterpriseLevelComboList', {
    extend: 'SIE.control.ComboList',
    alias: 'widget.enterpriseLevelComboList',
    triggerCls: "x-form-arrow-trigger",
    //选择分类层级下拉框时，自动带出分类类型。
    setValue: function (value, doSelect) {
        if (value !== null && value.data) {
            var curr = this.up().context.record;
            if (curr != null) {
                curr.set("IsResource", value.data.IsResource);
                curr.set("LevelIsResource", value.data.IsResource);
            }
        }
        this.callParent(arguments);
    },
    listeners: {
        select: function (combo, record, index) {
            var me = this;
            var enterprise = me._getSIEView().getData().data.items;
            var childNodes = enterprise[me.up().context.rowIdx].childNodes;
            if (enterprise[me.up().context.rowIdx].isNew()) {
                me.setChildNodesLevelId(childNodes);
            }
        }
    },
    setChildNodesLevelId: function (childNodes) {
        for (var i = 0; i < childNodes.length; i++) {
            childNodes[i].setLevelId(null);
            if (childNodes[i].childNodes.length > 0) {
                this.setChildNodesLevelId(childNodes[i].childNodes);
            }
        }
    },
    _onSearchBoxTriggerClick: function (pageNum) {
        pageNum = pageNum || 1;
        var me = this;

        if (me.queryMode == 'remote') {
            me._searchByDSP(pageNum);
        }
        else {
            me.doLocalQuery();
        }
    },
    _searchByDSP: function (pageNum) {
        //继承时发现_isQuerySelectItems偶尔会未定义，而基类又会直接使用_isQuerySelectItems，使得报错。所以这里再定义一次。  
        if (typeof (_isQuerySelectItems) === "undefined")
            _isQuerySelectItems = this._isQuerySelectItems;
        var me = this;

        var sieView = me._getSIEView();
        if (!sieView) {
            me._searchByRawValue();
            return;
        }

        var enterprise = sieView.getData();
        var searchValue = me.cbSearch.getRawValue();
        var parentId = enterprise.data.items[me.up().context.rowIdx].getparentId();
        var enterpriseDatas = enterprise.data.items;
        var levelId;
        for (var i = 0; i < enterpriseDatas.length; i++) {
            if (enterpriseDatas[i].id == parentId)
                levelId = enterpriseDatas[i].getLevelId();
        }
        me._view.loadData({
            action: 'queryer',
            type: 'SIE.Web.Resources.DataQueryers.EnterpriseLevelDataQueryer',
            filter: Ext.encode({ Method: 'GetEnterpriseLevel', Parameters: [levelId, pageNum, me.pageSize, (searchValue ? '%' + searchValue + '%' : '')] })
        });
        me._lastSearchValue = searchValue;
    },
});