SIE.defineCommand('SIE.Web.Resources.Employees.Commands.ChangeGroupCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'empty', targetClassName: 'SIE.Resources.Employees.WorkGroup' },
        gridCfg: { multiSelect: false, },
    },
    meta: { text: "转班组", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        return view.getCurrent() !== null;
    },

    getRowClass: function (record, index, rowParams, store) {
        if (this.view.getCurrent().getWorkGroupId() == record.getId()) {
            return 'gridRowLock';
        }
    },
    save: function (win) {
        var me = this;
        var entityList = this.view.getSelection();
        var list = [];
        var selections = this._targetView.getSelection();
        if (selections && selections.length === 1) {
            for (var i = 0; i < entityList.length; i++) {
                entityList[i].data.WorkGroupId = selections[0].data.Id;
                list[i] = entityList[i].data;
            }
            me._targetView.execute({
                data: list,
                success: function (res) {
                    SIE.Msg.showInstantMessage('班组转换成功'.t());
                    win.close();
                    me._ownerView.loadChildData(true);
                    me._ownerView.getParent().reloadData();
                }
            }, me._ownerView);
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
});