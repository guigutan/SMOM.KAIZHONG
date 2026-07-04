Ext.define('SIE.Web.Core.QmsStaticConst.Behaviors.StaticConstK1Behavior',
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
            var defaultRowArray = [];//2-3为默认行，不能删除
            for (var i = 2; i <= 3; i++) {
                defaultRowArray.push(i);
            }
            if (Ext.Array.contains(defaultRowArray, record.TestQty)) {

                //默认数据不允许编辑
                return false;
            }
        },
    });
