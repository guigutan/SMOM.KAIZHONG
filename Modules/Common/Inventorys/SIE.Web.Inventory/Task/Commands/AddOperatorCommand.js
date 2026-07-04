SIE.defineCommand('SIE.Web.Inventory.Task.Commands.AddOperatorCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'EmployeeId', targetClassName: 'SIE.Resources.Employees.Employee' },
    },
    meta: { text: "添加", group: "edit", iconCls: "iconfont icon-AddEntity icon-green" },
    canExecute: function (view) {
        if (view.getParent().getSelection() == null || view.getParent().getSelection().length != 1) {
            return false;
        }
        var task = view.getParent().getCurrent();
        if (task != null && (task.data.State == SIE.Inventory.Task.TaskState.Create.value || task.data.State == SIE.Inventory.Task.TaskState.Release.value
                || task.data.State == SIE.Inventory.Task.TaskState.Appoint.value))
            return true;
        return false;
    },
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        me._sourceViewSelectItems = [];
        me.view.getData().data.items.forEach(function (item) {
            if (item)
                me._sourceViewSelectItems.push(parseFloat(item[0].EmployeeId));
        });
        //me._sourceViewSelectItems = this.cloneStore.collect(me.dataParams.specKeyPrototyName);
        var dialogView = me._targetView;
        if (me._targetView !== null) {
            var store = dialogView.getData();
            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) { //存在查询面板时
                    //me.setQueryCriteria(dialogView, me.view.getParent().getCurrent());
                    dialogView._relations[0]._target.tryExecuteQuery();
                    //me.setQueryCriteria(dialogView, me.view.getParent().getCurrent());
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = this._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var employeeId = item.getId();
                if (me._sourceViewSelectItems.indexOf(employeeId) === -1) {
                    var employee = { TaskManagementId: me._sourceId, Id: employeeId };
                    operationDatas.push(employee);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close();  //关闭模态窗口
                    me._ownerView.loadChildData(true); //重载视图数据
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
});