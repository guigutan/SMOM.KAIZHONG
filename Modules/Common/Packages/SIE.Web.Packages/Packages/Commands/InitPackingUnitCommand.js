SIE.defineCommand('SIE.Web.Packages.Packages.Commands.InitPackingUnitCommand', {
    meta: { text: "初始化", group: "edit", iconCls: "icon-NetworkNormal icon-green" },

    execute: function (view, source) {
        view.execute({
            data: {},
            withIds: true,
            success: function (res) { //回调
                SIE.Msg.showInstantMessage('初始化完成！'.t());
                view.reloadData();
            }
        });
    }
});