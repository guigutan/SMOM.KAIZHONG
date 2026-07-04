SIE.defineCommand('SIE.Web.RedCardManagment.RedCardApplyBills.Commands.StartWorkflowCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "提交", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        return true;
    },

    execute: function (view, source) {
        var me = this;
        SIE.Msg.askQuestion("是否发起流程？发起后申请单不能更改。".t(),
            function () {
                //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
                view.getCurrent().dirty = true;
                me.doSave(view);
            });
    },

    doSave: function (view) {
        var me = this;
        var children = view.getChildren();
        var withChildren = children.length > 0;
        var ctl = view.getControl();
        if (ctl && ctl.up() && ctl.up().up())
            ctl.up().up().setLoading(true); //开始提交
        view.execute({
            withChildren: withChildren,
            success: function (res) {
                me.onSaved(view, res);
            },
            callback: function (res) {
                if (ctl && ctl.up() && ctl.up().up())
                    ctl.up().up().setLoading(false); //提交结束
            }
        });
    },

    /**
     * @override
     * @param {} view 
     * @returns {} 
     */
    onSaved: function (view, res) {
        var me = this;
        
        //数据已保存到服务器，修改状态
        SIE.Web.Core.CommonFuns.markViewSaved(view);
        me.onSavedMsg(view, res);


        //当前页面提交后不再给修改
        var proChgHandlers = view.getProChgHandlers();
        if (proChgHandlers && proChgHandlers.length > 0)
            proChgHandlers.splice(0, proChgHandlers.length);
        view.getControl().items.items.forEach(function (ctl) {
            if (ctl.setReadOnly) {
                ctl.setReadOnly(true);
            }
        });
        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
        var currentTab = CRT.Workbench.getTabPanel().getActiveTab();
        CRT.Workbench.closeTab(currentTab);
    },
   /**
    * override 重写保存后提示信息
    * @param {type} view
    * @param {type} res
    */
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage('发起成功！'.t());
    },
});