Ext.define('SIE.Web.WorkBenchCommon.Workbench.KPI.Behaviors.QuotaTargetSettingEditBehavior',
    {
        _view: null,
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {

            var me = this;
            this._view = view;
            this.InitChildrenView(view);
            var entity = view.getCurrent();  
            view.mon(entity, 'propertyChanged', this.PropertyChanged, this);
        },

        PropertyChanged: function (e) {
            var entity = e.entity;
            if (e.property.length > 0) {
                if (e.property.indexOf('DataType') >= 0) {
                    var data = e.entity;
                    var value = e.value;
                    if (this._view.getChildren().length < 1) return;
                    var childrenView = this._view.getChildren()[0];
                    var columns = childrenView.getControl().columnManager.columns || childrenView.getControl().columns;
                    if (columns == null) return;
                    var yearColumn = columns.first(function (c) {return c.dataIndex == "Year"});
                    var weekColumn = columns.first(function (c) {return c.dataIndex == "Week"});
                    var monthColumn = columns.first(function (c) { return c.dataIndex == "Month" });

                    var store = childrenView.getData();

                    if (value == SIE.WorkBenchCommon.Workbench.KPI.DateType.YEAR) {
                        //数据设置
                        store.getData().items.forEach(function (c) { c.setDataType(value); c.setMonth(null); c.setWeek(null); });
                        //设置列不显示
                        yearColumn.setVisible(true);
                        weekColumn.setVisible(false);
                        monthColumn.setVisible(false);
                    }
                    else if (value == SIE.WorkBenchCommon.Workbench.KPI.DateType.MONTH) {
                        //数据设置
                        store.getData().items.forEach(function (c) { c.setDataType(value);  c.setWeek(null); });
                        //设置列不显示
                        yearColumn.setVisible(true);
                        weekColumn.setVisible(false);
                        monthColumn.setVisible(true);
                    }
                    else if (value == SIE.WorkBenchCommon.Workbench.KPI.DateType.WEEK) {
                        //数据设置
                        store.getData().items.forEach(function (c) { c.setDataType(value); c.setMonth(null); });
                        //设置列不显示
                        yearColumn.setVisible(true);
                        weekColumn.setVisible(true);
                        monthColumn.setVisible(false);
                    }
                }

                if (e.property.indexOf('Code') >= 0) {
                    this._view.getCurrent().setName(null);
                }
            }
        },

        InitChildrenView: function () {
            if (this._view.getChildren().length < 1) return;
            var childrenView = this._view.getChildren()[0];
            var columns = childrenView.getControl().columnManager.columns || childrenView.getControl().columns;
            if (columns == null) return;
            var yearColumn = columns.first(function (c) { return c.dataIndex == "Year" });
            var weekColumn = columns.first(function (c) { return c.dataIndex == "Week" });
            var monthColumn = columns.first(function (c) { return c.dataIndex == "Month" });

            var value = this._view.getCurrent().getDataType();  

            if (value == SIE.WorkBenchCommon.Workbench.KPI.DateType.YEAR) {

                //设置列不显示
                yearColumn.setVisible(true);
                weekColumn.setVisible(false);
                monthColumn.setVisible(false);
            }
            else if (value == SIE.WorkBenchCommon.Workbench.KPI.DateType.MONTH) {

                //设置列不显示
                yearColumn.setVisible(true);
                weekColumn.setVisible(false);
                monthColumn.setVisible(true);
            }
            else if (value == SIE.WorkBenchCommon.Workbench.KPI.DateType.WEEK) {

                //设置列不显示
                yearColumn.setVisible(true);
                weekColumn.setVisible(true);
                monthColumn.setVisible(false);
            }
        }
    });