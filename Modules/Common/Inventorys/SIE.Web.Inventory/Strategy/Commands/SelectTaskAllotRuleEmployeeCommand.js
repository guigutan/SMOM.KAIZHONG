SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.SelectTaskAllotRuleEmployeeCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'EmployeeId', targetClassName: 'SIE.Resources.Employees.Employee' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = this._targetView.getSelection();
        if (selections!=null && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var employeeId = item.getId();
                if (me._sourceViewSelectItems.indexOf(employeeId) === -1) {
                    var employeeData = { TaskAllotRuleId: me._sourceId, EmployeeId: employeeId };
                    operationDatas.push(employeeData);
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
    // end 
});