SIE.defineCommand('SIE.Web.Warehouses.Commands.InEmployeeWarehouseCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'WarehouseId',
            targetClassName: 'SIE.Warehouses.Warehouses.InWarehouseEmployeeSelect',
            targetCriteriaClassName: 'SIE.Warehouses.Warehouses.InWarehouseEmployeeSelectCriteria'
        },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = this._targetView.getSelection();
        if (selections!=null && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var warehouseId = item.getId();
                if (me._sourceViewSelectItems.indexOf(warehouseId) === -1) {
                    var warehouseEmployee = { WarehouseId: warehouseId, EmployeeId: me._sourceId };
                    operationDatas.push(warehouseEmployee);
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