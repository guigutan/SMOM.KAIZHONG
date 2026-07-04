SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.WritingRuleCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "编写规则", group: "edit", iconCls: "icon-ClipboardVariantEdit icon-blue" },
    canExecute: function (view) {
        return view.getCurrent();
    },
    execute: function (view, source) {
        var me = this;
        var indata = {};
        var editEntity = this.getEditEntity();
        indata.Data = Ext.encode(editEntity.data);
        view.execute({
            data: indata,
            success: function (res) {
                if (res.Success) {
                    me.onEditting(editEntity);
                    me.edit(editEntity);
                    me.onEdited(editEntity);
                    me.showView(editEntity);
                }
            },
            error: function (res) {
                Ext.Msg.alert('提示', res.Message);
            }
        });
    },
    //弹出页签效果
    showView: function (editEntity) {
        var me = this;
        CRT.Workbench.addPage({
            entityType:"SIE.AbnormalInfo.AbnormalMonitors.AbnormalDecisionRule",
            recordId: editEntity.getId(),
            title: me.getEditViewTitle(editEntity),
            viewGroup: "WritingRule",
            isDetail: true,
            ignoreQuery: true,
            params: {
                Id: editEntity.getId(),
                MonitorName: editEntity.getMonitorName(),
                MonitorType: editEntity.getMonitorType(),
                MonitorTabName: editEntity.getMonitorTabName()
            },
            pageClass: 'SIE.Web.AbnormalInfo.AnomalyMonitors.WritingRuleViewPage'
        });
    }
});