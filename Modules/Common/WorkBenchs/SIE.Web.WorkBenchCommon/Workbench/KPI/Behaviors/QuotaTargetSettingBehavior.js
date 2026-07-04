Ext.define('SIE.Web.WorkBenchCommon.Workbench.KPI.Behaviors.QuotaTargetSettingBehavior',
    {
        _view: null,
        /**
         * view生命周期函数--view聚合后
         * @param {*} view 生成的view
         */
        onViewReady: function (view) {

            var me = this;
            this._view = view;
            view.mon(view, "selectionChanged", me.onCustomSelectionChanged, this);//选中行事件
        },

        onCustomSelectionChanged: function (e) {
            var me = this;
            var value = this._view.getCurrent().getDataType();
            if (this._view.getChildren().length < 1) return;
            var childrenView = this._view.getChildren()[0];
            var columns = childrenView.getControl().columnManager.columns || childrenView.getControl().columns;
            if (columns == null) return;
            var yearColumn = columns.first(function (c) {
                return c.dataIndex == "Year"
            });
            var weekColumn = columns.first(function (c) {
                return c.dataIndex == "Week"
            });
            var monthColumn = columns.first(function (c) {
                return c.dataIndex == "Month"
            })

            if (value == SIE.WorkBenchCommon.Workbench.KPI.DateType.YEAR) {
                yearColumn.setVisible(true);
                weekColumn.setVisible(false);
                monthColumn.setVisible(false);
            }
            else if (value == SIE.WorkBenchCommon.Workbench.KPI.DateType.MONTH) {
                yearColumn.setVisible(true);
                weekColumn.setVisible(false);
                monthColumn.setVisible(true);
            }
            else if (value == SIE.WorkBenchCommon.Workbench.KPI.DateType.WEEK) {
                yearColumn.setVisible(true);
                weekColumn.setVisible(true);
                monthColumn.setVisible(false);
            }

        }
    });