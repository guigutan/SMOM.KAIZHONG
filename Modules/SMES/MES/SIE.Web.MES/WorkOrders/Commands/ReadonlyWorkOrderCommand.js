SIE.defineCommand('SIE.Web.MES.WorkOrders.ReadonlyWorkOrderCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看工单", group: "edit", iconCls: "icon-PageSearch icon-blue" },
    canExecute: function (listView) {
        return listView != null && listView.getSelection().length == 1;
    },
    /**
      * 显示界面绑定属性变更事件并设置默认数据
      * @param editEntity 当前实体      
      */
    showView: function (editEntity) {
        var me = this;
        CRT.Workbench.addPage({
            entityType: me.view.model,
            recordId: editEntity.getId(),
            viewGroup: "ReadonlyView",
            title: Ext.String.format('查看工单-{0}'.L10N(), editEntity.getNo()),
            isDetail: true,
            params: {
                token: me.view.token,
                action: 3
            }
        });
    },
});