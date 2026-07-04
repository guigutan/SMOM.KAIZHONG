SIE.defineCommand('SIE.Web.EMS.EquipRepair.EquipRepairs.Commands.SelWorkingHourCommand', {
    extend: 'SIE.cmd.LookupCommandBase',
    userConfig: {
        dataParams: {
            specKeyPrototyName: 'RepairerId',
            //targetClassName: 'SIE.Resources.Employees.Employee',
            //targetCriteriaClassName: 'SIE.Resources.Employees.Criterias.EmployeeByRepairAccountCriteria'
            targetClassName: 'SIE.EMS.EquipRepair.EquipRepairs.ViewModels.Employees.EmployeeByRepairAccount',
            //targetCriteriaClassName: ' '

        },
    },
    canExecute: function (view) {
        //var entity = view.getParent().getCurrent();
        //if (entity != null && entity.data) {
        //    return entity.data.INV_ORG_ID !== null;
        //}
        //如果没有父视图
        
        if (view._parent == null) {
            return false;
        }
        var parentEntity = view.getParent().getCurrent();
        if (parentEntity == null) {
            return false;
        }

        //单据状态为“待维修”、“维修中”、“暂停中”、“待确认”、“待评分”、“已完成”
        if (parentEntity.getRepairState() >= 1 && parentEntity.getRepairState() <= 6) {
            return true;
        }
        return false;
    },
    meta: { text: "选择", group: "edit", iconCls: "icon-PlaylistCheck icon-blue" },
    save: function (win) {
        //获取页面弹选框新勾选的数据
        var newSelectedDatas = this._targetSelectItems.items;
        //按钮所在view
        var ownerView = this._ownerView;
        for (var i = 0; i < newSelectedDatas.length; i++) {
            //创建新的
            var newItem = ownerView.createNewItem();

            var repairerId = newSelectedDatas[i].getId();
            var repairerId_Display = newSelectedDatas[i].getName()
            newItem.setRepairerId(repairerId);
            newItem.setRepairerId_Display(repairerId_Display);

        }

        //刷新页面
        ownerView.refresh();
        //关闭页面
        win.close();
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
                if (dialogView._relations[0]) { //存在查询面板时
                    var clearCM = me._targetView.getConditionView().getCmdControl("SIE.cmd.ClearCondition");
                    clearCM.setHidden(true);
                    var cmds = me._targetView.getConditionView().getCommands();
                    cmds.items.splice(cmds.items.indexOf(clearCM, 1));
                    cmds.keys.splice(cmds.keys.indexOf("SIE.cmd.ClearCondition", 1));

                    var criteria = dialogView._relations[0]._target.getData();

                    
                    var equipAccountId = this._ownerView.getParent().getCurrent().getEquipAccountId();//查询条件
                    var equipAccountId_Display = this._ownerView.getParent().getCurrent().getEquipAccountId_Display();
                    criteria.setEquipAccountId(equipAccountId);//设置查询条件
                    criteria.setEquipAccountId_Display(equipAccountId_Display);
                    //criteria.setIsReadOnly(true);
                    //criteria.IsInvalid = true;
                    dialogView._relations[0]._target.tryExecuteQuery();
                }
                else {
                    dialogView.loadData();
                }
            }
        }
    },
});