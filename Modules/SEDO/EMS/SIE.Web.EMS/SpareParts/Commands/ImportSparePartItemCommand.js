SIE.defineCommand('SIE.Web.EMS.SpareParts.Commands.ImportSparePartItemCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "同步物料信息", group: "edit", iconCls: "icon-Receive icon-blue" },
    canExecute: function (listview) {
        return true;
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion('确认同步物料信息?'.t(), function () {
            SIE.Msg.wait('正在同步物料信息，请稍候...'.t());
            view.execute({
                withIds: true,
                selectIds: [0],
                data:[],
                success: function (res) {
                    SIE.Msg.hide();
                    SIE.Msg.showToast('同步物料信息成功'.t(), '完成'.t());
                    view.reloadData();
                }
            });
        });
    }
});