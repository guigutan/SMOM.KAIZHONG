SIE.defineCommand('SIE.Web.Fixtures.Repairs.Commands.SaveRepairCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    executeIntervalMode: SIE.cmd.IntervalMode.Debounce.value,//使用防抖模式

    /**
    * 保存
    * @param {} view
    */
    doSave: function (view) {
        var me = this;
        me.view = view;
        var data = {};
        data.FixtureRepair = view.getCurrent().data;
        data.FixtureRepairDetailList = [];
        data.FixtureRepairRecordList = [];
        me.childView = view.findChild("SIE.Fixtures.Repairs.FixtureRepairDetail");
        if (me.childView) {
            me.childView.getData().getData().items.forEach(function (item) {
                data.FixtureRepairDetailList.push(item.getData());
                  item.SpareList().getData().items.forEach(function (record) {
                     data.FixtureRepairRecordList.push(record.getData());
                    });
            });
        }

        var indata = {};
        indata.Data = Ext.encode(data);
        view.execute({
            data: indata,
            success: function (res) {
                var errMsg = res.Result;
                if (errMsg !== '') {
                    SIE.Msg.showError(errMsg);
                    return;
                }
                else
                    me.onSavedHandler(me, res);
            }
        });
    },

    /**
     * 保存成功后处理
     * @param {any} me 当前页面
     * @param {any} res
     */
    onSavedHandler: function (me, res) {
        SIE.Msg.showInstantMessage('保存成功！', '提示', 3);
        var entity = me.view.getCurrent();
        me.entity = entity;

        //标记当前页签内容为unchange
        me.markSaved(me);
        me.view.syncCmdState();

        //刷新主界面
        CRT.Event.fire(me.view.model + "_refresh", entity.data.Id);

        //关闭当前页签
        me.closeView(me.view.tabId);
    },

    /**
    * 数据已保存到服务器,前端标记已保存
    * @param {} me 当前页面
    * @returns {} 
    */
    markSaved: function (me) {
        me.entity.markSaved();
        if (me.childView) {
            me.childView.getData().getData().items.forEach(function (childEntity) {
                childEntity.markSaved();
            });
        }
    },

    /**
    * 关闭页签
    * @param {} tabId
    * @returns {} 
    */
    closeView: function (tabId) {
        var tab = CRT.Workbench.getTabById(tabId);
        var tabPanel = CRT.Workbench.getTabPanel();
        tabPanel.remove(tab);
    }
});