Ext.define('SIE.Web.EMS.InventoryTasks.FixtureEncodeListBehavior',
    {
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
                blankText:'工治具编码或序列号'
            });
            inputCode.setValue('');
            //控件插入工具栏
            toolBar.items.splice(4, 0,inputCode);
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

        if (e.property === 'FirstStockPassQty' || e.property === 'FirstOnline' || e.property === 'FirstStockNgQty') {
            var  totalFirst = e.entity.getFirstStockPassQty() + e.entity.getFirstOnline() + e.entity.getFirstStockNgQty();
            e.entity.setFirstTotal(totalFirst);
            var diff = e.entity.getFirstTotal() - e.entity.getTotal();
            e.entity.setFirstDiff(diff);
            //校验FirstGoodStock/FirstNgStock/FirstOnline三个字段同时为0则为0；同时为空时表示清空带“初盘”字段数据
            if (e.entity.getFirstStockPassQty() === "" && e.entity.getFirstOnline() === "" && e.entity.getFirstStockNgQty() ==="") {
                e.entity.setFirstTotal("");
                e.entity.setFirstDiff("");
            }

            // 根据初盘差异数变更初盘结果和盘点状态
            if (diff > 0) {
                e.entity.setFirstResult(30);
            }
            else if (diff < 0) {
                e.entity.setFirstResult(40);
            }
            else if (diff === 0 && (e.entity.getFirstStockPassQty() !== e.entity.getStockPassQty() || e.entity.getFirstStockNgQty() !== e.entity.getStockNgQty() || e.entity.getFirstOnline() !== e.entity.getOnline())) {
                e.entity.setFirstResult(20);
            }
            else if (diff === 0 && (e.entity.getFirstStockPassQty() === e.entity.getStockPassQty() && e.entity.getFirstStockNgQty() === e.entity.getStockNgQty() && e.entity.getFirstOnline() === e.entity.getOnline())) {
                e.entity.setFirstResult(10);
            }
            else {
                //
            }

            // 初盘结果有值
            if (e.entity.getFirstResult() !== null) {
                e.entity.setInventoryStatus(20);
            }
        }

        if (e.property === 'SecStockPassQty' || e.property === 'SecondOnline' || e.property === 'SecStockNgQty') {
            var totalFirst = e.entity.getSecStockPassQty() + e.entity.getSecondOnline() + e.entity.getSecStockNgQty();
            e.entity.setSecondTotal(totalFirst);
            var diff = e.entity.getSecondTotal() - e.entity.getTotal();
            e.entity.setSecondDiff(diff);
            //校验FirstGoodStock/FirstNgStock/FirstOnline三个字段同时为0则为0；同时为空时表示清空带“初盘”字段数据
            if (e.entity.getSecStockPassQty() === "" && e.entity.getSecondOnline() === "" && e.entity.getSecStockNgQty() === "") {
                e.entity.setSecondTotal("");
                e.entity.setSecondDiff("");
            }

        }
            
    }
});