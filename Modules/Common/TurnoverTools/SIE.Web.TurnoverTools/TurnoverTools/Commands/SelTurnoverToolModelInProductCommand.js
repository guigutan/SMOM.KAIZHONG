SIE.defineCommand('SIE.Web.Elec.MES.TurnoverTools.Commands.SelTurnoverToolModelInProductCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'ProductId', targetClassName: 'SIE.Items.Item' }
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    _itemId: null,
    canExecute: function (view) {
        var parent = view.getParent();
        if (parent==null) return false;
        var data = view.getParent().getCurrent();
        if (data == null) return false;
        return true;
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
                if (me._sourceViewSelectItems.indexOf(sel.getId()) === -1) {
                    //创建产品容量model
                    var model = Ext.create(me.view.model);
                    model.setProductId(sel.getId());
                    model.setProductId_Display(sel.getCode());
                    model.setProductCode(sel.getCode());
                    model.setProductName(sel.getName());
                    //获取默认容量
                    var defaultCapacity = me.view.getParent().getCurrent().getDefaultCapacity();
                    model.setCapacity(defaultCapacity);
                    lists.push(model);
                }
            });
            me._ownerView.getData().setData(lists);
            win.close();
        }
    },
    onLoad: function (store, records, successful, operation, eOpts) {
        var me = this;
        var entitys = store.data.items;
        store.setData(entitys);
        this.callParent(arguments);
    },
    execute: function (view, source) {
        var me = this;
        SIE.AutoUI.getMeta({
            model: me.dataParams.targetClassName,
            ignoreChild: true,
            ignoreCommands: true,
            isReadonly: false,
            ignoreQuery: false,
            isAggt: true,
            callback: function (res) {
                var blocks = res;
                me._gridBlockProcess(blocks);
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                me._popupWin(ui, source);
                me.cloneStore = view.getData();
                me._reloadTargetViewData();
            }
        });
    }
});
