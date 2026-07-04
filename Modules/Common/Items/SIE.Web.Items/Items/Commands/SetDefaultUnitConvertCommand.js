SIE.defineCommand('SIE.Web.Items.Items.Commands.SetDefaultUnitConvertCommand', {
    meta: { text: "设为默认", group: "edit", iconCls: "iconfont icon-SettingFinish icon-blue" },
    canExecute: function (view) {
        this.selectedItems = view.getSelection();
        if (this.selectedItems == null || this.selectedItems.length === 0 || this.selectedItems.length !== 1 || this.selectedItems[0].data.IsDefault || this.selectedItems[0].data.CreateBy == null || this.selectedItems[0].data.IsBaseUnit) {
            return false;
        }
        return true;
    },
    execute: function (view, source) {
        SIE.Msg.askQuestion(Ext.String.format('您确认进行设置默认操作吗?'.t()), function () {
            view.execute({
                data: view.getSelectionIds(),
                success: function (res) { //回调
                    view.reloadData();
                }
            });
        });
    }
});