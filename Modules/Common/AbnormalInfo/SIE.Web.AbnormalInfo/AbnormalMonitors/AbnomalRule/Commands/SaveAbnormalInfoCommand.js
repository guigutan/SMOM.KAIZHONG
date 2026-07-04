SIE.defineCommand('SIE.Web.AbnormalInfo.AbnormalMonitors.AbnomalRule.Commands.SaveAbnormalRuleCommand', {
    extend: 'SIE.cmd.FormSave',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },
    /**
     * 是否可执行
     * @param {any} view
     */
    canExecute: function () {
        return true;
    },
    doSave: function (view) {
        var me = this;
        var mainView = CRT.Context.PageContext.getLogicalView();
        me.indicatorViewAssign(mainView);
        var indata = me.getIndata(view);
        if (!indata) return;
        indata.success = function (res) {
            me.onSaved(mainView, res);
        };
        view.execute(indata);
    },
    /**
     * @override 保存后处理
     * @param {any} view
     * @param {any} res
     */
    onSaved: function (view, res) {
        var me = this;
        var current = view.getCurrent();
        CRT.Event.fire(view.model + "_refresh", current.data.Id);
        current.markSaved();
        view.syncCmdState();
        me.onSavedMsg(view, res);
    },
    getIndata: function (opt) {
        var mainView = CRT.Context.PageContext.getLogicalView();
        var me = mainView;
        opt = opt || {};
        var indata = {};
        indata.model = me.model;

        opt._changeSetData = me.serializeData(true);
        if (!opt._changeSetData.isEmpty()) {
            var submitData = opt._changeSetData.getSubmitData();
            indata.data = submitData;
        }

        if (opt.withIds) {
            indata.SelectedIds = opt.selectIds;
        }

        if (indata.data || indata.SelectedIds) {
            if (me.getSourceCmd() && me._sourceCmd.ownerCt && me._sourceCmd.ownerCt.ownerCt) {
                var view = me._sourceCmd.ownerCt.ownerCt.SIEView;
                if (view && view._parent) {
                    indata.ParentType = view._parent.model;
                }
            }
            return indata;
        }
    },
    /**
     * 数据赋值
     * @param {any} mainView
     */
    indicatorViewAssign: function (mainView) {
        var sourceView = mainView.getChildren()[1].indicatorOpraView;
        var ctl = mainView.getController();
        //表关系赋值
        var entity = mainView.getCurrent();
        var jsonStr = Ext.JSON.encode(ctl.tabRelations);
        entity.setTabRelations(jsonStr);
        //SQL语句赋值
        var sqlFiled = Ext.getCmp("whereConditionSqlFiled");
        if (sqlFiled) entity.setDisPlaySelect(sqlFiled.getValue());
        //指标运算视图数据赋值
        ctl.viewDataCopy(mainView,sourceView);
    }
});