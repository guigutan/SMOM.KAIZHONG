SIE.defineCommand('SIE.Web.EMS.EquipRepair.PlanRepairs.Commands.SelPlanRepairsProjectCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    userConfig: {
        dataParams: { specKeyPrototyName: 'ProjectDetailId', targetClassName: 'SIE.EMS.Equipments.Models.EquipModelRepairProject' }
    },
    /**
     * canExecute 是否执行
     * @param {} view 当前视图
     * @returns {}
     */
    canExecute: function (view) {

        
        var entity = view.getParent().getCurrent();
        if (entity == null|| entity.data.EquipAccountModelId === null) {
            //所属父对象（设备型号）为空 不能点击选择项目
            return false;
        }

        if (entity.data === null) {
            return false;
        }

        //新增时，不能点击选择项目
        if (entity.isNew()) {
            return false;
        }

        return true;
    },
    onLoad: function (store, records, successful, operation, eOpts) {
        var me = this;
        var entitys = store.data.items.where(function (p) { return p.getEquipModelId() == me.view.getParent().getCurrent().getEquipAccountModelId();});
        store.setData(entitys);
        this.callParent(arguments);
    },
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        var indata = {};
        var selections = me._targetSelectItems;
        if (selections && selections.items.length > 0) {
            var items = [];

            SIE.each(selections.items, function (sel) {
                var id = sel.getId();
                if (me._sourceViewSelectItems.indexOf(id) === -1) {
                    var item = {
                        SourceId: me._sourceId,
                        ProjectDetailId: id,
                    };
                    items.push(item);
                }
            });
            if (items.length > 0) {
                indata = items;
                me._targetView.execute({
                    data: indata,
                    success: function (res) {
                        debugger;
                        win.close();
                        me._ownerView.loadChildData(true);
                    }
                }, me._ownerView);
                return true;
            }
        }
        Ext.Msg.alert('提示'.t(), '没有可提交的数据'.t());
    },
    _reloadTargetViewData: function () {
        /// <summary>
        /// 加载弹窗视图的数据
        /// </summary>
        var me = this;
        me._sourceViewSelectItems = this.cloneStore.collect(me.dataParams.specKeyPrototyName);

        var dialogView = me._targetView;

        if (me._targetView !== null) {

            var store = dialogView.getData();

            if (store !== null) {
                me.mon(store, 'load', me.onLoad, this);
                if (dialogView._relations[0]) {
                    //存在查询面板时
                    var clearCM = me._targetView.getConditionView().getCmdControl("SIE.cmd.ClearCondition");
                    clearCM.setHidden(true);
                    var cmds = me._targetView.getConditionView().getCommands();
                    cmds.items.splice(cmds.items.indexOf(clearCM, 1));
                    cmds.keys.splice(cmds.keys.indexOf("SIE.cmd.ClearCondition", 1));
                    var criteria = dialogView._relations[0]._target.getData().data;

                    //计划维修
                    criteria.ProjectType = 11;
                    criteria.IsReadonly = true;
                    dialogView._relations[0]._target.tryExecuteQuery();

                    dialogView.loadData({
                        callback: function (res) {
                            var projectTypeField = dialogView._relations[0]._target.getControl().getForm().findField("ProjectType");
                            if (projectTypeField) {
                                projectTypeField.setReadOnly(true);
                            }
                        }
                    });
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
});