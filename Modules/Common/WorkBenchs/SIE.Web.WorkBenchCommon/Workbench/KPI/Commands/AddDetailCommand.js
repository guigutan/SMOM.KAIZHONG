SIE.defineCommand('SIE.Web.WorkBenchCommon.Workbench.KPI.Commands.AddDetailCommand', {
    extend: 'SIE.cmd.Add',
    meta: { text: "添加", group: "edit", iconCls: "icon-AddEntity icon-green" },

    onItemCreated: function (entity) {
        entity.setDataType(this.view.getParent().getCurrent().getDataType());
        this.callParent(arguments);
    }
});