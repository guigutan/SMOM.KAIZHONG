SIE.defineCommand('SIE.Web.Tech.Routings.Commands.AddProductRoutingBom', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'Id', targetClassName: 'SIE.Elec.Core.ProductBoms.ProductBomDetailViewModel' }
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    canExecute: function (view) {
        return true;
    },
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: 'SIE.Elec.Core.ProductBoms.ProductBomDetail',
            viewGroup: 'ProductBomDetailSelectViewConfig',
            ignoreChild: true,
            ignoreCommands: true,
            isReadonly: true,
            ignoreQuery: false,
            isAggt: true,
            callback: function (res) {
                var blocks = res;
                me._queryBlockProcess(blocks);
                me._gridBlockProcess(blocks);
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                var conditionView = ui._view.getConditionView();
                //禁用清除过滤条件按钮
                //if (conditionView._commands && conditionView._commands.items) {
                //    conditionView._commands.items.forEach(function (item) {
                //        if (item.meta.name == 'SIE.cmd.ClearCondition')
                //            item.canExecute = function (view) { return false; }
                //    });
                //}
                
                me._popupWin(ui, source);
                me.cloneStore = view.getData();
                me._reloadTargetViewData();
            }
        });
    },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        var selections = this._targetView.getSelection();
        if (selections && selections.length > 0) {
            var lists = me._ownerView.getData().getData().items;
            selections.forEach(function (sel) {
                if (me._sourceViewSelectItems.indexOf(sel.getId()) === -1)
                    lists.push(sel);
            });
            me._ownerView.getData().setData(lists);
            win.close();
        }
    }
});
