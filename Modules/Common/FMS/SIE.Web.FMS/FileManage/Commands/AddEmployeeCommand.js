SIE.defineCommand('SIE.Web.FMS.FileManage.Commands.AddEmployeeCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'EmployeeId', targetClassName: 'SIE.Resources.Employee' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        /* post数据结构*/
        var indata = {};
        /* post数据结构*/
        var selections = this._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var employeeId = item.getId();
                if (me._sourceViewSelectItems.indexOf(employeeId) === -1) {
                    var userInFileUserGroup = { FileUserGroupId: me._sourceId, EmployeeId: employeeId };
                    operationDatas.push(userInFileUserGroup);
                }
            });
            indata = operationDatas;
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close();
                    me._ownerView.loadChildData(true);
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.L10N());
        }
    }
});
