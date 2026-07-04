SIE.defineCommand('SIE.Web.Fixtures.Accounts.Commands.EditSaveIDAccountCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onValidation: function (view) {
        var me = this;
        var current = view.getCurrent();
        if (current.getProprietorship() == null) {
            SIE.Msg.showInstantMessage('【产权归属】不能为空!'.t());
            return false;
        }
        if (current.getProprietorship() === 10 && current.getSupplierId() == null) {
            SIE.Msg.showInstantMessage('产权归属为【租凭】时供应商编码不能为空!'.t());
            return false;
        }
        if (current.getProprietorship() === 15 && current.getCustomerId() == null) {
            SIE.Msg.showInstantMessage('产权归属为【客供】时客户编码不能为空！'.t());
            return false;
        }
        return true;
    },
    /**
 *  保存后事件
 * @param {} view 
 * @returns {} 
 */
    onSaved: function (view, res) {
        var current = view.getCurrent();
        current.markSaved();
        SIE.Msg.showInstantMessage('保存成功！'.t());
        window.setTimeout(function () {
            CRT.Event.fire(view.model + "_refresh", current.data.Id);
            CRT.Workbench.closeCurrentTab();
        }, 3000);
    }
});