Ext.define('SIE.Web.EMS.InventoryTasks.FixtureEncodeIDListBehavior', {
    _view: null,
    /**
          * view生命周期函数--view生成前
          * @param {*} meta 实体视图元数据
          * @param {*} curEntity 当前操作实体(可空)
          */
    beforeCreate: function (meta, curEntity) {
        me = this;
        _view = null;
        if (!meta) {
            return;
        }
        var gridConfig = meta.gridConfig;

        var toolBar = gridConfig.dockedItems.first(function (p) { return p.xtype == "toolbar" });
        if (!Ext.isEmpty(toolBar)) {

            var inputCode = new Ext.form.TextField({
                width: 200,
                //判断是否允许空白
                allowBlank: true,
                maxLength: 200,
                name: 'inputCode',
                fieldLabel: '',
                blankText: '工治具编码或序列号'
            });
            inputCode.setValue('');
            //控件插入工具栏
            toolBar.items.splice(4, 0, inputCode);
        }

    },
    onViewReady: function (view) {
        _view = view;
    },
    /**
     * view生命周期函数--view准备完成
     * @param {ListLogicView} view 生成的view
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
        if (e.property === 'FirstResult') {
            if (e.entity.data.InventoryAssetSource === 10 && e.value === 30) {
                e.entity.setFirstResult(null);
                SIE.Msg.showMessage("来源为【账内资产】,不能选择盘盈".t());
            } else {
                e.entity.setInventoryStatus(20);
            }
            if (e.value === 10) {//初盘结果为正常时，不能编辑，取值为当前值
                e.entity.setFirstStatus(e.entity.getFixtureStatus());
                e.entity.setFirstQualityState(e.entity.getFixtureQualityState());
            }
            if (e.value === 40) {//初盘结果为盘亏时，不能编辑，只能为空
                e.entity.setFirstStatus(null);
                e.entity.setFirstQualityState(null);
            }

        }
        if (e.property === 'SecondResult') {
            if (e.entity.data.InventoryAssetSource === 10 && e.value === 30) {
                e.entity.setSecondResult(null);
                SIE.Msg.showMessage("来源为【账内资产】,不能选择盘盈".t());
            }
            if (e.entity.data.InventoryAssetSource === 20 && (e.value === 10 || e.value === 20)) {
                e.entity.setSecondResult(null);
                SIE.Msg.showMessage("来源为【盘盈新增】,只可选【盘盈、盘亏】".t());
            }

            if (e.value === 10) {//复盘结果为正常时，不能编辑，取值为当前值
                e.entity.setSecondStatus(e.entity.getFixtureStatus());
                e.entity.setSecondQualityState(e.entity.getFixtureQualityState());
            }
            if (e.value === 40) {//复盘结果为盘亏时，不能编辑，只能为空
                e.entity.setSecondStatus(null);
                e.entity.setSecondQualityState(null);
            }
        }

    }
});