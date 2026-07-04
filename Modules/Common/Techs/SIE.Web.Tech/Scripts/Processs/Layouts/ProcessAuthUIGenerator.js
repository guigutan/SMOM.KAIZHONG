/**
 * UI生成器
 * 设置从表历史数据灰色处理
 */
Ext.define('SIE.Web.Tech.Processs.ProcessAuthUIGenerator', {
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
        mainView.uIGenerator = me;
        mainView.mon(mainView._control, 'cellclick', me.cellClickFun, mainView);
        mainView._resetChildrenData = function () {
            Ext.each(this._children, function (n, i, s) {
                if (n.model == 'SIE.Tech.Processs.ProcessParameter') {
                    me.resetEditorItems('Type');
                }
                else if (n.model == 'SIE.Tech.Processs.ProcessCollectStep') {
                    me.resetEditorItems('BarcodeType');
                }
                n.loadChildData();
            });
        }
        if (mainView.getChildren().length <= 0) return;
        var paramView = mainView.getChildren().first(function (m) { return m.model === 'SIE.Tech.Processs.ProcessParameter' });
        if (paramView) {
            paramView.mon(paramView._control, 'cellclick', SIE.Web.Tech.ProcessCommonFun.setStepResultEditorItems, paramView);
        }
        var stepView = mainView.getChildren().first(function (m) { return m.model === 'SIE.Tech.Processs.ProcessCollectStep' });
        if (stepView) {
            stepView.mon(stepView._control, 'cellclick', SIE.Web.Tech.ProcessCommonFun.setBarcodeTypeEditorItems, stepView);
        }
        //用了currentChanged后newValue的belongView会导致页面表头没了
        //页面生成后，需要加载出当前活动的子页签数据;
        mainView._resetChildrenData();
        return aggtView;
    },
    /**
     * 设置批次和单体显示的列
     * @param aggtMeta 聚合块元数据
     */
    cellClickFun: function (g, row, col, record, tr, rowindex) {
        var me = this;
        if (me.getChildren().length <= 0) return;
        var pType = record.data.Type;
        var stepView = me.getChildren().first(function (m) { return m.model === 'SIE.Tech.Processs.ProcessCollectStep' });
        if (stepView) {
            var stepControl = stepView.getControl();
            if (pType < 25) {
                Ext.each(stepControl.columns, function (item) {
                    var dIndex = item.dataIndex;
                    if (dIndex === "PlugType" || dIndex === "IsGenerateBatch") {
                        item.hide();
                    }
                    else {
                        item.show();
                    }
                });
            }
            else {
                Ext.each(stepControl.columns, function (item) {
                    if (item.dataIndex === "IsUnbound") {
                        item.hide();
                    }
                    else {
                        item.show();
                    }
                });
            }
        }
        //if (record.getIsOutsourcing() ===true) {
        //    record.setEnableMoveInControl(true);
        //}

    },
    resetEditorItems: function (name) {
        var stepCobox = Ext.ComponentQuery.query('[xtype=StepBarcodeTypeEditor]').first(function (p) { return p.name == name; });
        if (stepCobox) {
            if (stepCobox.AllData !== undefined) {
                var items = stepCobox.AllData.data.items;
                var newStore = new Ext.data.Store();
                newStore.data.add(items);
                stepCobox.setStore(newStore);
            }
        }
    }
});