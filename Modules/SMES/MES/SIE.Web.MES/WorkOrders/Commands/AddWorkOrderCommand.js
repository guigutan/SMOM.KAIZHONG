SIE.defineCommand('SIE.Web.MES.WorkOrders.AddWorkOrderCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", hierarchy: "工单生成", iconCls: "icon-AddEntity icon-green" },

    /**
        * 显示界面绑定属性变更事件并设置默认数据
        * @param editEntity 当前实体      
        */
    showView: function (editEntity) {
        var me = this;
        CRT.Workbench.addPage({
            entityType: this.view.model,
            recordId: editEntity.getId(),
            isNew: true,
            title: this.getEditViewTitle(editEntity),
            isDetail: true,
            params: {
                token: me.view.token,
                woTabId: CRT.Workbench.getTabPanel().getActiveTab().getId(),
                action: 0
            }
        });
    }
});