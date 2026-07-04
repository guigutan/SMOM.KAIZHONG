Ext.define('SIE.Web.EMS.InventoryTasks.InventoryTaskSparePartDetailBehavior', {
    /*
     * view生命周期函数--view生成前
     * @param {*} meta 实体视图元数据
     * @param {*} curEntity 当前操作实体(可空)
     */
    beforeCreate: function (meta, curEntity) {
        if (!meta) {
            return;
        }

        var gridConfig = meta.gridConfig;

        var toolBar = gridConfig.dockedItems.first(function (p) {
            return p.xtype == "toolbar"
        });

        if (!Ext.isEmpty(toolBar)) {
            var txtSparePartDetailKeyWord = new Ext.form.TextField({
                width: 250,
                allowBlank: true,
                name: 'txtSparePartDetailKeyWord',
                labelAlign: 'right',
                fieldLabel: '',
                emptyText: '请输入备件编码/批次号/序列号'.t(),
            });

            //控件插入工具栏
            toolBar.items.splice(2, 0, txtSparePartDetailKeyWord);
        }
    },
    /**
     * view生命周期函数--view聚合后
     * @param {*} view 生成的view
     */
    onViewReady: function (view) {
        var headers = view.gridConfig.columns.filter(function (p) { return p.header == "初盘" || p.header == "复盘" });
        headers.forEach(function (p) {
            p.columns.forEach(function (item, idx, arr) {
                if (item.readonlyLambda && item.readonlyLambda != "") {
                    let func = view.getFunc(item.readonlyLambda);
                    view.addProChgHandler({ pro: item.dataIndex, effect: 'setReadOnly', lambda: func });
                }
                if (item.cascade && item.cascade.length > 0) {
                    item.cascade.forEach(function (e, i, arrc) {
                        let func = view.getFunc(e);
                        view.addProChgHandler({ pro: item.dataIndex, effect: 'cascade', lambda: func });
                    });
                }
            });
        });
    },
    /**
    * view生命周期函数--数据加载后
    * @param {any} view
    */
    onDataLoaded: function (view) {
        var me = this;
        var store = view.getData();
        view.mon(store, 'propertyChanged', me.onEntityPropertyChanged, view);
    },
    /**
     * 属性变更事件
     * @param {any} e
     */
    onEntityPropertyChanged: function (e) {
        var entity = e.entity;
        var entityData = e.entity.data;

        if (e.property === 'FirstGood' || e.property === 'FirstNg') {
            var firstGoodQty = (entityData.FirstGood == null) ? 0 : entityData.FirstGood;
            var firstNgQty = (entityData.FirstNg == null) ? 0 : entityData.FirstNg;
            entity.setFirstTotal(firstGoodQty + firstNgQty);

            //序列号管控 良品数+不良品数；序列号管控的校验不能大于1
            if (entityData.ControlMethod == 30 && entityData.FirstTotal > 1) {
                
                if (e.property === 'FirstGood') {
                    entity.setFirstGood(1);
                    entity.setFirstNg(0);
                } else {
                    entity.setFirstGood(0);
                    entity.setFirstNg(1);
                }

                entity.setFirstTotal(1);

                SIE.MessageBox.showMessage("序列号管控的[良品数+不良品数]不能大于1".t());
            }

            entity.setFirstDiff(entityData.FirstTotal - entityData.Total);

            if (entityData.FirstDiff > 0) {
                //初盘差异数大于0时，更新为【盘盈】
                entity.setFirstResult(30);
            } else if (entityData.FirstDiff < 0) {
                //初盘差异数小于0时，更新为【盘亏】
                entity.setFirstResult(40);
            } else if (entityData.FirstDiff == 0
                && entityData.GoodQty == firstGoodQty
                && entityData.NgQty == firstNgQty) {
                //初盘差异数等于0，且初盘良品数和与良品数相等、初盘不良品数和不良品数相等更新为【正常】
                entity.setFirstResult(10);
            } else {
                //其他场景，更新为【信息变动】
                entity.setFirstResult(20);
            }

            //状态	更新为【已盘点】
            entity.setInventoryStatus(20);
        }

        if (e.property === 'SecondGoodQty' || e.property === 'SecondNgQty') {
            var secondGoodQty = (entityData.SecondGoodQty == null) ? 0 : entityData.SecondGoodQty;
            var secondNgQty = (entityData.SecondNgQty == null) ? 0 : entityData.SecondNgQty;
            entity.setSecondTotal(secondGoodQty + secondNgQty);

            //序列号管控 良品数+不良品数；序列号管控的校验不能大于1
            if (entityData.ControlMethod == 30 && entityData.SecondTotal > 1) {
                if (e.property === 'SecondGoodQty') {
                    entity.setSecondGoodQty(1);
                    entity.setSecondNgQty(0);
                } else {
                    entity.setSecondGoodQty(0);
                    entity.setSecondNgQty(1);
                }
                entity.setSecondTotal(1);

                SIE.MessageBox.showMessage("序列号管控的[良品数+不良品数]不能大于1".t());                
            }

            entity.setSecondDiff(entityData.SecondTotal - entityData.Total);

            if (entityData.SecondDiff > 0) {
                //初盘差异数大于0时，更新为【盘盈】
                entity.setSecondResult(30);
            } else if (entityData.SecondDiff < 0) {
                //初盘差异数小于0时，更新为【盘亏】
                entity.setSecondResult(40);
            } else if (entityData.SecondDiff == 0
                && secondGoodQty == entityData.GoodQty
                && secondNgQty == entityData.NgQty) {
                //初盘差异数等于0，且初盘良品数和与良品数相等、初盘不良品数和不良品数相等更新为【正常】
                entity.setSecondResult(10);
            } else {
                //其他场景，更新为【信息变动】
                entity.setSecondResult(20);
            }
        }
    }
});