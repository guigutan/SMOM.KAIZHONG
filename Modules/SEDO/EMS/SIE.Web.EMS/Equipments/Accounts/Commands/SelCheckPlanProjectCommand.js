SIE.defineCommand('SIE.Web.EMS.Equipments.Accounts.Commands.SelCheckPlanProjectCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: { specKeyPrototyName: 'ProjectDetailId', targetClassName: 'SIE.EMS.MainenanceProjects.ProjectDetail' }
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    /**
     * 是否执行选择
     * @param {view} view 当前视图
     */
    canExecute: function (view) {
        return true;
    },

    /**
     * 保存选择的操作列表。
     * @param {win} win 窗体
     */
    save: function (win) {
        /// <summary>
        /// 保存选择的操作列表。
        /// </summary>
        var me = this;
        var selections = this._targetView.getSelection();
        if (selections && selections.length > 0) {
            //直接setData(selections)会因查询而丢失数据，所以改用此方法
            var lists = me._ownerView.getData().getData().items;
            selections.forEach(function (sel) {
                var projectDetail = sel.data;
                var projectDetailId = sel.getId();
                if (me._sourceViewSelectItems.indexOf(projectDetailId) === -1) {
                    var result = me.validateData(projectDetailId, lists);
                    if (result) {
                        var entity = me.createCheckProject(projectDetail);
                        lists.push(entity);
                    }
                }
            });
            me._ownerView.getData().setData(lists);
            win.close();
        }
    },

    /**
    * 创建选择项目
    * @param {projectDetail} projectDetail 选择项目
    */
    createCheckProject: function (projectDetail) {
        var me = this;
        var entity = Ext.create("SIE.EMS.Equipments.Accounts.EquipAccountCheckProject");
        entity.setEquipAccountId(me._equipAccountId);
        entity.setEquipAccountCode(me.EquipAccountCode);
        entity.setEquipAccountName(me.EquipAccountName);
        entity.setProjectDetailId(projectDetail.Id);
        entity.setProjectName(projectDetail.Name);
        entity.setProjectType(projectDetail.ProjectType);
        entity.setPart(projectDetail.Part);
        entity.setConsumable(projectDetail.Consumable);
        entity.setMethod(projectDetail.Method);
        entity.setStandard(projectDetail.Standard);
        entity.setMinValue(projectDetail.MinValue);
        entity.setMaxValue(projectDetail.MaxValue);
        entity.setUnit(projectDetail.Unit);
        entity.setUseTime(projectDetail.UseTime);
        return entity;
    },

    /**
    * 验证数据是否已勾选
    * @param {projectDetailId} projectDetailId 当前勾选项目Id
    * @param {lists} lists 已选数据源列表
    */
    validateData: function (projectDetailId, lists) {
        lists.forEach(function (list) {
            if (list.getId() == projectDetailId)
                return false;
        });
        return true;
    },

    /**
    * 加载弹框数据源
    * @param {store} store
    * @param {records} records
    * @param {successful} successful
    * @param {operation} operation
    * @param {eOpts} eOpts
    */
    onLoad: function (store, records, successful, operation, eOpts) {
        var me = this;
        var entitys = store.data.items.where(function (p) { return p.getProjectType() === 0 && p.getCycleType() === me.cycleType });
        store.setData(entitys);
        this.callParent(arguments);
    },

    /**
    * 选择命令执行方法
    * @param {view} view 项目列表逻辑视图
    * @param {source} source 源
    */
    execute: function (view, source) {
        var me = this;
        var addCheckPlanVM = view.getParent().getData();
        me._equipAccountId = addCheckPlanVM.getEquipAccountId();
        if (me._equipAccountId == null) {
            SIE.Msg.showInstantMessage('请选择设备!'.t());
            return false;
        }

        me.EquipAccountCode = addCheckPlanVM.getEquipAccountId_Display();
        me.EquipAccountName = addCheckPlanVM.getMachineNo();

        var checkCycleType = addCheckPlanVM.getCheckCycleType();
        if (checkCycleType == null) {
            SIE.Msg.showInstantMessage('请选择周期类型!'.t());
            return false;
        }

        me.cycleType = checkCycleType === 0 ? 1 : 0;

        SIE.AutoUI.getMeta({
            model: me.dataParams.targetClassName,
            ignoreChild: true,
            ignoreCommands: true,
            isReadonly: true,
            ignoreQuery: false,
            isAggt: true,
            callback: function (res) {
                var blocks = res;
                me._queryBlockProcess(blocks);
                me._gridBlockProcess(blocks);
                var mainBlock = res.mainBlock;
                mainBlock.storeConfig.pageSize = 9999999;
                var ui = SIE.AutoUI.generateAggtControl(blocks);
                me._popupWin(ui, source);
                me.cloneStore = view.getData();
                me._reloadTargetViewData();
            }
        });
    },

    /**
    * 加载弹窗视图的数据
    */
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
                if (dialogView._relations[0]) { //存在查询面板时
                    var clearCM = me._targetView.getConditionView().getCmdControl("SIE.cmd.ClearCondition");
                    clearCM.setHidden(true);
                    var cmds = me._targetView.getConditionView().getCommands();
                    cmds.items.splice(cmds.items.indexOf(clearCM, 1));
                    cmds.keys.splice(cmds.keys.indexOf("SIE.cmd.ClearCondition", 1));
                    var criteria = dialogView._relations[0]._target.getData().data;
                    criteria.ProjectType = 0;
                    criteria.IsReadonly = true;
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
});
