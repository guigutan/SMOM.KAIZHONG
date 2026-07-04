SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.SaveTaskCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
     * override 重写保存方法
     * @param {} view 
     * @returns {} 
     */
    doSave: function (view) {
        var me = this;
        var children = view.getChildren();
        var withChildren = children.length > 0;
        view.execute({
            withChildren: withChildren,
            success: function (res) {
                view.mon(view, 'beforeClosewin', me.closeAddView);    //关闭页签时添加处理   
                me.onSavedHandler(view, res);
            }
        });
    },
    /**
    * 关闭新增页面
    * @param {any} returnObj
    */
    closeAddView: function (returnObj) {
        var data = this.getCurrent();
        returnObj.data = data;
        returnObj.hasData = false;
    },
    /**
     * 重写保存后方法，保存后打开填写检验报告
     * @param {} view 
     * @returns {} 
     */
    onSavedHandler: function (view, res) {
        var me = this;
        SIE.Msg.showInstantMessage('保存成功！'.t(), '提示', 3);
        var current = view.getCurrent();
        SIE.Web.Core.CommonFuns.markSaved(current);
        if (res && res.Result) {
            CRT.Event.fire(view.model + "_refresh", current.data.Id);
            var currentTab = CRT.Workbench.getTabPanel().getActiveTab();
            CRT.Workbench.closeTab(currentTab);
        }
    },
});