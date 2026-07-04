SIE.defineCommand('SIE.Web.Inventory.Strategy.Commands.ViewOnShelvesRuleDetailCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "查看", group: "edit", iconCls: "icon-TextQuality icon-blue" },

    canExecute: function (view) {
        if (view.getSelection() == null || view.getCurrent() == null || view.getSelection().length != 1) {
            return false;
        }

        return true;
    },
    showView: function (editEntity) {
        var me = this;
        var meta = null;
        SIE.AutoUI.getMeta({
            isDetail: true,
            ignoreCommands: false,
            ignoreQuery: false,
            isAggt: true,
            token: editEntity.token,
            model: this.view.model,
            viewGroup: "OnShelvesRuleReadOnlyView",
            callback: function (res) {                   
                var detailView = SIE.AutoUI.generateAggtControl(res);
                detailView._view.setCurrent(editEntity);
                var ui = detailView.getControl();                
                var win = SIE.Window.show({
                    title: me.getEditViewTitle(editEntity),
                    width: '75%',
                    height: '70%',
                    buttons: [],
                    items: ui,
                    //callback: function (btn) {
                    //    if (btn == "确定".t()) {
                    //    }
                    //}
                });
            }
        });
    },
});