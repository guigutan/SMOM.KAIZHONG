SIE.defineCommand('SIE.Web.Fixtures.Accounts.Commands.SaveIDAccountCommand', {
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    isCanExecute: true,
    canExecute: function (view) {
        var me = this;
        return me.isCanExecute;
    },
    execute: function (view, source) {
        var me = this;
        //防止多次重复点击保存
        if (this.isExecuting == true) return;
        this.isExecuting = true;
        setTimeout(function () { me.isExecuting = false; }, 10000, me);
        var current = view.getCurrent();
        var isValidation = me.validation(current);
        if (isValidation !== "") {
            me.isCanExecute = true;
            SIE.Msg.showInstantMessage(isValidation);
            return false;
        }
        var indata = {};
        indata.Data = Ext.encode(current.data);
        view.execute({
            data: indata,
            success: function (res) {
                SIE.Msg.showInstantMessage('保存成功！', '提示', 3);
                me.isCanExecute = false;
                entity.markSaved();
                me.view.syncCmdState();
                window.setTimeout(function () {
                    CRT.Event.fire("SIE.Fixtures.Fixtures.Accounts.FixtureIDAccount_refresh");
                    CRT.Workbench.closeCurrentTab();
                }, 3);

            }
        });
    },
    validation: function (current) {
        if (current.getFixtureEncodeId() == null) {
            return '【工治具编码】不能为空!'.t();
        }
        if (current.getCode() == "") {
            return '【工治具ID】不能为空!'.t();
        }
        if (current.getProprietorship() == null) {
            return '【产权归属】不能为空!'.t();
        }
        if (current.getFixedStorage() == 1 && (current.getFixtureWarehouseId() == null || current.getFixtureStorageLocationId() == null)) {
            return '当【固定储位】为是时,必须维护【仓库编码】和【库位编码】!'.t();
        }
        if (current.getProprietorship() === 10 && current.getSupplierId() == null) {
            return '产权归属为【租凭】时供应商编码不能为空！'.t();
        }
        if (current.getProprietorship() === 15 && current.getCustomerId() == null) {
            return '产权归属为【客供】时客户编码不能为空！'.t();
        }
        return "";
    }
});