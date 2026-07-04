SIE.defineCommand("SIE.Web.EMS.EquipLends.Commands.EquipLendSaveSubmitCommand", {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "提交", group: "edit", iconCls: "icon-Submit icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view) {
        var me = this;
        var entity = view.getCurrent();
        // 标记脏数据提交到后台
        entity.dirty = true;
        var errorMsg = "";
        if (entity == null) {
            SIE.Msg.showError("页面数据错误，请刷新！".t());
            return;
        }
        if (entity.getEquipAccountId() == null) {
            errorMsg += "设备编码必填;".t() + '\r\n';
        }
        if (entity.getLendObject() == 0) { // 内部
            if (entity.getLendEnterpriseId() == null) {
                errorMsg += "借机部门必填;".t() + '\r\n';
            }
            if (entity.getLendEmployeeId() == null) {
                errorMsg += "借机人必填;".t() + '\r\n';
            }
        }
        else { // 外部
            if (entity.getSupplierId() == null) {
                errorMsg += "供应商必填;".t() + '\r\n';
            }
        }
        if (entity.getReason().length <= 0) {
            errorMsg += "借出原因必填;".t() + '\r\n';
        }
        if (errorMsg.length > 0) {
            SIE.Msg.showError(errorMsg);
            return;
        }
        me.doSave(view);
    },

    onSavedMsg: function (view, res) {
        var isClose = false;
        SIE.Msg.showInstantMessage('提交成功'.t(), "", 3, function () {
            isClose = true;
            CRT.Workbench.closeCurrentTab();
        });
        if (!isClose) {
            Ext.defer(function () {
                CRT.Workbench.closeCurrentTab();
            }, 3000);
        }
    },
})

