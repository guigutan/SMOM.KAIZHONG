SIE.defineCommand('SIE.Web.Equipments.EquipAccounts.Commands.CopyAccountCommand', {
    extend: 'SIE.cmd.Copy',
    meta: { text: "复制新增", group: "edit", iconCls: "iconfont icon-AddEntity icon-green", splitTo: "添加" },
    addPage: function (opt) {
        var entity = this.view.getCurrent();
        if (entity)
            opt["params"] = { TreePId: null, SourceEquipId: entity.getId() };

        CRT.Workbench.addPage(opt);
    }
});