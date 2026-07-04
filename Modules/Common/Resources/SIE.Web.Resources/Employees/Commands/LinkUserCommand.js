SIE.defineCommand('SIE.Web.Resources.Employees.Commands.LinkUserCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'empty', targetClassName: 'SIE.Rbac.Users.User' },
        gridCfg: { multiSelect: false, },
    },
    meta: { text: "关联账号", group: "edit", iconCls: "icon-LinkVariant icon-blue" },
    canExecute: function (view) {
        return view.getCurrent() !== null && view.getSelection().length == 1 && view.getCurrent().data.UserId === null;
    },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        var indata = {};
        var entity = this.view.getCurrent().data;
        var selections = this._targetView.getSelection();
        if (selections && selections.length === 1) {
            indata.Data = Ext.encode({ UserId: selections[0].data.Id, EmployeeId: entity.Id });
            me._targetView.execute({
                data: indata,
                success: function (res) {
                    win.close();  
                    me._ownerView.reloadData();
                },
                error: function () {
                    win.close();
                    me._ownerView.loadData();
                },
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    },
    //将弹出框中的用户里已关联员工的用户样式调整一下。
    getRowClass: function (record, index, rowParams, store) {
        if (record.data.EmployeeId !== null) {
            return 'gridRowLock';
        }
    }
});