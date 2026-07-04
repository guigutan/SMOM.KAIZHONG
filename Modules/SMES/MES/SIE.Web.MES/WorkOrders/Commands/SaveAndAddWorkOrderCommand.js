SIE.defineCommand('SIE.Web.MES.WorkOrders.SaveAndAddWorkOrderCommand', {
    extend: 'SIE.Web.MES.WorkOrders.SaveWorkOrderCommand',
    meta: { text: "保存添加", group: "edit", iconCls: "icon-SaveIncrease icon-blue" },
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        current.markSaved();
        SIE.Msg.showInstantMessage('保存成功'.t(), '工单添加', 2, function () {
            Ext.MessageBox.hide();
        });
        //这里延迟处理，等待当前视图资源执行完毕，否则在IE中报错
        window.setTimeout(function () {
            me.SaveAndAdd(view);
        }, 1000);
    },
    /**
     * 保存并添加命令
     * @param {any} view 视图
     */
    SaveAndAdd: function (view) {
        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
        var tabPanel = CRT.Workbench.getTabPanel();
        if (tabPanel) {
            var tab = tabPanel.getActiveTab();
            if (tab) {
                tabPanel.remove(tab);
                CRT.Event.fire("workOrderAdd");
            }
        }
    }
});