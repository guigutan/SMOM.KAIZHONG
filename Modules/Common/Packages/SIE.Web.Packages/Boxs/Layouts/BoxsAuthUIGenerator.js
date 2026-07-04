/**
 * UI生成器
 * 设置从表历史数据灰色处理
 */
Ext.define('SIE.Web.Packages.Boxs.BoxsAuthUIGenerator', {
    extend: 'SIE.autoUI.AggtUIGeneratorDefault',

    /**
     * 生成控件重写框架的方法
     * @param aggtMeta 聚合块元数据
     * @param entity 实体
     * @returns 聚合控件
     */
    generateControl: function (aggtMeta, entity) {
        var mainView = null;
        var mk = aggtMeta.mainBlock;
        if (mk.gridConfig) {
            mainView = this._vf.createListView(mk);
        }
        else {
            mainView = this._vf.createDetailView(mk, entity);
        }
        var aggtView = this._generateAggt(aggtMeta, mainView, true);
        if (mainView.hasListeners['isready']) {
            mainView.fireEvent('isReady', true);
        }
        var me = this;
        SIE.invokeDataQuery({
            method: 'GetProductTrunoverBoxType',
            params: [],
            action: 'queryer',
            type: 'SIE.Web.Packages.Boxs.DataQuery.BoxDataQuery',
            token: mainView.token,
            success: function (res) {
                mainView.ProductTrunoverBoxType = res.Result;
                mainView.mon(mainView._control, 'rowclick', me.rowclick, mainView);
            }
        });
        //页面生成后，需要加载出当前活动的子页签数据;
        mainView._resetChildrenData();
        return aggtView;
    },
    _capacityColumn: null,
    rowclick: function (g, record, element, rowIndex, e, eOpts) {
        var me = this;
        var pType = record.data.Type;
        var children = g.up().SIEView.findChild("SIE.Packages.Boxs.ProductCapacity");
        if (children) {
            var childControl = children.getControl();
            if (pType == me.ProductTrunoverBoxType) {
                childControl.setVisible(true);
                childControl.up().up().up().setVisible(true);
            }
            else {
            //非生产周转箱
            childControl.setVisible(false);
            childControl.up().up().up().setVisible(false);
            }
        }
        //TODO huchuqiang 数据绑定同时列隐藏存在问题，需先绑定数据再列隐藏
        //if (!me._capacityColumn) {
        //    var controlColumns = g.up().SIEView.getControl().columns;
        //    me._capacityColumn = controlColumns.first(function (p) { return p.dataIndex === "Capacity" });
        //}
        //if (!me._capacityColumn)
        //    return;
        //if (pType == "生产周转箱")
        //    me._capacityColumn.show();
        //else
        //    me._capacityColumn.hide();
    }
});