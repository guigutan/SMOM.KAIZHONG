SIE.defineCommand('SIE.Web.EMS.Lubrications.Commands.AddReportCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "添加记录", group: "edit", iconCls: "icon-PageSearch icon-blue" },
    /**
      * 显示界面绑定属性变更事件并设置默认数据
      * @param editEntity 当前实体      
      */
    canExecute: function (View) {
        var entity = View.getCurrent();
        var res = false;
        if (entity != null && entity.data) {
            if ((entity.data.LubricationStatus == SIE.EMS.Enums.LubricationStatus.Pending.value ||
                entity.data.LubricationStatus == SIE.EMS.Enums.LubricationStatus.Doing.value
            ) && View != null && View.getSelection().length == 1) {
                res = true;
            }
        }
        return res;
    },
    showView: function (editEntity) {
        var me = this;
        var entityId = editEntity.entityName + '-' + "AddReportView" + '-' + editEntity.getId();
        var tabId = ('tab_' + entityId.replace(/\./g, '')).replace(/[.|,]/g, '');
        CRT.Workbench.addPage({
            tabId: tabId,
            recordId: editEntity.data.Id,
            ignoreQuery: true,
            title: me.getEditViewTitle(editEntity),
            entityType: this.view.model,
            viewGroup: "AddReportView",
            isDetail: true,
            params: {
                action: 0
            }
        });
    }
});