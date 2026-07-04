SIE.defineCommand('SIE.Web.EMS.SpecialEquipment.RegularInspections.Commands.InputSubmitCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "提交", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },

    /**
     * @override 是否可执行
     * @param {} view 
     * @returns {} 
     */
    canExecute: function (view) {
        var current = view.getCurrent();
        if (current) {
            var ApprovalStatu = current.getApprovalStatus();
            var InspectionStatu = current.getInspectionStatus();
            return ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.PendingReview.value
                && ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.UnderReview.value
                && ApprovalStatu !== SIE.Equipments.Enums.ApprovalStatus.Audited.value
                && InspectionStatu !== SIE.EMS.Enums.InspectionStatus.Calirated.value;
        }

        return this.callParent(arguments);
    },

    /**
     * @override 执行提交
     * @returns {} 
     */
    execute: function (view, source) {
        var me = this;
        var isValidator = me.onSaving(view);
        if (isValidator) {
            SIE.Msg.askQuestion("是否提交？提交后单据不能修改。".t(),
                function () {
                    //提交时，数据设置为脏，重新保存并校验所有内容,包括明细。
                    view.getCurrent().dirty = true;
                    me.doSave(view);
                });
        }
    },

    /**
    * 视图数据提交保存回调处理
    * @param view 当前视图
    */
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
 *  保存后事件
 * @param {} view
 * @returns {}
 */
    onSaved: function (view, res) {
        var current = view.getCurrent();
        current.markSaved();
        SIE.Msg.showInstantMessage('提交成功！'.t());
        window.setTimeout(function () {
            CRT.Event.fire(view.model + "_refresh", current.data.Id);
            CRT.Workbench.closeCurrentTab();
        }, 2000);
    }
});