SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalInfos.Commands.ConfirmAbnormalInfoCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "确认异常", group: "edit", iconCls: "icon-EditEntity icon-blue" },

    /**
    * @override 是否可执行
    * @param {} view 
    * @returns {} 
    */
    canExecute: function (view) {
        var current = view.getCurrent();
        if (!current || current.getAbnormalStatus() == SIE.AbnormalInfo.AbnormalInfos.AbnormalStatus.Close)
            return false;
        if (!Ext.isEmpty(current.getHandlersDisplay())) {
            var handlers = current.getHandlersDisplay().split(',');
            var curName = CRT.Context.GlobalContext.getContext('userInfo').Name;
            if (handlers.indexOf(curName) < 0)  //处理人不为空，且不包括当前人时，不可操作
                return false;
        }
        return true;
    },

    /**
     * @override 修改页签ViewGroup
     * @param {any} opt  页签参数
     */
    addPage: function (opt) {
        var current = this.view.getCurrent();
        opt.viewGroup = "ConfirmView";
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