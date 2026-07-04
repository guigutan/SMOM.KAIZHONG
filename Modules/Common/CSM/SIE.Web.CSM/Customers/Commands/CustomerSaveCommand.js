/**
 * 客户保存按钮
 */
SIE.defineCommand('SIE.Web.CSM.Customers.Commands.CustomerSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onValidation: function (view) {
        if (view.getCurrent() && view.getCurrent().getData().CustomerType === null) {
            SIE.Msg.showError('客户类型不能为空!'.t());
            return false;
        } else {
            return true;
        }
    }
});