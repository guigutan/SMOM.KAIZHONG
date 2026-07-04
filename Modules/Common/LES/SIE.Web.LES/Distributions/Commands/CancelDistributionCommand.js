SIE.defineCommand('SIE.Web.LES.Distributions.Commands.CancelDistributionCommand', {
    extend: 'SIE.cmd.Command',
    meta: { text: "强制完成", group: "edit", iconCls: "icon-CloseView icon-blue" },
    canExecute: function (view) {
        if (!view.hasSelectedEntities()) {
            return false;
        }
        else {
            for (i = 0; i < view.getSelectedEntities().length; i++) {
                var label = view.getSelectedEntities()[i].data;
                if (label.OrderState != 0) {
                    return false;
                }
            }
        }

        return true;
    },
    onSavedMsg: function (view, res) {
        SIE.Msg.showInstantMessage('取消成功'.t());
    },
    execute: function (view, source) {
        var me = this;
        var selIds = view.getSelectionIds();
        SIE.Msg.askQuestion('是否取消选中的配送单?'.t(), function () {
            view.execute({
                data: selIds,
                success: function (res) {
                    if (res.Result) {
                        SIE.Msg.showInstantMessage('取消成功'.t());
                        view.reloadData();
                    }
                }
            });

        });
    }
});