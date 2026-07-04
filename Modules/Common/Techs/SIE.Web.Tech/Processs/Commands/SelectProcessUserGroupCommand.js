/**
  * 选择工序命令
  * 
*/
SIE.defineCommand('SIE.Web.Tech.Processs.Commands.SelectProcessUserGroupCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'ProcessId', targetClassName: 'SIE.Tech.Processs.Process' },
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    /**
     * 重写保存方法
     * @param {win} 窗口
     */
    save: function (win) {
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems.items;
        if (selections && selections.length > 0) {
            var operationDatas = [];
            SIE.each(selections, function (item) {
                var processId = item.getId();
                if (me._sourceViewSelectItems.indexOf(processId) === -1) {
                    var processUserGroup = { UserGroupId: me._sourceId, ProcessId: processId };
                    operationDatas.push(processUserGroup);
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
            SIE.Msg.showWarning('没有可提交的数据'.t());
        }
    }
});
