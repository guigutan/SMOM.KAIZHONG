Ext.define('SIE.Web.Core.QmsStaticConst.Behaviors.ControlChartConstBehavior',
    {
        onViewReady: function (view) {
            var me = this;
            this._view = view;
            var grid = view.getControl();
            //默认值的单元格不可编辑
            grid.on("beforecelldblclick", me.beforecelldblclick, this);
        },
        beforecelldblclick: function (td, cellIndex, record, tr, rowIndex, e, eOpts) {
            var me = this;
            var record = tr.data;
            var defaultRowArray = [];//2-25为默认行，不能删除
            for (var i = 2; i <= 25; i++) {
                defaultRowArray.push(i);
            }
            if (Ext.Array.contains(defaultRowArray, record.SampleQty)) {

                if (tr.data[td.getColumnManager().columns[cellIndex.cellIndex].dataIndex] == null)
                    return true;//2到25样本数中值为null的可以编辑
                else
                    //2到25样本数中值不等于null的是默认数据，不可以编辑
                    return false;
            }

        },
    });
