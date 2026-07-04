SIE.defineCommand('SIE.Web.Inventory.Transactions.Commands.InitExcuteQueryCommand', {
    meta: { text: "查询", iconCls: "icon-Search icon-blue" },

    canExecute: function (view) {
        return true;
    },

    /*
    * 客制化查询参数 
    * @param view 查询逻辑视图
    */
    execute: function (view) {
        var me = this;
        me.view = view;
        var record = view.getCurrent();
        var criteria = [];
        criteria.push(record.data["Code"]);
        criteria.push(record.data["Name"]);
        var type = 'SIE.Web.Inventory.Transactions.DataQueryer.FunctionDataQuery';
        var method = 'GetInitFunctions';
        me.tryDataQuery(type, method, criteria);
    },

    /*
        * 执行查询方法 
        * @param type 查询方法类
        * @param method 查询方法
        * @param criteria 查询条件
        */
    tryDataQuery: function (type, method, criteria) {
        var me = this;
        SIE.invokeDataQuery({
            method: method,
            params: criteria,
            action: 'queryer',
            type: type,
            token: me.view.getToken(),
            success: function (res) {
                var mainView = me.view._relations[0]._target;
                var control = mainView.getControl();
                control.setStore(null);
                control.setStore(res.Result);
            }
        });
    }

})