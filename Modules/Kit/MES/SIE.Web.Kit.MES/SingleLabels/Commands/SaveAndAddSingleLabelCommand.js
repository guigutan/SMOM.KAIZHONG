SIE.defineCommand('SIE.Web.Kit.Mes.SingleLabels.Commands.SaveAndAddSingleLabelCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveIncrease icon-blue" },
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        if (current.isNew()) {
            current.markSaved();  
            SIE.Msg.showInstantMessage('保存成功', '单体条码添加'.t(), 3, function () {
                Ext.MessageBox.hide();
            });
            //这里延迟处理，等待当前视图资源执行完毕，否则在IE中报错
            window.setTimeout(function () {
                me.SaveAndAdd(view);
            }, 1000);

        } else {
            current.markSaved();                                                                                                                     
            CRT.Event.fire(view.model + '_refresh', view.getCurrent().getId());
            view.getChildren().forEach(function (item) {
                item._curPid = "";
                item.reloadData(); //重新刷新子列表的数据
            });
            view.setCurrent(view.getCurrent(), true);//重新刷新store保持当前行数据跟数据库相同
            me.onSavedMsg(view, res);
        }
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
                CRT.Event.fire("singleLabelAdd");
                tabPanel.remove(tab);
            }
        }
    }
});