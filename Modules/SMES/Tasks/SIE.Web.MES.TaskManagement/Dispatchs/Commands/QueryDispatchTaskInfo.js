/**
 * 派工管理查询命令
 */
SIE.defineCommand('SIE.Web.MES.TaskManagement.Dispatchs.Commands.QueryDispatchTaskInfo', {
    meta: { text: "查询", iconCls: "icon-Search icon-blue" },
    executeIntervalMode: SIE.cmd.IntervalMode.None.value,
    execute: function (view) {
        var record = view.getCurrent();
        delete record.data['CriteriaModuleKey'];
        delete record.data['CriteriaType'];
        delete record.data["CriteriaString"];
        var istrue = true;
        view.getControl().items.items.forEach(function (item) {
            if (!item.validate()) {
                istrue = false;
            }
        });
        if (istrue) {
            view.tryExecuteQuery({
                clearSort: true,
                action: 'entity'
            });
        }

        var adoNameBoxControl = Ext.getCmp("adoNameBoxId");
        var store = adoNameBoxControl.getStore();
        store.setData(null);
        adoNameBoxControl.setStore(store);

        var dragControl = Ext.getCmp('taskPerfomerId');
        var grid1Id = dragControl.items.items[0].id;//todo 
        var grid1Control = Ext.getCmp(grid1Id);
        var grid2Id = dragControl.items.items[1].id;//todo 
        grid2Control = Ext.getCmp(grid2Id);

        if (grid1Control.getStore().data)
            grid1Control.getStore().data.clear();
        if (grid2Control.getStore().data)
        grid2Control.getStore().data.clear();
        var store = grid1Control.getStore();
        store.setData(null);
        grid1Control.setStore(store);

        var store = grid2Control.getStore();
        store.setData(null);
        grid2Control.setStore(store);
    }
});