Ext.define('SIE.Web.MES.TaskManagement.SchedulingInfs.Scripts.SchedulingInfBehavior', {
    endIndex: null,

    /**
     * view生命周期函数--view生成后
     * @param {DetailView} view 生成的view
     */
    onViewReady: function (view) {

    },

    onDataLoaded: function (view) {
        var me = this;
        var _view = view;
        //清空动态列
        this.clearColumn(_view);
        //添加动态列
        this.addDynamicColumn(_view);
    },

    //清空动态列
    clearColumn: function (_view) {
        var me = this;
        var gridPanel = _view.getControl();
        var position = _view.getControl().columns.length;
        var colIndexList = [];
        if (this.endIndex != null) {
            for (var i = this.endIndex; position <= i; i--) {
                //colIndexList.push(i);
                gridPanel.removeColumn(position);
            }
            //gridPanel.removeColumns(colIndexList.orderBy());
        }
    },

    //添加动态列
    addDynamicColumn: function (_view) {
        var me = this;
        var token = _view.token;
        var ids = _view.getData().data.items.select(p => p.getId());
        var gridPanel = _view.getControl();
        var result = null;
        if (ids == null || ids.length < 1)
            return;
        var position = _view.getControl().columns.length;
        var queryData = _view.getRelations()[0]._target.getCurrent().data;
        var data = _view.getData().data.items.select(p => p.data);
        SIE.invokeDataQuery({
            method: 'GetSchedulingInfValues',
            params: [ids],
            action: 'queryer',
            async: false,
            type: 'SIE.Web.MES.TaskManagement.SchedulingInfs.SchedulingInfDataQueryer',
            token: token,
            success: function (res) {
                if (res.Success) {
                    result = res.Result;
                    if (res.Result && res.Result.dates.length > 0) {

                        //添加动态列
                        var field_check = [];
                        var columns_check = [];

                        var dates = res.Result.dates;
                        dates.forEach(w => {
                            var field = {
                                name: w, type: 'string'
                            };

                            var columns = [];
                            columns.push({
                                text: w + 'value1', header: '白班', dataIndex: w + 'value1', dateTimeValue: w, sortable: false, editable: false, renderer: me.colorChangeEvent
                            });
                            columns.push({
                                text: w + 'value2', header: '晚班', dataIndex: w + 'value2', dateTimeValue: w, sortable: false, editable: false, renderer: me.colorChangeEvent
                            });
                            if (columns.length > 0) {
                                //合并的单元格,存起来
                                field_check.splice(0, 0, field);
                                //合并单位格下的每一列,存起来
                                columns_check.splice(0, 0, { text: w, columns: columns });
                                //以合并的单元格为主，每次所在位置+1
                                position = position + 1;
                            }
                        });

                        //将动态列加到界面上
                        if (field_check.length > 0 && columns_check.length > 0)
                            gridPanel.addColumns(field_check, columns_check, position);

                        
                        this.endIndex = position;
                        me.endIndex = position;
                        //开始对动态列填充值
                        res.Result.list.forEach(w => {
                            var first = _view.getData().data.items.first(p => p.getId() == w.SchedulingInfId);
                            if (first != null) {

                                first.set(w.DateStr + 'value1', w.Value1);
                                first.set(w.DateStr + 'value2', w.Value2);
                                first.set(w.DateStr + 'IsTask1', w.DispatchTask1Id);
                                first.set(w.DateStr + 'IsTask2', w.DispatchTask2Id);
                                first.dirty = false;
                            }

                        });


                    }
                }
            }
        });
    },

    colorChangeEvent: function (e, m, x, q) {
        if (e > 0)
        {
            if (m.column.header = '白班') {
                if (x.get(m.column.dateTimeValue + 'IsTask1') && x.get(m.column.dateTimeValue + 'IsTask1') > 0)
                    m.tdStyle = 'background:green';
            }
            if (m.column.header = '晚班') {
                if (x.get(m.column.dateTimeValue + 'IsTask2') && x.get(m.column.dateTimeValue + 'IsTask2') > 0)
                    m.tdStyle = 'background:green';
            }
        }
        return e;
    },
});