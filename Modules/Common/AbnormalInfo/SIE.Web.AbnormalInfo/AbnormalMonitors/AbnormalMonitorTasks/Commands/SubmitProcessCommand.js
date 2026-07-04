SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.Commands.SubmitProcessCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "提交", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        return view.getCurrent() &&
            view.getCurrent().getTaskState() != SIE.AbnormalInfo.Common.TaskStateEnum.Done
    },

    execute: function (view, source) {
        var me = this;
        SIE.Msg.askQuestion("是否提交？提交后任务不能处理。".t(),
            function () {
                //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
                view.getCurrent().dirty = true;
                me.doSave(view);
            });
    },

    /**
     * @override
     * @param {} view 
     * @returns {} 
     */
    onSaved: function (view, res) {
        var me = this;
        var ent = view.getCurrent();


        ent.setTaskState(SIE.AbnormalInfo.Common.TaskStateEnum.Done); //改为处理完成
        
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
        var viewAttach = view.getChildren()[0];//附件清单
        if (viewAttach) {
            viewAttach.getCommands().items.filter(function (p) { return p.meta.command === 'SIE.Web.Common.Attachments.Commands.UploadAttachmentCommand' || p.meta.command === 'SIE.Web.Common.Attachments.Commands.DeleteAttachmentCommand' }).forEach(function (item) {
                item.canVisible =
                    function (view, source) {
                        var parent = view.getParent().getCurrent();
                        return parent && parent.getTaskState() != SIE.AbnormalInfo.Common.TaskStateEnum.Done;
                    }
            });
            viewAttach.syncCmdState();
        }

        view.getChildren().forEach(function (checkView) {
            checkView.setIsReadonly(true);
        });
        CRT.Event.fire(view.model + "_refresh", view.getCurrent().data.Id);
    },
   /**
    * override 重写保存后提示信息
    * @param {type} view
    * @param {type} res
    */
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage('提交成功！'.t());
    },
});