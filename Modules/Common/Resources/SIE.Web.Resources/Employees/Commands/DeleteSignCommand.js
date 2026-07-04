SIE.defineCommand('SIE.Web.Resources.Employees.Commands.DeleteSignCommand', {
    extend: 'SIE.cmd.Delete',
    meta: { text: "删除", group: "edit", iconCls: "icon-DeleteEntity icon-red" },
    canExecute: function (listview) {
        this.selectedItems = listview.getSelection();
        if (this.selectedItems.length === 0 || this.selectedItems.length > 1) {
            return false;
        }
        if (listview.getData().isDirty())
            return false;

        for (i = 0, len = this.selectedItems.length; i < len; i++) {
            var item = this.selectedItems[i];
            if (item.data.State === true) {
                return false;
            }
        }
        return true;
    },
    execute: function (view, source) {
        var me = this;
        var curr = view.getCurrent();
        var postdata = {
            Id: curr.data.Id,
        };
        SIE.Msg.askQuestion(Ext.String.format('确定删除选中数据？'.L10N(), this.selectedItems.length),
            function () {
                //减少网络传输，此自定义命令只需要传选中的ID
                view.execute({
                    data: postdata,
                    success: function (res) {
                        view.reloadData();
                    },
                });
            });
    }
});