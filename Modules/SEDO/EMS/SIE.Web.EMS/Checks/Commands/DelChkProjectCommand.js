SIE.defineCommand('SIE.Web.EMS.Checks.Commands.DelChkProjectCommand', {
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity" },
    canExecute: function (view) {
        return view.getCurrent();
    },
    execute: function (view, source) {
        if (view.getSelection().length >= view.getData().getData().length) {
            SIE.Msg.showInstantMessage('任务必须存在一个或以上的项目，请重新选择！'.t());
            return;
        }
        var sels = view.getSelection();
        var delChkProjects = [];
        Ext.each(sels, function (item) {
            delChkProjects.push(item.data);
        });
        var indata = {};
        var data = { DelChkProjects: delChkProjects };
        indata.Data = Ext.encode(data);

        view.execute({
            data: indata,
            success: function (res) { //回调
                Ext.each(sels, function (item) {
                    view.getData().remove(item);
                });
                SIE.Msg.showInstantMessage('删除成功'.t());
            }
        });
    }
});
