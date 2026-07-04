SIE.defineCommand('SIE.Web.EMS.EarlierStage.Projects.Commands.SelectProjectMemberCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'EmployeeId', targetClassName: 'SIE.Resources.Employees.Employee' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        return true;
    },
    onLoad: function (store, records, successful, operation, eOpts) {
        /// <summary>
        /// 根据数据实现勾选上
        /// </summary>
        var me = this;
        me._ownerView.getData().data.items.forEach(item => {
            me._sourceViewSelectItems.push(item.getEmployeeId());
        });
        if ((me._sourceViewSelectItems && me._sourceViewSelectItems.length > 0)
            || (me._targetSelectItems && me._targetSelectItems.items.length > 0)) {
            var selModel = me._targetView.getSelectionModel();
            if (records && records.length > 0) {
                for (var i = 0, len = records.length; i < len; i++) {
                    var record = records[i];
                    if (me._sourceViewSelectItems.indexOf(record.getId()) > -1) {
                        selModel.select(record, true, true); //勾选上.
                    }
                    if (me._targetSelectItems.keys.indexOf(record.getId()) > -1) {
                        selModel.select(record, true, true);
                    }
                }
            }
        }
    },
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var datas = selections;
            var childData = me.view.getData();
            SIE.each(datas, function (item) {
                var model = SIE.getModel('SIE.EMS.EarlierStage.Projects.ProjectMember');
                var entity = new model();
                entity.setEmployeeId_Display(item.getCode());
                entity.setEmployeeId(item.getId());
                entity.setEmployeeCode(item.getCode());
                entity.setEmployeeName(item.getName());
                entity.setPhone(item.getPhone());
                childData.add(entity);
            });
            win.close();
        }
        else {
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    },
});