SIE.defineCommand("SIE.Web.LES.MaterialPreparations.Commands.SubmitPrepareCommand", {
    extend: 'SIE.Web.LES.MaterialPreparations.Commands.SavePrepareCommand',
    meta: { text: "提交", group: "edit", iconCls: "icon-Submit icon-blue" },
    canExecute: function (view) {
        var entity = view.getCurrent();
        if (entity == null) {
            return false;
        }
        if (entity.getPrepareStatus() != 0) {
            return false;
        }
        return true;
    },
    doSave: function (view) {
        var me = this;
        var entity = view.getCurrent();
        if (entity) {
            entity.dirty = true; // 继承保存用于触发dosave
            var childStore = view.findChild("SIE.LES.MaterialPreparations.MaterialPreparationDetail").getData();
            var data = childStore.data.items;
            for (var i = data.length - 1; i >= 0; i--) {
                data[i].dirty = true;
                if (entity.getPrepareType() != 2 && data[i].getQty() <= 0) {
                    childStore.splice(i, 1);
                }
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
    /**
    * override 重写保存后提示信息
    * @param {type} view
    * @param {type} res
    */
    onSavedMsg: function (view) {
        var isClose = false;
        view.getCurrent().markSaved();
        SIE.Msg.showInstantMessage('提交成功'.t(), '备料需求单'.t(), 3, function () {
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