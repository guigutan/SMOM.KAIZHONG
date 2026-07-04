Ext.define('SIE.Web.Fixtures.InboundOrders.Scripts.InboundOrdersChildListBehavior',
    {
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {
            meFixtureModelBehavior = this;
            meFixtureModelBehavior.myView = view;
            var gridPanel = view.getControl();

            if (gridPanel.actionables) {
                var grid = gridPanel.actionables[0].grid;
                grid.mon(grid, 'cellclick', meFixtureModelBehavior.cellclick, meFixtureModelBehavior);
            }
        },

        /**
         * onEntityPropertyChanged 属性变更事件
         * @param {*} e 参数
         */
        onEntityPropertyChanged: function (e) {
            if (e.property.length > 0) {
                var entity = e.entity;
                var parent = meFixtureModelBehavior.myView.getParent().getCurrent();
                if (e.property === 'StorageLocationId_Display' && parent != null) {
                    if (e.value != e.oldvalue) {
                        if (e.value == "" && e.oldvalue != "") {
                            parent.setScanedNum(parent.getScanedNum() -1);
                        }
                        if (e.value != "" && e.oldvalue == "") {
                            parent.setScanedNum(parent.getScanedNum() + 1);
                        }
                    }
                    
                }
            }
        },
        cellclick: function (t, td, cellIndex, record, tr, rowIndex, e, eOpts) {
            if (record != null) {
                meFixtureModelBehavior.myView.mun(record, 'propertyChanged', this.onEntityPropertyChanged, meFixtureModelBehavior.myView);
                meFixtureModelBehavior.myView.mon(record, 'propertyChanged', this.onEntityPropertyChanged, meFixtureModelBehavior.myView);
            }
        },
        
    });



