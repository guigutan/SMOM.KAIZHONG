SIE.defineCommand('SIE.Web.EMS.FixedAssets.Accounts.Commands.OpenResumeBillViewCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "打开", group: "edit", iconCls: "icon-Search icon-blue" },
    canExecute: function (listView) {
        var current = listView.getCurrent();
        if (current === null) { return false; }
        var no = current.getNo();
        if (no =="") return false;
        return true;
    },
    execute: function (listView, source) {
        var no = listView.getCurrent().getNo();
        var type = listView.getCurrent().getResumeType();
        if (no != "") {//报修 维修
           
        }
    }
});