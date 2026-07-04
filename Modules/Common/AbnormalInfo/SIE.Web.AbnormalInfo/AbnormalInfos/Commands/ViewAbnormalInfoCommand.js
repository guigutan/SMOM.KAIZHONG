SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalInfos.Commands.ViewAbnormalInfoCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看异常", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    /**
     * @override 修改页签ViewGroup
     * @param {any} opt  页签参数
     */
    addPage: function (opt) {
        var current = this.view.getCurrent();
        opt.viewGroup = "ReadOnlyView";
        var entityId = current.entityName + '-' + opt.viewGroup + '-' + current.getId();
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        opt.tabId = tabId;  //防止确认异常和查看异常按钮打开同一个页签
        var additionParams = {
            JoinDefectCodes: current.getJoinDefectCodes(),
            JoinDefectCodeDescriptions: current.getJoinDefectCodeDescriptions()
        };
        opt.params = opt.params || {};
        opt.parmas = Ext.merge(opt.params, additionParams);
        this.callParent(arguments);
    },
    
});