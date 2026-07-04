SIE.defineCommand("SIE.Web.LES.MaterialReturnApplys.Commands.MaterialReturnSubmitCommand", {
    extend: 'SIE.Web.LES.MaterialReturnApplys.Commands.MaterialReturnSaveCommand',
    meta: { text: "提交", group: "edit", iconCls: "icon-Submit icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null) {
            return false;
        }
        if (entity.getReStatus() != 0) {
            return false;
        }
        return true;
    },
    doSave: function (view) {
        var me = this;
        var entity = view.getCurrent();
        if (entity) {
            entity.dirty = true; // 继承保存用于触发dosave
            var childStore = view.findChild("SIE.LES.MaterialReturnApplys.MaterialReturnApplyDetail").getData();
            var data = childStore.data.items;
            for (var i = 0; i < data.length; i++) {
                data[i].dirty = true;
            }
        }
        var children = view.getChildren();
        var withChildren = children.length > 0;
        SIE.Msg.wait("正在提交中,请稍等......".t());
        view.execute({
            withChildren: withChildren,
            success: function (res) {
                me.onSaved(view, res);
            }
        });
    },
    onSavedMsg: function (view) {
        var isClose = false;
        view.getCurrent().markSaved();
        SIE.Msg.showInstantMessage('提交成功'.t(), '退料申请'.t(), 3, function () {
            isClose = true;
            CRT.Workbench.closeCurrentTab();
        });
        if (!isClose) {
            Ext.defer(function () {
                CRT.Workbench.closeCurrentTab();
            }, 3000);
        }
    }
})