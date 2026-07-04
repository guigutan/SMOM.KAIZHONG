SIE.defineCommand('SIE.Web.Core.Prints.Commands.InitDefaultTplCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "初始化默认模板", group: "edit", iconCls: "icon-PrintData icon-blue" },
    execute: function (view, source) {
        //var entity = view && view.getCurrent() && view.getCurrent().data;
        //if (entity == null) return;
        //var id = entity.Id;
        //var dataKey = entity.DataKey;
        var command = Ext.getClassName(this);
        view.execute({
            command: command,
            data: id,
            success: function (res) {
                view.reloadData();
                SIE.Msg.showInstantMessage('初始化完成!'.t(), "初始化", 3);

            }
        });
    }
});