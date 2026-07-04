Ext.define('SIE.Web.ProductIntfc.ProductStorages.Scripts.CheckBoxUIGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',

    oldRowIndex: null,
    barcodeView: null,
    barcodeDetailControl: null,

    /**
     * 生成控件
     * @param aggtMeta 聚合块元数据
     * @param entity 实体
     * @returns 聚合控件
     */
    generateControl: function (aggtMeta, entity) {
        var me = this;
        me.oldRowIndex = -1;
        var mainView = null;
        var mk = aggtMeta.mainBlock;
        mainView = this._vf.createListView(mk);
        var aggtView = this._generateAggt(aggtMeta, mainView, true);
        if (mainView.hasListeners['isready']) {
            mainView.fireEvent('isReady', true);
        }

        me.barcodeView = Ext.Array.findBy(aggtView._view._children, function (item) {
            if (item.model == 'SIE.ProductIntfc.ProductStorages.ToStorageBarcode') { return true; }
        });
        if (me.barcodeView) {
            var barcodeDetailView = Ext.Array.findBy(me.barcodeView._children, function (item) {
                if (item.model == 'SIE.ProductIntfc.ProductStorages.ToStorageBarcodeDetail') { return true; }
            });

            if (barcodeDetailView) {
                me.barcodeDetailControl = barcodeDetailView.getControl();

            }
            var barcodeControl = me.barcodeView.getControl();
            var _barcodeView = barcodeControl.SIEView;
            barcodeControl.mon(barcodeControl, 'cellclick', this._onControlCellClick, me);
            _barcodeView.mun(_barcodeView, 'currentChanged');
        }

        //页面生成后，需要加载出当前活动的子页签数据;
        mainView._resetChildrenData();

        return aggtView;
    },
    /**
     * 选中数据变更处理事件
     * @param {} selection
     * @returns {}
     */
    _onControlCellClick: function (g, row, col, record, tr, rowindex) {
        var me = this;
        if (!record.data)
            return;
        var data = record.data;
        if (me.oldRowIndex == rowindex)
            return;
        else
            me.oldRowIndex = rowindex;

        if (!me.barcodeView)
            return;
        SIE.invokeDataQuery({
            type: "SIE.Web.ProductIntfc.ProductStorages.DataQuery.StorageWorkOrderDataQueryer",
            method: "GetBarcodeDetails",
            params: [data.Id],
            async: false,
            token: me.barcodeView.token,
            callback: function (res) {
                if (res.Success) {
                    var barcodeDetails = res.Result.data;
                    if (me.barcodeDetailControl) {
                        var store = me.barcodeDetailControl.getStore();
                        store.setData(barcodeDetails);
                        me.barcodeDetailControl.setStore(store);
                    }

                    //_aggtView._view._children[0].getControl().SIEView.setCurrent(record);
                }
            }
        });
    }
});