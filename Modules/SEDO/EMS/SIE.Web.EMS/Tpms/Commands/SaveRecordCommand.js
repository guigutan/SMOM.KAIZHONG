SIE.defineCommand('SIE.Web.EMS.Tpms.Commands.SaveRecordCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },    
    /**
    * 保存
    * @param {} view
    */
    doSave: function (view) {
        var me = this;
        me.view = view;
        var data = {};
        data.TpmRecord = view.getCurrent().data;
        data.TpmRecordDetailList = [];
        me.childView = view.findChild("SIE.EMS.Tpms.TpmRecordDetail");
        if (me.childView) {
            me.childView.getData().getData().items.forEach(function (item) {
                data.TpmRecordDetailList.push(item.getData());
            });
        }

        var indata = {};
        indata.Data = Ext.encode(data);
        view.execute({
            data: indata,
            success: function (res) {
                var recordInfo = res.Result;
                if (recordInfo.ErrMsg !== '') {
                    SIE.Msg.showError(recordInfo.ErrMsg);
                    return;
                }
                else
                    me.onSavedHandler(me);
            }
        });
    },

    /**
     * 保存成功后处理
     * @param {any} me
     */
    onSavedHandler: function (me) {
        var entity = me.view.getCurrent();

        SIE.Msg.showInstantMessage('保存成功！'.t(), '提示', 3);
        
        //标记当前页签内容为unchange
        me.markSaved(me, entity);
        me.view.syncCmdState();

        //刷新主界面
        CRT.Event.fire(me.view.model + "_refresh", entity.data.Id);

        //关闭当前页签
        //me.closeView(view.tabId);
    },

    /**
    * 数据已保存到服务器,前端标记已保存
    * @param {} entity 
    * @returns {} 
    */
    markSaved: function (me, entity) {
        entity.markSaved();
        if (me.childView) {
            var childEntity = me.childView.getCurrent();
            childEntity.markSaved();
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