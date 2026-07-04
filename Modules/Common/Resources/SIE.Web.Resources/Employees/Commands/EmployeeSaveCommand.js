SIE.defineCommand('SIE.Web.Resources.Employees.Commands.EmployeeSaveCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    onSaved: function (view, res) {
        this.callParent(arguments);
        var me = this;
        var control = me.view.getControl().query('[name=Code]')[0];
        control["setReadOnly"](true);
    }
});