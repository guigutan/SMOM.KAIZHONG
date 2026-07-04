SIE.defineCommand("SIE.Web.KZ.Base.Interfaces.Commands.InfNcDataLogGroupReUploadCommand", {
    meta: { text: "重新同步", group: "edit", iconCls: "icon-Sync icon-yellow" },//Delete.icon - Delete 黄色


    //自定义命令
    //是否可执行
    canExecute: function (view) {
        //var entity = view.getCurrent();
        //if (entity == null) {
        //    return false;
        //}


        //this.selectedItems = view.getSelection();
        //if (this.selectedItems.length === 0) {
        //    return false;
        //}
        //for (i = 0, len = this.selectedItems.length; i < len; i++) {
        //    var item = this.selectedItems[i];
        //    if (item.data.SyncState >= 2) {
        //        return false;
        //    }
        //}
        return true;
    },

    execute: function (ListView, source) {
        var me = this;
        //当前页面 的list

        var selections = ListView.getSelection();
        var ids = selections.map(function (item) { return item.getId(); });
        SIE.Msg.wait("正在处理，请稍等...".t());
        ListView.execute({
            data: ids,
            success: function (res) {
                SIE.Msg.showMessage(res.Result);
                ListView.reloadData();
            }
        });
    }
});