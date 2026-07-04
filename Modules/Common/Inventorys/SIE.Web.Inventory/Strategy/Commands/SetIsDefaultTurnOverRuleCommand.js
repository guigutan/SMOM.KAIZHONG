SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.SetIsDefaultTurnOverRuleCommand', {
    meta: { text: "设为默认", group: "edit", iconCls: "iconfont icon-SettingFinish icon-blue" },
    canExecute: function (view) {
        this.selectedItems = view.getSelection();
        if (this.selectedItems == null || this.selectedItems.length === 0 || this.selectedItems.length !== 1) {
            return false;
        }

        for (i = 0, len = this.selectedItems.length; i < len; i++) {
            var item = this.selectedItems[i];
            if (SIE.Domain.State.Enable.value !== item.getState()) {
                return false;
            }

            if (item.data.IsDefault === true) {
                return false;
            }
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